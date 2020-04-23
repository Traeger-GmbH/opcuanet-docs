// DOC
Opc.UaFx.Client.Licenser.LicenseKey = "<insert your license code here>";

// DOC
Opc.UaFx.Licenser.LicenseKey = "<insert your license code here>";

// DOC
ILicenseInfo license = Opc.UaFx.Client.Licenser.LicenseInfo;
 
if (license.IsExpired)
    Console.WriteLine("The OPA UA Framework Advanced license is expired!");


// DOC
#if DEBUG
    Opc.UaFx.Client.Licenser.FailIfUnlicensed();
#else
    Opc.UaFx.Client.Licenser.ThrowIfUnlicensed();
#endif
