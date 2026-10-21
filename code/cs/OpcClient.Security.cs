// DOC
var client = new OpcClient("opc.tcp://192.168.0.10:4840/");

// Only an encrypted endpoint with a current algorithm.
client.Security.EndpointPolicy = new OpcSecurityPolicy(
        OpcSecurityMode.SignAndEncrypt,
        OpcSecurityAlgorithm.Basic256Sha256);
client.Security.AutoUpgradeEndpointPolicy = true;

// Trust only known server certificates and check them strictly.
client.Security.AutoAcceptUntrustedCertificates = false;
client.Security.VerifyServersCertificateDomains = true;
client.Security.CertificateValidationRules =
        OpcCertificateValidationRules.RejectSHA1Signed
        | OpcCertificateValidationRules.DisableBackwardsCompatibleValidation;

// Use a managed certificate instead of a generated one.
client.CertificateStores.AutoCreateCertificate = false;
client.Certificate = OpcCertificateManager.LoadCertificate(certificatePath, certificatePassword);

// Log on as a user instead of anonymously.
client.Security.UserIdentity = new OpcClientIdentity(userName, password);

// DOC
client.CertificateValidationFailed += (sender, e) => {
    // Accept only the expected server, and only if its certificate is merely unknown.
    e.Accept = e.Result == OpcCertificateValidationResult.Untrusted
            && string.Equals(
                    e.Certificate.Thumbprint,
                    expectedThumbprint,
                    StringComparison.OrdinalIgnoreCase);
};

// DOC
client.Connect();

if (client.UsedEndpoint.Policy.Mode != OpcSecurityMode.SignAndEncrypt) {
    client.Disconnect();
    throw new InvalidOperationException("The server offers no encrypted endpoint.");
}

// DOC
var settings = new OpcCertificateSettings(client.ApplicationName) {
    KeySize = 4096,
    Lifetime = 24
};

client.Certificate = OpcCertificateManager.CreateCertificate(settings);

// DOC
client.CertificateStores.AutoCreateCertificate = false;

client.CertificateRequested += (sender, e) => {
    e.AlternativeCertificate = OpcCertificateManager.LoadCertificate(
            certificatePath,
            certificatePassword);
};
