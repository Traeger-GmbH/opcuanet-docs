// DOC
var certificate = OpcCertificateManager.LoadCertificate("MyServerCertificate.pfx");
server.Certificate = certificate;

// DOC
[assembly: AssemblyTitle("<Common Name (CN) in Certificate>")]


// DOC
server.ApplicationName = "<Common Name (CN) in Certificate>";
server.ApplicationUri = new Uri("<Domain Component (DC) in Certificate>");

// DOC
server.ApplicationName = "MyDifferentServerAppName";

// DOC
server.ApplicationUri = new Uri("<ApplicationUri>");
