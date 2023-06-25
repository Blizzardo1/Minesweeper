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
        public static string GetDescription(this Enum value)
        {
            FieldInfo? fi = value.GetType().GetField(value.ToString());
            DescriptionAttribute[]? attributes = fi?.GetCustomAttributes(typeof(DescriptionAttribute), false) as DescriptionAttribute[];
            return attributes?.FirstOrDefault()?.Description ?? value.ToString();
        }
    }
}
