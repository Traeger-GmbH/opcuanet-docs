AddHandler server.SessionActivated, _
    Sub(sender, e)
        Dim identityEntry = GetUserEntry(server.Security, e.Session)

        If identityEntry IsNot Nothing Then identityEntry.Disable(OpcEndpointIdentity.GetCurrent())
    End Sub

AddHandler server.SessionClosing, _
    Sub(sender, e)
        Dim identityEntry = GetUserEntry(server.Security, e.Session)

        If identityEntry IsNot Nothing Then identityEntry.Enable(OpcEndpointIdentity.GetCurrent())
    End Sub


Private Shared Function GetUserEntry( _
        ByVal security As OpcServerSecurity, _
        ByVal session As OpcSession) As OpcAccessControlEntry
    Dim identity = session.UsedIdentity

    'In case of anonymous.
    If identity Is Nothing Then Return Nothing

    Return (From userEntry In security.UserNameAcl.Entries _
            Let userPrincipal = userEntry.Principal _
            Let userIdentity = CType(userPrincipal.Identity, OpcUserIdentity) _
            Where userIdentity.DisplayName = identity.DisplayName _
            Select userEntry).FirstOrDefault()
End Function


