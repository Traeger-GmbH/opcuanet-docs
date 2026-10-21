// DOC
server.Configuration = OpcApplicationConfiguration.LoadServerConfig("Opc.UaFx.Server");

// DOC
server.Configuration = OpcApplicationConfiguration.LoadServerConfigFile("MyServerAppNameConfig.xml");

// DOC
// Default: Value of AssemblyTitleAttribute of entry assembly.
server.ApplicationName = "MyServerAppName";

// Default: A null reference to auto complete on start to "urn::" + ApplicationName
server.ApplicationUri = "http://my.serverapp.uri/";

// DOC
// Default: ".\CertificateStores\Trusted"
server.CertificateStores.ApplicationStore.Path
        = @"%LocalApplicationData%\MyServerAppName\App Certificates";
        
// Default: ".\CertificateStores\Rejected"
server.CertificateStores.RejectedStore.Path
        = @"%LocalApplicationData%\MyServerAppName\Rejected Certificates";
        
// Default: ".\CertificateStores\Trusted"
server.CertificateStores.TrustedIssuerStore.Path
        = @"%LocalApplicationData%\MyServerAppName\Trusted Issuer Certificates";
        
// Default: ".\CertificateStores\Trusted"
server.CertificateStores.TrustedPeerStore.Path
        = @"%LocalApplicationData%\MyServerAppName\Trusted Peer Certificates";

// DOC
var certificate = OpcCertificateManager.LoadCertificate("MyServerCertificate.pfx");

// DOC
var certificate = OpcCertificateManager.CreateCertificate(server);

// DOC
OpcCertificateManager.SaveCertificate("MyServerCertificate.pfx", certificate);

// DOC
server.Certificate = certificate;

// DOC
if (!server.CertificateStores.ApplicationStore.Contains(certificate))
    server.CertificateStores.ApplicationStore.Add(certificate);


// DOC
server.CertificateStores.AutoCreateCertificate = false;

// DOC
server.Security.AnonymousAcl.IsEnabled = false;

// DOC
var acl = server.Security.UserNameAcl;

acl.AddEntry("username1", "password1");
acl.AddEntry("username2", "password2");
acl.AddEntry("username3", "password3");
...
acl.IsEnabled = true;

// DOC  
var acl = server.Security.CertificateAcl;
  
acl.AddEntry(new X509Certificate2(@".\user1.der"));
acl.AddEntry(new X509Certificate2(@".\user2.der"));
acl.AddEntry(new X509Certificate2(@".\user3.der"));
...
acl.IsEnabled = true;

// DOC
var user1 = acl.AddEntry("username1", "password1");

// DOC
user1.Deny(OpcRequestType.Write);
user1.Deny(OpcRequestType.HistoryUpdate);

// DOC
user1.Allow(OpcRequestType.HistoryUpdate);

// DOC
server.RegisterAddress("https://mydomain.com/");
server.RegisterAddress("net.tcp://192.168.0.123:12345/");

// DOC
server.UnregisterAddress("https://mydomain.com/");

// server.Address becomes: "net.tcp://192.168.0.123:12345/"
server.UnregisterAddress("opc.tcp://localhost:4840/");

// DOC
server.Security.EndpointPolicies.Add(new OpcSecurityPolicy(
	OpcSecurityMode.Sign, OpcSecurityAlgorithm.Basic256Sha256));

// DOC
server.Security.EndpointPolicies.Add(new OpcSecurityPolicy(
	OpcSecurityMode.None, OpcSecurityAlgorithm.None));

// DOC
server.Security.AutoAcceptUntrustedCertificates = false;

// DOC
server.CertificateValidationFailed += HandleCertificateValidationFailed;
...
private void HandleCertificateValidationFailed(object sender, OpcCertificateValidationFailedEventArgs e)
{
    if (e.Certificate.Thumbprint == "...")
        e.Accept = true;
}

