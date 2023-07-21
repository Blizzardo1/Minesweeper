using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Libsweeper;
using SDL2;
using Point = SDL2.Point;

namespace SDLsweeper
{
    internal class Dialog : Window
    {
        #region Implementation of IGameObject

        /// <inheritdoc />
        public override string Name => "Dialog";

        private readonly Window? _owner;
        private readonly string? _text;
        private readonly SDL2.Color _background = new() { R = 190, G = 190, B = 190, A = 255 };

        private bool _shown;

        public Dialog(Window? owner, string? text, string title)
            : base(
                title,
                new Point { X = 0x7FFFFFFF, Y = 0x7FFFFFFF},
                new Size(0,0),
                WindowFlags.Hidden)
        {
            AttachListeners();
            _owner = owner;
            _text = text;
            
            Initialize(SDL.CreateRenderer(WindowPtr, -1, RendererFlags.Accelerated));
        }

        public Dialog(string? text, string title) : this(null, text, title) { }

        private void AttachListeners() {
            Quit += OnQuit;
            WindowEvent += OnWindowEvent;
        }

        private void OnWindowEvent(object? sender, WindowEvent e) {
            try {
                switch (e.Event) {
                    case WindowEventID.None: break;
                    case WindowEventID.Shown: break;
                    case WindowEventID.Hidden: break;
                    case WindowEventID.Exposed: break;
                    case WindowEventID.Moved: break;
                    case WindowEventID.Resized: break;
                    case WindowEventID.SizeChanged: break;
                    case WindowEventID.Minimized: break;
                    case WindowEventID.Maximized: break;
                    case WindowEventID.Restored: break;
                    case WindowEventID.Enter: break;
                    case WindowEventID.Leave: break;
                    case WindowEventID.FocusGained: break;
                    case WindowEventID.FocusLost: break;
                    case WindowEventID.Close:
                        Close();
                        break;
                    case WindowEventID.TakeFocus: break;
                    case WindowEventID.HitTest: break;
                    case WindowEventID.ICCProfileChanged: break;
                    case WindowEventID.DisplayChanged: break;
                    default:
                        throw new ArgumentOutOfRangeException(e.Event.ToString());
                }
            }
            catch (ArgumentOutOfRangeException exception)
            {
                Debug.Write(exception);
                throw;
            }
        }

        private void OnQuit(object? sender, QuitEvent e) {
            Close();
        }

        /// <inheritdoc />
        public override void Draw() {
            _ = SDL.SetRenderDrawColor(RendererPtr, _background.R, _background.G, _background.B, _background.A);
            _ = SDL.RenderClear(RendererPtr);
            RenderText(_text, 10, 10, new() { R = 0, G = 0, B = 0, A = 255 });
        }

        public void Close() {
            _shown = false;
            SDL.DestroyRenderer(RendererPtr);
            SDL.DestroyWindow(WindowPtr);
        }

        public void ShowDialog() {
            _shown = true;
            SDL.ShowWindow(WindowPtr);
            while (_shown)
            {
                Update();
                Draw();
                SDL.Delay(10);
            }
        }

        #endregion
    }
}
