using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Libsweeper;
using SDL2;
using Color = SDL2.Color;
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

        private List<string> _textList;

        private readonly Size _textSize;
        private readonly Color _background = new() { R = 190, G = 190, B = 190, A = 255 };

        private bool _shown;

        public Dialog(Window? owner, string? text, string? title)
            : base(
                title ?? "Error",
                new Point { X = 0x7FFFFFFF, Y = 0x7FFFFFFF},
                new Size(300,250),
                WindowFlags.Hidden)
        {
            AttachListeners();
            _owner = owner;
            _text = text;
            _textSize = MeasureString(_text ?? string.Empty);

            InitializeComponents();
        }

        private void InitializeComponents() {
            SDL.SetWindowSize(WindowPtr, _textSize.Width + 40, _textSize.Height + 120);
            UpdateSize(WindowPtr);

            _textList = _text?.Split('\n').ToList() ?? new List<string>();
        }

        public Dialog(string? text, string? title) : this(null, text, title) { }

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

            for(int i = 0; i < _textList.Count; i++) {
                RenderText(_textList[i], 10, 10 + i * 20, new Color { R = 0, G = 0, B = 0, A = 255 });
            }

            SDL.RenderPresent(RendererPtr);
        }

        public void Close() {
            _shown = false;
            SDL.DestroyRenderer(RendererPtr);
            SDL.DestroyWindow(WindowPtr);
        }

        public void ShowDialog(Window owner) {
            _shown = true;
            SDL.ShowWindow(WindowPtr);
            while (_shown)
            {
                _ = SDL.PollEvent(out Event e);
                
                // Completely separate from the main Event Engine. Until I figure something out...
                // Update: Using owner var to get event data, it no longer updates. It gets stuck because main event
                // handler has nothing to update. I need to figure out how to make it update. For now, ... this.
                // ~Adonis
                
                Update(e); // I wonder if this will break anything
                Draw();
                SDL.Delay(10);
            }
        }

        #endregion
    }
}
