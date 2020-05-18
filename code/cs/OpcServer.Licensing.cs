// DOC
Opc.UaFx.Server.Licenser.LicenseKey = "<insert your license code here>";

// DOC
Opc.UaFx.Licenser.LicenseKey = "<insert your license code here>";

// DOC
ILicenseInfo license = Opc.UaFx.Server.Licenser.LicenseInfo;
 
if (license.IsExpired)
    Console.WriteLine("The OPA UA SDK license is expired!");


// DOC
#if DEBUG
    Opc.UaFx.Server.Licenser.FailIfUnlicensed();
#else
    Opc.UaFx.Server.Licenser.ThrowIfUnlicensed();
#endif
