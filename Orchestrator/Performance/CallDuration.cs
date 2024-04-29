﻿using System.Diagnostics;

namespace Orchestrator.Performance;

public class CallDuration
{
    private Stopwatch _stopwatch = new Stopwatch();

    public void Start()
    {
        _stopwatch.Start();
    }

    public void Stop() {
        _stopwatch.Stop();
        
        // Get the elapsed time as a TimeSpan value.
        TimeSpan ts = _stopwatch.Elapsed;

        // Format and display the TimeSpan value.
        string elapsedTime = String.Format("{0:00}:{1:00}:{2:00}.{3:00}",
            ts.Hours, ts.Minutes, ts.Seconds,
            ts.Milliseconds / 10);
        Console.WriteLine("Call Duration " + elapsedTime);
    }
}
