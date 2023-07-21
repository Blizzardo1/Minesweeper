using Libsweeper;
using SDL2;
using SDL2.TTF;
using static SDL2.SDL;

namespace SDLsweeper;

internal class CellTile : Renderer, IGameObject {
    internal const int CellSize = 32;
    internal const int CellPadding = 2;
    internal const int XOffset = 10;
    internal const int YOffset = 100;


    public event EventHandler? Click;


    private readonly Cell _cell;
    private Rect _rect;

    public int X => _rect.X;
    public int Y => _rect.Y;

    public int Width => _rect.W;
    public int Height => _rect.H;

    /// <inheritdoc />
    public string Name => "Tile";

    public int Row => _cell.Row;
    public int Column => _cell.Column;

    public bool Highlight { get; set; }
    public Cell Cell => _cell;

    public SDL2.Color Color { get; set; } = new() { R = 32, G = 75, B = 128, A = 255 };
    public SDL2.Color HighlightColor { get; set; } = new() { R = 64, G = 150, B = 255, A = 255 };
    public SDL2.Color VisitedColor { get; set; } = new() { R = 64, G = 64, B = 64, A = 255 };
    public SDL2.Color FlaggedColor { get; set; } = new() { R = 255, G = 128, B = 0, A = 255 };
    public SDL2.Color BombColor { get; set; } = new() { R = 255, G = 150, B = 32, A = 255 };

    public SDL2.Color Foreground { get; } = new() { R = 255, G = 255, B = 255, A = 255 }; // White

    private SDL2.Color _selectedColor;
    private bool _inverse;

    /// <summary>
    /// Creates a new Cell Tile
    /// </summary>
    /// <param name="cell">A Reference to a Cell</param>
    /// <param name="rendererPtr">A pointer to a rendererPtr</param>
    public CellTile(Cell cell , IntPtr rendererPtr)
    {
        _cell = cell;
        Initialize(rendererPtr);
        _rect = new Rect { X = XOffset + _cell.Column * (CellSize + CellPadding), Y = YOffset + _cell.Row * (CellSize + CellPadding), W = CellSize, H = CellSize };
        _selectedColor = Color;
        _inverse = false;
    }

    public void OnClick(object? sender, MouseButtonEvent e) {
        if (_cell.Visited) return;
        
        switch (e.Button) {
            case (byte)MouseButton.Left:
                if(_cell.Flagged) break;
                _cell.Visited = true;
                
                break;
            case (byte)MouseButton.Right:
                _cell.Flagged = !_cell.Flagged;
                break;
        }

        Click?.Invoke(sender, new Event { Button = e});
    }

    #region Implementation of IGameObject

    /// <inheritdoc />
    public void Draw() {
        _ = SetRenderDrawColor(RendererPtr, _selectedColor.R, _selectedColor.G, _selectedColor.B, _selectedColor.A);

        // Draw Untouched Square
        _ = RenderFillRect(RendererPtr, ref _rect);
        _ = SetRenderDrawColor(RendererPtr, 255, 255, 255, 16);
        if (_inverse)
        {
            _ = RenderDrawLine(RendererPtr, _rect.X + _rect.W, _rect.Y + _rect.H, _rect.X, _rect.Y + _rect.H);
            _ = RenderDrawLine(RendererPtr, _rect.X + _rect.W, _rect.Y, _rect.X + _rect.W, _rect.Y + _rect.H);
            return;
        }

        _ = RenderDrawLine(RendererPtr, _rect.X, _rect.Y, _rect.X + _rect.W, _rect.Y);
        _ = RenderDrawLine(RendererPtr, _rect.X, _rect.Y, _rect.X, _rect.Y + _rect.H);
        
        if (_cell.Flagged)
        {
            // Draw Flag
        }

        if (_cell is not { Visited: true, LiveNeighbors: > 0 }) return;

        // Draw Number
        RenderText(_cell.LiveNeighbors.ToString(), _rect.X + 8, _rect.Y + 8, Foreground);

    }

    /// <inheritdoc />
    public void Update() {
        if (_cell.Visited)
        {
            // Ignore, already selected
            _selectedColor = VisitedColor;
            _inverse = true;
            return;
        }
        // Update Mouse Events here
        if (_cell.Flagged)
        {
            // If Flag, unflag or question mark
            _selectedColor = FlaggedColor;
            return;
        }
        if(_cell is {LiveBomb: true, Visited:true})
        {
            _selectedColor = BombColor;
            return;
        }
        


        // It's Untouched Square, Select
        _selectedColor = Highlight ? HighlightColor : Color;
    }

    #endregion
}