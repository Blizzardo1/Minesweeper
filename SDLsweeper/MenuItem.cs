using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SDL2;

namespace SDLsweeper
{
    internal class MenuItem : Button
    {
        public Action? Action { get;  init; }
        public List<MenuItem> Children { get; set; } = new();

        /// <inheritdoc />
        public MenuItem(IntPtr rendererPtr) : base(rendererPtr) {
            TextPosition = new SDL2.Point { X = 4, Y = 0 };
            BackgroundColor = new SDL2.Color { R = 240, G = 240, B = 240, A = 255 };
            ForegroundColor = new SDL2.Color { R = 0, G = 0, B = 0, A = 255 };
            Flat = true;
        }
    }
}
