using (var server = new OpcServer()) {
    server.Start();

    using (var client = new OpcClient(server.Address)) {
        client.Connect();
        ...
    }
}
