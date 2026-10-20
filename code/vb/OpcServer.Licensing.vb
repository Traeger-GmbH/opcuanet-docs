'DOC
Opc.UaFx.Server.Licenser.LicenseKey = "<insert your license code here>"

'DOC
Opc.UaFx.Licenser.LicenseKey = "<insert your license code here>"

'DOC
Dim license As ILicenseInfo = Opc.UaFx.Server.Licenser.LicenseInfo

If license.IsExpired Then
    Console.WriteLine("The OPC UA SDK license is expired!")
End If

'DOC
#If DEBUG Then
    Opc.UaFx.Server.Licenser.FailIfUnlicensed()
#Else
    Opc.UaFx.Server.Licenser.ThrowIfUnlicensed()
#End If
