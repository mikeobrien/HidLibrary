Public Class HidDeviceCapabilities

#Region "--> Private Fields "

    Private _Usage As Short
    Private _UsagePage As Short
    Private _InputReportByteLength As Short
    Private _OutputReportByteLength As Short
    Private _FeatureReportByteLength As Short
    Private _Reserved As Short()
    Private _NumberLinkCollectionNodes As Short
    Private _NumberInputButtonCaps As Short
    Private _NumberInputValueCaps As Short
    Private _NumberInputDataIndices As Short
    Private _NumberOutputButtonCaps As Short
    Private _NumberOutputValueCaps As Short
    Private _NumberOutputDataIndices As Short
    Private _NumberFeatureButtonCaps As Short
    Private _NumberFeatureValueCaps As Short
    Private _NumberFeatureDataIndices As Short

#End Region

#Region "--> Constructor "

    Friend Sub New(ByVal Capabilities As NativeMethods.HIDP_CAPS)

        _Usage = Capabilities.Usage
        _UsagePage = Capabilities.UsagePage
        _InputReportByteLength = Capabilities.InputReportByteLength
        _OutputReportByteLength = Capabilities.OutputReportByteLength
        _FeatureReportByteLength = Capabilities.FeatureReportByteLength
        _Reserved = Capabilities.Reserved
        _NumberLinkCollectionNodes = Capabilities.NumberLinkCollectionNodes
        _NumberInputButtonCaps = Capabilities.NumberInputButtonCaps
        _NumberInputValueCaps = Capabilities.NumberInputValueCaps
        _NumberInputDataIndices = Capabilities.NumberInputDataIndices
        _NumberOutputButtonCaps = Capabilities.NumberOutputButtonCaps
        _NumberOutputValueCaps = Capabilities.NumberOutputValueCaps
        _NumberOutputDataIndices = Capabilities.NumberOutputDataIndices
        _NumberFeatureButtonCaps = Capabilities.NumberFeatureButtonCaps
        _NumberFeatureValueCaps = Capabilities.NumberFeatureValueCaps
        _NumberFeatureDataIndices = Capabilities.NumberFeatureDataIndices

    End Sub

#End Region

#Region "--> Public Properties "

    Public ReadOnly Property Usage() As Short

        Get

            Return _Usage

        End Get

    End Property

    Public ReadOnly Property UsagePage() As Short

        Get

            Return _UsagePage

        End Get

    End Property

    Public ReadOnly Property InputReportByteLength() As Short

        Get

            Return _InputReportByteLength

        End Get

    End Property

    Public ReadOnly Property OutputReportByteLength() As Short

        Get

            Return _OutputReportByteLength

        End Get

    End Property

    Public ReadOnly Property FeatureReportByteLength() As Short

        Get

            Return _FeatureReportByteLength

        End Get

    End Property

    Public ReadOnly Property Reserved() As Short()

        Get

            Return _Reserved

        End Get

    End Property

    Public ReadOnly Property NumberLinkCollectionNodes() As Short

        Get

            Return _NumberLinkCollectionNodes

        End Get

    End Property

    Public ReadOnly Property NumberInputButtonCaps() As Short

        Get

            Return _NumberInputButtonCaps

        End Get

    End Property

    Public ReadOnly Property NumberInputValueCaps() As Short

        Get

            Return _NumberInputValueCaps

        End Get

    End Property

    Public ReadOnly Property NumberInputDataIndices() As Short

        Get

            Return _NumberInputDataIndices

        End Get

    End Property

    Public ReadOnly Property NumberOutputButtonCaps() As Short

        Get

            Return _NumberOutputButtonCaps

        End Get

    End Property

    Public ReadOnly Property NumberOutputValueCaps() As Short

        Get

            Return _NumberOutputValueCaps

        End Get

    End Property

    Public ReadOnly Property NumberOutputDataIndices() As Short

        Get

            Return _NumberOutputDataIndices

        End Get

    End Property

    Public ReadOnly Property NumberFeatureButtonCaps() As Short

        Get

            Return _NumberFeatureButtonCaps

        End Get

    End Property

    Public ReadOnly Property NumberFeatureValueCaps() As Short

        Get

            Return _NumberFeatureValueCaps

        End Get

    End Property

    Public ReadOnly Property NumberFeatureDataIndices() As Short

        Get

            Return _NumberFeatureDataIndices

        End Get

    End Property

#End Region

End Class
