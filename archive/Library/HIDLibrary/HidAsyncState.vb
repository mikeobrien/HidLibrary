Public Class HidAsyncState

#Region "--> Private Varibles "

    Private _CallerDelegate As Object
    Private _CallbackDelegate As Object

#End Region

#Region "--> Constructor "

    Public Sub New(ByVal CallerDelegate As Object, ByVal CallbackDelegate As Object)

        _CallerDelegate = CallerDelegate
        _CallbackDelegate = CallbackDelegate

    End Sub

#End Region

#Region "--> Public Properties "

    Public ReadOnly Property CallerDelegate() As Object

        Get

            Return _CallerDelegate

        End Get

    End Property

    Public ReadOnly Property CallbackDelegate() As Object

        Get

            Return _CallbackDelegate

        End Get

    End Property

#End Region

End Class
