// DOC
using Opc.UaFx.Client;

// DOC
var client = new OpcClient("opc.tcp://localhost:4840/");

// DOC
client.Connect();

// DOC
// Your code to interact with the server.

// DOC
client.Disconnect();

// DOC
using (var client = new OpcClient("opc.tcp://localhost:4840/")) {
    client.Connect();
    // Your code to interact with the server.
}
