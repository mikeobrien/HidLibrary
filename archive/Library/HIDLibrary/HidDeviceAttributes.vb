Public Class HidDeviceAttributes

#Region "--> Private Fields "

    Private _VendorId As Integer
    Private _ProductId As Integer
    Private _Version As Integer
    Private _VendorHexId As String
    Private _ProductHexId As String

#End Region

#Region "--> Constructor "

    Friend Sub New(ByVal Attributes As NativeMethods.HIDD_ATTRIBUTES)

        _VendorId = Attributes.VendorID
        _ProductId = Attributes.ProductID
        _Version = Attributes.VersionNumber

        _VendorHexId = "0x" & Hex(Attributes.VendorID).PadLeft(4, "0")
        _ProductHexId = "0x" & Hex(Attributes.ProductID).PadLeft(4, "0")

    End Sub

#End Region

#Region "--> Public Properties "

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

    Public ReadOnly Property Version() As Integer

        Get

            Return _Version

        End Get

    End Property

    Public Property VendorHexId() As String

        Get

            Return _VendorHexId

        End Get

        Set(ByVal Value As String)

            _VendorHexId = Value

        End Set

    End Property

    Public Property ProductHexId() As String

        Get

            Return _ProductHexId

        End Get

        Set(ByVal Value As String)

            _ProductHexId = Value

        End Set

    End Property

#End Region

End Class
