using Libsweeper;
using SDL2;
using SDL2.TTF;
using static SDL2.SDL;

namespace SDLsweeper;

internal class CellTile : Button {
    internal const int CellSize = 32;
    internal const int CellPadding = 2;
    internal const int XOffset = 10;
    internal const int YOffset = 100;

    private readonly Cell _cell;
    private Rect _rect;
    
    public new string Name => "Tile";

    public int Row => _cell.Row;
    public int Column => _cell.Column;

    public Cell Cell => _cell;
    
    public SDL2.Color VisitedColor { get; set; } = new() { R = 64, G = 64, B = 64, A = 255 };
    public SDL2.Color FlaggedColor { get; set; } = new() { R = 255, G = 128, B = 0, A = 255 };
    public SDL2.Color BombColor { get; set; } = new() { R = 255, G = 150, B = 32, A = 255 };

    private SDL2.Color _selectedColor;
    private bool _inverse;

    /// <summary>
    /// Creates a new Cell Tile
    /// </summary>
    /// <param name="cell">A Reference to a Cell</param>
    /// <param name="rendererPtr">A pointer to a rendererPtr</param>
    public CellTile(Cell cell, IntPtr rendererPtr) : base(rendererPtr) {
        _cell = cell;
        _rect = new Rect {
            X = XOffset + _cell.Column * ( CellSize + CellPadding ),
            Y = YOffset + _cell.Row * ( CellSize + CellPadding ), W = CellSize, H = CellSize
        };
    }

    #region Implementation of IGameObject

    /// <inheritdoc />
    public override void Draw() {
        _ = SetRenderDrawColor(RendererPtr, _selectedColor.R, _selectedColor.G, _selectedColor.B, _selectedColor.A);

        // Draw Untouched Square
        _ = RenderFillRect(RendererPtr, ref _rect);
        _ = SetRenderDrawColor(RendererPtr, 255, 255, 255, 16);
        if (_inverse) {
            _ = RenderDrawLine(RendererPtr, _rect.X + _rect.W, _rect.Y + _rect.H, _rect.X, _rect.Y + _rect.H);
            _ = RenderDrawLine(RendererPtr, _rect.X + _rect.W, _rect.Y, _rect.X + _rect.W, _rect.Y + _rect.H);
        }
        else {
            _ = RenderDrawLine(RendererPtr, _rect.X, _rect.Y, _rect.X + _rect.W, _rect.Y);
            _ = RenderDrawLine(RendererPtr, _rect.X, _rect.Y, _rect.X, _rect.Y + _rect.H);
        }

        if (_cell.Flagged) {
            // Draw Flag
        }

        if (_cell is not { Visited: true, LiveNeighbors: > 0 }) return;

        // Draw Number
        RenderText(_cell.LiveNeighbors.ToString(), _rect.X + 10, _rect.Y + 6, ForegroundColor);

    }

    /// <inheritdoc />
    public override void Update(Event e) {
        base.Update(e);

        if (_cell.Visited) {
            // Ignore, already selected
            _selectedColor = VisitedColor;
            _inverse = true;
            return;
        }

        // Update Mouse Events here
        if (_cell.Flagged) {
            // If Flag, unflag or question mark
            _selectedColor = FlaggedColor;
            return;
        }

        if (_cell is { LiveBomb: true, Visited: true }) {
            _selectedColor = BombColor;
            return;
        }

        // It's Untouched Square, Select
        _selectedColor = Highlight ? HighlightColor : BackgroundColor;
    }

    #endregion
}