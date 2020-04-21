using (var discoveryClient = new OpcClassicDiscoveryClient("<host>")) {
    var servers = discoveryClient.DiscoverServers();

    foreach (var server in servers) {
        Console.WriteLine(
                "- {0}, ClassId={1}, ProgId={2}",
                server.Name,
                server.ClassId,
                server.ProgId);
    }
}