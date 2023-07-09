using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Libsweeper.WinApi
{
    public enum DialogResult
    {

        /// <summary>
        /// The Abort button was selected.
        /// </summary>
        Abort = 3,

        /// <summary>
        /// The Cancel button was selected.
        /// </summary>
        Cancel = 2,

        /// <summary>
        /// The Continue button was selected.
        /// </summary>
        Continue = 11,

        /// <summary>
        /// The Ignore button was selected.
        /// </summary>
        Ignore = 5,

        /// <summary>
        /// The No button was selected.
        /// </summary>
        No = 7,

        /// <summary>
        /// The OK button was selected.
        /// </summary>
        Ok = 1,

        /// <summary>
        /// The Retry button was selected.
        /// </summary>
        Retry = 4,

        /// <summary>
        /// The Try Again button was selected.
        /// </summary>
        TryAgain = 10,

        /// <summary>
        /// The Yes button was selected. 
        /// </summary>
        Yes = 6
    }
}
