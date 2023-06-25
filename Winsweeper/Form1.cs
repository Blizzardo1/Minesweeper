using System.Net.Mime;
using Libsweeper;
using Minesweeper;

namespace Winsweeper;

public partial class Form1 : Form {
    private Board _board;
    private const int CellSize = 32;
    private const int XOffset = 10;
    private const int YOffset = 10;
    private const int CellPadding = 0;

    private readonly Size _cellSize;

    public Form1() {
        InitializeComponent();
        _board = new Board(new Size(12, 12));
        _cellSize = new Size(CellSize, CellSize);
        _board.Reset();
        FitWindow();
        CreateCells();
    }

    private void FitWindow() {
        ClientSize = new Size(_board.Size.Width * CellSize + XOffset * 2, _board.Size.Height * CellSize + YOffset * 2);
    }

    private void CreateCells() {
        Controls.Clear();
        for (int y = 0; y < _board.Size.Height; y++) {
            for (int x = 0; x < _board.Size.Width; x++) {
                Button b = new() {
                    Size = _cellSize,
                    Tag = _board.Cells[ y, x ],
                    Location = new Point(x * CellSize + XOffset, y * CellSize + YOffset),
                    Font = new Font("Arial", 12, FontStyle.Bold)
                };
                b.MouseClick += Cell_Click;
                Controls.Add(b);
            }
        }
    }

    private void Cell_Click(object? sender, MouseEventArgs e) {
        if (sender is not Button b) return;
        if (b.Tag is not Cell c) return;

        switch (e.Button) {
            case MouseButtons.Right:
                // Add more logic to turn a flag into a question
                c.Flagged = !c.Flagged;
                DrawBoard();
                return;
            case MouseButtons.Left:
                SelectCell(c);
                return;
            case MouseButtons.None:
                break;
            case MouseButtons.Middle:
                break;
            case MouseButtons.XButton1:
                break;
            case MouseButtons.XButton2:
                break;
            default:
                return;
        }
    }

    /// <summary>
    /// Inputs the user's move and marks a cell as visited
    /// </summary>
    /// <param name="cell">The cell to mark</param>
    private void SelectCell(Cell cell) {
        if (cell.Flagged) return;

        if (cell.LiveBomb) {
            _board.Cells.VisitBombs();
            DrawBoard();
            GameOver();
            return;
        }

        // Automatically mark all the cells around this one as visited if neighbors are less than 2
        // This is a recursive function
        _board.VisitNeighbors(cell);


        cell.Visited = true;
        DrawBoard();
    }

    private void DrawBoard() {
        foreach(Button b in Controls)
        {
            if (b.Tag is not Cell c) continue;
            if (!c.Visited) continue;
            b.Text = c.LiveNeighbors.ToString();
            b.BackColor = Color.LightGray;
        }
    }

    /// <summary>
    /// Game Over, Sorry!
    /// </summary>
    private void GameOver()
    {
        Text = @"Game Over";
        MessageBox.Show(@"Game Over", @"Oh no!", MessageBoxButtons.OK, MessageBoxIcon.Error);
        _board.Reset();
        CreateCells();
    }
}