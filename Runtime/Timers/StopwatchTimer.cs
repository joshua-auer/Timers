using System.Collections.Generic;
using UnityEngine;

namespace Timers
{
    /// <summary>
    /// A timer that continually increments up from zero and has lap functionality.
    /// </summary>
    public class StopwatchTimer : Timer
    {
        /// <summary>
        /// A struct used by stopwatch timers that represents a lap.
        /// </summary>
        public struct Lap
        {
            public int LapNumber;
            public float LapTime;
            public float OverallTime;
            public float? LapTimeDifference;

            /// <summary>
            /// Constructor to create a new lap.
            /// </summary>
            /// <param name="lapNumber">The lap's number.</param>
            /// <param name="lapTime">The time of this lap.</param>
            /// <param name="overallTime">The overall elapsed time.</param>
            /// <param name="lapTimeDifference">The difference between this lap time and the previous. Should be null if there isn't a previous lap to compare to.</param>
            public Lap(int lapNumber, float lapTime, float overallTime, float? lapTimeDifference)
            {
                LapNumber = lapNumber;
                LapTime = lapTime;
                OverallTime = overallTime;
                LapTimeDifference = lapTimeDifference;
            }

            public override readonly string ToString()
            {
                if (LapTimeDifference != null)
                    return $"Lap {LapNumber} [Time: {LapTime}, Overall: {OverallTime}, Difference: {LapTimeDifference}]";
                else
                    return $"Lap {LapNumber} [Time: {LapTime}, Overall: {OverallTime}, Difference: N/A]";
            }
        }

        /// <summary>
        /// A collection of all laps currently stored by the stopwatch timer.
        /// </summary>
        public readonly Dictionary<int, Lap> Laps = new();

        /// <summary>
        /// The current lap number. Zero if no laps have been created yet.
        /// </summary>
        public int CurrentLapNumber { get; private set; }

        /// <summary>
        /// The current lap. Null if no laps have been created yet.
        /// </summary>
        public Lap CurrentLap => Laps[CurrentLapNumber];

        /// <summary>
        /// The current number of laps that have been logged.
        /// </summary>
        public int LapCount => Laps.Count;

        /// <summary>
        /// Constructor to create a new stopwatch timer.
        /// </summary>
        public StopwatchTimer() : base(0f)
        {
            CurrentLapNumber = 0;
        }

        /// <summary>
        /// While the timer is running, increments the timer by <see cref="Time.deltaTime"/>.
        /// </summary>
        public override void Tick()
        {
            if (IsRunning)
                CurrentTime += Time.deltaTime;
        }

        /// <summary>
        /// If the timer is not currently running, clears out all laps and starts running the timer from zero, while registering it onto the TimerManager.
        /// </summary>
        public override void Start()
        {
            if (!IsRunning)
            {
                CurrentTime = 0f;
                CurrentLapNumber = 0;
                Laps.Clear();
                IsRunning = true;
                TimerManager.RegisterTimer(this);
                OnStart.Invoke();
            }
        }

        /// <summary>
        /// Resets the timer to zero and clears out all laps.
        /// </summary>
        public override void Reset()
        {
            CurrentTime = 0f;
            CurrentLapNumber = 0;
            Laps.Clear();
            OnReset.Invoke();
        }

        /// <summary>
        /// Creates a new lap entry and adds it to the Laps dictionary.
        /// </summary>
        public void AddLap()
        {
            if (!IsRunning) return;

            CurrentLapNumber++;

            if (Laps.TryGetValue(CurrentLapNumber - 1, out var lastLap))
            {
                var lapTime = CurrentTime - lastLap.OverallTime;
                var lapTimeDiff = lapTime - lastLap.LapTime;
                Laps.Add(CurrentLapNumber, new(CurrentLapNumber, lapTime, CurrentTime, lapTimeDiff));
            }
            else
            {
                Laps.Add(CurrentLapNumber, new(CurrentLapNumber, CurrentTime, CurrentTime, null));
            }
        }

        /// <summary>
        /// Tries to get the lap at the provided <paramref name="lapNumber"/>.
        /// </summary>
        /// <param name="lapNumber">The lap number of the lap to retrieve.</param>
        /// <param name="lap">The lap to retrieve.</param>
        /// <returns>True if a lap exists at the provided <paramref name="lapNumber"/>, otherwise false.</returns>
        public bool TryGetLap(int lapNumber, out Lap lap) => Laps.TryGetValue(lapNumber, out lap);
    }
}
