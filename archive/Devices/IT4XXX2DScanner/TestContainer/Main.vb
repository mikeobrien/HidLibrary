Imports System.Runtime.InteropServices
Imports IT4XXXLibrary

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
    Friend WithEvents Scan As System.Windows.Forms.Button
    Friend WithEvents ErrorBeep As System.Windows.Forms.Button
    Friend WithEvents SuccessBeep As System.Windows.Forms.Button
    Friend WithEvents Status As System.Windows.Forms.StatusBar
    Friend WithEvents Output As System.Windows.Forms.TextBox
    Friend WithEvents Listen As System.Windows.Forms.Button
    Friend WithEvents ScannerStatus As System.Windows.Forms.StatusBarPanel
    Friend WithEvents DataRecievedEvents As System.Windows.Forms.ListBox
    Friend WithEvents StressTest As System.Windows.Forms.Button
    Friend WithEvents TotalScans As System.Windows.Forms.StatusBarPanel
    <System.Diagnostics.DebuggerStepThrough()> Private Sub InitializeComponent()
        Dim resources As System.Resources.ResourceManager = New System.Resources.ResourceManager(GetType(Main))
        Me.Listen = New System.Windows.Forms.Button
        Me.Output = New System.Windows.Forms.TextBox
        Me.Scan = New System.Windows.Forms.Button
        Me.ErrorBeep = New System.Windows.Forms.Button
        Me.SuccessBeep = New System.Windows.Forms.Button
        Me.Status = New System.Windows.Forms.StatusBar
        Me.ScannerStatus = New System.Windows.Forms.StatusBarPanel
        Me.TotalScans = New System.Windows.Forms.StatusBarPanel
        Me.DataRecievedEvents = New System.Windows.Forms.ListBox
        Me.StressTest = New System.Windows.Forms.Button
        CType(Me.ScannerStatus, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.TotalScans, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.SuspendLayout()
        '
        'Listen
        '
        Me.Listen.Location = New System.Drawing.Point(0, 0)
        Me.Listen.Name = "Listen"
        Me.Listen.Size = New System.Drawing.Size(80, 24)
        Me.Listen.TabIndex = 2
        Me.Listen.Text = "Start Listen"
        '
        'Output
        '
        Me.Output.Anchor = CType(((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left) _
                    Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.Output.BorderStyle = System.Windows.Forms.BorderStyle.None
        Me.Output.Font = New System.Drawing.Font("Courier New", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Output.Location = New System.Drawing.Point(0, 232)
        Me.Output.Multiline = True
        Me.Output.Name = "Output"
        Me.Output.ReadOnly = True
        Me.Output.ScrollBars = System.Windows.Forms.ScrollBars.Both
        Me.Output.Size = New System.Drawing.Size(688, 232)
        Me.Output.TabIndex = 3
        Me.Output.Text = ""
        '
        'Scan
        '
        Me.Scan.Location = New System.Drawing.Point(80, 0)
        Me.Scan.Name = "Scan"
        Me.Scan.Size = New System.Drawing.Size(56, 24)
        Me.Scan.TabIndex = 4
        Me.Scan.Text = "Scan"
        '
        'ErrorBeep
        '
        Me.ErrorBeep.Location = New System.Drawing.Point(232, 0)
        Me.ErrorBeep.Name = "ErrorBeep"
        Me.ErrorBeep.Size = New System.Drawing.Size(88, 24)
        Me.ErrorBeep.TabIndex = 6
        Me.ErrorBeep.Text = "Error Beep"
        '
        'SuccessBeep
        '
        Me.SuccessBeep.Location = New System.Drawing.Point(136, 0)
        Me.SuccessBeep.Name = "SuccessBeep"
        Me.SuccessBeep.Size = New System.Drawing.Size(96, 24)
        Me.SuccessBeep.TabIndex = 7
        Me.SuccessBeep.Text = "Success Beep"
        '
        'Status
        '
        Me.Status.Location = New System.Drawing.Point(0, 461)
        Me.Status.Name = "Status"
        Me.Status.Panels.AddRange(New System.Windows.Forms.StatusBarPanel() {Me.ScannerStatus, Me.TotalScans})
        Me.Status.ShowPanels = True
        Me.Status.Size = New System.Drawing.Size(688, 22)
        Me.Status.TabIndex = 9
        '
        'ScannerStatus
        '
        Me.ScannerStatus.AutoSize = System.Windows.Forms.StatusBarPanelAutoSize.Spring
        Me.ScannerStatus.Width = 336
        '
        'TotalScans
        '
        Me.TotalScans.AutoSize = System.Windows.Forms.StatusBarPanelAutoSize.Spring
        Me.TotalScans.Text = "Total Scans: 0"
        Me.TotalScans.Width = 336
        '
        'DataRecievedEvents
        '
        Me.DataRecievedEvents.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
                    Or System.Windows.Forms.AnchorStyles.Left) _
                    Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.DataRecievedEvents.Location = New System.Drawing.Point(0, 32)
        Me.DataRecievedEvents.Name = "DataRecievedEvents"
        Me.DataRecievedEvents.Size = New System.Drawing.Size(672, 186)
        Me.DataRecievedEvents.TabIndex = 10
        '
        'StressTest
        '
        Me.StressTest.Location = New System.Drawing.Point(320, 0)
        Me.StressTest.Name = "StressTest"
        Me.StressTest.Size = New System.Drawing.Size(112, 24)
        Me.StressTest.TabIndex = 11
        Me.StressTest.Text = "Start Stress Test"
        '
        'Main
        '
        Me.AutoScaleBaseSize = New System.Drawing.Size(5, 13)
        Me.ClientSize = New System.Drawing.Size(688, 483)
        Me.Controls.Add(Me.StressTest)
        Me.Controls.Add(Me.DataRecievedEvents)
        Me.Controls.Add(Me.Status)
        Me.Controls.Add(Me.SuccessBeep)
        Me.Controls.Add(Me.ErrorBeep)
        Me.Controls.Add(Me.Scan)
        Me.Controls.Add(Me.Output)
        Me.Controls.Add(Me.Listen)
        Me.Icon = CType(resources.GetObject("$this.Icon"), System.Drawing.Icon)
        Me.Name = "Main"
        Me.Text = "IT4600 Test"
        CType(Me.ScannerStatus, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.TotalScans, System.ComponentModel.ISupportInitialize).EndInit()
        Me.ResumeLayout(False)

    End Sub

#End Region

    Private WithEvents IT4600Device As IT4XXX

    Private Delegate Sub DataRecieved(ByVal Data As Byte())

    Private StressTestEnabled As Boolean

    Private Sub Main_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load

        Dim Devices As IT4XXX()

        Devices = IT4XXX.Enumerate(New Integer() {&H207, &H1C7})

        If Devices.Length > 0 Then

            IT4600Device = Devices(0)

            Debug.WriteLine(IT4600Device.DevicePath)

        End If

    End Sub

    Private Sub ErrorBeep_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ErrorBeep.Click

        IT4600Device.ErrorBeep()

    End Sub

    Private Sub SuccessBeep_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles SuccessBeep.Click

        IT4600Device.SuccessBeep()

    End Sub

    Private Sub Scan_MouseDown(ByVal sender As Object, ByVal e As System.Windows.Forms.MouseEventArgs) Handles Scan.MouseDown

        IT4600Device.StartScan()

    End Sub

    Private Sub Scan_MouseUp(ByVal sender As Object, ByVal e As System.Windows.Forms.MouseEventArgs) Handles Scan.MouseUp

        IT4600Device.EndScan()

    End Sub

    Private Sub SelectedDevice_Inserted() Handles IT4600Device.Inserted

        ScannerStatus.Text = "Connected"

    End Sub

    Private Sub SelectedDevice_Removed() Handles IT4600Device.Removed

        ScannerStatus.Text = "Not Connected"

    End Sub

    Private Sub Listen_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Listen.Click

        If IT4600Device.IsListening = True Then

            Listen.Text = "Start Listen"
            IT4600Device.StopListen()

        Else

            Listen.Text = "Stop Listen"
            IT4600Device.StartListen()

        End If

    End Sub

    Private Sub IT4600Device_DataRecieved(ByVal Data() As Byte) Handles IT4600Device.DataRecieved

        Me.BeginInvoke(New DataRecieved(AddressOf DataRecievedEvent), New Object() {Data})

    End Sub

    Private Sub DataRecievedEvent(ByVal Data As Byte())

        Static ScanTotal As Integer

        ScanTotal += 1

        DataRecievedEvents.Items.Add(Now.ToString("hh:mm:ss") & " - " & System.Text.ASCIIEncoding.ASCII.GetString(Data))

        DataRecievedEvents.SelectedIndex = DataRecievedEvents.Items.Count - 1

        Output.Text = System.Text.ASCIIEncoding.ASCII.GetString(Data)

        Dim OutputFile As New IO.BinaryWriter(New IO.FileStream("ScannedData.dat", IO.FileMode.Create))

        OutputFile.Write(Data)

        OutputFile.Flush()
        OutputFile.Close()

        TotalScans.Text = "Total Scans: " & ScanTotal

        If StressTestEnabled = True Then

            Dim StressTestLog As New IO.BinaryWriter(New IO.FileStream("StressTestLog.dat", IO.FileMode.Append))

            StressTestLog.Write(Data)

            StressTestLog.Close()

            IT4600Device.StartScan()

        End If

    End Sub

    Private Sub StressTest_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles StressTest.Click

        If StressTestEnabled = False Then

            StressTest.Text = "Stop Stress Test"
            StressTestEnabled = True
            IT4600Device.StartScan()

        Else

            StressTest.Text = "Start Stress Test"
            StressTestEnabled = False

        End If

    End Sub

End Class
