Public Class HidReport

#Region "--> Private Fields "

    Private _ReportId As Byte
    Private _Data As Byte() = {}
    Private _Exists As Boolean = False
    Private _Status As HidDeviceData.ReadStatus

#End Region

#Region "--> Constructor "

    Public Sub New(ByVal ReportSize As Integer, ByVal DeviceData As HidDeviceData)

        _Status = DeviceData.Status

        ReDim Preserve _Data(ReportSize - 2)

        If Not DeviceData.Data Is Nothing Then

            If DeviceData.Data.Length > 0 Then

                _ReportId = DeviceData.Data(0)
                _Exists = True

                If DeviceData.Data.Length > 1 Then

                    Dim DataLength As Integer = ReportSize - 1

                    If DeviceData.Data.Length < ReportSize - 1 Then

                        DataLength = DeviceData.Data.Length

                    End If

                    Array.Copy(DeviceData.Data, 1, _Data, 0, DataLength)

                End If

            Else

                _Exists = False

            End If

        Else

            _Exists = False

        End If

    End Sub

    Public Sub New(ByVal ReportSize As Integer)

        ReDim _Data(ReportSize - 1)

    End Sub

#End Region

#Region "--> Public Properties "

    Public ReadOnly Property Exists() As Boolean
        Get

            Return _Exists

        End Get
    End Property

    Public Property ReportId() As Byte
        Get

            Return _ReportId

        End Get
        Set(ByVal Value As Byte)

            _ReportId = Value
            _Exists = True

        End Set
    End Property

    Public Property Data() As Byte()
        Get

            Return _Data

        End Get
        Set(ByVal Value As Byte())

            _Data = Value
            _Exists = True

        End Set
    End Property

    Public ReadOnly Property ReadStatus() As HidDeviceData.ReadStatus
        Get

            Return _Status

        End Get
    End Property

#End Region

#Region "--> Public Methods "

    Public Function GetBytes() As Byte()

        Dim Data As Byte()

        ReDim Data(_Data.Length)

        Data(0) = _ReportId

        Array.Copy(_Data, 0, Data, 1, _Data.Length)

        Return Data

    End Function

#End Region

End Class
