using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SDLsweeper
{
    internal static class Extensions
    {
        public static string LongestString(this List<MenuItem> list) {
            int index = 0;
            if (list.Count == 0) return "";
            for(int i = 1; i < list.Count; i++)
            {
                if (list[index].Text.Length < list[i].Text.Length) {
                    index = i;
                }
            }

            return list[ index ].Text;
        }
    }
}
