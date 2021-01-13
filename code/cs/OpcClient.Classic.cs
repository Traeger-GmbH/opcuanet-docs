// DOC
using Opc.UaFx.Client;
using Opc.UaFx.Client.Classic;

// DOC
var client = new OpcClient("opc.com://localhost:48410/<progId>/<classId>");

// DOC
client.Connect();

// DOC
// Your code to interact with the server.

// DOC
client.Disconnect();

// DOC
using (var client = new OpcClient("opc.com://localhost:48410/<progId>/<classId>")) {
    client.Connect();
    // Your code to interact with the server.
}
