// DOC
var server = new OpcServer("opc.tcp://192.168.0.10:4840/");
 
// Only encrypted endpoints with current algorithms (removes the implicit None endpoint).
server.Security.EndpointPolicies.Add(new OpcSecurityPolicy(
        OpcSecurityMode.SignAndEncrypt,
        OpcSecurityAlgorithm.Basic256Sha256));
server.Security.EndpointPolicies.Add(new OpcSecurityPolicy(
        OpcSecurityMode.SignAndEncrypt,
        OpcSecurityAlgorithm.Aes256_Sha256_RsaPss));
 
// Listen only on the network interface that clients use.
server.Binding.Addresses.Add(IPAddress.Parse("192.168.0.10"));
 
// Trust only known client certificates and check them strictly.
server.Security.AutoAcceptUntrustedCertificates = false;
server.Security.CertificateValidationRules
        = OpcCertificateValidationRules.RejectSHA1Signed
        | OpcCertificateValidationRules.DisableBackwardsCompatibleValidation;
 
// No anonymous access, only known users.
server.Security.UserNameAcl.AddEntry("operator", operatorPassword);
server.Security.UserNameAcl.IsEnabled = true;
server.Security.AnonymousAcl.IsEnabled = false;
 
// Report operation limits to the clients.
server.OperationLimits.AutoDetermineLimits = true;
 
// Use a managed certificate instead of a generated one.
server.CertificateStores.AutoCreateCertificate = false;
server.Certificate = OpcCertificateManager.LoadCertificate(certificatePath, certificatePassword);
 
server.Start();

// DOC
public sealed class HashedServerIdentity : OpcServerIdentity
{
    private const int Iterations = 100000;
    private readonly byte[] salt;
 
    public HashedServerIdentity(string userName, byte[] passwordHash, byte[] salt)
        : base(userName, password: passwordHash)
    {
        this.salt = salt;
    }
 
    public static byte[] Hash(string password, byte[] salt)
    {
        using (var pbkdf2 = new Rfc2898DeriveBytes(
                password,
                salt,
                Iterations,
                HashAlgorithmName.SHA256)) {
            return pbkdf2.GetBytes(32);
        }
    }
 
    protected override byte[] TransformPassword(string password)
    {
        return Hash(password, this.salt);
    }
}

// DOC
// Hash and salt come from the user store of the application.
var identity = new HashedServerIdentity("operator", storedHash, storedSalt);
 
server.Security.UserNameAcl.AddEntry(identity);
server.Security.UserNameAcl.IsEnabled = true;

// DOC
var endpoint = new OpcEndpointIdentity(
        "opc.tcp://192.168.0.10:4840/",
        new OpcSecurityPolicy(OpcSecurityMode.SignAndEncrypt, OpcSecurityAlgorithm.Basic256Sha256));
 
// Endpoint access mode Blacklist: only explicitly enabled endpoints are permitted.
var service = server.Security.UserNameAcl.AddEntry(
        "service",
        servicePassword,
        operationAccessMode: OpcAccessControlMode.Whitelist,
        endpointAccessMode: OpcAccessControlMode.Blacklist);
 
service.Enable(endpoint);

// DOC
public sealed class UserCertificateValidator : X509CertificateValidator
{
    public override void Validate(X509Certificate2 certificate)
    {
        using (var chain = new X509Chain()) {
            chain.ChainPolicy.RevocationMode = X509RevocationMode.Online;
 
            if (!chain.Build(certificate))
                throw new SecurityException("The user certificate is not trusted.");
        }
    }
}


// DOC
server.Security.CertificateAcl.CertificateValidator = new UserCertificateValidator();
server.Security.CertificateAcl.AddEntry(userCertificate);
server.Security.CertificateAcl.IsEnabled = true;

// DOC
server.RequestValidating += HandleRequestValidating;
...
private static void HandleRequestValidating(
        object sender,
        OpcRequestValidatingEventArgs e)
{
    // Clients must not change the structure of the address space.
    if (e.RequestType == OpcRequestType.AddNodes
            || e.RequestType == OpcRequestType.DeleteNodes
            || e.RequestType == OpcRequestType.AddReferences
            || e.RequestType == OpcRequestType.DeleteReferences) {
        e.Cancel = true;
    }
}

// DOC
var adminCertificate = OpcCertificateManager.LoadCertificate(adminCertificatePath);
 
var admin = server.Security.CertificateAcl.AddEntry(adminCertificate);
admin.Assign(server.Security.Roles.SecurityAdmin);
server.Security.CertificateAcl.IsEnabled = true;
 
server.CertificateChanged += HandleCertificateChanged;
...
private static void HandleCertificateChanged(
        object sender,
        OpcCertificateChangedEventArgs e)
{
    Console.WriteLine(
            "New certificate: {0} ({1})",
            e.Certificate.Subject,
            e.Certificate.Thumbprint);
}

// DOC
server.SessionActivated += HandleSessionActivated;
server.RequestValidating += HandleCriticalRequest;
...
private static void HandleSessionActivated(object sender, OpcSessionEventArgs e)
{
    Console.WriteLine(
            "{0:u} session {1}: {2}",
            DateTime.UtcNow,
            e.Session.Name,
            e.Session.UsedIdentity);
}
 
private static void HandleCriticalRequest(object sender, OpcRequestValidatingEventArgs e)
{
    if (e.RequestType == OpcRequestType.Write || e.RequestType == OpcRequestType.Call) {
        Console.WriteLine(
                "{0:u} {1} in session {2}",
                DateTime.UtcNow,
                e.RequestType,
                e.Context.SessionId);
    }
}
