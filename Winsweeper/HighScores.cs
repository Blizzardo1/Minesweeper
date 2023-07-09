using System.Diagnostics;
using Libsweeper;
using Timer = System.Windows.Forms.Timer;

namespace Winsweeper
{
    public partial class HighScores : Form
    {
        private Timer _timer;

        public HighScores()
        {
            InitializeComponent();
            Font = new Font(Font.FontFamily, 16, FontStyle.Bold);
            DoubleBuffered = true;
            BuildStats();
            _timer = new Timer { Interval = 128 };
            _timer.Tick += _timer_Tick;
            _timer.Start();
        }

        private void _timer_Tick(object? sender, EventArgs e)
        {
            hPlayerLbl.Invalidate();
        }

        private void BuildStats()
        {
            // TODO: For now, leave it as it is. Eventually, we want to separate by Board Size and Difficulty. This will be a pain.
            // Order by Difficulty, then by BoardSize, lastly, by the amount of ticks it took to win
            IOrderedEnumerable<Player> players = Stats.PlayersOrdered;

            Player? highScorePlayer = players.FirstOrDefault();
            if (highScorePlayer is null)
            {
                MessageBox.Show(@"There are no players");
                return;
            }

            Size boardSize = highScorePlayer?.BoardSize ?? new Size(0, 0);
            hPlayerLbl.Text =
                $@"{highScorePlayer?.Name} (Board Size: {boardSize.Width}x{boardSize.Height} - Difficulty: {((Difficulty)(int)(highScorePlayer!.Difficulty * 10)).GetDescription()}) Time: {highScorePlayer?.TimeTaken}";

            foreach (Player? player in players.Skip(1).Take(5))
            {
                var label = new Label();
                label.Text = $@"{player.Name} (Board Size: {player.BoardSize.Width}x{player.BoardSize.Height} - Difficulty: {((Difficulty)(int)(player.Difficulty * 10)).GetDescription()}) Time: {player.TimeTaken}";
                label.AutoSize = true;
                scoresFlp.Controls.Add(label);
            }
        }

        private void HighScores_Load(object sender, EventArgs e)
        {

        }

        private void clearBtn_Click(object sender, EventArgs e)
        {
            Stats.Clear();
            BuildStats();
        }
    }
}
