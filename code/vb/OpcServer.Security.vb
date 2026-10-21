'DOC
Dim acl = server.Security.UserNameAcl

Dim operator1 = acl.AddEntry("operator1", password:="password1")
operator1.Assign(server.Security.Roles.Operator)

Dim admin = acl.AddEntry("admin", password:="password2")
admin.Assign(server.Security.Roles.ConfigureAdmin, server.Security.Roles.SecurityAdmin)

acl.IsEnabled = True

'DOC
admin.Revoke(server.Security.Roles.ConfigureAdmin)

'DOC
Dim guest = acl.AddEntry( _
        "guest", _
        password:="password3", _
        operationAccessMode:=OpcAccessControlMode.Blacklist)

'In blacklist mode only explicitly allowed operations are permitted.
guest.Allow(OpcRequestType.Browse, OpcRequestType.Read)

'DOC
server.AuthenticationLimits.MaxFailedAttempts = 3                  'Default: 5
server.AuthenticationLimits.DelayWindow = TimeSpan.FromSeconds(10) 'Default: 5 seconds
server.AuthenticationLimits.Delay = TimeSpan.FromSeconds(2)        'Default: 1 second
server.AuthenticationLimits.MaxDelay = TimeSpan.FromMinutes(1)     'Default: 30 seconds

'DOC
'Default: False
server.AuthenticationLimits.AutoDetermineLimits = True

'DOC
Public Class MyUserNameAcl
    Inherits OpcAccessControlList

    Private ReadOnly principals As New Dictionary(Of String, OpcPrincipal)()

    Public Sub New(ByVal owner As OpcServerBase)
        MyBase.New( _
                owner, _
                operationAccessMode:=OpcAccessControlMode.Whitelist, _
                endpointAccessMode:=OpcAccessControlMode.Whitelist)
    End Sub

    Protected Overrides ReadOnly Property TokenType As OpcAccessControlTokenType
        Get
            Return OpcAccessControlTokenType.UserName
        End Get
    End Property

    Public Overrides Function Authenticate(ByVal identityToken As OpcIdentityToken) As OpcPrincipal
        Dim token = TryCast(identityToken, OpcUserNameIdentityToken)

        'Verify the user name and password, e.g. using an existing user management.
        If token Is Nothing OrElse Not UserStore.Verify(token.UserName, token.DecryptedPassword) Then
            Return Nothing
        End If

        SyncLock Me.principals
            Dim principal As OpcPrincipal = Nothing

            If Not Me.principals.TryGetValue(token.UserName, principal) Then
                principal = New OpcPrincipal(New OpcServerIdentity(token.UserName))
                Me.AddEntry(principal)

                'Authenticated users are expected to be a member of the AuthenticatedUser role.
                Me.Owner.Security.Roles.AuthenticatedUser.Assign(principal)
                Me.principals.Add(token.UserName, principal)
            End If

            Return principal
        End SyncLock
    End Function
End Class

'DOC
Dim customAcl = New MyUserNameAcl(server)
server.Security.AddAccessControlList(customAcl)

customAcl.IsEnabled = True

'DOC
Dim admin = server.Security.UserNameAcl.AddEntry("admin", password:="password")
admin.Assign(server.Security.Roles.SecurityAdmin)

server.Security.UserNameAcl.IsEnabled = True
AddHandler server.CertificateChanged, AddressOf HandleCertificateChanged
...
Private Shared Sub HandleCertificateChanged( _
        ByVal sender As Object, _
        ByVal e As OpcCertificateChangedEventArgs)
    Console.WriteLine("New application certificate: {0}", e.Certificate.Thumbprint)
End Sub

