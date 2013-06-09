using System;
using System.Collections;
using Microsoft.SPOT;

namespace IntervalTimer
{
    public delegate void IntervalEventHandler(object sender, IntervalReachedEventArgs e);

    public class IntervalTimer
    {
        public event IntervalEventHandler IntervalCompleted;

        public bool Repeat;

        private int _position = 0;
        private ArrayList _durations;
        private ExtendedTimer _timer;

        public IntervalTimer(bool shouldRepeat)
        {
            _durations = new ArrayList();
            Repeat = shouldRepeat;
        }

        public IntervalTimer(bool shouldRepeat, IEnumerable intervals)
        {
            Repeat = shouldRepeat;
            _durations = new ArrayList();
            foreach (var interval in intervals)
            {
                try
                {
                    TimeSpan currentInterval = (TimeSpan) interval;
                    _durations.Add(currentInterval);
                }
                catch (InvalidCastException)
                {
                    throw new Exception("Intervals should only contain TimeSpan objects.");
                }
            }
        }

        public void AddInterval(TimeSpan interval)
        {
            _durations.Add(interval);
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
