using System.Drawing;
using Minesweeper;

namespace Libsweeper; 

/// <summary>
/// Minesweeper Game Board
/// </summary>
public class Board {


    private Size _size;
    private Cell[,] _cells;
    private double _difficulty;
    private int _bombCount;
    private int _safeCells;

    public Cell[,] Cells
    {
        get => _cells;
        set => _cells = value;
    }

    public Size Size
    {
        get => _size;
        set => _size = value;
    }

    /// <summary>
    /// Level of difficulty
    /// </summary>
    public double Difficulty
    {
        get => _difficulty;
        set => _difficulty = value > 0.9d ? 0.9d : Math.Round(value, 1) < 0.1 ? 0.1d : Math.Round(value, 1);
    }

    /// <summary>
    /// Instantiates a new Game Board
    /// </summary>
    /// <param name="size">The size of the board</param>
    /// <param name="difficulty">The difficulty of the game</param>
    public Board(Size size, double difficulty = 0.2) {
        _size = size;
        Difficulty = difficulty;
        _cells = new Cell[size.Width, size.Height];
    }

    /// <summary>
    /// Builds a new Board
    /// </summary>
    public void SetupLiveNeighbors()
    {
        var rand = new Random();
        int bombsGenerated = 0;
        for (int row = 0; row < _size.Width; row++)
        {
            for (int col = 0; col < _size.Height; col++) {
                double g = rand.NextDouble();
                bool bomb = g < _difficulty;
                bombsGenerated = bomb ? bombsGenerated + 1 : bombsGenerated;
                _cells[row, col] = new Cell(row, col, bomb);
            }
        }
        _bombCount = bombsGenerated;
        _safeCells = _size.MultiplySize() - _bombCount;
    }

    /// <summary>
    /// Calculate all the bombs "live neighbors" across the board.
    /// </summary>
    public void CalculateLiveNeighbors()
    {
        for (int row = 0; row < _size.Height; row++)
        {
            for (int column = 0; column < _size.Width; column++)
            {
                int liveNeighborCount = 0;

                if (_cells[row, column].LiveBomb)
                {
                    // If the cell itself is live, set the neighbor count to -1
                    // It's a bomb, made in a factory, a Bomb Factory.
                    _cells[row, column].LiveNeighbors = 9;
                    continue;
                }

                // Check the surrounding cells for live bombs
                for (int i = row - 1; i <= row + 1; i++)
                {
                    for (int j = column - 1; j <= column + 1; j++) {
                        if (i < 0 || i >= _size.Height || j < 0 || j >= _size.Width) continue;
                        if (!_cells[ i, j ].LiveBomb) continue;
                        liveNeighborCount++;
                    }
                }

                _cells[row, column].LiveNeighbors = liveNeighborCount;
            }
        }
    }



    /// <summary>
    /// Visits all the neighbors of a cell if the cell has less than (difficulty * 10) neighbors
    /// </summary>
    /// <param name="cell">The cell to check surrounding neighbors</param>
    public void VisitNeighbors(Cell cell) {
        // Live neighbors and difficulty Multiplier are the same
        if (cell.LiveNeighbors > (int)Math.Round(_difficulty * 10)) return;
        cell.Visited = true;
        for (int i = cell.Row - 1; i <= cell.Row + 1; i++) {
            for (int j = cell.Column - 1; j <= cell.Column + 1; j++) {
                if (i < 0 || i >= _size.Height || j < 0 || j >= _size.Width) continue;
                if (_cells[ i, j ].Visited) continue;
                VisitNeighbors(_cells[ i, j ]);
            }
        }
    }

    /// <summary>
    /// Resets the board.
    /// </summary>
    public void Reset() {
        _cells = null!;
        _cells = new Cell[_size.Width, _size.Height];
        SetupLiveNeighbors();
        CalculateLiveNeighbors();
    }

    /// <summary>
    /// Allows resize of the board
    /// </summary>
    /// <param name="newSize">The new size of the board.</param>
    public void Resize(Size newSize) {
        _size = newSize;
    }

       

    /// <summary>
    /// Checks if the game is over (all cells are visited)
    /// </summary>
    public bool CheckWin() {
        int winCheck = 0;
            
        for (int row = 0; row < _cells.GetLength(0); row++) {
            for(int col = 0; col < _cells.GetLength(1); col++) {
                winCheck += (_cells[ row, col ] is { Visited: false}) ? 1 : 0;
            }
        }

        Console.WriteLine("");
        return ( winCheck == _bombCount );
            
    }

    public void FlagCell(int row, int column) {
        if (_cells[row, column].Visited) return;
        _cells[row, column].Flagged = !_cells[row, column].Flagged;
    }
}