Imports System.Runtime.InteropServices
Imports HIDLibrary

Public Class Main
    Inherits System.Windows.Forms.Form

#Region " Windows Form Designer generated code "

    Public Sub New()
        MyBase.New()

        'This call is required by the Windows Form Designer.
        InitializeComponent()

        'Add any initialization after the InitializeComponent() call

    End Sub

    'Form overrides dispose to clean up the component list.
    Protected Overloads Overrides Sub Dispose(ByVal disposing As Boolean)
        If disposing Then
            If Not (components Is Nothing) Then
                components.Dispose()
            End If
        End If
        MyBase.Dispose(disposing)
    End Sub

    'Required by the Windows Form Designer
    Private components As System.ComponentModel.IContainer

    'NOTE: The following procedure is required by the Windows Form Designer
    'It can be modified using the Windows Form Designer.  
    'Do not modify it using the code editor.
    Friend WithEvents Devices As System.Windows.Forms.ListBox
    Friend WithEvents Read As System.Windows.Forms.Button
    Friend WithEvents RefreshDeviceList As System.Windows.Forms.Button
    Friend WithEvents Status As System.Windows.Forms.StatusBar
    Friend WithEvents OpenClose As System.Windows.Forms.Button
    Friend WithEvents Output As System.Windows.Forms.TextBox
    <System.Diagnostics.DebuggerStepThrough()> Private Sub InitializeComponent()
Me.Devices = New System.Windows.Forms.ListBox
Me.Read = New System.Windows.Forms.Button
Me.Output = New System.Windows.Forms.TextBox
Me.RefreshDeviceList = New System.Windows.Forms.Button
Me.Status = New System.Windows.Forms.StatusBar
Me.OpenClose = New System.Windows.Forms.Button
Me.SuspendLayout()
'
'Devices
'
Me.Devices.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
Me.Devices.IntegralHeight = False
Me.Devices.Location = New System.Drawing.Point(8, 8)
Me.Devices.Name = "Devices"
Me.Devices.Size = New System.Drawing.Size(320, 47)
Me.Devices.TabIndex = 1
'
'Read
'
Me.Read.Location = New System.Drawing.Point(114, 70)
Me.Read.Name = "Read"
Me.Read.Size = New System.Drawing.Size(104, 24)
Me.Read.TabIndex = 2
Me.Read.Text = "Read Device"
'
'Output
'
Me.Output.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
            Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
