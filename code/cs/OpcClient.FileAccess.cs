// DOC
string[] lines = OpcFile.ReadAllLines(client, "ns=2;s=Machine/Recipe");
byte[] content = OpcFile.ReadAllBytes(client, "ns=2;s=Machine/Snapshot");

// DOC
var file = new Opc.UaFx.Client.OpcFileInfo(client, "ns=2;s=Machine/Recipe");

Console.WriteLine($"{file.Name}: {file.Length} bytes, {file.MimeType}");

if (file.Exists && file.CanUserWrite) {
    using (var stream = file.OpenWrite())
    using (var writer = new StreamWriter(stream))
        writer.Write("Speed=1200");
}

// DOC
using (var source = OpcFile.OpenRead(client, "ns=2;s=Machine/Snapshot"))
using (var target = File.Create(@"C:\Temp\snapshot.bin"))
    source.CopyTo(target);

// DOC
using (var stream = OpcFile.Open(client, "ns=2;s=Machine/Log", OpcFileMode.ReadWrite)) {
    stream.Seek(0, SeekOrigin.End);

    var entry = Encoding.UTF8.GetBytes("Job started" + Environment.NewLine);
    stream.Write(entry, 0, entry.Length);
}

// DOC
try {
    string recipe = OpcFile.ReadAllText(client, "ns=2;s=Machine/Recipe");
}
catch (InvalidOperationException) {
    // The node does not exist or is not a file node.
}
catch (OpcException ex) {
    // The server rejected a file method, e.g. with BadUserAccessDenied.
    Console.WriteLine($"{ex.Code}: {ex.Message}");
}
