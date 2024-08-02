## [1.0.0] - 2024-08-01
### First Release
- Includes Timer class, which can be extended to create various timers
- Includes the following concrete implementations of timers:
    - CountdownTimer
    - StopwatchTimer
    - FrequencyTimer
    - SporadicTimer
- Includes TimerManager class, which is responsible for tracking and updating all active timers
- Includes TimerManagerBootstrapper class, which inserts the TimerManager into the PlayerLoop