using Libsweeper;
using SDL2;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static SDL2.SDL;
using Point = SDL2.Point;

namespace SDLsweeper
{
    internal class Button : Renderer, IGameObject
    {
        public event EventHandler<MouseButtonEvent> Click;
        public event EventHandler<MouseButtonEvent> RightClick;
        public event EventHandler< MouseMotionEvent > MouseEnter;
        public event EventHandler<MouseMotionEvent> MouseLeave;

        private Rect _rect;
        private bool _inverse;

        private bool _flat;

        public bool Flat
        {
            get => _flat;
            set => _flat = value;
        }

        public int X
        {
            get => _rect.X;
            set => _rect.X = value;
        }

        public int Y
        {
            get => _rect.Y;
            set => _rect.Y = value;
        }

        public int Width
        {
            get => _rect.W;
            set => _rect.W = value;
        }

        public int Height
        {
            get => _rect.H;
            set => _rect.H = value;
        }

        /// <inheritdoc />
        public string Name { get; set; } = "Button";

        public string Text { get; set; }

        public Point TextPosition { get; set; } = new Point { X = 10, Y = 6 };

        public bool Highlight { get; set; }


        private SDL2.Color _selectedColor;

        public SDL2.Color BackgroundColor { get; set; } = new() { R = 32, G = 75, B = 128, A = 255 };
        public SDL2.Color HighlightColor { get; set; } = new() { R = 64, G = 150, B = 255, A = 255 };
        public SDL2.Color ForegroundColor { get; set; } = new() { R = 255, G = 255, B = 255, A = 255 }; // White
        
        

        /// <summary>
        /// Creates a new Button
        /// </summary>
        /// <param name="rendererPtr">A pointer to a rendererPtr</param>
        public Button(IntPtr rendererPtr)
        {
            Initialize(rendererPtr);
            _rect = new Rect { X = 0, Y = 0, W = 75, H = 23};
            _inverse = false;
            // Cheap hacky way to preserve original background color
            _selectedColor = BackgroundColor;
        }

        public virtual void OnClick(object? sender, MouseButtonEvent e)
        {
            switch (e.Button)
            {
                case (int)MouseButton.Left:
                    Click?.Invoke(sender, e);
                    break;
                case (int)MouseButton.Right:
                    RightClick?.Invoke(sender, e);
                    break;
            }
        }

        public virtual void OnMouseEnter(object? sender, MouseMotionEvent e)
        {
            MouseEnter?.Invoke(sender, e);
        }

        public virtual void OnMouseLeave(object? sender, MouseMotionEvent e)
        {
            MouseLeave?.Invoke(sender, e);
        }

        #region Implementation of IGameObject

        /// <inheritdoc />
        public virtual void Draw()
        {
            _ = SetRenderDrawColor(
                RendererPtr,
                _selectedColor.R,
                _selectedColor.G,
                _selectedColor.B,
                _selectedColor.A
                );

            // Draw Untouched Square
            _ = RenderFillRect(RendererPtr, ref _rect);
            _ = SetRenderDrawColor(RendererPtr, 255, 255, 255, 16);
            if (!_flat || Highlight) {
                if (_inverse) {
                    _ = RenderDrawLine(
                        RendererPtr, X + Width, Y + Height, X, Y + Height);
                    _ = RenderDrawLine(
                        RendererPtr, X + Width, Y, X + Width, Y + Height);
                }
                else {
                    _ = RenderDrawLine(
                        RendererPtr, X, Y, X + Width, Y);
                    _ = RenderDrawLine(
                        RendererPtr, X, Y, X, Y + Height);
                }
            }

            // Draw Number
            RenderText(Text, X + TextPosition.X, Y + TextPosition.Y, ForegroundColor);

        }

        /// <inheritdoc />
        public virtual void Update(Event e)
        {

            if (e.Button is { Clicks: 1, Type: EventType.MouseButtonDown })
            {
                OnClick(this, e.Button);
            }

            if (e.Type is EventType.MouseMotion)
            {
                if (e.Motion.X >= X && e.Motion.X <= X + Width && e.Motion.Y >= Y && e.Motion.Y <= Y + Height)
                {
                    OnMouseEnter(this, e.Motion);
                    _selectedColor = HighlightColor;
                }
                else
                {
                    OnMouseLeave(this, e.Motion);
                    _selectedColor = BackgroundColor;
                }
            }

            // It's Untouched, Highlight or not
            _selectedColor = Highlight ? HighlightColor : BackgroundColor;
        }
        #endregion
    }
}
