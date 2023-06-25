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
    public partial class GameDialog : Form {
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
            Paint += OnPaint;
            button1.Click += Button_Click;
            button2.Click += Button_Click;
            button3.Click += Button_Click;
            Design();
        }

        private void Button_Click(object? sender, EventArgs e) {
            if (sender is not Button b) return;
            DialogResult = Enum.Parse<DialogResult>(b.Text, true);
            Close();
        }

        private void OnPaint(object? sender, PaintEventArgs e) {
            Graphics g = CreateGraphics();
            var size = g.MeasureString(_text, Font).ToSize();
            g.DrawImage(_img, new Point(Width / 2 - _img.Width / 2, 10));
            g.DrawString(_text, Font, Brushes.Black, new Point(Width / 2 - size.Width / 2, _img.Height + 10));
            
            g.Dispose();
        }

        private void Design()
        {
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
