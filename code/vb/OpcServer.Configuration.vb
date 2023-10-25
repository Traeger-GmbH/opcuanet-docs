' DOC
server.Configuration = OpcApplicationConfiguration.LoadServerConfig("Opc.UaFx.Server")

' DOC
server.Configuration = OpcApplicationConfiguration.LoadServerConfigFile("MyServerAppNameConfig.xml")

' DOC
' Default: Value of AssemblyTitleAttribute of entry assembly.
server.ApplicationName = "MyServerAppName"

' Default: A null reference to auto complete on start to "urn::" + ApplicationName
server.ApplicationUri = "http://my.serverapp.uri/"

' DOC
' Default: ".\CertificateStores\Trusted"
server.CertificateStores.ApplicationStore.Path 
		= "%LocalApplicationData%\MyServerAppName\App Certificates"

' Default: ".\CertificateStores\Rejected"
server.CertificateStores.RejectedStore.Path 
		= "%LocalApplicationData%\MyServerAppName\Rejected Certificates"

' Default: ".\CertificateStores\Trusted"
server.CertificateStores.TrustedIssuerStore.Path 
		= "%LocalApplicationData%\MyServerAppName\Trusted Issuer Certificates"

' Default: ".\CertificateStores\Trusted"
server.CertificateStores.TrustedPeerStore.Path 
		= "%LocalApplicationData%\MyServerAppName\Trusted Peer Certificates"

' DOC
Dim certificate As OpcCertificateManager.LoadCertificate("MyServerCertificate.pfx")

' DOC
certificate = OpcCertificateManager.CreateCertificate(server)

' DOC
OpcCertificateManager.SaveCertificate("MyServerCertificate.pfx", certificate)

' DOC
server.Certificate = certificate

' DOC
If Not server.CertificateStores.ApplicationStore.Contains(certificate) Then
    server.CertificateStores.ApplicationStore.Add(certificate)
End If

' DOC
server.CertificateStores.AutoCreateCertificate = False

' DOC
server.Security.AnonymousAcl.IsEnabled = False

' DOC
Dim acl = server.Security.UserNameAcl

acl.AddEntry("username1", "password1")
acl.AddEntry("username2", "password2")
acl.AddEntry("username3", "password3")
'...
acl.IsEnabled = True

' DOC
Dim acl = server.Security.CertificateAcl

acl.AddEntry(New X509Certificate2(".\user1.pfx"))
acl.AddEntry(New X509Certificate2(".\user2.pfx"))
acl.AddEntry(New X509Certificate2(".\user3.pfx"))
'...
acl.IsEnabled = True

' DOC
Dim user1 As user1 = acl.AddEntry("username1", "password1")

' DOC
user1.Deny(OpcRequestType.Write)
user1.Deny(OpcRequestType.HistoryUpdate)

' DOC
user1.Allow(OpcRequestType.HistoryUpdate)

' DOC
server.RegisterAddress("https://mydomain.com/")
server.RegisterAddress("net.tcp://192.168.0.123:12345/")

' DOC
server.UnregisterAddress("https://mydomain.com/")

' server.Address becomes: "net.tcp://192.168.0.123:12345/"
server.UnregisterAddress("opc.tcp://localhost:4840/")

' DOC
server.Security.EndpointPolicies.Add(New OpcSecurityPolicy(
	OpcSecurityMode.Sign, OpcSecurityAlgorithm.Basic256, 3))

' DOC
server.Security.EndpointPolicies.Add(New OpcSecurityPolicy(
	OpcSecurityMode.None, OpcSecurityAlgorithm.None, 0))

' DOC
server.Security.AutoAcceptUntrustedCertificates = False

' DOC
AddHandler server.CertificateValidationFailed, AddressOf HandleCertificateValidationFailed
'...
Private Sub HandleCertificateValidationFailed(sender As Object, e As OpcCertificateValidationFailedEventArgs)
    If e.Certificate.SerialNumber = "..." Then
        e.Accept = True
    End If
End Sub

' DOC
' In context of the event handler the sender is an OpcServer.
Dim server As OpcServer = CType(sender, OpcServer)

If Not server.CertificateStores.TrustedPeerStore.Contains(e.Certificate) Then
    server.CertificateStores.TrustedPeerStore.Add(e.Certificate)
End If

' DOC
Dim server As New OpcServer("opc.tcp://localhost:4840/")
server.Address = New Uri("opc.tcp://localhost:4840/")
