// DOC
var certificate = OpcCertificateManager.LoadCertificate("MyClientCertificate.pfx");
client.Certificate = certificate;

// DOC
[assembly: AssemblyTitle("<Common Name (CN) in Certificate>")]

// DOC
client.ApplicationName = "<Common Name (CN) in Certificate>";
client.ApplicationUri = new Uri("<Domain Component (DC) in Certificate>");

// DOC
client.ApplicationName = "MyDifferentClientAppName";

// DOC
client.ApplicationUri = new Uri("<ApplicationUri>");

// DOC
// The lifetime is specified in months, the default is 600.
var settings = new OpcCertificateSettings("MyClientAppName") {
    ApplicationUri = new Uri("urn:localhost:MyClientAppName"),
    Lifetime = 60
};
 
settings.DomainNames.Add("localhost");
 
var certificate = OpcCertificateManager.CreateCertificate(settings);
OpcCertificateManager.SaveCertificate("MyClientCertificate.pfx", certificate);
