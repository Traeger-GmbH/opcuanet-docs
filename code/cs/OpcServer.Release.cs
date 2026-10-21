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

// DOC
var certificate = OpcCertificateManager.LoadCertificate("MyServerCertificate.pfx");
server.Certificate = certificate;

// DOC
// Lifetime in months, default: 600.
var settings = new OpcCertificateSettings("MyServerAppName") {
    ApplicationUri = new Uri("urn:localhost:MyServerAppName"),
    Lifetime = 60
};
 
settings.DomainNames.Add("localhost");
settings.DomainNames.Add("myserver.mydomain.com");
 
var certificate = OpcCertificateManager.CreateCertificate(settings);
OpcCertificateManager.SaveCertificate("MyServerCertificate.pfx", certificate);

// DOC
using (var discoveryServer = new OpcDiscoveryServer("opc.tcp://localhost:4840/")) {
    discoveryServer.Start();
 
    Console.WriteLine("Press any key to stop the discovery server.");
    Console.ReadKey(intercept: true);
}

// DOC
using (var discovery = new OpcDiscoveryClient("opc.tcp://localhost:4840/")) {
    foreach (var application in discovery.DiscoverServers())
        Console.WriteLine("{0}: {1}", application.Name, application.Uri);
}

