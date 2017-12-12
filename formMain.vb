Public Class formMain

    Private Sub btnNextStep_Click(sender As Object, e As EventArgs) Handles btnNextStep.Click
        'We will tell the universe to go to the next generation.
        Universe1.nextGeneration()
        lblGenerationCount.Text = String.Format("Universe Generations: {0}", Universe1.Generations)
    End Sub

    Private Sub btnStartStep_Click(sender As Object, e As EventArgs) Handles btnStartStep.Click
        universeTime.Start()
    End Sub

    Private Sub btnStepStop_Click(sender As Object, e As EventArgs) Handles btnStepStop.Click
        universeTime.Stop()
    End Sub

    Private Sub stepTimer_Tick(sender As Object, e As EventArgs) Handles universeTime.Tick
        'The timer will tell the universe to go to the next generation every time it executes.
        Universe1.nextGeneration()
        lblGenerationCount.Text = String.Format("Universe Generations: {0}", Universe1.Generations)
    End Sub

    Private Sub numTimerValue_ValueChanged(sender As Object, e As EventArgs) Handles numTimerValue.ValueChanged
        'This numeric control controls the rate at which the universe goes to the next generation.
        universeTime.Interval = numTimerValue.Value
    End Sub

    Private Sub btnRestart_Click(sender As Object, e As EventArgs) Handles btnRestart.Click
        'By populating the arrays we assign each cell it's location and it's status as dead.
        Universe1.clearArrays()
        universeTime.Stop()
    End Sub 
End Class

Public Class universe
    Inherits Control

#Region "Properties"
    Public Property cellSize As New Size(5, 5)
    Public Property aliveColor As Color = Color.Black
    Private _generations As Integer = 0
    Public ReadOnly Property Generations As Integer
        Get
            Return _generations
        End Get
    End Property
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

    Private cells(_universeSize.Width - 1, _universeSize.Height - 1) As cell

    Sub New()
        DoubleBuffered = True
        clearArrays()
    End Sub

    Public Sub clearArrays()
        _generations = 0
        For y As Integer = 0 To _universeSize.Height - 1
            For x As Integer = 0 To _universeSize.Width - 1
                cells(x, y) = New cell(New Point(x, y))
            Next
        Next
    End Sub

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
            g.DrawLine(Pens.Gray, x * cellSize.Width, 0, x * cellSize.Width, Height)
        Next
        For y As Integer = 0 To _universeSize.Height
            g.DrawLine(Pens.Gray, 0, y * cellSize.Height, Width, y * cellSize.Height)
        Next

        'Draw border on entire control.
        g.DrawRectangle(Pens.Gray, 0, 0, Width - 1, Height - 1)
    End Sub

    Public Sub nextGeneration()
        For y As Integer = 0 To _universeSize.Height - 1
            For x As Integer = 0 To _universeSize.Width - 1
                updateCell(cells(x, y))
            Next
        Next
        _generations += 1
        Invalidate()
    End Sub

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

    Public Function getNeighbourhood(sourceCell As cell) As Integer
        Dim neighbourCount As Integer = 0
        'North
        If getCellStatus(sourceCell.location.X, sourceCell.location.Y - 1) = status.Alive Then neighbourCount += 1
        'North East
        If getCellStatus(sourceCell.location.X + 1, sourceCell.location.Y - 1) = status.Alive Then neighbourCount += 1
        'East
        If getCellStatus(sourceCell.location.X + 1, sourceCell.location.Y) = status.Alive Then neighbourCount += 1
        'South East
        If getCellStatus(sourceCell.location.X + 1, sourceCell.location.Y + 1) = status.Alive Then neighbourCount += 1
        'South
        If getCellStatus(sourceCell.location.X, sourceCell.location.Y + 1) = status.Alive Then neighbourCount += 1
        'South West
        If getCellStatus(sourceCell.location.X - 1, sourceCell.location.Y + 1) = status.Alive Then neighbourCount += 1
        'West
        If getCellStatus(sourceCell.location.X - 1, sourceCell.location.Y) = status.Alive Then neighbourCount += 1
        'North West
        If getCellStatus(sourceCell.location.X - 1, sourceCell.location.Y - 1) = status.Alive Then neighbourCount += 1
        Return neighbourCount
    End Function

    Private Function getCellStatus(x As Integer, y As Integer) As status
        Return cells(toroidalArray(x, y).X, toroidalArray(x, y).Y).status
    End Function

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

    Protected Overrides Sub OnMouseMove(e As MouseEventArgs)
        MyBase.OnMouseMove(e)
        If e.Button = Windows.Forms.MouseButtons.Left Then
            Try
                changeCell(cells(Math.Floor(e.X / cellSize.Width), Math.Floor(e.Y / cellSize.Height)), status.Alive)
            Catch
                'The selected cell does not exist.
            End Try
        ElseIf e.Button = Windows.Forms.MouseButtons.Right Then
            Try
                changeCell(cells(Math.Floor(e.X / cellSize.Width), Math.Floor(e.Y / cellSize.Height)), status.Dead)
            Catch
                'The selected cell does not exist.
            End Try
        End If
        Invalidate()
    End Sub

    Private Sub changeCell(sourceCell As cell, toStats As status)
        sourceCell.status = toStats
    End Sub

    Private Sub toggleCell(cell As cell)
        cell.status = IIf(cell.status = status.Alive, status.Dead, status.Alive)
    End Sub

    Protected Overrides Sub OnMouseDown(e As MouseEventArgs)
        MyBase.OnMouseDown(e)
        Select Case e.Button
            Case Windows.Forms.MouseButtons.Left
                OnMouseMove(New MouseEventArgs(Windows.Forms.MouseButtons.Left, 1, e.X, e.Y, e.Delta))
            Case Right
                OnMouseMove(New MouseEventArgs(Windows.Forms.MouseButtons.Right, 1, e.X, e.Y, e.Delta))
        End Select
    End Sub
End Class

Public Class cell
    Public status As status = status.Dead
    Public location As Point
    Public shouldToggle As Boolean = False
    Sub New(x As Point)
        Me.location = x
    End Sub
End Class

Public Enum status
    Dead = 1
    Alive = 2
End Enum