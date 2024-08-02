using UnityEngine;

namespace Timers
{
    /// <summary>
    /// A timer that counts down from a start time to zero.
    /// </summary>
    public class CountdownTimer : Timer
    {
        /// <summary>
        /// A value ranging from 0 to 1 that represents the progress of the timer. This can be thought of as the timer's completion percentage.
        /// </summary>
        public float Progress => 1f - Mathf.Clamp01(CurrentTime / StartTime);

        /// <summary>
        /// Has the timer reached zero?
        /// </summary>
        public bool IsFinished => CurrentTime <= 0f;

        /// <summary>
        /// Constructor to create a new countdown timer.
        /// </summary>
        /// <param name="startTime">The time at which the timer should start.</param>
        public CountdownTimer(float startTime) : base(startTime)
        {
        }

        /// <summary>
        /// Constructor to create a new countdown timer with a random start time between <paramref name="minStartTime"/> and <paramref name="maxStartTime"/>.
        /// </summary>
        /// <param name="minStartTime">The minimum allowed start time.</param>
        /// <param name="maxStartTime">The maximum allowed start time.</param>
        public CountdownTimer(float minStartTime, float maxStartTime) : base(Random.Range(minStartTime, maxStartTime))
        {
        }

        /// <summary>
        /// While the timer is running, decrements the timer by <see cref="Time.deltaTime"/> and stops when it reaches zero.
        /// </summary>
        public override void Tick()
        {
            if (!IsRunning) return;

            if (CurrentTime > 0f)
                CurrentTime -= Time.deltaTime;
            else
                Stop();
        }

        /// <summary>
        /// Resets the timer with a new start time.
        /// </summary>
        /// <param name="startTime">The new start time to use.</param>
        public void Reset(float startTime)
        {
            CurrentTime = StartTime = startTime;
            OnReset.Invoke();
        }

        /// <summary>
        /// Resets the timer with a random start time between <paramref name="minStartTime"/> and <paramref name="maxStartTime"/>.
        /// </summary>
        /// <param name="minStartTime">The minimum allowed start time.</param>
        /// <param name="maxStartTime">The maximum allowed start time.</param>
        public void Reset(float minStartTime, float maxStartTime)
        {
            CurrentTime = StartTime = Random.Range(minStartTime, maxStartTime);
            OnReset.Invoke();
        }
    }
}
