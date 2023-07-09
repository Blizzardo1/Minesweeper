using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Libsweeper;

namespace Winsweeper
{
    /// <summary>
    /// Represents the Difficulty of the game
    /// </summary>
    public enum Difficulty
    {
        [Description("Baby")] Beginner = 1,
        [Description("Easy")] Easy = 2,
        [Description("Moderate")] Moderate = 4,
        [Description("Difficult")] Difficult = 7,
        [Description("God Help You")] GodHelpYou = 9
    }

    /// <summary>
    /// Represents the Board Size
    /// </summary>
    public enum BoardSize
    {
        [Description("Small")] Small = 1,
        [Description("Medium")] Medium = 2,
        [Description("Large")] Large = 3,
        [Description("Extra Large")] ExtraLarge = 4
    }

    public partial class DecisionMaker : Form
    {
        private Difficulty _difficulty;
        private BoardSize _boardSize;

        /// <summary>
        /// Instantiates the <see cref="DecisionMaker"/> Form
        /// </summary>
        public DecisionMaker()
        {
            InitializeComponent();
            _difficulty = Difficulty.Beginner;
            _boardSize = BoardSize.Small;
            playerNameTxt.Text = Stats.PlayersOrdered.FirstOrDefault()?.Name ?? "Player";
            BuildDecisions();
            BuildBoardSize();
        }

        /// <summary>
        /// Builds a table of <see cref="RadioButton"/>s for the <see cref="Difficulty"/> enum
        /// </summary>
        private void BuildDecisions()
        {
            // Get the Descriptions from the Difficulty Enum
            // Map Values and Descriptions to a Dictionary<int,string>

            Dictionary<int, string>? descriptions = Enum.GetValues<Difficulty>()
                .Select(n => new { Value = (int)n, Description = n.GetDescription() })
                .ToDictionary(x => x.Value, x => x.Description);
            foreach (RadioButton? radio in descriptions.Select(kp => new RadioButton
            {
                Text = kp.Value,
                Tag = kp.Key,
            }))
            {
                radio.CheckedChanged += Radio_CheckedChanged;
                decisionFlp.Controls.Add(radio);
            }
            ((RadioButton)decisionFlp.Controls[0]).Checked = true;
        }

        /// <summary>
        /// Builds a table of <see cref="RadioButton"/>s for the Board Size
        /// </summary>
        private void BuildBoardSize()
        {
            // Get the Descriptions from the Difficulty Enum
            // Map Values and Descriptions to a Dictionary<int,string>

            Dictionary<int, string>? descriptions = Enum.GetValues<BoardSize>()
                .Select(n => new { Value = (int)n, Description = n.GetDescription() })
                .ToDictionary(x => x.Value, x => x.Description);
            foreach (RadioButton? radio in descriptions.Select(kp => new RadioButton
            {
                Text = kp.Value,
                Tag = kp.Key,
            }))
            {
                radio.CheckedChanged += BoardSizeOnChanged;
                boardSizeFlp.Controls.Add(radio);
            }
            ((RadioButton)boardSizeFlp.Controls[0]).Checked = true;
        }

        /// <summary>
        /// Updates the Board Size based on the selected radio button
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void BoardSizeOnChanged(object? sender, EventArgs e)
        {
            if (sender is not RadioButton radio) return;
            if (radio.Checked)
            {
                _boardSize = (BoardSize)radio.Tag;
            }
        }

        /// <summary>
        /// Updates the Difficulty based on the selected radio button
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Radio_CheckedChanged(object? sender, EventArgs e)
        {
            if (sender is not RadioButton r) return;
            levelChooserNud.Value = (int)r.Tag;
        }

        /// <summary>
        /// GO!
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        /// <exception cref="ArgumentOutOfRangeException"></exception>
        private void goBtn_Click(object sender, EventArgs e)
        {

            int boardSize = _boardSize switch
            {
                BoardSize.Small => 8,
                BoardSize.Medium => 12,
                BoardSize.Large => 16,
                BoardSize.ExtraLarge => 20,
                _ => throw new ArgumentOutOfRangeException(nameof(_boardSize), _boardSize, "Board Size out of Range")
            };

            new GameWindow(playerNameTxt.Text, boardSize, (int)levelChooserNud.Value).Show(this);
            Hide();
        }

        /// <summary>
        /// Show the High Score Sheet
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void hScoreBtn_Click(object sender, EventArgs e)
        {
            new HighScores().ShowDialog(this);
        }
    }
}
