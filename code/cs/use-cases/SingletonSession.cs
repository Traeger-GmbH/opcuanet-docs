server.SessionClosing += (sender, e) => {
    var identityEntry = GetUserEntry(server.Security, e.Session);

    if (identityEntry != null) {
        var endpoint = OpcEndpointIdentity.GetCurrent();

        if (endpoint == null)
            identityEntry.Enable(identityEntry.DisabledEndpoints);
        else
            identityEntry.Enable(endpoint);
    }
};