Me.Output.Font = New System.Drawing.Font("Courier New", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
Me.Output.Location = New System.Drawing.Point(8, 100)
Me.Output.Multiline = True
Me.Output.Name = "Output"
Me.Output.ReadOnly = True
Me.Output.ScrollBars = System.Windows.Forms.ScrollBars.Both
Me.Output.Size = New System.Drawing.Size(524, 309)
Me.Output.TabIndex = 3
'
'RefreshDeviceList
'
Me.RefreshDeviceList.Location = New System.Drawing.Point(224, 70)
Me.RefreshDeviceList.Name = "RefreshDeviceList"
Me.RefreshDeviceList.Size = New System.Drawing.Size(104, 24)
Me.RefreshDeviceList.TabIndex = 8
Me.RefreshDeviceList.Text = "Refresh"
'
'Status
'
Me.Status.Location = New System.Drawing.Point(0, 422)
Me.Status.Name = "Status"
Me.Status.Size = New System.Drawing.Size(540, 22)
Me.Status.TabIndex = 9
'
'OpenClose
'
Me.OpenClose.Location = New System.Drawing.Point(10, 70)
Me.OpenClose.Name = "OpenClose"
Me.OpenClose.Size = New System.Drawing.Size(98, 23)
Me.OpenClose.TabIndex = 10
Me.OpenClose.Text = "Open"
Me.OpenClose.UseVisualStyleBackColor = True
'
'Main
'
Me.AutoScaleBaseSize = New System.Drawing.Size(5, 13)
Me.ClientSize = New System.Drawing.Size(540, 444)
Me.Controls.Add(Me.OpenClose)
Me.Controls.Add(Me.Status)
Me.Controls.Add(Me.RefreshDeviceList)
Me.Controls.Add(Me.Output)
Me.Controls.Add(Me.Read)
Me.Controls.Add(Me.Devices)
Me.Name = "Main"
Me.Text = "Hid Tester"
Me.ResumeLayout(False)
Me.PerformLayout()

End Sub

#End Region

    Private DeviceList As HIDLibrary.HidDevice()
    Private WithEvents SelectedDevice As HidDevice

    Private Sub Main_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load

        RefreshDevices()

    End Sub

    Private Sub RefreshDevices()

        DeviceList = HidDevices.Enumerate()

        Devices.DisplayMember = "Description"
        Devices.DataSource = DeviceList

    End Sub

    Private Sub Devices_DoubleClick(ByVal sender As Object, ByVal e As System.EventArgs) Handles Devices.DoubleClick

        If Devices.SelectedIndex > -1 Then

            MsgBox("Connected: " & DeviceList(Devices.SelectedIndex).IsConnected)

        End If

    End Sub

    Private Sub Read_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Read.Click

        SelectedDevice.Open()
        BeginRead()

    End Sub

    Private Sub BeginRead()

        If Devices.SelectedIndex > -1 Then

            SelectedDevice = DeviceList(Devices.SelectedIndex)

            SelectedDevice.ReadReport(AddressOf ReadProcess)

        End If

    End Sub

    Delegate Sub ReadHandlerDelegate(ByVal Report As HidReport)

    Private Sub ReadProcess(ByVal Report As HidReport)

        Me.BeginInvoke(New ReadHandlerDelegate(AddressOf ReadHandler), New Object() {Report})

    End Sub

    Private Sub ReadHandler(ByVal Report As HidReport)

        Dim Buffer As String = String.Empty
        Dim Character As Byte

        For Each Character In Report.Data

            Buffer += Convert.ToChar(Character)

        Next

        Output.Text = Buffer

        'Dim Device As HidDevice
        'Dim InReport As HidReport = Report

        'Device = DeviceList(Devices.SelectedIndex)

        'Debug.WriteLine("Report.ReadStatus=" & Report.ReadStatus.ToString)

        'If Report.ReadStatus = HidDeviceData.ReadStatus.WaitTimedOut Then

        'ErrorBeep_Click(Nothing, Nothing)

        'ElseIf Report.ReadStatus = HidDeviceData.ReadStatus.Success Then

        '    Dim DataRead As Boolean = True
        '    Dim DataFile As New IO.BinaryWriter(New IO.FileStream("ScanData.dat", IO.FileMode.Create))

        '   Do

        'If InReport.Data(InReport.Data.GetUpperBound(0)) = 1 And DataRead = False Then

        ' InReport = Device.ReadReport

        ' End If
        '
        'DataRead = False

        'If InReport.Exists = True Then

        'Output.AppendText(System.Text.ASCIIEncoding.ASCII.GetString(InReport.Data))

        'DataFile.Write(InReport.Data)

        'End If

        '    Loop While InReport.Data(InReport.Data.GetUpperBound(0)) = 1 And InReport.Exists = True

        'DataFile.Close()

        'End If

        'If Report.ReadStatus <> HidDeviceData.ReadStatus.NotConnected Then Device.ReadReport(AddressOf ReadProcess)

    End Sub

    Private Sub WriteResult(ByVal Success As Boolean)

        MsgBox("Write result: " & Success)

    End Sub

    Private Sub Refresh_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles RefreshDeviceList.Click

        RefreshDevices()

    End Sub

    Private Delegate Sub InsertedDelegate()
    Private Sub SelectedDevice_Inserted() Handles SelectedDevice.Inserted

        If Me.InvokeRequired Then

            Me.BeginInvoke(New InsertedDelegate(AddressOf SelectedDevice_Inserted))

        Else

            Status.Text = "Connected"

        End If

    End Sub

    Private Delegate Sub RemovedDelegate()
    Private Sub SelectedDevice_Removed() Handles SelectedDevice.Removed

        If Me.InvokeRequired Then

            Me.BeginInvoke(New RemovedDelegate(AddressOf SelectedDevice_Removed))

        Else

            Status.Text = "Not Connected"

        End If

    End Sub

    Private Sub Main_Closing(ByVal sender As Object, ByVal e As System.ComponentModel.CancelEventArgs) Handles MyBase.Closing

        If Not SelectedDevice Is Nothing Then SelectedDevice.Close()

    End Sub

    Private Sub OpenClose_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles OpenClose.Click



    End Sub
End Class
