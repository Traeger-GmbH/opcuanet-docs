// DOC
string reportText = OpcFile.ReadAllText(client, "ns=2;s=Machine/Report");

// DOC
OpcFile.AppendAllText(client, "ns=2;s=Machine/Report", "Lorem ipsum");

// DOC
using (var stream = OpcFile.OpenRead(client, "ns=2;s=Machine/Report")) {
    var reader = new StreamReader(stream);
 
    while (!reader.EndOfStream)
        Console.WriteLine(reader.ReadLine());
}

// DOC
using (var stream = OpcFile.OpenWrite(client, "ns=2;s=Machine/Report")) {
    var writer = new StreamWriter(stream);
 
    writer.WriteLine("Lorem ipsum");
    writer.WriteLine("dolor sit");
    // ...
}

// DOC
var file = new OpcFileInfo(client, "ns=2;s=Machine/Report");

// DOC
if (file.Exists) {
    Console.WriteLine($"File Length: {file.Lengh}");
 
    if (file.CanUserWrite) {
        using (var stream = file.OpenWrite()) {
            // Your code to write via stream.
        }
    }
    else {
        using (var stream = file.OpenRead()) {
            // Your code to read via stream.
        }
    }
}

// DOC
using (var handle = OpcFileMethods.SecureOpen(client, "ns=2;s=Machine/Report", OpcFileMode.ReadWrite)) {
    byte[] data = OpcFileMethods.SecureRead(handle, 100);
 
    long position = OpcFileMethods.SecureGetPosition(handle);
    OpcFileMethods.SecureSetPosition(handle, position + data[data.Length - 1]);
 
    OpcFileMethods.SecureWrite(handle, new byte[] { 1, 2, 3 });
}

// DOC
uint handle = OpcFileMethods.Open(client, "ns=2;s=Machine/Report", OpcFileMode.ReadWrite);
 
try {
    byte[] data = OpcFileMethods.Read(client, "ns=2;s=Machine/Report", handle, 100);
 
    ulong position = OpcFileMethods.GetPosition(client, "ns=2;s=Machine/Report", handle);
    OpcFileMethods.SetPosition(client, "ns=2;s=Machine/Report", handle, position + data[data.Length - 1]);
 
    OpcFileMethods.Write(client, "ns=2;s=Machine/Report", handle, new byte[] { 1, 2, 3 });
}
finally {
    OpcFileMethods.Close(client, "ns=2;s=Machine/Report", handle);
}

// DOC
if (OpcFileMethods.IsFileNode(client, "ns=2;s=Machine/Report")) {
    // Your code to operate on the file node.
}
