# Timers

A collection of timers for development inside of Unity. Timers are managed by a `TimerManager` class that is inserted directly into Unity's Update loop. There are 4 types of timers included by default in this package, but feel free to create your own timers by extending the `Timer` class.

Included Timers:
- `CountdownTimer` - Counts down from a specified start time to zero.
- `StopwatchTimer` - Counts up from zero to infinity and includes lap functionality, just like a real stopwatch.
- `FrequencyTimer` - Ticks at a specified regular interval.
- `SporadicTimer` - Ticks at irregular intervals within a specified range.

## Example Usages

### CountdownTimer

The following example demonstrates a typical use case for a `CountdownTimer`.

The timer in this example will count down for 3 seconds, logging the `CurrentTime` every frame while the timer is running. When the timer reaches zero, a message will be logged to the console.

```c#
using UnityEngine;
using Timers;

public class CountdownTimerExample : MonoBehaviour
{
    readonly CountdownTimer _timer = new CountdownTimer(3f);

    void Start()
    {
        _timer.OnStop += () => Debug.Log("Timer stopped");
        _timer.Start();
    }

    void Update()
    {
        if (_timer.IsRunning)
            Debug.Log(_timer.CurrentTime);
    }

    void OnDestroy()
    {
        _timer.Dispose();
    }
}
```

### StopwatchTimer

In the following example, the timer will run for 2 seconds, add a lap and then stop. Upon stopping, we will retrieve the first lap and log it to the console.

```c#
using UnityEngine;
using Timers;

public class StopwatchTimerExample : MonoBehaviour
{
    readonly StopwatchTimer _timer = new StopwatchTimer();

    void Start()
    {
        _timer.OnStop += PrintLap;
        _timer.Start();
    }

    void Update()
    {
        if (!_timer.IsRunning) return;

        if (_timer.CurrentTime >= 2f)
        {
            _timer.AddLap();
            _timer.Stop();
        }
    }

    void PrintLap()
    {
        if (_timer.TryGetLap(1, out Lap lap))
            Debug.Log(lap);
        
        // The format of the lap looks like this:
        // lapNumber [lapTime, overallTime, lapTimeDifference]

        // Output:
        // Lap 1 [Time: 2.0, Overall: 2.0, Difference: N/A]
    }

    void OnDestroy()
    {
        _timer.Dispose();
    }
}
```

#### Lap Struct

`Lap` is a struct contained within the `StopwatchTimer` class. It contains the following data:
- `LapNumber` - The lap number.
- `LapTime` - The duration of this lap.
- `OverallTime` - The overall elapsed time on the timer when this lap was added.
- `LapTimeDifference` - The difference between this LapTime and the previous LapTime. If a previous lap doesn't exist, this will be null.

### FrequencyTimer

The following example will log a message to the console every 0.2 seconds (200 milliseconds).

```c#
using UnityEngine;
using Timers;

public class FrequencyTimerExample : MonoBehaviour
{
    readonly FrequencyTimer _timer = new FrequencyTimer(0.2f);

    void Start()
    {
        _timer.OnTick += () => Debug.Log("Tick");
        _timer.Start();
    }

    void OnDestroy()
    {
        _timer.Dispose();
    }
}
```

### SporadicTimer

The following example will log a message to the console randomly between every 0.2 and 1.5 seconds (200-1500 milliseconds).

```c#
using UnityEngine;
using Timers;

public class SporadicTimerExample : MonoBehaviour
{
    readonly SporadicTimer _timer = new SporadicTimer(0.2f, 1.5f);

    void Start()
    {
        _timer.OnTick += () => Debug.Log("Tick");
        _timer.Start();
    }

    void OnDestroy()
    {
        _timer.Dispose();
    }
}
```

## Notes

Always remember to call `Dispose()` on a timer when it is no longer needed to make sure that it is handled properly by the garbage collector.

If you want to make your own custom timer, simply extend the `Timer` class. Classes extending the `Timer` class are required to implement the `Tick()` method and a constructor that extends the base constructor.

```c#
using UnityEngine;
using Timers;

public class MyCustomTimer : Timer
{
    // Place any creation logic specific to your timer in this constructor.
    public MyCustomTimer(float startTime) : base(startTime)
    {
    }

    public override void Tick()
    {
        // Increment or decrement CurrentTime here...
    }
}
```

## How To Install

- ### Package Manager (recommended)

    1. Open the Package Manager from within Unity by going to `Window->Package Manager`
    2. Click the `+` icon in the top left corner of the Package Manager window and select `Add package from git URL...`
    3. Copy paste the following URL into the text box and click import:

        ```
        https://github.com/joshua-auer/Timers.git
        ```

- ### Add To Manifest

    - Locate your project's `manifest.json` file and add the following line:

        ```json
        "com.joshua-auer.timers": "https://github.com/joshua-auer/Timers.git"
        ```