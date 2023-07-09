using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Libsweeper;
using Winsweeper.Properties;

namespace Winsweeper
{
    public partial class CheatSheet : Form
    {

        private const int CellSize = 32;
        private const int XOffset = 10;
        private const int YOffset = 10;

        private readonly Size _cellSize;

        private Board _board;

        public CheatSheet(Board board)
        {
            InitializeComponent();
            _board = board;
            _cellSize = new Size(CellSize, CellSize);
            Location = new Point(-786, -12);
            Move += OnMove;
            NewGame();
        }

        private void OnMove(object? sender, EventArgs e)
        {
            Text = $"Cheat Sheet - {_board.Size.Width}x{_board.Size.Height} - {Location.X},{Location.Y}";
        }

        private void NewGame()
        {
            Text = "Minesweeper (CHEAT)";
            Controls.Clear();
            List<Button> lst = new();

            for (int y = 0; y < _board.Size.Height; y++)
            {
                for (int x = 0; x < _board.Size.Width; x++)
                {
                    Button b = new()
                    {
                        Size = _cellSize,
                        Text = _board.Cells[y, x].ToString(),
                        BackColor = _board.Cells[y, x].LiveBomb ? Color.Red : Color.White,
                        Tag = _board.Cells[y, x],
                        Location = new Point(x * CellSize + XOffset, y * CellSize + YOffset),
                        Font = new Font("Arial", 12, FontStyle.Bold)
                    };

                    lst.Add(b);
                }
            }

            Controls.AddRange(lst.Cast<Control>().ToArray());
        }
    }
}
