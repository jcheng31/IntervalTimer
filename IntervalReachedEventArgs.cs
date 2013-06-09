using System;
using Microsoft.SPOT;

namespace IntervalTimer
{
    public class IntervalReachedEventArgs : EventArgs
    {
        public int DurationNumber { get; set; }

        public IntervalReachedEventArgs(int durationNumber)
        {
            DurationNumber = durationNumber;
        }
    }
}
