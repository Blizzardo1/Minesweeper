using System.Diagnostics;
using Libsweeper;
using SDL2;
using SDL2.TTF;

using Color = SDL2.Color;
using Font = SDL2.TTF.Font;
using Point = SDL2.Point;
using Rect = SDL2.Rect;
using Timer = System.Timers.Timer;

namespace SDLsweeper
{
    public delegate void EventHandler(object? sender, Event e);
    public delegate void EventHandler<in T>(object? sender, T e);
    public delegate void MouseMotionEventHandler(object? sender, MouseMotionEvent e);
    public delegate void MouseButtonEventHandler(object? sender, MouseButtonEvent e);

    internal class Game : Window {
        

        private const int LocationX = 100;
        private const int LocationY = 100;

        private Board? _board;
        private Size _cellSize;
        private Size _size;         // Mutable
        private double _difficulty; // Mutable

        private List< IGameObject >? _tiles;
        
        private CellTile? _selectedTile;
        private int _currentTileX = -1;
        private int _currentTileY = -1;
        


        private string? _motionMessage;

        /// <summary>
        /// Creates a new Game
        /// </summary>
        /// <param name="size">The Size of the board</param>
        /// <param name="difficulty">The difficulty from 1 to 9</param>
        public Game(int size, int difficulty)
            : base(
                "Minesweeper",
                new Point { X = 0x7FFFFFFF, Y = 0x7FFFFFFF},
                new Size(300, 300),
                WindowFlags.AllowHighdpi | WindowFlags.Resizable | WindowFlags.Shown) {
            _size = new Size { Width = size, Height = size };
            _difficulty = difficulty / 10d;
            InitializeComponents();
            
            IsRunning = true;
        }

        /// <summary>
        /// Initializes all components of the Game Window
        /// </summary>
        /// <exception cref="Exception">Failure to create the Window and Renderer</exception>
        private void InitializeComponents() {
            AttachListeners();
            _ = SDL.Init(InitFlags.Everything);
            _ = TTF.Init();


            _board = new Board(_size, _difficulty);
            _cellSize = new Size(CellTile.CellSize, CellTile.CellSize);
            _tiles = new List<IGameObject>();

            // f(x) = 2(XOffset) + _board.Size.Width(CellSize + CellPadding) - CellPadding
            Width = 2 * CellTile.XOffset + _board.Size.Width * (CellTile.CellSize + CellTile.CellPadding) -
                        CellTile.CellPadding;

            Height = CellTile.XOffset + CellTile.YOffset + _board.Size.Width * (CellTile.CellSize + CellTile.CellPadding) -
                         CellTile.CellPadding;
            
            SDL.SetWindowSize(WindowPtr, Width, Height);

            _motionMessage = "";
        }
        
        private void AttachListeners() {
            FirstEvent += OnFirstEvent;
            Quit += OnQuit;
            MouseMove += OnMouseMove;
            MouseDown += OnMouseDown;
            MouseUp += OnMouseUp;
            WindowEvent += OnWindowEvent;
        }

        private void OnWindowEvent(object? sender, WindowEvent e) {
            try {
                switch (e.Event) {
                    case WindowEventID.None: break;
                    case WindowEventID.Shown: break;
                    case WindowEventID.Hidden: break;
                    case WindowEventID.Exposed: break;
                    case WindowEventID.Moved:
                        UpdatePosition(WindowPtr);
                        break;
                    case WindowEventID.Resized:
                        UpdateSize(WindowPtr);
                        break;
                    case WindowEventID.SizeChanged: break;
                    case WindowEventID.Minimized: break;
                    case WindowEventID.Maximized: break;
                    case WindowEventID.Restored: break;
                    case WindowEventID.Enter: break;
                    case WindowEventID.Leave: break;
                    case WindowEventID.FocusGained: break;
                    case WindowEventID.FocusLost: break;
                    case WindowEventID.Close: break; // Handled elsewhere
                    case WindowEventID.TakeFocus: break;
                    case WindowEventID.HitTest: break;
                    case WindowEventID.ICCProfileChanged: break;
                    case WindowEventID.DisplayChanged: break;
                    default:
                        throw new ArgumentOutOfRangeException(e.Event.ToString());
                }
            }
            catch (ArgumentOutOfRangeException exception) {
                Debug.WriteLine(exception);
            }
        }

        private void OnFirstEvent(object? sender, Event e) { }

        /// <summary>
        /// Whether or not the game is running
        /// </summary>
        public bool IsRunning { get; set; }

        /// <summary>
        /// Start the Game
        /// </summary>
        public void Start()
        {
            _board!.Reset();
            NewGame();
            //_timer.Start();
        }

