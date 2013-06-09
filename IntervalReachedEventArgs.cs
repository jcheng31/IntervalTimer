using System;
using Microsoft.SPOT;

namespace IntervalTimer
{
    public class IntervalReachedEventArgs : EventArgs
    {
        public String Message { get; set; }

        public IntervalReachedEventArgs(String message)
        {
            Message = message;
        }
    }
}
