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

        private ExtendedTimer _timer;

        public IntervalTimer(TimeSpan shortDuration, TimeSpan longDuration)
        {
            if (shortDuration.CompareTo(longDuration) > 0)
            {
                throw new Exception("Short duration must be less than the long duration.");
            }

            ShortDuration = shortDuration;
            LongDuration = longDuration;
        }

        public IntervalTimer(int shortSeconds, int longSeconds)
        {
            if (shortSeconds > longSeconds)
            {
                throw new Exception("Short duration must be less than the long duration.");
            }
            ShortDuration = new TimeSpan(0, 0, shortSeconds);
            LongDuration = new TimeSpan(0, 0, longSeconds);
        }

        public void Start()
        {
            _timer = new ExtendedTimer(InternalTimerTriggered, true, TimeSpan.Zero, ShortDuration);
        }

        public void Stop()
        {
            if (_timer != null)
            {
                _timer.Dispose();
            }
        }

        private void InternalTimerTriggered(object state)
        {
            _timer.Dispose();
            bool isLastDurationShort = (bool)state;

            TimeSpan nextDuration;
            String eventMessage;
            IntervalType previousDurationType;

            if (isLastDurationShort)
            {
                nextDuration = LongDuration;
                eventMessage = "Short Duration Triggered";
                previousDurationType = IntervalType.Short;
            }
            else
            {
                nextDuration = ShortDuration;
                eventMessage = "Long Duration Triggered";
                previousDurationType = IntervalType.Long;
            }

            _timer = new ExtendedTimer(InternalTimerTriggered, !isLastDurationShort, TimeSpan.Zero, nextDuration);
            
            IntervalReachedEventArgs arguments = new IntervalReachedEventArgs(previousDurationType, eventMessage);
            OnIntervalCompleted(arguments);
        }

        private void OnIntervalCompleted(IntervalReachedEventArgs arguments)
        {
            IntervalEventHandler handler = IntervalCompleted;
            if (handler != null)
            {
                handler(this, arguments);
            }
        }
    }
}
