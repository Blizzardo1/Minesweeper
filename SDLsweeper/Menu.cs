using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SDL2;
using static SDL2.SDL;

namespace SDLsweeper
{
    internal class Menu : Renderer, IGameObject {
        private readonly List< MenuItem > _items;

        #region Implementation of IGameObject

        /// <inheritdoc />
        public int X => 0;

        /// <inheritdoc />
        public int Y => 0;

        /// <inheritdoc />
        public int Width
        {
            get => _rect.W;
            set => _rect.W = value;
        }

        /// <inheritdoc />
        public int Height
        {
            get => _rect.H;
            set => _rect.H = value;
        }

        private Rect _rect;

        /// <inheritdoc />
        public string Name { get; set; } = "Menu";

        public Menu(IntPtr rendererPtr){
            Initialize(rendererPtr);
            InitializeComponents();
            Height = 24;
            _items = new List< MenuItem >();
        }

        private void InitializeComponents() {
            IntPtr windowPtr = GetWindowFromID(Game.WindowId);
            GetWindowSize(windowPtr, out int width, out int _);
            Width = width;
        }

        public void AddMenuItem(string text, Action action) {
            var mi = new MenuItem(RendererPtr) { Text = text, Action = action, Height = Height - 1};
            mi.Click += MenuItem_Click;
            
            _items.Add(mi);
            for (int index = 0; index < _items.Count; index++) {
                MenuItem? i = _items[ index ];
                i.Width = MeasureString(mi.Text).Width + 8;
                i.X = index * i.Width;
            }

            _rect = _rect with { X = X, Y = Y };
        }

        private void MenuItem_Click(object? sender, MouseButtonEvent e) {
            if (sender is not MenuItem mi) return;
            mi.Action?.Invoke();
        }

        /// <inheritdoc />
        public void Draw() {
            _ = SetRenderDrawColor(RendererPtr, 240, 240, 240, 255); // White
            _ = RenderFillRect(RendererPtr, ref _rect);
            foreach (MenuItem item in _items)
            {
                item.Draw();
            }
            _ = SetRenderDrawColor(RendererPtr, 0, 0, 0, 255); // Black
            _ = RenderDrawLine(RendererPtr, X, Y + Height, Width, Y + Height);

            _ = SetRenderDrawColor(RendererPtr, 190, 190, 190, 255); // Black
            _ = RenderDrawLine(RendererPtr, X, Y + Height-1, Width, Y + Height-1);
        }

        /// <inheritdoc />
        public void Update(Event e) {
            
        }

        #endregion
    }
}