// DOC
// In context of the event handler the sender is an OpcServer.
var server = (OpcServer)sender;

if (!server.CertificateStores.TrustedPeerStore.Contains(e.Certificate))
    server.CertificateStores.TrustedPeerStore.Add(e.Certificate);


// DOC
var server = new OpcServer("opc.tcp://localhost:4840/");
server.Address = new Uri("opc.tcp://localhost:4840/");

// DOC
var configuration = OpcApplicationConfiguration.LoadServerConfigFile(
        "MyServerAppNameConfig.xml");
 
// The method returns a null reference in case the file could not be loaded.
if (configuration == null)
    throw new InvalidOperationException("The server configuration could not be loaded.");
 
server.Configuration = configuration;


// DOC
// Default: Value of AssemblyTitleAttribute of entry assembly.
server.ApplicationName = "MyServerAppName";
 
// Default: A null reference to use the URI of the application certificate.
// The value is used to create a new application certificate.
server.ApplicationUri = new Uri("urn:localhost:MyServerAppName");

// DOC
// Default: ".\CertificateStores\App"
server.CertificateStores.ApplicationStore.Path
        = @"%LocalApplicationData%\MyServerAppName\App Certificates";
 
// Default: ".\CertificateStores\Rejected"
server.CertificateStores.RejectedStore.Path
        = @"%LocalApplicationData%\MyServerAppName\Rejected Certificates";
 
// Default: ".\CertificateStores\Trusted"
server.CertificateStores.TrustedIssuerStore.Path
        = @"%LocalApplicationData%\MyServerAppName\Trusted Issuer Certificates";
 
// Default: ".\CertificateStores\Trusted"
server.CertificateStores.TrustedPeerStore.Path
        = @"%LocalApplicationData%\MyServerAppName\Trusted Peer Certificates";

// DOC
var certificate = OpcCertificateManager.LoadCertificate("MyServerCertificate.pfx");

// DOC
var certificate = OpcCertificateManager.LoadCertificate(
        "MyServerCertificate.pfx",
        password: "password");

// DOC
// Lifetime in months, default: 600.
var settings = new OpcCertificateSettings("MyServerAppName") {
    ApplicationUri = new Uri("urn:localhost:MyServerAppName"),
    Lifetime = 60
};
 
settings.DomainNames.Add("localhost");
 
var certificate = OpcCertificateManager.CreateCertificate(settings);

// DOC
server.RegisterAddress("https://mydomain.com/");
server.RegisterAddress("opc.tcp://192.168.0.123:12345/");

// DOC
server.UnregisterAddress("https://mydomain.com/");
 
// server.Address becomes: "opc.tcp://192.168.0.123:12345/"
server.UnregisterAddress("opc.tcp://localhost:4840/");

// DOC
server.Security.EndpointPolicies.Add(new OpcSecurityPolicy(
        OpcSecurityMode.Sign,
        OpcSecurityAlgorithm.Basic256Sha256));

// DOC
// Default: OpcAddressScope.IPvX (IPv4 and IPv6)
server.Binding.AddressScope = OpcAddressScope.IPv4;
 
// Default: empty (bind to all network interfaces of the address scope)
server.Binding.Addresses.Add(IPAddress.Parse("192.168.0.123"));
 
// Default: true (only one process can bind to the port)
server.Binding.UseExclusiveAddress = false;

// DOC
server.Security.CertificateValidationRules
        = OpcCertificateValidationRules.RejectSHA1Signed
        | OpcCertificateValidationRules.DisableBackwardsCompatibleValidation;

// DOC
server.Transport.MaxStringLength = 1048576;      // Default: 65535
server.Transport.MaxByteStringLength = 10485760; // Default: 5242880
server.Transport.MaxArrayLength = 1000000;       // Default: 65535
server.Transport.MaxMessageSize = 20971520;      // Default: 10485760

// DOC
// Default: false (the server does not announce any operation limits)
server.OperationLimits.AutoDetermineLimits = true;
