using System.Collections.Generic;

namespace Timers
{
    /// <summary>
    /// Class that handles registration/deregistration, updating and disposal of all active timers.
    /// </summary>
    public static class TimerManager
    {
        static readonly List<Timer> s_timers = new();
        static readonly List<Timer> s_sweep = new();

        /// <summary>
        /// Adds the <paramref name="timer"/> to the list of registered timers.
        /// </summary>
        /// <param name="timer">The timer to add.</param>
        public static void RegisterTimer(Timer timer)
        {
            if (!s_timers.Contains(timer))
                s_timers.Add(timer);
        }

        /// <summary>
        /// Removes the <paramref name="timer"/> from the list of registered timers.
        /// </summary>
        /// <param name="timer">The timer to remove.</param>
        public static void DeregisterTimer(Timer timer) => s_timers.Remove(timer);

        /// <summary>
        /// Loops through all registered timers, calling Tick() on each one.
        /// </summary>
        public static void UpdateTimers()
        {
            if (s_timers.Count == 0) return;

            RefreshSweepList();
            foreach (var timer in s_sweep)
                timer.Tick();
        }

        /// <summary>
        /// Disposes all registered timers and clears the registered timers list.
        /// </summary>
        public static void Clear()
        {
            RefreshSweepList();
            foreach (var timer in s_sweep)
                timer.Dispose();

            s_timers.Clear();
            s_sweep.Clear();
        }

        static void RefreshSweepList()
        {
            s_sweep.Clear();
            s_sweep.AddRange(s_timers);
        }
    }
}
