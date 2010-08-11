Imports HIDLibrary

Public Class IT4XXXReport

#Region "--> Private Fields "

    Private _ReportId As Byte
    Private _Length As Byte
    Private _AIMSymbologyId0 As Byte
    Private _AIMSymbologyId1 As Byte
    Private _AIMSymbologyId2 As Byte
    Private _Data As Byte() = {}
    Private _HHPSymbologyId As Byte
    Private _Reserved As Byte
    Private _MoreData As Boolean

    Private _Exists As Boolean
    Private _Status As HidDeviceData.ReadStatus

#End Region

#Region "--> Constructor "

    Public Sub New(ByVal HidReport As HidReport)

        _Status = HidReport.ReadStatus
        _ReportId = HidReport.ReportId
        _Exists = HidReport.Exists

        If HidReport.Data.Length > 0 Then _Length = HidReport.Data(0)
        If HidReport.Data.Length > 1 Then _AIMSymbologyId0 = HidReport.Data(1)
        If HidReport.Data.Length > 2 Then _AIMSymbologyId1 = HidReport.Data(2)
        If HidReport.Data.Length > 3 Then _AIMSymbologyId2 = HidReport.Data(3)

        If HidReport.Data.Length > _Length + 3 Then

            ReDim _Data(_Length - 1)
            Array.Copy(HidReport.Data, 4, _Data, 0, _Length)

        End If

        If HidReport.Data.Length > 60 Then _HHPSymbologyId = HidReport.Data(60)
        If HidReport.Data.Length > 61 Then _Reserved = HidReport.Data(61)
        If HidReport.Data.Length > 62 Then _MoreData = HidReport.Data(62) = 1

    End Sub

#End Region

#Region "--> Public Properties "

    Public ReadOnly Property ReadStatus() As HidDeviceData.ReadStatus

        Get

            Return _Status

        End Get

    End Property

    Public ReadOnly Property Exists() As Boolean

        Get

            Return _Exists

        End Get

    End Property

    Public ReadOnly Property ReportId() As Byte

        Get

            Return _ReportId

        End Get

    End Property

    Public ReadOnly Property Length() As Byte

        Get

            Return _Length

        End Get

    End Property

    Public ReadOnly Property AIMSymbologyId0() As Byte

        Get

            Return _AIMSymbologyId0

        End Get

    End Property

    Public ReadOnly Property AIMSymbologyId1() As Byte

        Get

            Return _AIMSymbologyId1

        End Get

    End Property

    Public ReadOnly Property AIMSymbologyId2() As Byte

        Get

            Return _AIMSymbologyId2

        End Get

    End Property

    Public ReadOnly Property Data() As Byte()

        Get

            Return _Data

        End Get

    End Property

    Public ReadOnly Property HHPSymbologyId() As Byte

        Get

            Return _HHPSymbologyId

        End Get

    End Property

    Public ReadOnly Property Reserved() As Byte

        Get

            Return _Reserved

        End Get

    End Property

    Public ReadOnly Property MoreData() As Boolean

        Get

            Return _MoreData

        End Get

    End Property

#End Region

End Class
