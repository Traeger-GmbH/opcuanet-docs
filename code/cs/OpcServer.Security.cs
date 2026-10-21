// DOC
var acl = server.Security.UserNameAcl;
 
var operator1 = acl.AddEntry("operator1", password: "password1");
operator1.Assign(server.Security.Roles.Operator);
 
var admin = acl.AddEntry("admin", password: "password2");
admin.Assign(server.Security.Roles.ConfigureAdmin, server.Security.Roles.SecurityAdmin);
 
acl.IsEnabled = true;

// DOC
admin.Revoke(server.Security.Roles.ConfigureAdmin);

// DOC
var guest = acl.AddEntry(
        "guest",
        password: "password3",
        operationAccessMode: OpcAccessControlMode.Blacklist);
 
// In blacklist mode only explicitly allowed operations are permitted.
guest.Allow(OpcRequestType.Browse, OpcRequestType.Read);

// DOC
server.AuthenticationLimits.MaxFailedAttempts = 3;                  // Default: 5
server.AuthenticationLimits.DelayWindow = TimeSpan.FromSeconds(10); // Default: 5 seconds
server.AuthenticationLimits.Delay = TimeSpan.FromSeconds(2);        // Default: 1 second
server.AuthenticationLimits.MaxDelay = TimeSpan.FromMinutes(1);     // Default: 30 seconds

// DOC
// Default: false
server.AuthenticationLimits.AutoDetermineLimits = true;

// DOC
public class MyUserNameAcl : OpcAccessControlList
{
    private readonly Dictionary<string, OpcPrincipal> principals
            = new Dictionary<string, OpcPrincipal>();
 
    public MyUserNameAcl(OpcServerBase owner)
        : base(
                owner,
                operationAccessMode: OpcAccessControlMode.Whitelist,
                endpointAccessMode: OpcAccessControlMode.Whitelist)
    {
    }
 
    protected override OpcAccessControlTokenType TokenType
    {
        get { return OpcAccessControlTokenType.UserName; }
    }
 
    public override OpcPrincipal Authenticate(OpcIdentityToken identityToken)
    {
        var token = identityToken as OpcUserNameIdentityToken;
 
        // Verify the user name and password, e.g. using an existing user management.
        if (token == null || !UserStore.Verify(token.UserName, token.DecryptedPassword))
            return null;
 
        lock (this.principals) {
            OpcPrincipal principal;
 
            if (!this.principals.TryGetValue(token.UserName, out principal)) {
                principal = new OpcPrincipal(new OpcServerIdentity(token.UserName));
                this.AddEntry(principal);
 
                // Authenticated users are expected to be a member of the AuthenticatedUser role.
                this.Owner.Security.Roles.AuthenticatedUser.Assign(principal);
                this.principals.Add(token.UserName, principal);
            }
 
            return principal;
        }
    }
}

// DOC
var customAcl = new MyUserNameAcl(server);
server.Security.AddAccessControlList(customAcl);
 
customAcl.IsEnabled = true;

// DOC
var admin = server.Security.UserNameAcl.AddEntry("admin", password: "password");
admin.Assign(server.Security.Roles.SecurityAdmin);
 
server.Security.UserNameAcl.IsEnabled = true;
server.CertificateChanged += HandleCertificateChanged;
...
private static void HandleCertificateChanged(
        object sender,
        OpcCertificateChangedEventArgs e)
{
    Console.WriteLine("New application certificate: {0}", e.Certificate.Thumbprint);
}
