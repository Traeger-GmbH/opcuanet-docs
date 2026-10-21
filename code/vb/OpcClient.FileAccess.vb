' DOC
Dim lines As String() = OpcFile.ReadAllLines(client, "ns=2;s=Machine/Recipe")
Dim content As Byte() = OpcFile.ReadAllBytes(client, "ns=2;s=Machine/Snapshot")

' DOC
Dim file = New Opc.UaFx.Client.OpcFileInfo(client, "ns=2;s=Machine/Recipe")

Console.WriteLine($"{file.Name}: {file.Length} bytes, {file.MimeType}")

If file.Exists AndAlso file.CanUserWrite Then
    Using stream = file.OpenWrite()
        Using writer = New StreamWriter(stream)
            writer.Write("Speed=1200")
        End Using
    End Using
End If

' DOC
Using source = OpcFile.OpenRead(client, "ns=2;s=Machine/Snapshot")
    Using target = IO.File.Create("C:\Temp\snapshot.bin")
        source.CopyTo(target)
    End Using
End Using

' DOC
Using stream = OpcFile.Open(client, "ns=2;s=Machine/Log", OpcFileMode.ReadWrite)
    stream.Seek(0, SeekOrigin.End)

    Dim entry = Encoding.UTF8.GetBytes("Job started" & Environment.NewLine)
    stream.Write(entry, 0, entry.Length)
End Using

' DOC
Try
    Dim recipe As String = OpcFile.ReadAllText(client, "ns=2;s=Machine/Recipe")
Catch ex As InvalidOperationException
    ' The node does not exist or is not a file node.
Catch ex As OpcException
    ' The server rejected a file method, e.g. with BadUserAccessDenied.
    Console.WriteLine($"{ex.Code}: {ex.Message}")
End Try
