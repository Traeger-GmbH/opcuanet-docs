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
