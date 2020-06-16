server.SessionActivated += (sender, e) => {
    var identityEntry = GetUserEntry(server.Security, e.Session);

    if (identityEntry != null)
        identityEntry.Disable(OpcEndpointIdentity.GetCurrent());
};

server.SessionClosing += (sender, e) => {
    var identityEntry = GetUserEntry(server.Security, e.Session);

    if (identityEntry != null)
        identityEntry.Enable(OpcEndpointIdentity.GetCurrent());
};


private static OpcAccessControlEntry GetUserEntry(
        OpcServerSecurity security,
        OpcSession session)
{
    var identity = session.UsedIdentity;

    // In case of anonymous.
    if (identity == null)
        return null;

    return (from userEntry in security.UserNameAcl.Entries
            let userPrincipal = userEntry.Principal
            let userIdentity = (OpcUserIdentity)userPrincipal.Identity
            where userIdentity.DisplayName == identity.DisplayName
            select userEntry).FirstOrDefault();
}
