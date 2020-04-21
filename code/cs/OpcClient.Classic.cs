// DOC
using Opc.UaFx.Classic;
using Opc.UaFx.Client;

// DOC
var client = new OpcClient("opc.com://localhost:4840/<progId>/<classId>");

// DOC
client.Connect();

// DOC
// Your code to interact with the server.

// DOC
client.Disconnect();

// DOC
using (var client = new OpcClient("opc.com://localhost:4840/<progId>/<classId>")) {
    client.Connect();
    // Your code to interact with the server.
}
