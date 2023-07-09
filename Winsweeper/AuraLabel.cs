using System;
using System.Drawing;
using System.Windows.Forms;
namespace Winsweeper;

public class AuraLabel : Label
{
    private Color glowColor = Color.Yellow; // Color of the glow effect
    private int glowSize = 10; // Size of the glow effect

    protected override void OnPaint(PaintEventArgs e)
    {
        // Set up text layers for the glowing effect
        for (int i = 1; i <= glowSize; i++)
        {
            // Calculate the alpha value based on the layer's position
            int alpha = 255 - (255 * i / glowSize);

            // Create a semi-transparent color for the glow effect
            glowColor = glowColor.ShiftHue(-30);
            Color glowColorWithAlpha = Color.FromArgb(alpha, glowColor);

            // Draw the text with the glow effect
            var biggerFont = new Font(Font.FontFamily, Font.Size + glowSize - i, Font.Style);
            TextRenderer.DrawText(e.Graphics, Text, biggerFont, ClientRectangle, glowColorWithAlpha,
                TextFormatFlags.HorizontalCenter | TextFormatFlags.VerticalCenter |
                TextFormatFlags.PreserveGraphicsClipping);
        }

        // Draw the main text layer
        SizeF szStr = e.Graphics.MeasureString(Text, Font);
        var textRect = new RectangleF(ClientRectangle.Width / 2 - szStr.Width / 2 - 1,
            ClientRectangle.Height / 2 - szStr.Height / 2 - 1, szStr.Width + 2, szStr.Height + 2);

        BackColor = Color.FromArgb(190, BackColor);
        e.Graphics.FillRectangle(new SolidBrush(BackColor), textRect);

        TextRenderer.DrawText(e.Graphics, Text, Font, ClientRectangle, ForeColor,
            TextFormatFlags.HorizontalCenter | TextFormatFlags.VerticalCenter);
    }
}