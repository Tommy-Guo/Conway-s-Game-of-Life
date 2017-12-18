Imports System.IO

Public Class formMain

    Private Sub btnNextStep_Click(sender As Object, e As EventArgs) Handles btnNextStep.Click
        'We will tell the universe to go to the next generation.
        Universe1.nextGeneration()
        'Updates the generation count label.
        updateGenerationLabel()
    End Sub

    Private Sub btnStartStep_Click(sender As Object, e As EventArgs) Handles btnStartStep.Click
        'Start auto second by second evolution
        universeTime.Start()
        'Enables/Disables buttons
        updateBtnStartStop()
    End Sub

    Private Sub updateBtnStartStop()
        'Enables/Disables buttons
        btnStartStep.Enabled = IIf(universeTime.Enabled, False, True)
        btnStepStop.Enabled = IIf(universeTime.Enabled, True, False)
    End Sub

    Private Sub btnStepStop_Click(sender As Object, e As EventArgs) Handles btnStepStop.Click
        'Stop auto second by second evolution
        universeTime.Stop()
        'Enables/Disables buttons
        updateBtnStartStop()
    End Sub
    
 Public Sub colorGeneration()
        'For random color feature. -asakaz
        Dim A As Integer
        Dim R As Integer
        Dim G As Integer
        Dim b As Integer
        Randomize()
        A = CInt(Int((254 * Rnd()) + 0))
        Randomize()
        R = CInt(Int((254 * Rnd()) + 0))
        Randomize()
        G = CInt(Int((254 * Rnd()) + 0))
        Randomize()
        b = CInt(Int((254 * Rnd()) + 0))

        Universe1.aliveColor = Color.FromArgb(A, R, G, b)

    End Sub
    
    Private Sub universeTime_Tick(sender As Object, e As EventArgs) Handles universeTime.Tick
        'This timer speeds up time in the universe.
        Universe1.nextGeneration()
        'Updates the generation count label.
        updateGenerationLabel()
    End Sub

    Private Sub numTimerValue_ValueChanged(sender As Object, e As EventArgs) Handles numTimerValue.ValueChanged
        'This numeric control controls how fast time in the universe ticks at.
        universeTime.Interval = numTimerValue.Value
    End Sub

    Private Sub btnRestart_Click(sender As Object, e As EventArgs) Handles btnRestart.Click
        universeTime.Stop() 'Stop time in the universe incase it's still running.
        'By repopulating the universe will dead cells, we're basically killing everyone and resetting the universe.
        Universe1.clearArrays() 'Clears the universe cell arrays. This is what sets every cell as dead.
        Universe1.Invalidate() 'Not really necessary but redraws just incase.
        updateGenerationLabel() 'Updates the label to the new population count which will be 0 because everyone is dead.
        updateBtnStartStop() 'Enables/Disables buttons
    End Sub

    Private Sub updateGenerationLabel()
        'Updating the generation count label.
        lblGenerationCount.Text = String.Format("Universe Generations: {0}", Universe1.Generations)
    End Sub

    Private Sub btnExport_Click(sender As Object, e As EventArgs) Handles btnExport.Click
        'Create save dialog with .txt only filter
        Dim nSfd As New SaveFileDialog With {.Filter = "Text File|.txt"}
        If nSfd.ShowDialog = Windows.Forms.DialogResult.OK Then
            'If use sucessfully exits the dialog the universe will save
            Universe1.export(nSfd.FileName)
            'Prompt the user their world has saved.
            MsgBox("World Saved!", MsgBoxStyle.OkOnly, "Your world has been saved!")
        End If
    End Sub

    Private Sub btnImport_Click(sender As Object, e As EventArgs) Handles btnImport.Click
        'Prompt user to confirm the world override.
        Dim confirmation As DialogResult = MsgBox("Are you sure you would like to import a external world?" & vbNewLine & " Your current world will be deleted.", MsgBoxStyle.YesNoCancel, "Are you sure?")
        If confirmation = Windows.Forms.DialogResult.Yes Then
            'If user sucessfully exits the dialog the world will be imported
            Dim nOfd As New OpenFileDialog With {.Filter = "Text File|.txt"}
            If nOfd.ShowDialog = Windows.Forms.DialogResult.OK Then
                Universe1.import(nOfd.FileName)
            End If
        End If
    End Sub
End Class

