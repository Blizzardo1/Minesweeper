using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Winsweeper.Properties;

namespace Winsweeper
{
    public partial class GameDialog : Form
    {

        private const int ImagePadding = 10;
        private const int Amplifier = 2;
        private Image _img;
        private string _text;
        private string _caption;
        private MessageBoxButtons _buttons;
        private MessageBoxIcon _icon;
        private DialogResult _result;

        public GameDialog() : this(Resources.Genius, "Hurp?", "What am I even doing here")
        {
        }

        public GameDialog(Image img, string text, string caption, MessageBoxButtons buttons = MessageBoxButtons.OK, MessageBoxIcon icon = MessageBoxIcon.None)
        {
            InitializeComponent();
            _img = img;
            _text = text;

            _caption = caption;
            _buttons = buttons;
            _icon = icon;
            _result = DialogResult.None;

            button1.Click += Button_Click;
            button2.Click += Button_Click;
            button3.Click += Button_Click;
            Design();
        }

        private void Button_Click(object? sender, EventArgs e)
        {
            if (sender is not Button b) return;
            DialogResult = Enum.Parse<DialogResult>(b.Text, true);
            Close();
        }

        public new DialogResult ShowDialog(IWin32Window window)
        {
            if (FromHandle(window.Handle) is not Form f) return ShowDialog();

            int wx = f.Location.X + (Width / 2 - f.Width / 2);      // f.Location.X + (f.Width - Width) / 2;
            int hy = f.Location.Y + (Height / 2 - f.Height / 2);  // f.Location.Y + ( f.Height - Height ) / 2;
            Location = new Point(wx, hy);
            return base.ShowDialog(window);
        }

        private void Design()
        {
            label1.Text = _text;
            pictureBox1.SizeMode = PictureBoxSizeMode.Zoom;
            pictureBox1.Image = _img;
            Text = _caption;
            switch (_buttons)
            {
                case MessageBoxButtons.OK:
                    button1.Text = @"OK";
                    button2.Visible = false;
                    button3.Visible = false;
                    break;
                case MessageBoxButtons.OKCancel:
                    button1.Text = @"OK";
                    button2.Text = @"Cancel";
                    button3.Visible = false;
                    break;
                case MessageBoxButtons.YesNo:
                    button1.Text = @"Yes";
                    button2.Text = @"No";
                    button3.Visible = false;
                    break;
                case MessageBoxButtons.YesNoCancel:
                    button1.Text = @"Yes";
                    button2.Text = @"No";
                    button3.Text = @"Cancel";
                    break;
                case MessageBoxButtons.RetryCancel:
                    button1.Text = @"Retry";
                    button2.Text = @"Cancel";
                    button3.Visible = false;
                    break;
                case MessageBoxButtons.AbortRetryIgnore:
                    button1.Text = @"Abort";
                    button2.Text = @"Retry";
                    button3.Text = @"Ignore";
                    break;
                case MessageBoxButtons.CancelTryContinue:
                    button1.Text = @"Cancel";
                    button2.Text = @"Try";
                    button3.Text = @"Continue";
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(_buttons), _buttons, null);
            }

            switch (_icon)
            {
                case MessageBoxIcon.None:
                    break;
                case MessageBoxIcon.Error:
                    break;
                case MessageBoxIcon.Question:
                    break;
                case MessageBoxIcon.Exclamation:
                    break;
                case MessageBoxIcon.Information:
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(_icon), _icon, null);
            }
        }
    }

}
