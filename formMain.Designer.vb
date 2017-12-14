<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class formMain
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
        Me.components = New System.ComponentModel.Container()
        Me.btnNextStep = New System.Windows.Forms.Button()
        Me.btnStartStep = New System.Windows.Forms.Button()
        Me.btnStepStop = New System.Windows.Forms.Button()
        Me.universeTime = New System.Windows.Forms.Timer(Me.components)
        Me.numTimerValue = New System.Windows.Forms.NumericUpDown()
        Me.btnRestart = New System.Windows.Forms.Button()
        Me.lblSpeed = New System.Windows.Forms.Label()
        Me.lblMS = New System.Windows.Forms.Label()
        Me.lblGenerationCount = New System.Windows.Forms.Label()
        Me.Universe1 = New Conways_Game_of_Life.universe()
        Me.btnExport = New System.Windows.Forms.Button()
        Me.btnImport = New System.Windows.Forms.Button()
        CType(Me.numTimerValue, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.SuspendLayout()
        '
        'btnNextStep
        '
        Me.btnNextStep.Location = New System.Drawing.Point(710, 375)
        Me.btnNextStep.Name = "btnNextStep"
        Me.btnNextStep.Size = New System.Drawing.Size(101, 23)
        Me.btnNextStep.TabIndex = 0
        Me.btnNextStep.Text = "Next Generation"
        Me.btnNextStep.UseVisualStyleBackColor = True
        '
        'btnStartStep
        '
        Me.btnStartStep.Font = New System.Drawing.Font("Consolas", 9.0!)
        Me.btnStartStep.Location = New System.Drawing.Point(127, 376)
        Me.btnStartStep.Name = "btnStartStep"
        Me.btnStartStep.Size = New System.Drawing.Size(23, 23)
        Me.btnStartStep.TabIndex = 3
        Me.btnStartStep.Text = "►"
        Me.btnStartStep.UseVisualStyleBackColor = True
        '
        'btnStepStop
        '
        Me.btnStepStop.Enabled = False
        Me.btnStepStop.Font = New System.Drawing.Font("Consolas", 9.0!)
        Me.btnStepStop.Location = New System.Drawing.Point(156, 376)
        Me.btnStepStop.Name = "btnStepStop"
        Me.btnStepStop.Size = New System.Drawing.Size(23, 23)
        Me.btnStepStop.TabIndex = 4
        Me.btnStepStop.Text = "■"
        Me.btnStepStop.UseVisualStyleBackColor = True
        '
        'universeTime
        '
        Me.universeTime.Interval = 300
        '
        'numTimerValue
        '
        Me.numTimerValue.Location = New System.Drawing.Point(46, 378)
        Me.numTimerValue.Maximum = New Decimal(New Integer() {1316134912, 2328, 0, 0})
        Me.numTimerValue.Minimum = New Decimal(New Integer() {1, 0, 0, 0})
        Me.numTimerValue.Name = "numTimerValue"
        Me.numTimerValue.Size = New System.Drawing.Size(54, 20)
        Me.numTimerValue.TabIndex = 5
        Me.numTimerValue.Value = New Decimal(New Integer() {10, 0, 0, 0})
        '
        'btnRestart
        '
        Me.btnRestart.Location = New System.Drawing.Point(629, 375)
        Me.btnRestart.Name = "btnRestart"
        Me.btnRestart.Size = New System.Drawing.Size(75, 23)
        Me.btnRestart.TabIndex = 6
        Me.btnRestart.Text = "Restart"
        Me.btnRestart.UseVisualStyleBackColor = True
        '
        'lblSpeed
        '
        Me.lblSpeed.AutoSize = True
        Me.lblSpeed.Location = New System.Drawing.Point(7, 380)
        Me.lblSpeed.Name = "lblSpeed"
        Me.lblSpeed.Size = New System.Drawing.Size(41, 13)
        Me.lblSpeed.TabIndex = 7
        Me.lblSpeed.Text = "Speed:"
        '
        'lblMS
        '
        Me.lblMS.AutoSize = True
        Me.lblMS.Location = New System.Drawing.Point(101, 380)
        Me.lblMS.Name = "lblMS"
        Me.lblMS.Size = New System.Drawing.Size(20, 13)
        Me.lblMS.TabIndex = 8
        Me.lblMS.Text = "ms"
        '
        'lblGenerationCount
        '
        Me.lblGenerationCount.Location = New System.Drawing.Point(434, 380)
        Me.lblGenerationCount.Name = "lblGenerationCount"
        Me.lblGenerationCount.Size = New System.Drawing.Size(189, 13)
        Me.lblGenerationCount.TabIndex = 9
        Me.lblGenerationCount.Text = "Universe Generations: 0"
        Me.lblGenerationCount.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        '
        'Universe1
        '
        Me.Universe1.aliveColor = System.Drawing.Color.Black
        Me.Universe1.cellSize = New System.Drawing.Size(5, 5)
        Me.Universe1.Location = New System.Drawing.Point(10, 12)
        Me.Universe1.Name = "Universe1"
        Me.Universe1.Size = New System.Drawing.Size(801, 351)
        Me.Universe1.TabIndex = 10
        Me.Universe1.Text = "Universe1"
        Me.Universe1.universeSize = New System.Drawing.Size(160, 71)
        '
        'btnExport
        '
        Me.btnExport.Location = New System.Drawing.Point(300, 375)
        Me.btnExport.Name = "btnExport"
        Me.btnExport.Size = New System.Drawing.Size(75, 23)
        Me.btnExport.TabIndex = 11
        Me.btnExport.Text = "Export"
        Me.btnExport.UseVisualStyleBackColor = True
        '
        'btnImport
        '
        Me.btnImport.Location = New System.Drawing.Point(221, 375)
        Me.btnImport.Name = "btnImport"
        Me.btnImport.Size = New System.Drawing.Size(75, 23)
        Me.btnImport.TabIndex = 12
        Me.btnImport.Text = "Import"
        Me.btnImport.UseVisualStyleBackColor = True
        '
        'formMain
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(823, 404)
        Me.Controls.Add(Me.btnImport)
        Me.Controls.Add(Me.btnExport)
        Me.Controls.Add(Me.Universe1)
        Me.Controls.Add(Me.lblGenerationCount)
        Me.Controls.Add(Me.numTimerValue)
        Me.Controls.Add(Me.lblMS)
        Me.Controls.Add(Me.lblSpeed)
        Me.Controls.Add(Me.btnRestart)
        Me.Controls.Add(Me.btnStepStop)
        Me.Controls.Add(Me.btnStartStep)
        Me.Controls.Add(Me.btnNextStep)
        Me.Name = "formMain"
        Me.Text = "Conway's Game of Life"
        CType(Me.numTimerValue, System.ComponentModel.ISupportInitialize).EndInit()
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub
    Friend WithEvents btnNextStep As System.Windows.Forms.Button
    Friend WithEvents btnStartStep As System.Windows.Forms.Button
    Friend WithEvents btnStepStop As System.Windows.Forms.Button
    Friend WithEvents universeTime As System.Windows.Forms.Timer
    Friend WithEvents numTimerValue As System.Windows.Forms.NumericUpDown
    Friend WithEvents btnRestart As System.Windows.Forms.Button
    Friend WithEvents lblSpeed As System.Windows.Forms.Label
    Friend WithEvents lblMS As System.Windows.Forms.Label
    Friend WithEvents lblGenerationCount As System.Windows.Forms.Label
    Friend WithEvents Universe1 As Conways_Game_of_Life.universe
    Friend WithEvents btnExport As System.Windows.Forms.Button
    Friend WithEvents btnImport As System.Windows.Forms.Button

End Class
