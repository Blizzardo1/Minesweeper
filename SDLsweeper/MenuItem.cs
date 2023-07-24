using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SDL2;
using static SDL2.SDL;

namespace SDLsweeper
{
    internal class MenuItem : Button
    {
        public Action? Action { get;  init; }
        public List<MenuItem> Children { get; set; } = new();

        private bool _visible;
        public bool Visible
        {
            get => _visible;
            set => _visible = value;
        }

        public bool Parent { get; set; }

        /// <inheritdoc />
        public MenuItem(IntPtr rendererPtr) : base(rendererPtr) {
            TextPosition = new SDL2.Point { X = 4, Y = 0 };
            BackgroundColor = new SDL2.Color { R = 240, G = 240, B = 240, A = 255 };
            HighlightColor = new SDL2.Color { R = 255, G = 255, B = 255, A = 255 };
            ForegroundColor = new SDL2.Color { R = 0, G = 0, B = 0, A = 255 };
            Flat = true;
            Click += OnClick;
        }

        private void OnClick(object? sender, MouseButtonEvent e) {
            _visible = !_visible;
            Action?.Invoke();
        }

        #region Overrides of Button

        /// <inheritdoc />
        public override void Draw() {
            if (!_visible && !Parent) return;
            base.Draw();
            if (Children.Count <= 0 && !_visible) return;
            var rect = new Rect { X = X + 8, Y = Y + Height, W = MeasureString(Children.LongestString()).Width, H = Height };

            RenderText(Text, rect.X + 4, rect.Y, ForegroundColor);
            foreach (MenuItem menuItem in Children) {
                menuItem.Draw();
            }
        }

        /// <inheritdoc />
        public override void Update(Event e) {
            if (!_visible && !Parent) return;
            
            base.Update(e);
        }

        #endregion
    }
}
