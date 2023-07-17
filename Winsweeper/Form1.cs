using System.ComponentModel;
using System.Net.Mime;
using Libsweeper;
using Winsweeper.Properties;
using Timer = System.Windows.Forms.Timer;

namespace Winsweeper;


// TODO: Cleanup this file

public partial class GameWindow : Form
{
    private Board _board;
    private const int CellSize = 32;
    private const int XOffset = 10;
    private const int YOffset = 10;
    private const int CellPadding = 0;

    private readonly Size _cellSize;

    private Timer _timer;

    private readonly string _playerName;
    private Player? _player;
    private CheatSheet? _cs;

    /// <summary>
    /// Instantiates the <see cref="GameWindow"/> Form
    /// </summary>
    /// <param name="playerName"></param>
    /// <param name="boardSize"></param>
    /// <param name="difficulty"></param>
    public GameWindow(string playerName, int boardSize, int difficulty)
    {
        InitializeComponent();
        _playerName = playerName.IsEmpty() ? "Player" : playerName;
        Closing += OnClosing;
        _timer = new Timer { Interval = 18 };
        _timer.Tick += TimerOnTick;
        //_board = new Board(new Size(10 + difficulty * 2, 10 + difficulty * 2), (double)difficulty / 10);
        _board = new Board(new Size(boardSize, boardSize), (double)difficulty / 10);
        _cellSize = new Size(CellSize, CellSize);
        _board.Reset();
        FitWindow();
        NewGame();
        _timer.Start();

    }

    /// <summary>
    /// Handles the timing
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    private void TimerOnTick(object? sender, EventArgs e)
    {
        CheckWin();
    }

    /// <summary>
    /// Handle closing events
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    private void OnClosing(object? sender, CancelEventArgs e)
    {
        Application.Exit(e);
    }

    /// <summary>
    /// Resizes the window to fit the board
    /// </summary>
    private void FitWindow()
    {
        ClientSize = new Size(_board.Size.Width * CellSize + XOffset * 2, (_board.Size.Height * CellSize + YOffset * 2) + scoresBtn.Height + 5);
        scoresBtn.Location = new Point(ClientSize.Width / 2 - scoresBtn.Width / 2, 5);
    }

    /// <summary>
    /// Creates a new Game
    /// </summary>
    private void NewGame()
    {
        _board.Reset();
        _player ??= new Player(_playerName, _board.Size, _board.Difficulty, "");
        _cs?.Dispose();
        _cs = new CheatSheet(_board);
        _cs.ShowIcon = false;
        _cs.Show();
        Text = "Minesweeper";
        Controls.Clear();
        Controls.Add(scoresBtn);
        List<Button> lst = new();

        for (int y = 0; y < _board.Size.Height; y++)
        {
            for (int x = 0; x < _board.Size.Width; x++)
            {
                Button b = new()
                {
                    Size = _cellSize,
                    BackColor = Color.White,
                    BackgroundImage = Resources.Tile,
                    BackgroundImageLayout = ImageLayout.Zoom,
                    FlatStyle = FlatStyle.Flat,
                    FlatAppearance = { BorderSize = 0 },
                    Tag = _board.Cells[y, x],
                    Location = new Point(x * CellSize + XOffset, y * CellSize + YOffset + scoresBtn.Height + 5),
                    Font = new Font("Arial", 12, FontStyle.Bold)
                };
                b.MouseEnter += Cell_MouseEnter;
                b.MouseLeave += Cell_MouseLeave;
                b.MouseDown += Cell_Click;

                lst.Add(b);
            }
        }

        Controls.AddRange(lst.Cast<Control>().ToArray());
        _board.Stopwatch.Reset();
        _board.Stopwatch.Start();

    }

    private void Cell_MouseLeave(object? sender, EventArgs e) {
        if (sender is not Button b) return;
        if (b.Tag is not Cell c) return;
        if (c.Flagged || c.Visited) return;
        b.BackgroundImage = Resources.Tile;
    }

    private void Cell_MouseEnter(object? sender, EventArgs e)
    {
        if (sender is not Button b) return;
        if (b.Tag is not Cell c) return;
        if (c.Flagged || c.Visited) return;
        b.BackgroundImage = Resources.Tile.Lighter(0.4d);
    }

    /// <summary>
    /// Handles the click event of a cell
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    private void Cell_Click(object? sender, MouseEventArgs e)
    {
        if (sender is not Button b) return;
        if (b.Tag is not Cell c) return;
        switch (e.Button)
        {
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
    private void SelectCell(Cell cell)
    {
        if (cell.Flagged) return;

        if (cell.LiveBomb)
        {
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

    /// <summary>
    /// Iterates and draws the board
    /// </summary>
    private void DrawBoard()
    {
        foreach (Button b in Controls)
        {
            if (b.Tag is not Cell c) continue;
            if (!c.Visited)
            {
                b.BackgroundImage = c.Flagged ? Resources.Flag : Resources.Tile;
                continue;
            }

            b.BackgroundImage = null!;

            if (c.LiveBomb)
            {
                if (c.Flagged)
                {
                    b.BackgroundImage = Resources.Cool;
                    continue;
                }
                b.BackgroundImage = Resources.Bomb;

                b.BackColor = Color.Red;
                continue;
            }
            b.Text = c.LiveNeighbors is > 0 and < 9 ? c.LiveNeighbors.ToString() : "";
            b.BackColor = Color.LightGray;
        }
    }

    /// <summary>
    /// Checks if the player has won the game
    /// </summary>
    private void CheckWin()
    {
        if (!_board.CheckWin()) return;
        
        _player!.RecordTime(_board.Stopwatch.Elapsed);
        Stats.Players.Add((Player)_player.Clone());
        
        GameOverRoutine(new GameDialog(Resources.Cool,
            $"You won!\nElapsed Time: {_board.Stopwatch.Elapsed.Format()}", "Congratulations!"));
    }

    /// <summary>
    /// Game Over, Sorry!
    /// </summary>
    private void GameOver()
    {
        GameOverRoutine(
            new GameDialog(Resources.Boom,
                $"Oh no! You have just stepped on a mine!\nBUMMER!\nElapsed Time: {_board.Stopwatch.Elapsed.Format()}",
                "Game Over!"));
    }

    /// <summary>
    /// Runs through the end of the game sequences
    /// </summary>
    /// <param name="dialog"></param>
    private void GameOverRoutine(GameDialog dialog)
    {
        _board.Stopwatch.Stop();
        _timer.Stop();

        
        Stats.Save();
        Text = @"Game Over";

        dialog.ShowDialog(this);
        _board.Reset();
        NewGame();

        _timer.Start();

        dialog.Dispose();
    }

    /// <summary>
    /// Shows the high scores, also is a cool face
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    private void scoresBtn_Click(object sender, EventArgs e)
    {
        new HighScores { ShowIcon = false }.ShowDialog(this);
    }
}