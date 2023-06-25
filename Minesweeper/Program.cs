using System.Drawing;
using System.Runtime.CompilerServices;
using Libsweeper;

namespace Minesweeper;
internal static class Program {
    private const int CellWidth = 4;
    private const int CellPadding = -1;

    private const ConsoleColor FrameColor = ConsoleColor.DarkGray;
    private const ConsoleColor CoordColor = ConsoleColor.Cyan;
    
    private const int MinimumBoardSize = 12;
    private const int MaximumBoardSize = 50;
    private const string GameName = "Toaster Network's Minesweeper";

    private static Board? _board;
    private static bool _firstMove;

    /// <summary>
    /// Displays the help menu
    /// </summary>
    private static void Help()
    {
        Console.WriteLine("\n#,#[,F] > Row, Column coordinates to choose a Cell, optionally flagging it");
        Console.WriteLine("C       > Decrease Difficulty by 1 and resets the game");
        Console.WriteLine("D       > Increase Difficulty by 1 and resets the game");
        Console.WriteLine("H       > This Help");
        Console.WriteLine("?       > This Help");
        Console.WriteLine("R       > Reset Board");
        Console.WriteLine("S       > Resize Board");
        Console.WriteLine("Q       > Exit");
        Console.Write("Press any key to continue...");
        Console.ReadKey(true);
    }

    /// <summary>
    /// Generates a Character row for the entire board
    /// </summary>
    /// <param name="length">The length of which to generate</param>
    /// <param name="even">Character for even cells</param>
    /// <param name="odd">Character for odd cells</param>
    /// <returns>An alternating row of characters</returns>
    private static string GenerateRow(int length, char even, char odd)
    {
        string str = "";

        for (int i = 0; i < length; i++)
        {
            str += i % CellWidth < CellWidth - 1 ? even : odd;
        }

        return str;
    }

    /// <summary>
    /// Prints with a specified foregroundColor
    /// </summary>
    /// <param name="obj">The <see cref="object"/> to print out</param>
    /// <param name="foregroundColor">The foreground <see cref="ConsoleColor"/>.</param>
    private static void Print(object obj, ConsoleColor foregroundColor = ConsoleColor.White)
    {
        Console.ForegroundColor = foregroundColor;
        Console.Write(obj);
        Console.ResetColor();
    }

    /// <summary>
    /// Prints the Board
    /// </summary>
    private static void PrintBoard()
    {
        Console.WriteLine($"Difficulty {_board!.Difficulty * 10}");

        Print(" "); // This is for the start of the Column Coordinate
        for (int c = 0; c < _board.Size.Width + 1; c++)
        {
            // A simple dirty hack, Create a space where there is no column present
            // for the game. Quick, dirty, and needs fixing so each column and row
            // number are tabular. That meaning the blank spot has no border.
            Print($"{(c == 0 ? $"  " : c - 1 > 9 ? $"{c - 1,1}" : $" {c - 1,-1}")} ", CoordColor);
            Print(" ", FrameColor);
        }

        Console.WriteLine();

        // Top Row Generation

        // Dirty hack; 
        for (int row = 0; row < _board.Size.Height; row++)
        {
            // Begin each row with a border line generated to conform to the physical board
            Print(row == 0
                ? $"    ╔{GenerateRow(_board.Size.Width * CellWidth + CellPadding, '═', '╦')}╗\n"
                : $"    ╠{GenerateRow(_board.Size.Width * CellWidth + CellPadding, '═', '╬')}╣\n", FrameColor);

            // The row hack; essentially, this will shift the numbers around
            // if greater than 9. 
            Print(" ", FrameColor);
            Print($"{(row > 9 ? $"{row} " : $" {row} ")}", CoordColor);

            for (int column = 0; column < _board.Size.Width; column++)
            {
                // Each Column has this for a starting character, then the cell data, then an ending char.
                Print("║ ", FrameColor);

                PrintCell(_board.Cells[row, column]);

                Print(" ");
            }
            Print("║\n", FrameColor);
        }

        // The bottom of the board frame
        Print($"    ╚{GenerateRow(_board.Size.Width * CellWidth + CellPadding, '═', '╩')}╝\n", FrameColor);
    }

