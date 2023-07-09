using Libsweeper;
using SDL2;
using static SDL2.SDL;

namespace SDLsweeper;

internal class CellTile : IGameObject {
    internal const int CellSize = 32;
    internal const int CellPadding = 2;
    internal const int XOffset = 10;
    internal const int YOffset = 100;


    public event EventHandler? Click;


    private readonly Cell _cell;
    private readonly IntPtr _renderer;
    private Rect _rect;

    public int X => _rect.X;
    public int Y => _rect.Y;

    /// <inheritdoc />
    public string Name => "Tile";

    public int Row => _cell.Row;
    public int Column => _cell.Column;

    public bool Highlight { get; set; }

    public SDL2.Color Color { get; set; } = new() { R = 0, G = 128, B = 0, A = 255 };
    public SDL2.Color HighlightColor { get; set; } = new() { R = 0, G = 255, B = 0, A = 255 };
    public SDL2.Color VisitedColor { get; set; } = new() { R = 128, G = 128, B = 128, A = 255 };
    public SDL2.Color FlaggedColor { get; set; } = new() { R = 255, G = 128, B = 0, A = 255 };
    public SDL2.Color BombColor { get; set; } = new() { R = 255, G = 0, B = 0, A = 255 };

    private SDL2.Color _selectedColor;

    /// <summary>
    /// Creates a new Cell Tile
    /// </summary>
    /// <param name="cell">A Reference to a Cell</param>
    /// <param name="renderer">A pointer to a renderer</param>
    public CellTile(Cell cell , IntPtr renderer)
    {
        _cell = cell;
        _renderer = renderer;
        _rect = new Rect { X = XOffset + _cell.Column * (CellSize + CellPadding), Y = YOffset + _cell.Row * (CellSize + CellPadding), W = CellSize, H = CellSize };
        _selectedColor = Color;
    }

    public void OnClick(object? sender, MouseButtonEvent e) {
        switch (e.Button) {
            case (byte)MouseButton.Left:
                Click?.Invoke(this, new Event { Button = e });
                break;
            case (byte)MouseButton.Right:
                _cell.Flagged = !_cell.Flagged;
                break;
        }

        Click?.Invoke(sender, new Event() { Button = e});
    }

    #region Implementation of IGameObject

    /// <inheritdoc />
    public void Draw() {
        _ = SetRenderDrawColor(_renderer, _selectedColor.R, _selectedColor.G, _selectedColor.B, _selectedColor.A);
        if (_cell.Flagged)
        {
            // Draw Flag
        }

        if (_cell.Visited)
        {
            // Draw Number
            return;
        }

        // Draw Untouched Square
        _ = RenderFillRect(_renderer, ref _rect);
    }

    /// <inheritdoc />
    public void Update() {
        
        // Update Mouse Events here
        if (_cell.Flagged)
        {
            // If Flag, unflag or question mark
            _selectedColor = FlaggedColor;
            return;
        }
        
        if (_cell.Visited)
        {
            // Ignore, already selected
            _selectedColor = VisitedColor;
            return;
        }

        // It's Untouched Square, Select
        _selectedColor = Highlight ? HighlightColor : Color;
    }

    #endregion
}