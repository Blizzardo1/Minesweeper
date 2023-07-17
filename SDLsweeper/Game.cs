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
    public delegate void EventHandler<in T>(T? sender, Event e);
    public delegate void MouseMotionEventHandler(object? sender, MouseMotionEvent e);
    public delegate void MouseButtonEventHandler(object? sender, MouseButtonEvent e);

    internal class Game : IGameObject {

        public event EventHandler? Quit;
        public event MouseMotionEventHandler? MouseMotion;
        public event MouseButtonEventHandler? MouseButton;

        private const int LocationX = 100;
        private const int LocationY = 100;

        private Board _board;
        private Size _cellSize;

        private IntPtr _window;
        private IntPtr _renderer;

        private List< IGameObject > _tiles;
        
        private CellTile? _selectedTile;
        private int currentTileX = -1;
        private int currentTileY = -1;

        private Font _font;
        private const int FontSize = 18;


        private string _motionMessage;

        /// <summary>
        /// Creates a new Game
        /// </summary>
        /// <param name="size">The Size of the board</param>
        /// <param name="difficulty">The difficulty from 1 to 9</param>
        public Game(int size, int difficulty) {
            _ = SDL.Init(InitFlags.Everything);
            _ = TTF.Init();
            _font = TTF.OpenFont("C:\\Windows\\Fonts\\Arial.ttf", FontSize);
            
            _board = new Board(new Size(size, size), (double)difficulty / 10);
            _cellSize = new Size(CellTile.CellSize, CellTile.CellSize);
            _tiles = new List< IGameObject >();

            // f(x) = 2(XOffset) + _board.Size.Width(CellSize + CellPadding) - CellPadding
            int width = 2 * CellTile.XOffset + _board.Size.Width * ( CellTile.CellSize + CellTile.CellPadding ) -
                        CellTile.CellPadding;

            int height = CellTile.XOffset + CellTile.YOffset + _board.Size.Width * (CellTile.CellSize + CellTile.CellPadding) -
                        CellTile.CellPadding;

            _window = SDL.CreateWindow("Minesweeper", SDL.WINDOWPOS_CENTERED, SDL.WINDOWPOS_CENTERED, width, height,
                WindowFlags.AllowHighdpi | WindowFlags.Resizable | WindowFlags.Shown);

            if(_window == IntPtr.Zero) {
                throw new Exception("Cannot create Window");
            }
            
            _renderer = SDL.CreateRenderer(_window, -1, RendererFlags.Accelerated);

            if(_renderer == IntPtr.Zero) {
                throw new Exception("Cannot create Renderer");
            }

            _motionMessage = "";

            IsRunning = true;
        }

        public bool IsRunning { get; set; }

        public void Start()
        {
            _board.Reset();
            NewGame();
            //_timer.Start();
        }

        private void Background(Color color) {
            _ = SDL.SetRenderDrawColor(_renderer, color.R, color.G, color.B, color.A);
        }

        #region Implementation of IGameObject

        /// <inheritdoc />
        public int X { get; private set; } = 0;

        /// <inheritdoc />
        public int Y { get; private set; } = 0;

        /// <inheritdoc />
        public string Name => "Minesweeper";

        public void RenderText(string text, int x, int y, Color color)
        {
            IntPtr surface = TTF.RenderTextSolid(_font, text, color);
            IntPtr texture = SDL.CreateTextureFromSurface(_renderer, surface);
            _ = SDL.QueryTexture(texture, out _, out _, out int textureWidth, out int textureHeight);
            var rect = new Rect() { X = x, Y = y, W = textureWidth, H = textureHeight };
            _ = SDL.RenderCopy(_renderer, texture, IntPtr.Zero, ref rect);
            
            SDL.FreeSurface(surface);
            SDL.DestroyTexture(texture);
        }

        /// <inheritdoc />
        public void Draw() {
            Background(new Color { A = 255, B = 23, G = 23, R = 23 });
            _ = SDL.RenderClear(_renderer);
            foreach(CellTile? cell in _tiles.Cast<CellTile?>())
            {
                cell!.Draw();
            }

            RenderText(_motionMessage, 10, 10, new Color { A = 255, B = 255, G = 255, R = 255 });

            
            SDL.RenderPresent(_renderer);
        }

        #region Event Handler
        private void HandleEvents()
        {
            while (SDL.PollEvent(out Event e) >= 1)
            {
                switch (e.Type)
                {
                    case EventType.FirstEvent:
                        break;
                    case EventType.Quit:
                        OnQuit(this, e);
                        break;
                    case EventType.AppTerminating:
                        break;
                    case EventType.AppLowMemory:
                        break;
                    case EventType.AppWillEnterBackground:
                        break;
                    case EventType.AppDidEnterBackground:
                        // Do a pause
                        break;
                    case EventType.AppWillEnterForeground:
                        break;
                    case EventType.AppDidEnterForeground:
                        // Resume?
                        break;
                    case EventType.DisplayEvent:
                        break;
                    case EventType.WindowEvent:
                        break;
                    case EventType.SyswmEvent:
                        break;
                    case EventType.KeyDown:
                        break;
                    case EventType.KeyUp:
                        break;
                    case EventType.TextEditing:
                        break;
                    case EventType.TextInput:
                        break;
                    case EventType.KeymapChanged:
                        break;
                    case EventType.MouseMotion:
                        OnMouseMove(this, e.Motion);
                        break;
                    case EventType.MouseButtonDown:
                        OnMouseDown(this, e.Button);
                        break;
                    case EventType.MouseButtonUp:
                        OnMouseUp(this, e.Button);
                        break;
                    case EventType.MouseWheel:
                        break;
                    case EventType.JoyAxisMotion:
                        break;
                    case EventType.JoyBallMotion:
                        break;
                    case EventType.JoyHatMotion:
                        break;
                    case EventType.JoyButtonDown:
                        break;
                    case EventType.JoyButtonUp:
                        break;
                    case EventType.JoyDeviceAdded:
                        break;
                    case EventType.JoyDeviceRemoved:
                        break;
                    case EventType.ControllerAxisMotion:
                        break;
                    case EventType.ControllerButtonDown:
                        break;
                    case EventType.ControllerButtonUp:
                        break;
                    case EventType.ControllerDeviceAdded:
                        break;
                    case EventType.ControllerDeviceRemoved:
                        break;
                    case EventType.ControllerDeviceRemapped:
                        break;
                    case EventType.FingerDown:
                        break;
                    case EventType.FingerUp:
                        break;
                    case EventType.FingerMotion:
                        break;
                    case EventType.DollarGesture:
                        break;
                    case EventType.DollarRecord:
                        break;
                    case EventType.MultiGesture:
                        break;
                    case EventType.ClipboardUpdate:
                        break;
                    case EventType.DropFile:
                        break;
                    case EventType.DropText:
                        break;
                    case EventType.DropBegin:
                        break;
                    case EventType.DropComplete:
                        break;
                    case EventType.AudioDeviceAdded:
                        break;
                    case EventType.AudioDeviceRemoved:
                        break;
                    case EventType.SensorUpdate:
                        break;
                    case EventType.RenderTargetsReset:
                        break;
                    case EventType.RenderDeviceReset:
                        break;
                    case EventType.UserEvent:
                        break;
                    case EventType.LastEvent:
                        break;
                    default:
                        throw new ArgumentOutOfRangeException();
                }
            }
        }
        #endregion

        #region Event Methods
        /// <summary>
        /// Handles Quit Procedures
        /// </summary>
        public void OnQuit(object? sender, Event e)
        {
            Quit?.Invoke(sender, e);
            TTF.CloseFont(_font);
            TTF.Quit();

            SDL.DestroyRenderer(_renderer);
            SDL.DestroyWindow(_window);
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
            MouseMotion?.Invoke(sender, e);
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
                if (tX == currentTileX && tY == currentTileY) return;
                
                currentTileX = tX;
                currentTileY = tY;

                if(_selectedTile is not null)
                    _selectedTile!.Highlight = false;
                _selectedTile = (CellTile?)_tiles.FirstOrDefault(t => ((CellTile)t).Column == tX && ((CellTile)t).Row == tY);
                _selectedTile!.Highlight = true;

                _motionMessage = $"Tx,Ty {tX}, {tY}";

                return;
            }

            currentTileX = -1;
            currentTileY = -1;
        }

        /// <summary>
        /// Handles Mouse Down Events
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public void OnMouseDown(object? sender, MouseButtonEvent e)
        {
            MouseButton?.Invoke(sender, e);
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
            MouseButton?.Invoke(sender, e);
        }
        #endregion

        /// <inheritdoc />
        public void Update() {
            HandleEvents();

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
                    CellTile c = new(_board.Cells[y,x], _renderer);
                    _tiles.Add(c);
                }
            }
        }

        private void Cell_Click(object? sender, MouseEventArgs e)
        {
            if (sender is not Button b) return;
            if (b.Tag is not Cell c) return;
            switch (e.Button)
            {
                case MouseButtons.Right:
                    // Add more logic to turn a flag into a question
                    c.Flagged = !c.Flagged;
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
            //new GameDialog(Resources.Boom, "Oh no! You have just stepped on a mine!\nBUMMER!", "Game Over!").ShowDialog(this);
            _board.Reset();
            NewGame();

        }
    }
}
