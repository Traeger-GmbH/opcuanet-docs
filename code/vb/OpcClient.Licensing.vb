'DOC
Opc.UaFx.Client.Licenser.LicenseKey = "<insert your license code here>"

'DOC
Opc.UaFx.Licenser.LicenseKey = "<insert your license code here>"

'DOC
Dim license As ILicenseInfo = Opc.UaFx.Client.Licenser.LicenseInfo

If license.IsExpired Then
    Console.WriteLine("The OPA UA Framework Advanced license is expired!")
End If

'DOC
#If DEBUG Then
    Opc.UaFx.Client.Licenser.FailIfUnlicensed()
#Else
    Opc.UaFx.Client.Licenser.ThrowIfUnlicensed()
#End If
