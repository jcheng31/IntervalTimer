# IntervalTimer
A basic timer for the .NET Micro Framework.

`IntervalTimer` takes in a series of durations (represented by `TimeSpan` objects), and triggers events when each expire.
It can be configured to repeatedly cycle through the duration chain, or stop after the final duration has expired.

(This project hasn't yet been run or tested. It's just a quick hack for something I might do in the future.)