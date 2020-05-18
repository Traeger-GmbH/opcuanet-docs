// DOC
using Opc.UaFx.Server;

// DOC
var server = new OpcServer("opc.tcp://localhost:4840/");

// DOC
server.Start();

// DOC
// Your code to process client requests.

// DOC
server.Stop();

// DOC
using (var server = new OpcServer("opc.tcp://localhost:4840/")) {
    server.Start();
    // Your code to process client requests.
}
