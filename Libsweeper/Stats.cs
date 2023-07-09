using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using Libsweeper.WinApi;

namespace Libsweeper {
    /// <summary>
    /// Container holding all stats on player records
    /// </summary>
    public static class Stats {
        private const string StatsFile = "stats.blob";

        private const byte Key = 170;
        

        private static readonly Encoding Encoding = Encoding.UTF8;

        // Bug: Public Property List<Player> exposes Clear and can unintentionally clear the stats to which after a save clears all scores.
        /// <summary>
        /// A list of player records obtained by completing a game
        /// </summary>
        public static List< Player > Players { get; set; }

        /// <summary>
        /// A list of player records sorted by Difficulty, Board Size, and Total time played
        /// </summary>
        public static IOrderedEnumerable<Player> PlayersOrdered =>
            Players
                .OrderByDescending(p => p.Difficulty * 10)
                .ThenByDescending(p => p.BoardSize.Width)
                .ThenBy(p => p.TotalTicks);

        static Stats() {
            Players = new List< Player >();
        }

        /// <summary>
        /// XOR a <see cref="byte"/> array
        /// </summary>
        /// <param name="b"></param>
        private static void Xor(ref byte[] b) {
            for (int i = 0; i < b.Length; i++) {
                byte a = b[ i ];
                a ^= Key;
                b[ i ] = a;
            }
        }

        /// <summary>
        /// Obtains the High Score <see cref="Player"/> record.
        /// </summary>
        /// <returns></returns>
        public static Player GetHighScore() {
            try {
                return PlayersOrdered.FirstOrDefault()!;
            }
            catch (Exception) {
                // You are HighScore, It should never reach here ;)
                SafeNativeMethods.MessageBox(IntPtr.Zero, "You have a high score!", "High Score", MessageBoxOptions.Ok,
                    MessageBoxIcon.Exclamation);

            }

            // This indicates that there was no real high scorer.
            return null!;
        }

        /// <summary>
        /// Load Statistics
        /// </summary>
        public static void Load() {
            // Reverse the Save!
            if (!File.Exists(StatsFile)) {
                Save();
            }

            byte[] b = File.ReadAllBytes(StatsFile);
            Xor(ref b);
            string json = Encoding.GetString(b);
            Players = JsonSerializer.Deserialize< Player[] >(json)?.ToList() ?? null!;
        }

        /// <summary>
        /// Save Statistics
        /// </summary>
        public static void Save() {
            // Encode in Json and save as a Binary XOrd Blob
            List< byte > bs = new();
            string json = JsonSerializer.Serialize(Players);

            bs.AddRange(Encoding.GetBytes(json));
            byte[] b = new byte[bs.Count];
            bs.CopyTo(b);
            Xor(ref b);

            File.WriteAllBytes(StatsFile, b);
        }

        /// <summary>
        /// Clear out the list of player records
        /// </summary>
        public static void Clear()
        {
            Players.Clear();
            Save();
        }
    }
}
