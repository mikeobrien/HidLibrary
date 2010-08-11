<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class OpenHidDevice
    Inherits System.Windows.Forms.Form

    'Form overrides dispose to clean up the component list.
    <System.Diagnostics.DebuggerNonUserCode()> _
    Protected Overrides Sub Dispose(ByVal disposing As Boolean)
        Try
            If disposing AndAlso components IsNot Nothing Then
                components.Dispose()
            End If
        Finally
            MyBase.Dispose(disposing)
        End Try
    End Sub

    'Required by the Windows Form Designer
    Private components As System.ComponentModel.IContainer

    'NOTE: The following procedure is required by the Windows Form Designer
    'It can be modified using the Windows Form Designer.  
    'Do not modify it using the code editor.
    <System.Diagnostics.DebuggerStepThrough()> _
    Private Sub InitializeComponent()
Me.ListView1 = New System.Windows.Forms.ListView
Me.SuspendLayout()
'
'ListView1
'
Me.ListView1.Location = New System.Drawing.Point(152, 63)
Me.ListView1.Name = "ListView1"
Me.ListView1.Size = New System.Drawing.Size(121, 97)
Me.ListView1.TabIndex = 0
Me.ListView1.UseCompatibleStateImageBehavior = False
'
'OpenHidDevice
'
Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
Me.ClientSize = New System.Drawing.Size(407, 287)
Me.Controls.Add(Me.ListView1)
Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog
Me.MaximizeBox = False
Me.MinimizeBox = False
Me.Name = "OpenHidDevice"
Me.ShowInTaskbar = False
Me.Text = "Open Hid Device"
Me.ResumeLayout(False)

End Sub
    Friend WithEvents ListView1 As System.Windows.Forms.ListView
End Class
