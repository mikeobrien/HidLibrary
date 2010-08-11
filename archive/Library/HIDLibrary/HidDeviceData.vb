Public Class HidDeviceData

#Region "--> Public Enums "

    Public Enum ReadStatus
        Success = 0
        WaitTimedOut = 1
        WaitFail = 2
        NoDataRead = 3
        ReadError = 4
        NotConnected = 5
    End Enum

#End Region

#Region "--> Private Fields "

    Private _Data As Byte()
    Private _Status As ReadStatus

#End Region

#Region "--> Constructor "

    Public Sub New(ByVal Status As ReadStatus)

        _Data = New Byte() {}
        _Status = Status

    End Sub

    Public Sub New(ByVal Data As Byte(), ByVal Status As ReadStatus)

        _Data = Data
        _Status = Status

    End Sub

#End Region

#Region "--> Public Properties "

    Public ReadOnly Property Data() As Byte()
        Get

            Return _Data

        End Get
    End Property

    Public ReadOnly Property Status() As ReadStatus
        Get

            Return _Status

        End Get
    End Property

#End Region

End Class