Public Class universe
    Inherits Control

    'This single line of code stores every living entity in the universe. Be careful.
    Private cells(_universeSize.Width - 1, _universeSize.Height - 1) As cell

#Region "Properties"
    'Tell the universe how big every creature is.
    Public Property cellSize As New Size(10, 10)
    'Is this racist?
    Public Property aliveColor As Color = Color.Black
    'Just a property to return how many generations have passed.
    Private _generations As Integer = 0
    Public ReadOnly Property Generations As Integer
        Get
            Return _generations
        End Get
    End Property
    'Sets the observable universe size.
    Private _universeSize As New Size(60, 30)
    Public Property universeSize As Size
        Get
            Return _universeSize
        End Get
        Set(value As Size)
            _universeSize = value
            ReDim cells(_universeSize.Width - 1, _universeSize.Height - 1)
            Invalidate()
            clearArrays()
        End Set
    End Property
#End Region

#Region "Janitorial stuff"
    Sub New()
        DoubleBuffered = True
    End Sub

    Public Sub clearArrays()
        'Kills every creature in the universe.
        For y As Integer = 0 To _universeSize.Height - 1
            For x As Integer = 0 To _universeSize.Width - 1
                cells(x, y) = New cell(New Point(x, y))
            Next
        Next
        'Update population count to dead.
        _generations = 0
    End Sub
#End Region

#Region "Drawing Control"
    Protected Overrides Sub OnPaint(e As PaintEventArgs)
        MyBase.OnPaint(e)
        Dim g As Graphics = e.Graphics : e.Graphics.SmoothingMode = Drawing2D.SmoothingMode.HighSpeed

        For Each cell As cell In cells
            'Update the cells that should be dead or alive after last generation.
            If cell.shouldToggle Then
                cell.shouldToggle = False
                toggleCell(cell)
            End If
        Next

        For Each cell As cell In cells
            'Draw cells that are alive in selected colour.
            If cell.status = status.Alive Then
                g.FillRectangle(New SolidBrush(aliveColor), New Rectangle(cell.location.X * cellSize.Width, cell.location.Y * cellSize.Height, cellSize.Width, cellSize.Height))
            End If
        Next

        'Draw grid, dependent on universe size.
        For x As Integer = 0 To _universeSize.Width
            g.DrawLine(Pens.LightGray, x * cellSize.Width, 0, x * cellSize.Width, Height)
        Next
        For y As Integer = 0 To _universeSize.Height
            g.DrawLine(Pens.LightGray, 0, y * cellSize.Height, Width, y * cellSize.Height)
        Next

        'Draw border on entire control.
        g.DrawRectangle(Pens.LightGray, 0, 0, Width - 1, Height - 1)
    End Sub
#End Region

#Region "Private functions"
    'Updates cell depending on its social status. If you have friends you get to live.
    Private Sub updateCell(cell As cell)
        Dim aliveNeighbours As Integer = getNeighbourhood(cell)
        If cell.status = status.Alive And aliveNeighbours < 2 Then
            cells(cell.location.X, cell.location.Y).shouldToggle = True
        ElseIf cell.status = status.Alive And (aliveNeighbours = 2 Or aliveNeighbours = 3) Then
            cells(cell.location.X, cell.location.Y).shouldToggle = False
        ElseIf cell.status = status.Alive And aliveNeighbours > 3 Then
            cells(cell.location.X, cell.location.Y).shouldToggle = True
        ElseIf cell.status = status.Dead And aliveNeighbours = 3 Then
            cells(cell.location.X, cell.location.Y).shouldToggle = True
        End If
    End Sub

    'This can probably be a lot cleaner and shorter...
    Private Function getNeighbourhood(sourceCell As cell) As Integer
        Dim neighbourCount As Integer = 0
        If getCellStatus(sourceCell.location.X, sourceCell.location.Y - 1) Then neighbourCount += 1 'North
        If getCellStatus(sourceCell.location.X + 1, sourceCell.location.Y - 1) Then neighbourCount += 1 'North East
        If getCellStatus(sourceCell.location.X + 1, sourceCell.location.Y) Then neighbourCount += 1 'East
        If getCellStatus(sourceCell.location.X + 1, sourceCell.location.Y + 1) Then neighbourCount += 1 'South East
        If getCellStatus(sourceCell.location.X, sourceCell.location.Y + 1) Then neighbourCount += 1 'South
        If getCellStatus(sourceCell.location.X - 1, sourceCell.location.Y + 1) Then neighbourCount += 1 'South West 
        If getCellStatus(sourceCell.location.X - 1, sourceCell.location.Y) Then neighbourCount += 1 'West
        If getCellStatus(sourceCell.location.X - 1, sourceCell.location.Y - 1) Then neighbourCount += 1 'North West
        Return neighbourCount
    End Function

    'Returns the status of a creature. (True = Alive, False = Dead)
    Private Function getCellStatus(x As Integer, y As Integer) As Boolean
        Return IIf(cells(toroidalArray(x, y).X, toroidalArray(x, y).Y).status = status.Alive, True, False)
    End Function

    'We want the creatures to wrap around because their plant is round. Not flat. It's round.
    Private Function toroidalArray(x As Integer, y As Integer) As Point
        Dim outputX, outputY As Integer
        If x > -1 Then
            outputX = x Mod _universeSize.Width
        Else
            outputX = _universeSize.Width - Math.Abs(x)
        End If
        If y > -1 Then
            outputY = y Mod _universeSize.Height
        Else
            outputY = _universeSize.Height - Math.Abs(y)
        End If
        Return New Point(outputX, outputY)
    End Function

    'Toggles because normall setting it is annoying.
    Private Sub toggleCell(cell As cell)
        cell.status = IIf(cell.status = status.Alive, status.Dead, status.Alive)
    End Sub
