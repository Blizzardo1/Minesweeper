using System.ComponentModel;
using System.Net.Mime;
using Libsweeper;
using Minesweeper;
using Winsweeper.Properties;
using Timer = System.Windows.Forms.Timer;

namespace Winsweeper;

public partial class GameWindow : Form {
    private Board _board;
    private const int CellSize = 32;
    private const int XOffset = 10;
    private const int YOffset = 10;
    private const int CellPadding = 0;

    private readonly Size _cellSize;

    private Timer _timer;

    public GameWindow(int boardSize, int difficulty) {
        InitializeComponent();
        Closing += OnClosing;
        _timer = new Timer { Interval = 18 };
        _timer.Tick += TimerOnTick;
        _board = new Board(new Size(10 + difficulty*2, 10 + difficulty * 2), (double)difficulty / 10);
        _cellSize = new Size(CellSize, CellSize);
        _board.Reset();
        FitWindow();
        NewGame();
        _timer.Start();
    }

    private void TimerOnTick(object? sender, EventArgs e) {
        CheckWin();
    }

    private void OnClosing(object? sender, CancelEventArgs e) {
        Application.Exit(e);
    }

    private void FitWindow() {
        ClientSize = new Size(_board.Size.Width * CellSize + XOffset * 2, _board.Size.Height * CellSize + YOffset * 2);
    }

    private void CheckWin()
    {
        if (!_board.CheckWin()) return;
        _timer.Stop();
        new GameDialog(Resources.Cool, "You won!", "Congratulations!").ShowDialog(this);
        _board.Reset();
        NewGame();
        _timer.Start();
    }

    private void NewGame() {
        Text = "Minesweeper";
        Controls.Clear();
        List< Button > lst = new();

        for (int y = 0; y < _board.Size.Height; y++) {
            for (int x = 0; x < _board.Size.Width; x++) {
                Button b = new() {
                    Size = _cellSize,
                    BackColor = Color.White,
                    Tag = _board.Cells[ y, x ],
                    Location = new Point(x * CellSize + XOffset, y * CellSize + YOffset),
                    Font = new Font("Arial", 12, FontStyle.Bold)
                };
                b.MouseClick += Cell_Click;
                lst.Add(b);
            }
        }

        Controls.AddRange(lst.Cast< Control >().ToArray());
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
            if (!c.Visited)
            {
                b.Text = c.Flagged ? "F" : "";
                continue;
            }
            
            if (c.LiveBomb)
            {
                b.Text = "💣";
                b.BackColor = Color.Red;
                continue;
            }
            b.Text = c.LiveNeighbors is > 0 and < 9 ? c.LiveNeighbors.ToString() : "";
            b.BackColor = Color.LightGray;
        }
    }

    /// <summary>
    /// Game Over, Sorry!
    /// </summary>
    private void GameOver()
    {
        Text = @"Game Over";

        _timer.Stop();

        new GameDialog(Resources.Boom, "Oh no! You have just stepped on a mine!\nBUMMER!", "Game Over!").ShowDialog(this);
        _board.Reset();
        NewGame();

        _timer.Start();
    }
}