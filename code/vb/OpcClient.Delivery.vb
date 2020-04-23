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
