Public Class HidDeviceIdentifier

#Region "Enums "

    Public Enum IdentifierTypes
        Any = 0
        VendorId = 1
        VendorAndProductId = 2
        DevicePath = 3
    End Enum

#End Region

#Region "Private Fields "

    Private _IdentifierType As IdentifierTypes
    Private _DevicePath As String
    Private _VendorId As Integer
    Private _ProductId As Integer

#End Region


#Region "Constructor "

    Public Sub New()
        _IdentifierType = IdentifierTypes.Any
    End Sub

    Public Sub New(ByVal VendorId As Integer, ByVal ProductId As Integer)

        _VendorId = VendorId
        _ProductId = ProductId
        _IdentifierType = IdentifierTypes.VendorAndProductId

    End Sub

    Public Sub New(ByVal VendorId As Integer)

        _VendorId = VendorId
        _IdentifierType = IdentifierTypes.VendorId

    End Sub

    Public Sub New(ByVal DevicePath As String)

        _DevicePath = DevicePath
        _IdentifierType = IdentifierTypes.DevicePath

    End Sub

#End Region

#Region "Constructor "

    Public Overloads Function Equals(ByVal VendorId As Integer) As Boolean

        Return (_VendorId = VendorId)

    End Function

    Public Overloads Function Equals(ByVal VendorId As Integer, ByVal ProductId As Integer) As Boolean

        Return (_VendorId = VendorId And _ProductId = ProductId)

    End Function

    Public Overloads Function Equals(ByVal DevicePath As String) As Boolean

        Return String.Compare(_DevicePath, DevicePath, True) = 0

    End Function

#End Region

#Region "Public Properties "

    Public ReadOnly Property Type() As IdentifierTypes
        Get
            Return _IdentifierType
        End Get
    End Property

    Public ReadOnly Property DevicePath() As String
        Get
            Return _DevicePath
        End Get
    End Property

    Public ReadOnly Property VendorId() As Integer
        Get
            Return _VendorId
        End Get
    End Property

    Public ReadOnly Property ProductId() As Integer
        Get
            Return _ProductId
        End Get
    End Property

#End Region

End Class
