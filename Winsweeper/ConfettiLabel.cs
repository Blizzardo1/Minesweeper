namespace Winsweeper;

using System;
using System.Drawing;
using System.Windows.Forms;

public class ConfettiLabel : Label
{
    private readonly Random random = new Random();

    protected override void OnPaint(PaintEventArgs e)
    {
        base.OnPaint(e);

        // Calculate the text bounding rectangle
        Rectangle textBounds = DisplayRectangle;

        // Offset the rectangle to match the location relative to the overlay control
        textBounds.Offset(Location);

        // Draw confetti around the text bounding rectangle
        for (int i = 0; i < 100; i++)
        {
            int x = random.Next(textBounds.Left, textBounds.Right);
            int y = random.Next(textBounds.Top, textBounds.Bottom);
            int size = random.Next(5, 10);
            Color color = Color.FromArgb(random.Next(256), random.Next(256), random.Next(256));
            var confettiRect = new Rectangle(x, y, size, size);

            e.Graphics.FillEllipse(new SolidBrush(color), confettiRect);
        }
    }
}