        /// <summary>
        /// Set the Draw Color
        /// </summary>
        /// <param name="color"></param>
        private void SetColor(Color color) {
            _ = SDL.SetRenderDrawColor(RendererPtr, color.R, color.G, color.B, color.A);
        }

        #region Implementation of IGameObject

        /// <inheritdoc />
        public override string Name => "Minesweeper";

        /// <inheritdoc />
        public override void Draw() {
            SetColor(new Color { A = 255, B = 23, G = 23, R = 23 });
            _ = SDL.RenderClear(RendererPtr);
            foreach(CellTile? cell in _tiles!.Cast<CellTile?>())
            {
                cell!.Draw();
            }

            RenderText(_motionMessage, 10, 10, new Color { A = 255, B = 255, G = 255, R = 255 });

            
            SDL.RenderPresent(RendererPtr);
        }

 

        #region Event Methods
        /// <summary>
        /// Handles Quit Procedures
        /// </summary>
        public void OnQuit(object? sender, QuitEvent e)
        {
            TTF.CloseFont(Font);
            TTF.Quit();

            SDL.DestroyRenderer(RendererPtr);
            SDL.DestroyWindow(WindowPtr);
            SDL.Quit();
            IsRunning = false;
        }

        /// <summary>
        /// Handles Mouse Move Events
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public void OnMouseMove(object? sender, MouseMotionEvent e)
        {
            int mX = e.X;
            int mY = e.Y;

            int mtX = 2 * CellTile.XOffset + mX * (CellTile.CellSize + CellTile.CellPadding) -
                     CellTile.CellPadding;

            int mtY = CellTile.XOffset + CellTile.YOffset + mY * (CellTile.CellSize + CellTile.CellPadding) -
                     CellTile.CellPadding;

            int tX = (mX - CellTile.XOffset - CellTile.CellPadding) / CellTile.CellSize;
            int tY = (mY - CellTile.YOffset - CellTile.CellPadding) / CellTile.CellSize;
            X = mtX - CellTile.CellSize / 2;
            Y = mtX - CellTile.CellSize / 2;

            int nT = _board.Size.Width;

            if (tX >= 0 && tX < nT && tY >=0 && tY < nT)
            {
                if (tX == _currentTileX && tY == _currentTileY) return;
                
                _currentTileX = tX;
                _currentTileY = tY;

                if(_selectedTile is not null)
                    _selectedTile!.Highlight = false;
                _selectedTile = (CellTile?)_tiles.FirstOrDefault(t => ((CellTile)t).Column == tX && ((CellTile)t).Row == tY);
                _selectedTile!.Highlight = true;

                _motionMessage = $"Tx,Ty {tX}, {tY}";

                return;
            }

            _currentTileX = -1;
            _currentTileY = -1;
        }

        /// <summary>
        /// Handles Mouse Down Events
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public void OnMouseDown(object? sender, MouseButtonEvent e)
        {
            if(e.Clicks is 1)
            {
                _selectedTile?.OnClick(this, e);
            }
        }

        /// <summary>
        /// Handles Mouse Up Events
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public void OnMouseUp(object? sender, MouseButtonEvent e)
        {
            
        }
        #endregion

        /// <inheritdoc />
        public override void Update() {
            base.Update();

            foreach (CellTile? cell in _tiles.Cast< CellTile? >()) {
                cell?.Update();
            }

            CheckWin();
        }

        #endregion

        private void CheckWin()
        {
            if (!_board.CheckWin()) return;
            // new GameDialog(Resources.Cool, "You won!", "Congratulations!").ShowDialog(this);
            _board.Reset();
            NewGame();
        }

        private void NewGame()
        {
            // Text = "Minesweeper";
            // Clear Contents and restart
            _tiles.Clear();

            for (int y = 0; y < _board.Size.Height; y++)
            {
                for (int x = 0; x < _board.Size.Width; x++) {
                    CellTile c = new(_board.Cells[y,x], RendererPtr);
                    c.Click += Cell_Click;
                    _tiles.Add(c);
                }
            }
        }

        private void Cell_Click(object? sender, Event e) {
            if (_selectedTile?.Cell is { LiveBomb: true }) {
                GameOver();
            }

            _board.VisitNeighbors(_selectedTile?.Cell);
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
                GameOver();
                return;
            }

            // Automatically mark all the cells around this one as visited if neighbors are less than 2
            // This is a recursive function
            _board.VisitNeighbors(cell);


            cell.Visited = true;
        }

        /// <summary>
        /// Game Over, Sorry!
        /// </summary>
        private void GameOver()
        {
            // Game Over
            
            // Display Message
            new Dialog(this, "Oh no! You have just stepped on a mine!\nBUMMER!", "Game Over!").ShowDialog();
            _board.Reset();
            NewGame();

        }
    }
}
