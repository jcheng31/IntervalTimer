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

        public void RemoveIntervalAt(int index)
        {
            _durations.RemoveAt(index);
        }

        public TimeSpan[] PeekAllDurations()
        {
            TimeSpan[] returnArray = new TimeSpan[_durations.Count];
            _durations.CopyTo(returnArray);
            return returnArray;
        }

        public void Start()
        {
            if (_durations.Count > 0)
            {
                TimeSpan currentDuration = (TimeSpan)_durations[_position];
                _timer = new ExtendedTimer(InternalTimerTriggered, _position, TimeSpan.Zero, currentDuration);
            }
        }

        public void Stop()
        {
            if (_timer != null)
            {
                _timer.Dispose();
            }
        }

        public void Reset()
        {
            Stop();
            _position = 0;
        }

        private void InternalTimerTriggered(object state)
        {
            _timer.Dispose();

            // Fire off an event.
            IntervalReachedEventArgs args = new IntervalReachedEventArgs(_position);
            OnIntervalCompleted(args);

            _position = (_position + 1) % _durations.Count;

            bool shouldTerminate = _position == 0 && !Repeat;
            if (shouldTerminate)
            {
                return;
            }

            // Set the next timer going.
            TimeSpan nextDuration = (TimeSpan)_durations[_position];
            _timer = new ExtendedTimer(InternalTimerTriggered, _position, TimeSpan.Zero, nextDuration);
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