    private static void PrintCell(Cell cell)
    {
        // We don't want to know where the bombs are physically,
        // we need to guess and if we happen to hit a bomb, game over.
        if (cell.Visited)
        {
            // Are we a live bomb?
            if (cell.LiveBomb)
            {
                if (cell.Flagged)
                    Print("O", ConsoleColor.Yellow);
                else
                    Print("×", ConsoleColor.Red);
                return;
            }

            ConsoleColor color = cell.LiveNeighbors switch
            {
                0 => ConsoleColor.DarkGray,
                1 => ConsoleColor.Blue,
                2 => ConsoleColor.Green,
                3 => ConsoleColor.Red,
                4 => ConsoleColor.DarkBlue,
                5 => ConsoleColor.DarkRed,
                6 => ConsoleColor.Cyan,
                7 => ConsoleColor.Magenta,
                8 => ConsoleColor.Yellow,
                _ => ConsoleColor.White
            };
            Print(cell, color); // Display live neighbor count

            return;
        }

        if (cell.Flagged)
        {
            Print("F", ConsoleColor.DarkMagenta); // Display flagged cell
        }
        else
        {
            Print("?"); // Display empty cell
        }
    }

    /// <summary>
    /// Attempts to resize the board
    /// </summary>
    /// <exception cref="ArgumentOutOfRangeException">Thrown when the board size constraint exceeds</exception>
    private static void BeginResize()
    {
        Console.Write($"Enter the new size of the board between {MinimumBoardSize} and {MaximumBoardSize}> ");
        try {
            int nSize = int.Parse(Console.ReadLine()!);
            if(nSize is < MinimumBoardSize or > MaximumBoardSize) {
                throw new ArgumentOutOfRangeException(null, nSize, $"Must be between {MinimumBoardSize} and {MaximumBoardSize}");
            }

            _board!.Resize(new Size(nSize, nSize));
            _board.Reset();
        }catch (Exception ex)
        {
            Console.WriteLine($"Not a valid selection; {ex.Message}");
            Thread.Sleep(2000);
        }
    }

    /// <summary>
    /// Inputs the user's move and marks a cell as visited
    /// </summary>
    /// <param name="row">The Row</param>
    /// <param name="column">The Column</param>
    private static void SelectCell(int row, int column)
    {
        Cell c = _board!.Cells[row, column];
        
        if (c.Flagged) return;

        if (c.LiveBomb)
        {
            Console.WriteLine("You hit a bomb!");
            _board.Cells.VisitBombs();
            PrintBoard();
            GameOver();
            return;
        }

        // Automatically mark all the cells around this one as visited if neighbors are less than 2
        // This is a recursive function
        _board.VisitNeighbors(c);


        _board.Cells[row, column].Visited = true;
    }


    /// <summary>
    /// Game Over, Sorry!
    /// </summary>
    private static void GameOver()
    {
        Console.Title = "Game Over";
        Console.Write("Press any key to start a new game...");
        Console.ReadKey(true);
        _board!.Reset();
    }

    /// <summary>
    /// Main Entry Point of the Program
    /// </summary>
    /// <exception cref="ArgumentException">Soft Exception handled within loop, marks as invalid selection</exception>
    private static void Main() {
        Console.Title = GameName;

        Console.CursorVisible = false;
        _board = new Board(new Size(MinimumBoardSize, MinimumBoardSize));
        _board.Reset();
            
        while (true)
        {
            Console.Title = GameName;
            // Do loop stuff here
            if(_board.CheckWin())
            {
                Console.WriteLine(Faces.Cool);
                Console.WriteLine("You win!");
                GameOver();
            }
            Console.Clear();

            if (!_firstMove)
            {
                Console.WriteLine("Welcome to Minesweeper!");
                Console.WriteLine("Press H or ? for help!");
                _firstMove = true;
            }
                
            PrintBoard();
            Console.Write("Input> ");
            string request = Console.ReadLine()!;
            if(request.IsEmpty()) continue;
            try {
                // To prevent a "feature", Grab the first two values
                // and either ignore the rest or be explicit and throw an error
                if (request.Contains(',')) {
                    // Might as well ignore the rest
                    string[] values = request.Split(',');
                    if (!int.TryParse(values[0], out int row) || !int.TryParse(values[1], out int column)) {
                        throw new ArgumentException("Must be row,column number coordinates.");
                    }
                    if (values.Length > 2  && values[2] is "F" or "f")
                    {
                        _board.FlagCell(row, column);
                        continue;
                    }
                        
                    SelectCell(row, column);
                    continue;
                }

                Action a = request.ToLower() switch {
                    "q" => () => Environment.Exit(0),
                    "r" => _board.Reset,
                    "s" => BeginResize,
                    "d" => () => { _board.Difficulty += 0.1;_board.Reset(); },
                    "c" => () => { _board.Difficulty -= 0.1;_board.Reset(); },
                    "h" or "?" => Help,
                    _ => () => throw new ArgumentException("Invalid selection.")
                };
                a();
                    
            }catch (Exception ex) {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine(ex.Message);
                Console.ResetColor();
                Thread.Sleep(1500);
            }
        }
    }
}