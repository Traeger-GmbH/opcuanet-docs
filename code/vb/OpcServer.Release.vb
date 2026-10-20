' DOC
Dim certificate As OpcCertificateManager.LoadCertificate("MyServerCertificate.pfx")
server.Certificate = certificate

' DOC
' The AssemblyTitle attribute declaration is used in the AssemblyInfo.vb file of a VB.NET project:
' <Assembly: AssemblyTitle("<Common Name (CN) in Certificate>")> 

' DOC
server.ApplicationName = "<Common Name (CN) in Certificate>"
server.ApplicationUri = New Uri("<Domain Component (DC) in Certificate>")

' DOC
server.ApplicationName = "MyDifferentServerAppName"

' DOC
server.ApplicationUri = New Uri("<ApplicationUri>")

' DOC
Dim certificate = OpcCertificateManager.LoadCertificate("MyServerCertificate.pfx")
server.Certificate = certificate

' DOC
Dim settings = New OpcCertificateSettings("MyServerAppName") With {
    .ApplicationUri = New Uri("urn:localhost:MyServerAppName"),
    .Lifetime = 60 ' Months, default: 600
}

settings.DomainNames.Add("localhost")
settings.DomainNames.Add("myserver.mydomain.com")

Dim certificate = OpcCertificateManager.CreateCertificate(settings)
OpcCertificateManager.SaveCertificate("MyServerCertificate.pfx", certificate)

' DOC
Using discoveryServer = New OpcDiscoveryServer("opc.tcp://localhost:4840/")
    discoveryServer.Start()

    Console.WriteLine("Press any key to stop the discovery server.")
    Console.ReadKey(True)
End Using

' DOC
Using discovery = New OpcDiscoveryClient("opc.tcp://localhost:4840/")
    For Each application In discovery.DiscoverServers()
        Console.WriteLine("{0}: {1}", application.Name, application.Uri)
    Next
End Using
