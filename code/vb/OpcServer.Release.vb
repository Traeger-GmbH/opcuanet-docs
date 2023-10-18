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
