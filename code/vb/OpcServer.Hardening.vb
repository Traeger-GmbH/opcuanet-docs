' DOC
Dim server = New OpcServer("opc.tcp://192.168.0.10:4840/")

' Only encrypted endpoints with current algorithms (removes the implicit None endpoint).
server.Security.EndpointPolicies.Add(New OpcSecurityPolicy(
        OpcSecurityMode.SignAndEncrypt, OpcSecurityAlgorithm.Basic256Sha256))
server.Security.EndpointPolicies.Add(New OpcSecurityPolicy(
        OpcSecurityMode.SignAndEncrypt, OpcSecurityAlgorithm.Aes256_Sha256_RsaPss))

' Listen only on the network interface that clients use.
server.Binding.Addresses.Add(IPAddress.Parse("192.168.0.10"))

' Trust only known client certificates and check them strictly.
server.Security.AutoAcceptUntrustedCertificates = False
server.Security.CertificateValidationRules =
        OpcCertificateValidationRules.RejectSHA1Signed _
        Or OpcCertificateValidationRules.DisableBackwardsCompatibleValidation

' No anonymous access, only known users.
server.Security.UserNameAcl.AddEntry("operator", operatorPassword)
server.Security.UserNameAcl.IsEnabled = True
server.Security.AnonymousAcl.IsEnabled = False

' Report operation limits to the clients.
server.OperationLimits.AutoDetermineLimits = True

' Use a managed certificate instead of a generated one.
server.CertificateStores.AutoCreateCertificate = False
server.Certificate = OpcCertificateManager.LoadCertificate(certificatePath, certificatePassword)

server.Start()

' DOC
Public NotInheritable Class HashedServerIdentity
    Inherits OpcServerIdentity

    Private Const Iterations As Integer = 100000
    Private ReadOnly salt As Byte()

    Public Sub New(userName As String, passwordHash As Byte(), salt As Byte())
        MyBase.New(userName, passwordHash)
        Me.salt = salt
    End Sub

    Public Shared Function Hash(password As String, salt As Byte()) As Byte()
        Using pbkdf2 = New Rfc2898DeriveBytes(password, salt, Iterations, HashAlgorithmName.SHA256)
            Return pbkdf2.GetBytes(32)
        End Using
    End Function

    Protected Overrides Function TransformPassword(password As String) As Byte()
        Return Hash(password, Me.salt)
    End Function
End Class

' DOC
' Hash and salt come from the user store of the application.
Dim identity = New HashedServerIdentity("operator", storedHash, storedSalt)

server.Security.UserNameAcl.AddEntry(identity)
server.Security.UserNameAcl.IsEnabled = True

' DOC
Dim endpoint = New OpcEndpointIdentity(
        "opc.tcp://192.168.0.10:4840/",
        New OpcSecurityPolicy(OpcSecurityMode.SignAndEncrypt, OpcSecurityAlgorithm.Basic256Sha256))

' Endpoint access mode Blacklist: only explicitly enabled endpoints are permitted.
Dim service = server.Security.UserNameAcl.AddEntry(
        "service",
        servicePassword,
        OpcAccessControlMode.Whitelist,
        OpcAccessControlMode.Blacklist)

service.Enable(endpoint)

' DOC
Public NotInheritable Class UserCertificateValidator
    Inherits X509CertificateValidator

    Public Overrides Sub Validate(certificate As X509Certificate2)
        Using chain = New X509Chain()
            chain.ChainPolicy.RevocationMode = X509RevocationMode.Online

            If Not chain.Build(certificate) Then
                Throw New SecurityException("The user certificate is not trusted.")
            End If
        End Using
    End Sub
End Class

' DOC
server.Security.CertificateAcl.CertificateValidator = New UserCertificateValidator()
server.Security.CertificateAcl.AddEntry(userCertificate)
server.Security.CertificateAcl.IsEnabled = True

' DOC
AddHandler server.RequestValidating,
    Sub(sender, e)
        ' Clients must not change the structure of the address space.
        If e.RequestType = OpcRequestType.AddNodes _
                OrElse e.RequestType = OpcRequestType.DeleteNodes _
                OrElse e.RequestType = OpcRequestType.AddReferences _
                OrElse e.RequestType = OpcRequestType.DeleteReferences Then
            e.Cancel = True
        End If
    End Sub

' DOC
Dim adminCertificate = OpcCertificateManager.LoadCertificate(adminCertificatePath)

Dim admin = server.Security.CertificateAcl.AddEntry(adminCertificate)
admin.Assign(server.Security.Roles.SecurityAdmin)
server.Security.CertificateAcl.IsEnabled = True

AddHandler server.CertificateChanged,
    Sub(sender, e)
        Console.WriteLine($"New certificate: {e.Certificate.Subject} ({e.Certificate.Thumbprint})")
    End Sub

' DOC
AddHandler server.SessionActivated,
    Sub(sender, e)
        Console.WriteLine($"{DateTime.UtcNow:u} session {e.Session.Name}: {e.Session.UsedIdentity}")
    End Sub

AddHandler server.RequestValidating,
    Sub(sender, e)
        If e.RequestType = OpcRequestType.Write OrElse e.RequestType = OpcRequestType.Call Then
            Console.WriteLine($"{DateTime.UtcNow:u} {e.RequestType} in session {e.Context.SessionId}")
        End If
    End Sub
