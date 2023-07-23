using SDL2.TTF;
using SDL2;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Color = SDL2.Color;
using Font = SDL2.TTF.Font;

namespace SDLsweeper {
    internal abstract class Renderer {
        protected IntPtr RendererPtr;
        protected Font Font;

        private const int FontSize = 18;

        /// <summary>
        /// Initializes the Rendering Engine for a class
        /// </summary>
        /// <param name="renderer">The renderer to pass through</param>
        public void Initialize(IntPtr renderer) {
            Font = TTF.OpenFont("C:\\Windows\\Fonts\\Arial.ttf", FontSize);
            RendererPtr = renderer;
        }

        /// <summary>
        /// Draws a string to the screen
        /// </summary>
        /// <param name="text">The text to be drawn</param>
        /// <param name="x">Absolute X Coordinate</param>
        /// <param name="y">Absolute Y Coordinate</param>
        /// <param name="color">The BackgroundColor to use</param>
        public void RenderText(string? text, int x, int y, Color color) {
            IntPtr surface = TTF.RenderTextSolid(Font, text, color);
            IntPtr texture = SDL.CreateTextureFromSurface(RendererPtr, surface);
            _ = SDL.QueryTexture(
                texture,
                out _,
                out _,
                out int textureWidth,
                out int textureHeight
            );
            var rect = new Rect { X = x, Y = y, W = textureWidth, H = textureHeight };
            _ = SDL.RenderCopy(RendererPtr, texture, IntPtr.Zero, ref rect);

            SDL.FreeSurface(surface);
            SDL.DestroyTexture(texture);
        }

        protected Size MeasureString(string text)
        {
            _ = TTF.SizeText(Font, text, out int width, out int height);
            return new Size { Width = width, Height = height };
        }
    }
}
