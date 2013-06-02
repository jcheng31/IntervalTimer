using System;
using Microsoft.SPOT;

namespace IntervalTimer
{
    public delegate void IntervalEventHandler(object sender, IntervalReachedEventArgs e);

    public class IntervalTimer
    {
        public event IntervalEventHandler IntervalCompleted;

        public TimeSpan ShortDuration { get; set; }
        public TimeSpan LongDuration { get; set; }

        public IntervalTimer(TimeSpan shortDuration, TimeSpan longDuration)
        {
            ShortDuration = shortDuration;
            LongDuration = longDuration;
        }

        public IntervalTimer(int shortSeconds, int longSeconds)
        {
            ShortDuration = new TimeSpan(0, 0, shortSeconds);
            LongDuration = new TimeSpan(0, 0, longSeconds);
        }

        
    }
}
