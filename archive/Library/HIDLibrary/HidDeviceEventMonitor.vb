Imports System.Threading

Friend Class HidDeviceEventMonitor

#Region "--> Public Events "

    Public Event Inserted()
    Public Event Removed()

#End Region

#Region "--> Private Fields "

    Private Delegate Sub _EventMonitorDelagate()
    Private _Device As HidDevice
    Private _Connected As Boolean = False
    Private _PollingFrequency As Integer = 500
    Private _Enabled As Boolean = False

#End Region

#Region "--> Constructor "

    Public Sub New(ByVal Device As HidDevice)

        _Device = Device

    End Sub

#End Region

#Region "--> Public Properties "

    Public Property PollingFrequency() As Integer
        Get
            Return _PollingFrequency
        End Get
        Set(ByVal value As Integer)
            _PollingFrequency = value
        End Set
    End Property

    Public ReadOnly Property Connected() As Boolean
        Get

            Return _Connected

        End Get
    End Property

    Public Property Enabled() As Boolean
        Get

            Return _Enabled

        End Get
        Set(ByVal value As Boolean)

            If value = True And _Enabled = False Then

                Init()

            End If

            _Enabled = value

        End Set
    End Property

#End Region

#Region "--> Private Methods "

    Private Sub Init()

        Dim EventMonitor As New _EventMonitorDelagate(AddressOf DeviceEventMonitor)

        EventMonitor.BeginInvoke(AddressOf DisposeDeviceEventMonitor, EventMonitor)

    End Sub

    Private Sub DeviceEventMonitor()

        Static DeviceWasConnected As Boolean

        _Connected = HidDevices.IsConnected(_Device.DevicePath)

        If _Connected <> DeviceWasConnected Then

            If _Connected = True Then

                RaiseEvent Inserted()

            Else

                RaiseEvent Removed()

            End If

            DeviceWasConnected = _Connected

        End If

        Thread.Sleep(_PollingFrequency)

        If _Enabled = True Then

            Init()

        End If

    End Sub

    Private Sub DisposeDeviceEventMonitor(ByVal ar As IAsyncResult)

        ar.AsyncState.EndInvoke(ar)

    End Sub

#End Region

End Class
