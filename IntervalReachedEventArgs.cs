using System;
using Microsoft.SPOT;

namespace IntervalTimer
{
    public enum IntervalType
    {
        Short,
        Long
    };
    
    public class IntervalReachedEventArgs : EventArgs
    {
        public IntervalType Type { get; set; }
        public String Message { get; set; }

        public IntervalReachedEventArgs(IntervalType type, String message)
        {
            Type = type;
            Message = message;
        }
    }
}
