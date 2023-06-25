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

namespace Winsweeper
{
    public enum Difficulty
    {
        [Description("Baby")]
        Beginner = 1,
        [Description("Easy")]
        Easy = 2,
        [Description("Moderate")]
        Moderate = 4,
        [Description("Difficult")]
        Difficult = 7,
        [Description("God Help You")]
        GodHelpYou = 9
    }
    public partial class DecisionMaker : Form
    {
        private Difficulty _difficulty;

        public DecisionMaker()
        {
            InitializeComponent();
            _difficulty = Difficulty.Beginner;
            BuildDecisions();
        }

        private void BuildDecisions() {
            // Get the Descriptions from the Difficulty Enum
            // Map Values and Descriptions to a Dictionary<int,string>

            Dictionary< int, string >? descriptions = Enum.GetValues< Difficulty >()
                .Select(n => new { Value = (int)n, Description = n.GetDescription() })
                .ToDictionary(x => x.Value, x => x.Description);
            foreach (RadioButton? radio in descriptions.Select(kp => new RadioButton {
                         Text = kp.Value,
                         Tag = kp.Key,
                     })) {
                radio.CheckedChanged += Radio_CheckedChanged;
                decisionFlp.Controls.Add(radio);
            }
        }

        private void Radio_CheckedChanged(object? sender, EventArgs e) {
            if (sender is not RadioButton r) return;
            levelChooserNud.Value = (int)r.Tag;
        }

        private void goBtn_Click(object sender, EventArgs e) {
            int boardSize = _difficulty switch {
                Difficulty.Beginner => 10,
                Difficulty.Easy => 11,
                Difficulty.Moderate => 12,
                Difficulty.Difficult => 13,
                Difficulty.GodHelpYou => 14,
                _ => throw new ArgumentOutOfRangeException(nameof(_difficulty), _difficulty, "Difficulty out of Range")
            };
            
            new GameWindow(boardSize, (int)levelChooserNud.Value).Show(this);
            Hide();
        }
    }
}
