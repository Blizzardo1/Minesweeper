using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace Libsweeper.WinApi {
    public static class SafeNativeMethods {
        /// <summary>
        /// An ASCII Message Box
        /// </summary>
        /// <param name="h">The Handle</param>
        /// <param name="s">The Body</param>
        /// <param name="t">The Title</param>
        /// <param name="o">A list of Options, typically <see cref="int"/> casts of <see cref="MessageBoxOptions"/> and <see cref="MessageBoxIcon"/></param>
        /// <returns>An <see cref="int"/> representing a <see cref="DialogResult"/></returns>
        [DllImport("User32.dll", EntryPoint = "MessageBoxA", CharSet = CharSet.Auto,
            CallingConvention = CallingConvention.StdCall)]
        private static extern int MessageBoxA(
            [MarshalAs(UnmanagedType.I4)] int h,
            [MarshalAs(UnmanagedType.LPWStr)] string s,
            [MarshalAs(UnmanagedType.LPWStr)] string t,
            [MarshalAs(UnmanagedType.I4)] int o
        );

        /// <summary>
        /// A Unicode Message Box
        /// </summary>
        /// <param name="h">The Handle</param>
        /// <param name="s">The Body</param>
        /// <param name="t">The Title</param>
        /// <param name="o">A list of Options, typically <see cref="int"/> casts of <see cref="MessageBoxOptions"/> and <see cref="MessageBoxIcon"/></param>
        /// <returns>An <see cref="int"/> representing a <see cref="DialogResult"/></returns>
        [DllImport("User32.dll", EntryPoint = "MessageBoxW", CharSet = CharSet.Auto,
            CallingConvention = CallingConvention.StdCall)]
        private static extern int MessageBoxW(
            [MarshalAs(UnmanagedType.I4)] int h,
            [MarshalAs(UnmanagedType.LPWStr)] string s,
            [MarshalAs(UnmanagedType.LPWStr)] string t,
            [MarshalAs(UnmanagedType.I4)] int o
        );

        /// <summary>
        /// A Win32 Message Box
        /// </summary>
        /// <param name="hWnd">The Handle else <see cref="IntPtr.Zero"/></param>
        /// <param name="message">A message in the body of the Message Box</param>
        /// <param name="title">The title of the Message Box</param>
        /// <param name="options">Any options to alter the behavior of the Message Box</param>
        /// <param name="icon">An icon representing the behavior</param>
        /// <returns>A <see cref="DialogResult"/> of the interaction within the Message Box</returns>
        public static DialogResult MessageBox(IntPtr hWnd, string message, string title, MessageBoxOptions options = 0, MessageBoxIcon icon = 0) {
            
            // Check for UTF-16 support
            return Marshal.SystemDefaultCharSize == 2
                ? (DialogResult)MessageBoxW(hWnd.ToInt32(), message, title, (int)options | (int)icon)
                : (DialogResult)MessageBoxA(hWnd.ToInt32(), message, title, (int)options | (int)icon);
        }
    }
}
