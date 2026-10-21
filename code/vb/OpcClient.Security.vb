' DOC
Dim client = New OpcClient("opc.tcp://192.168.0.10:4840/")

' Only an encrypted endpoint with a current algorithm.
client.Security.EndpointPolicy = New OpcSecurityPolicy(
        OpcSecurityMode.SignAndEncrypt,
        OpcSecurityAlgorithm.Basic256Sha256)
client.Security.AutoUpgradeEndpointPolicy = True

' Trust only known server certificates and check them strictly.
client.Security.AutoAcceptUntrustedCertificates = False
client.Security.VerifyServersCertificateDomains = True
client.Security.CertificateValidationRules =
        OpcCertificateValidationRules.RejectSHA1Signed _
        Or OpcCertificateValidationRules.DisableBackwardsCompatibleValidation

' Use a managed certificate instead of a generated one.
client.CertificateStores.AutoCreateCertificate = False
client.Certificate = OpcCertificateManager.LoadCertificate(certificatePath, certificatePassword)

' Log on as a user instead of anonymously.
client.Security.UserIdentity = New OpcClientIdentity(userName, password)

' DOC
AddHandler client.CertificateValidationFailed,
    Sub(sender, e)
        ' Accept only the expected server, and only if its certificate is merely unknown.
        e.Accept = e.Result = OpcCertificateValidationResult.Untrusted _
                AndAlso String.Equals(
                        e.Certificate.Thumbprint,
                        expectedThumbprint,
                        StringComparison.OrdinalIgnoreCase)
    End Sub

' DOC
client.Connect()

If client.UsedEndpoint.Policy.Mode <> OpcSecurityMode.SignAndEncrypt Then
    client.Disconnect()
    Throw New InvalidOperationException("The server offers no encrypted endpoint.")
End If

' DOC
Dim settings = New OpcCertificateSettings(client.ApplicationName) With {
    .KeySize = 4096,
    .Lifetime = 24
}

client.Certificate = OpcCertificateManager.CreateCertificate(settings)

' DOC
client.CertificateStores.AutoCreateCertificate = False

AddHandler client.CertificateRequested,
    Sub(sender, e)
        e.AlternativeCertificate = OpcCertificateManager.LoadCertificate(
                certificatePath,
                certificatePassword)
    End Sub
