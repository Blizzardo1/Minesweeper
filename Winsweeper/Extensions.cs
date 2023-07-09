using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Winsweeper
{
    internal static class Extensions
    {
        /// <summary>
        /// Normalizes the degree of Hue
        /// </summary>
        /// <param name="hue">The Hue Input</param>
        /// <returns>The normalized Hue Value</returns>
        private static float NormalizeHue(float hue)
        {
            if (hue >= 0)
            {
                return hue % 360;
            }
            else
            {
                return 360 - (-hue % 360);
            }
        }

        /// <summary>
        /// Shifts a color by a given amount
        /// </summary>
        /// <param name="color">The color to change</param>
        /// <param name="hueShiftAmount">The amount of which to shift the hue</param>
        /// <returns></returns>
        public static Color ShiftHue(this Color color, int hueShiftAmount)
        {
            float hue = color.GetHue(); // Get the original hue value
            float shiftedHue = hue + hueShiftAmount; // Shift the hue by the specified amount

            // Normalize the hue value to be within the range of 0-360
            shiftedHue = NormalizeHue(shiftedHue);

            // Create a new color with the shifted hue value
            return FromAhsb(color.A, shiftedHue, color.GetSaturation(), color.GetBrightness());

        }

        /// <summary>
        /// Gets a <see cref="Color"/> based on the Alpha, Hue, Saturation, and Brightness
        /// </summary>
        /// <param name="alpha">The Transparency Value</param>
        /// <param name="hue">The Hue</param>
        /// <param name="saturation">The Saturation</param>
        /// <param name="brightness">The Brightness or Value</param>
        /// <returns>A Color with modified alpha, hue, saturation, and brightness</returns>
        /// <exception cref="ArgumentOutOfRangeException"></exception>
        private static Color FromAhsb(int alpha, float hue, float saturation, float brightness)
        {
            if (saturation < 0.0 || saturation > 1.0)
            {
                throw new ArgumentOutOfRangeException(nameof(saturation), "Saturation must be between 0.0 and 1.0.");
            }
            if (brightness < 0.0 || brightness > 1.0)
            {
                throw new ArgumentOutOfRangeException(nameof(brightness), "Brightness must be between 0.0 and 1.0.");
            }

            if (alpha is < 0 or > 255)
            {
                throw new ArgumentOutOfRangeException(nameof(alpha), "Alpha must be between 0 and 255.");
            }

            if (Math.Abs(saturation) < 0.0001)
            {
                return Color.FromArgb(alpha, Convert.ToInt32(brightness * 255),
                    Convert.ToInt32(brightness * 255), Convert.ToInt32(brightness * 255));
            }

            float fMax, fMid, fMin;
            int iSextant, iMax, iMid, iMin;

            if (brightness <= 0.5)
            {
                fMax = brightness + brightness * saturation;
                fMin = brightness - brightness * saturation;
            }
            else
            {
                fMax = brightness - brightness * saturation;
                fMin = brightness + brightness * saturation;
            }

            iSextant = (int)Math.Floor(hue / 60f);
            hue %= 360;

            hue /= 60f;
            hue -= 2f * (float)Math.Floor((iSextant + 1f) % 6f / 2f);
            if (iSextant % 2 == 0)
            {
                fMid = hue * (fMax - fMin) + fMin;
            }
            else
            {
                fMid = fMin - hue * (fMax - fMin);
            }

            iMax = Math.Abs( Convert.ToInt32(fMax * 255) % 256);
            iMid = Math.Abs( Convert.ToInt32(fMid * 255) % 256);
            iMin = Math.Abs( Convert.ToInt32(fMin * 255) % 256);

            return iSextant switch {
                1 => Color.FromArgb(alpha, iMid, iMax, iMin),
                2 => Color.FromArgb(alpha, iMin, iMax, iMid),
                3 => Color.FromArgb(alpha, iMin, iMid, iMax),
                4 => Color.FromArgb(alpha, iMid, iMin, iMax),
                5 => Color.FromArgb(alpha, iMax, iMin, iMid),
                _ => Color.FromArgb(alpha, iMax, iMid, iMin)
            };
        }

        /// <summary>
        /// Gets the description of an <see cref="Enum"/>
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static string GetDescription(this Enum value)
        {
            FieldInfo? fi = value.GetType().GetField(value.ToString());
            DescriptionAttribute[]? attributes = fi?.GetCustomAttributes(typeof(DescriptionAttribute), false) as DescriptionAttribute[];
            return attributes?.FirstOrDefault()?.Description ?? value.ToString();
        }

        /// <summary>
        /// Formats a <see cref="TimeSpan"/> to a human readable string
        /// </summary>
        /// <param name="ts">The <see cref="TimeSpan"/> to be formatted</param>
        /// <returns>A Human Readable Time Frame</returns>
        public static string Format(this TimeSpan ts)
        {
            var sb = new StringBuilder();

            if (ts.Hours > 0)
            {
                sb.Append($"{ts.Hours} {(ts.Hours > 1 ? "hours" : "hour")} ");
            }

            if (ts.Minutes > 0)
            {
                sb.Append($" {ts.Minutes} {(ts.Minutes > 1 ? "minutes" : "minute")} ");
            }

            if (ts.Seconds > 0)
            {
                sb.Append($" {ts.Seconds} {(ts.Seconds > 1 ? "seconds" : "second")} ");
            }

            return sb.ToString();
        }
    }
}
