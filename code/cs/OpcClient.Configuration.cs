// DOC
client.Configuration = OpcApplicationConfiguration.LoadClientConfig("Opc.UaFx.Client");

// DOC
client.Configuration = OpcApplicationConfiguration.LoadClientConfigFile("MyClientAppNameConfig.xml");

// DOC
// Default: Value of AssemblyTitleAttribute of entry assembly.
client.ApplicationName = "MyClientAppName";
 
// Default: A null reference to auto complete on connect to "urn::" + ApplicationName
client.ApplicationUri = "http://my.clientapp.uri/";

// DOC
client.SessionTimeout = 30000;          // Default: 60000
client.SessionName = "My Session Name"; // Default: null

// DOC
client.OperationTimeout = 10000;        // Default: 60000
client.DisconnectTimeout = 5000;        // Default: 10000
client.ReconnectTimeout = 5000;         // Default: 10000

// DOC
// Default: ".\CertificateStores\Trusted"
client.CertificateStores.ApplicationStore.Path
        = @"%LocalApplicationData%\MyClientAppName\App Certificates";
 
// Default: ".\CertificateStores\Rejected"
client.CertificateStores.RejectedStore.Path
        = @"%LocalApplicationData%\MyClientAppName\Rejected Certificates";
 
// Default: ".\CertificateStores\Trusted"
client.CertificateStores.TrustedIssuerStore.Path
        = @"%LocalApplicationData%\MyClientAppName\Trusted Issuer Certificates";
 
// Default: ".\CertificateStores\Trusted"
client.CertificateStores.TrustedPeerStore.Path
        = @"%LocalApplicationData%\MyClientAppName\Trusted Peer Certificates";

// DOC
var certificate = OpcCertificateManager.LoadCertificate("MyClientCertificate.pfx");

// DOC
var certificate = OpcCertificateManager.CreateCertificate(client);

// DOC
OpcCertificateManager.SaveCertificate("MyClientCertificate.pfx", certificate);

// DOC
client.Certificate = certificate;

// DOC
if (!client.CertificateStores.ApplicationStore.Contains(certificate))
    client.CertificateStores.ApplicationStore.Add(certificate);


// DOC
client.CertificateStores.AutoCreateCertificate = false;

// DOC
client.Security.UserIdentity = new OpcClientIdentity("userName", "password");

// DOC
client.Security.UserIdentity = new OpcCertificateIdentity(new X509Certificate2("Doe.pfx"));

// DOC
client.Security.UserIdentity = null;

// DOC
client.Security.UseOnlySecureEndpoints = true;

// DOC
client.Security.UseHighLevelEndpoint = true;

// DOC
client.Security.EndpointPolicy = new OpcSecurityPolicy(
        OpcSecurityMode.None, OpcSecurityAlgorithm.Basic256Sha256);

// DOC
using (var client = new OpcDiscoveryClient("opc.tcp://localhost:4840/")) {
    var endpoints = client.DiscoverEndpoints();
 
    foreach (var endpoint in endpoints) {
        // Your code to operate on each endpoint.
    }
}

// DOC
client.Security.VerifyServersCertificateDomains = true;

// DOC
client.Security.AutoAcceptUntrustedCertificates = false;

// DOC
client.CertificateValidationFailed += HandleCertificateValidationFailed;
...
private void HandleCertificateValidationFailed(
        object sender,
        OpcCertificateValidationFailedEventArgs e)
{
    if (e.Certificate.Thumbprint == "...")
        e.Accept = true;
}

// DOC
// In context of the event handler the sender is an OpcClient.
var client = (OpcClient)sender;
 
if (!client.CertificateStores.TrustedPeerStore.Contains(e.Certificate))
    client.CertificateStores.TrustedPeerStore.Add(e.Certificate);


// DOC
var configuration = OpcApplicationConfiguration.LoadClientConfigFile("MyClientAppNameConfig.xml");

// The method returns a null reference in case the file could not be loaded.
if (configuration == null)
    throw new InvalidOperationException("The client configuration could not be loaded.");

client.Configuration = configuration;

// DOC
// Default: Value of AssemblyTitleAttribute of entry assembly.
client.ApplicationName = "MyClientAppName";

// Default: A null reference to use the URI of the application certificate.
// The value is used to create a new application certificate.
client.ApplicationUri = new Uri("urn:localhost:MyClientAppName");

// DOC
client.SessionTimeout = 30000;          // Default: 60000
client.SessionName = "My Session Name"; // Default: Value of ApplicationName

// DOC
client.OperationTimeout = 10000;        // Default: 120000
client.DisconnectTimeout = 5000;        // Default: 10000
client.ReconnectTimeout = 5000;         // Default: 10000

// DOC
// Default: ".\CertificateStores\App"
client.CertificateStores.ApplicationStore.Path
        = @"%LocalApplicationData%\MyClientAppName\App Certificates";

// Default: ".\CertificateStores\Rejected"
client.CertificateStores.RejectedStore.Path
        = @"%LocalApplicationData%\MyClientAppName\Rejected Certificates";

// Default: ".\CertificateStores\Trusted"
client.CertificateStores.TrustedIssuerStore.Path
        = @"%LocalApplicationData%\MyClientAppName\Trusted Issuer Certificates";

// Default: ".\CertificateStores\Trusted"
client.CertificateStores.TrustedPeerStore.Path
        = @"%LocalApplicationData%\MyClientAppName\Trusted Peer Certificates";

// DOC
var certificate = OpcCertificateManager.LoadCertificate("MyClientCertificate.pfx", "password");

// DOC
client.Security.EndpointPolicy = new OpcSecurityPolicy(
        OpcSecurityMode.SignAndEncrypt, OpcSecurityAlgorithm.Basic256Sha256);

// DOC
client.Security.EndpointPolicy = new OpcSecurityPolicy(
        OpcSecurityMode.SignAndEncrypt, OpcSecurityAlgorithm.Basic256Sha256);

// Use the next better endpoint in case the server does not offer the policy.
client.Security.AutoUpgradeEndpointPolicy = true;

client.Connect();
Console.WriteLine("Used endpoint: {0}", client.UsedEndpoint);

// DOC
client.Security.CertificateValidationRules
        = OpcCertificateValidationRules.RejectSHA1Signed
        | OpcCertificateValidationRules.DisableBackwardsCompatibleValidation;

// DOC
client.CertificateJudgement += HandleCertificateJudgement;
...
private void HandleCertificateJudgement(
        object sender,
        OpcCertificateJudgementEventArgs e)
{
    // The server uses a different certificate than when the connection was established.
    if (e.Impeachment == OpcCertificateImpeachment.Changed
            && e.ImpeachedCertificate.Thumbprint == "...")
        e.Dismiss = true;
}

// DOC
client.Transport.MaxStringLength = 1048576;      // Default: 65535
client.Transport.MaxByteStringLength = 10485760; // Default: 5242880
client.Transport.MaxArrayLength = 1000000;       // Default: 65535
client.Transport.MaxMessageSize = 20971520;      // Default: 10485760
