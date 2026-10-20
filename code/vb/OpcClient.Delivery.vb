'DOC
Dim certificate = OpcCertificateManager.LoadCertificate("MyClientCertificate.pfx")
client.Certificate = certificate

'DOC
<Assembly: AssemblyTitle("<Common Name (CN) in Certificate>")>

'DOC
client.ApplicationName = "<Common Name (CN) in Certificate>"
client.ApplicationUri = New Uri("<Domain Component (DC) in Certificate>")

'DOC
client.ApplicationName = "MyDifferentClientAppName"

'DOC
client.ApplicationUri = New Uri("<ApplicationUri>")

'DOC
Dim settings = New OpcCertificateSettings("MyClientAppName") With {
    .ApplicationUri = New Uri("urn:localhost:MyClientAppName"),
    .Lifetime = 60 'Months, default: 600
}

settings.DomainNames.Add("localhost")

Dim certificate = OpcCertificateManager.CreateCertificate(settings)
OpcCertificateManager.SaveCertificate("MyClientCertificate.pfx", certificate)
