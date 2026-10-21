'DOC
Dim server = New OpcServer("opc.tcp://192.168.0.10:4840/")

'Only encrypted endpoints with current algorithms (removes the implicit None endpoint).
server.Security.EndpointPolicies.Add(New OpcSecurityPolicy( _
        OpcSecurityMode.SignAndEncrypt, _
        OpcSecurityAlgorithm.Basic256Sha256))
server.Security.EndpointPolicies.Add(New OpcSecurityPolicy( _
        OpcSecurityMode.SignAndEncrypt, _
        OpcSecurityAlgorithm.Aes256_Sha256_RsaPss))

'Listen only on the network interface that clients use.
server.Binding.Addresses.Add(IPAddress.Parse("192.168.0.10"))

'Trust only known client certificates and check them strictly.
server.Security.AutoAcceptUntrustedCertificates = False
server.Security.CertificateValidationRules _
        = OpcCertificateValidationRules.RejectSHA1Signed _
        Or OpcCertificateValidationRules.DisableBackwardsCompatibleValidation

'No anonymous access, only known users.
server.Security.UserNameAcl.AddEntry("operator", operatorPassword)
server.Security.UserNameAcl.IsEnabled = True
server.Security.AnonymousAcl.IsEnabled = False

'Report operation limits to the clients.
server.OperationLimits.AutoDetermineLimits = True

'Use a managed certificate instead of a generated one.
server.CertificateStores.AutoCreateCertificate = False
server.Certificate = OpcCertificateManager.LoadCertificate(certificatePath, certificatePassword)

server.Start()

'DOC
Public NotInheritable Class HashedServerIdentity
    Inherits OpcServerIdentity

    Private Const Iterations As Integer = 100000
    Private ReadOnly salt As Byte()

    Public Sub New(ByVal userName As String, ByVal passwordHash As Byte(), ByVal salt As Byte())
        MyBase.New(userName, password:=passwordHash)
        Me.salt = salt
    End Sub

    Public Shared Function Hash(ByVal password As String, ByVal salt As Byte()) As Byte()
        Using pbkdf2 = New Rfc2898DeriveBytes( _
                password, _
                salt, _
                Iterations, _
                HashAlgorithmName.SHA256)
            Return pbkdf2.GetBytes(32)
        End Using
    End Function

    Protected Overrides Function TransformPassword(ByVal password As String) As Byte()
        Return Hash(password, Me.salt)
    End Function
End Class



'DOC
'Hash and salt come from the user store of the application.
Dim identity = New HashedServerIdentity("operator", storedHash, storedSalt)

server.Security.UserNameAcl.AddEntry(identity)
server.Security.UserNameAcl.IsEnabled = True

'DOC
Dim endpoint = New OpcEndpointIdentity( _
        "opc.tcp://192.168.0.10:4840/", _
        New OpcSecurityPolicy(OpcSecurityMode.SignAndEncrypt, OpcSecurityAlgorithm.Basic256Sha256))

'Endpoint access mode Blacklist: only explicitly enabled endpoints are permitted.
Dim service = server.Security.UserNameAcl.AddEntry( _
        "service", _
        servicePassword, _
        operationAccessMode:=OpcAccessControlMode.Whitelist, _
        endpointAccessMode:=OpcAccessControlMode.Blacklist)

service.Enable(endpoint)

'DOC
Public NotInheritable Class UserCertificateValidator
    Inherits X509CertificateValidator

    Public Overrides Sub Validate(ByVal certificate As X509Certificate2)
        Using chain = New X509Chain()
            chain.ChainPolicy.RevocationMode = X509RevocationMode.Online

            If Not chain.Build(certificate) Then
                Throw New SecurityException("The user certificate is not trusted.")
            End If
        End Using
    End Sub
End Class

'DOC
server.Security.CertificateAcl.CertificateValidator = New UserCertificateValidator()
server.Security.CertificateAcl.AddEntry(userCertificate)
server.Security.CertificateAcl.IsEnabled = True

'DOC
AddHandler server.RequestValidating, AddressOf HandleRequestValidating
...
Private Shared Sub HandleRequestValidating( _
        ByVal sender As Object, _
        ByVal e As OpcRequestValidatingEventArgs)
    'Clients must not change the structure of the address space.
    If e.RequestType = OpcRequestType.AddNodes _
            OrElse e.RequestType = OpcRequestType.DeleteNodes _
            OrElse e.RequestType = OpcRequestType.AddReferences _
            OrElse e.RequestType = OpcRequestType.DeleteReferences Then
        e.Cancel = True
    End If
End Sub


'DOC
Dim adminCertificate = OpcCertificateManager.LoadCertificate(adminCertificatePath)

Dim admin = server.Security.CertificateAcl.AddEntry(adminCertificate)
admin.Assign(server.Security.Roles.SecurityAdmin)
server.Security.CertificateAcl.IsEnabled = True

AddHandler server.CertificateChanged, AddressOf HandleCertificateChanged
...
Private Shared Sub HandleCertificateChanged( _
        ByVal sender As Object, _
        ByVal e As OpcCertificateChangedEventArgs)
    Console.WriteLine( _
            "New certificate: {0} ({1})", _
            e.Certificate.Subject, _
            e.Certificate.Thumbprint)
End Sub


'DOC
AddHandler server.SessionActivated, AddressOf HandleSessionActivated
AddHandler server.RequestValidating, AddressOf HandleCriticalRequest
...
Private Shared Sub HandleSessionActivated(ByVal sender As Object, ByVal e As OpcSessionEventArgs)
    Console.WriteLine( _
            "{0:u} session {1}: {2}", _
            DateTime.UtcNow, _
            e.Session.Name, _
            e.Session.UsedIdentity)
End Sub

Private Shared Sub HandleCriticalRequest( _
        ByVal sender As Object, _
        ByVal e As OpcRequestValidatingEventArgs)
    If e.RequestType = OpcRequestType.Write OrElse e.RequestType = OpcRequestType.Call Then
        Console.WriteLine( _
                "{0:u} {1} in session {2}", _
                DateTime.UtcNow, _
                e.RequestType, _
                e.Context.SessionId)
    End If
End Sub
