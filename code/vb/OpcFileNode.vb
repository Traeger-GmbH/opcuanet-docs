'DOC
Dim reportText As String = OpcFile.ReadAllText(client, "ns=2;s=Machine/Report")

'DOC
OpcFile.AppendAllText(client, "ns=2;s=Machine/Report", "Lorem ipsum")

'DOC
Using stream = OpcFile.OpenRead(client, "ns=2;s=Machine/Report")
    Dim reader = New StreamReader(stream)

    While Not reader.EndOfStream
        Console.WriteLine(reader.ReadLine())
    End While
End Using

'DOC
Using stream = OpcFile.OpenWrite(client, "ns=2;s=Machine/Report")
    Dim writer = New StreamWriter(stream)

    writer.WriteLine("Lorem ipsum")
    writer.WriteLine("dolor sit")
    '...
End Using

'DOC
Dim file = New OpcFileInfo(client, "ns=2;s=Machine/Report")

'DOC
If file.Exists Then
    Console.WriteLine("File Length: {0}", file.Lengh)

    If file.CanUserWrite Then
        Using stream = file.OpenWrite()
            'Your code to write via stream.
        End Using
    Else
        Using stream = file.OpenRead()
            'Your code to read via stream.
        End Using
    End If
End If


'DOC
Using handle = OpcFileMethods.SecureOpen(client, "ns=2;s=Machine/Report", OpcFileMode.ReadWrite)
    Dim data As Byte() = OpcFileMethods.SecureRead(handle, 100)

    Dim position As Long = OpcFileMethods.SecureGetPosition(handle)
    OpcFileMethods.SecureSetPosition(handle, position + data(data.Length - 1))

    OpcFileMethods.SecureWrite(handle, New Byte() {1, 2, 3})
End Using

'DOC
Dim handle As UInteger = OpcFileMethods.Open(client, "ns=2;s=Machine/Report", OpcFileMode.ReadWrite)

Try
    Dim data As Byte() = OpcFileMethods.Read(client, "ns=2;s=Machine/Report", handle, 100)

    Dim position As ULong = OpcFileMethods.GetPosition(client, "ns=2;s=Machine/Report", handle)
    OpcFileMethods.SetPosition(client, "ns=2;s=Machine/Report", handle, position + data(data.Length - 1))

    OpcFileMethods.Write(client, "ns=2;s=Machine/Report", handle, New Byte() {1, 2, 3})
Finally
    OpcFileMethods.Close(client, "ns=2;s=Machine/Report", handle)
End Try


'DOC
If OpcFileMethods.IsFileNode(client, "ns=2;s=Machine/Report") Then
    'Your code to operate on the file node.
End If
