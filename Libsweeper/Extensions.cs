using System.Drawing;

namespace Libsweeper
{
    /// <summary>
    /// A class of extensions for Minesweeper Console Game
    /// </summary>
    public static class Extensions {
        /// <summary>
        /// Is the instantiated <see cref="string"/> object empty?
        /// </summary>
        /// <param name="str">The extending <see cref="string"/></param>
        /// <returns>Whether or not <see cref="str"/> is empty</returns>
        public static bool IsEmpty(this string str) => string.IsNullOrEmpty(str) || string.IsNullOrWhiteSpace(str);


        /// <summary>
        /// Reveals the Bombs on the board
        /// </summary>
        /// <param name="cells">The 2-Dimensional array of <see cref="Cell"/></param>
        public static void VisitBombs(this Cell[,] cells) {
            for (int row = 0; row < cells.GetLength(0); row++) {
                for (int col = 0; col < cells.GetLength(1); col++) {
                    if (cells[ row, col ].Visited) continue;
                    cells[ row, col ].Visited = cells[ row, col ].LiveBomb;
                }
            }
        }

        /// <summary>
        /// Takes the <see cref="Size"/> and returns the <see cref="Size.Width"/> * <see cref="Size.Height"/>
        /// </summary>
        /// <param name="size">The <see cref="Size"/> structure</param>
        /// <returns></returns>
        public static int MultiplySize(this Size size) => size.Width * size.Height;
    }
}
