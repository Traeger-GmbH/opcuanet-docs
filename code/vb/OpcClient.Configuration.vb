'DOC
client.Configuration = OpcApplicationConfiguration.LoadClientConfig("Opc.UaFx.Client")

'DOC
client.Configuration = OpcApplicationConfiguration.LoadClientConfigFile("MyClientAppNameConfig.xml")

'DOC
'Default: Value of AssemblyTitleAttribute of entry assembly.
client.ApplicationName = "MyClientAppName"
 
'Default: Nothing to auto complete on connect to "urn::" + ApplicationName
client.ApplicationUri = "http://my.clientapp.uri/"

'DOC
client.SessionTimeout = 30000           'Default: 60000
client.SessionName = "My Session Name"  'Default: Nothing

'DOC
client.OperationTimeout = 10000          'Default: 60000
client.DisconnectTimeout = 5000          'Default: 10000
client.ReconnectTimeout = 5000           'Default: 10000

'DOC
'Default: ".\CertificateStores\Trusted"
client.CertificateStores.ApplicationStore.Path _
        = "%LocalApplicationData%\MyClientAppName\App Certificates"
 
'Default: ".\CertificateStores\Rejected"
client.CertificateStores.RejectedStore.Path _
        = "%LocalApplicationData%\MyClientAppName\Rejected Certificates"
 
'Default: ".\CertificateStores\Trusted"
client.CertificateStores.TrustedIssuerStore.Path _
        = "%LocalApplicationData%\MyClientAppName\Trusted Issuer Certificates"
 
'Default: ".\CertificateStores\Trusted"
client.CertificateStores.TrustedPeerStore.Path _
        = "%LocalApplicationData%\MyClientAppName\Trusted Peer Certificates"

'DOC
Dim certificate = OpcCertificateManager.LoadCertificate("MyClientCertificate.pfx")

'DOC
Dim certificate = OpcCertificateManager.CreateCertificate(client)

'DOC
OpcCertificateManager.SaveCertificate("MyClientCertificate.pfx", certificate)

'DOC
client.Certificate = certificate

'DOC
If Not client.CertificateStores.ApplicationStore.Contains(certificate)) Then
    client.CertificateStores.ApplicationStore.Add(certificate)
End If

'DOC
client.CertificateStores.AutoCreateCertificate = False

'DOC
client.Security.UserIdentity = New OpcClientIdentity("userName", "password")

'DOC
client.Security.UserIdentity = New OpcCertificateIdentity(New X509Certificate2("Doe.pfx"))

'DOC
client.Security.UserIdentity = Nothing

'DOC
client.Security.UseOnlySecureEndpoints = True

'DOC
client.Security.UseHighLevelEndpoint = True;

'DOC
client.Security.EndpointPolicy = New OpcSecurityPolicy( _
        OpcSecurityMode.None, OpcSecurityAlgorithm.Basic256)

'DOC
Using client As New OpcDiscoveryClient("opc.tcp://localhost:4840/")
    Dim endpoints = client.DiscoverEndpoints()
 
    For Each endpoint in endpoints
        'Your code to operate on each endpoint.
    Next
End Using

'DOC
client.Security.VerifyServersCertificateDomains = True

'DOC
client.Security.AutoAcceptUntrustedCertificates = False

'DOC
AddHandler client.CertificateValidationFailed, AddressOf HandleCertificateValidationFailed
...
Private Sub HandleCertificateValidationFailed( _
        ByVal sender As Object, _
        ByVal e As OpcCertificateValidationFailedEventArgs)
    If e.Certificate.Thumbprint = "..." Then
        e.Accept = True
    End If
End Sub

'DOC
'In context of the event handler the sender is an OpcClient.
Dim client = CType(sender, OpcClient)
 
If Not client.CertificateStores.TrustedPeerStore.Contains(e.Certificate) Then
    client.CertificateStores.TrustedPeerStore.Add(e.Certificate)
End If