#End Region

#Region "The only one public function"
    Public Sub nextGeneration()
        For y As Integer = 0 To _universeSize.Height - 1
            For x As Integer = 0 To _universeSize.Width - 1
                updateCell(cells(x, y))
            Next
        Next
        _generations += 1
        Invalidate()
    End Sub
#End Region

#Region "Mouse move/click detection to toggle cell status."
    Protected Overrides Sub OnMouseMove(e As MouseEventArgs)
        MyBase.OnMouseMove(e)
        'If you wave your magic wand the right way, you get to bring creatures to life.
        If Math.Floor(e.X / cellSize.Width) < Width And Math.Floor(e.Y / cellSize.Height) < Height And (e.Button = Windows.Forms.MouseButtons.Left Or e.Button = Windows.Forms.MouseButtons.Right) Then
            'Or accidently kill them :3
            cells(Math.Floor(e.X / cellSize.Width), Math.Floor(e.Y / cellSize.Height)).status = IIf(e.Button = Windows.Forms.MouseButtons.Left, status.Alive, status.Dead)
            Invalidate()
        End If
    End Sub
    Protected Overrides Sub OnMouseDown(e As MouseEventArgs)
        MyBase.OnMouseDown(e)
        'Secondary wand just triggers the main one.
        OnMouseMove(New MouseEventArgs(e.Button, 1, e.X, e.Y, e.Delta))
    End Sub
#End Region

#Region "Import/Export World"
    Public Sub import(filename As String)
        clearArrays() 'Start with a fresh canvas.
        Dim importedCells() As String = File.ReadAllLines(filename) 'Reads data from text file
        For Each cellData As String In importedCells
            cells(cellData.Split("-"c)(0), cellData.Split("-"c)(1)).status = status.Alive
            'Set the imported cells
        Next
        Invalidate() 'Redraw the world to show the imported world
    End Sub
    Public Sub export(filename As String)
        Dim aliveCells As New List(Of cell)
        For y As Integer = 0 To _universeSize.Height - 1
            For x As Integer = 0 To _universeSize.Width - 1
                If cells(x, y).status = status.Alive Then
                    aliveCells.Add(cells(x, y)) 'Get a list of all cells alive
                End If
            Next
        Next

        Dim sw As StreamWriter = File.CreateText(filename) 'Create and open new text file
        For Each cell As cell In aliveCells
            sw.WriteLine(String.Format("{0}-{1}", cell.location.X, cell.location.Y))
            'Write data to the text file.
        Next
        sw.Close()
    End Sub
#End Region

End Class

Public Class cell
    'Every creature starts off dead.
    Public status As status = status.Dead
    'You're nowhere in the universe.
    Public location As Point
    'If this is true you're lucky!
    Public shouldToggle As Boolean = False
    Sub New(x As Point)
        Me.location = x
    End Sub
End Class

Public Enum status
    'Status really could have just been a boolean value (true = alive, false = dead) but... why not.
    Dead = 1
    Alive = 2
End Enum
