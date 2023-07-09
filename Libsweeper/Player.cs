using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Libsweeper {
    /// <summary>
    /// Represents a player of the game.
    /// </summary>
    public class Player : IComparable< Player >, ICloneable {
        
        public string Name { get; set; }
        public Size BoardSize { get; set; }
        public double Difficulty { get; set; }
        
        public string UniqueId { get; set; }
        public string TimeTaken { get; set; }
        public long TotalTicks { get; set; }

        /// <summary>
        /// Instantiates a new Player
        /// </summary>
        public Player() : this("", new Size(0,0), 0.1d, "") {
        }

        /// <summary>
        /// Instantiates a new Player
        /// </summary>
        /// <param name="name">The name of the player</param>
        /// <param name="boardSize">The Size of the board used</param>
        /// <param name="difficulty">The difficulty</param>
        /// <param name="timeTaken">The Player's final time</param>
        public Player(string name, Size boardSize, double difficulty, string timeTaken) {
            Name = name;
            TimeTaken = timeTaken;
            BoardSize = boardSize;
            Difficulty = difficulty;
            UniqueId = GenerateNewId();
        }

        /// <summary>
        /// Resets the Player.
        /// </summary>
        /// <remarks>This is useful for reusing objects after cloning</remarks>
        public void Reset()
        {
            TimeTaken = "";
            BoardSize = new Size(0, 0);
            Difficulty = 0;
            UniqueId = GenerateNewId();
        }

        /// <summary>
        /// Records the Time taken by the player
        /// </summary>
        public void RecordTime(TimeSpan time)
        {
            TotalTicks = time.Ticks;
            TimeTaken = time.ToString("g", CultureInfo.CurrentCulture);
        }


        /// <summary>
        /// Compare Scores between another player
        /// </summary>
        /// <param name="other"></param>
        /// <returns>0 if the same, 1 if TotalTicks is greater than the other.</returns>
        public int CompareTo(Player? other) {
            return TotalTicks.CompareTo(other?.TotalTicks);
        }

        /// <summary>
        /// Create a Unique ID that is used to identify the instance of the Player
        /// </summary>
        /// <returns></returns>
        private static string GenerateNewId() {
            return Guid.NewGuid().ToString("N");
        }

        #region Implementation of ICloneable

        /// <inheritdoc />
        public object Clone() {
            Player p = new() {
                Name = Name,
                BoardSize = BoardSize,
                Difficulty = Difficulty,
                UniqueId = UniqueId,
                TimeTaken = TimeTaken,
                TotalTicks = TotalTicks
            };
            return p;
        }

        #endregion
    }
}