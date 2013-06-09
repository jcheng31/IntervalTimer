# IntervalTimer
## Introduction
A basic timer for the .NET Micro Framework.

`IntervalTimer` takes in a series of durations (represented by `TimeSpan` objects), and triggers `IntervalReachedEvent`s when each expire.
It can be configured to repeatedly cycle through the duration chain, or stop after the final duration has expired.

An `IntervalReachedEvent` contains the position of the duration that just expired.

# Disclaimer
This project hasn't yet been run or tested. It's just thrown together for something I might do in the future.