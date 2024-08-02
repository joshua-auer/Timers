using UnityEngine;

namespace Timers
{
    /// <summary>
    /// A timer that ticks at irregular intervals within a specific range.
    /// </summary>
    public class SporadicTimer : Timer
    {
        /// <summary>
        /// The current duration (in seconds) between ticks. This value changes with each tick.
        /// </summary>
        public float CurrentFrequency { get; private set; }

        /// <summary>
        /// Action that is invoked each tick.
        /// </summary>
        public System.Action OnTick = delegate { };

        float _minFrequency;
        float _maxFrequency;

        /// <summary>
        /// Constructor to create a new sporadic timer.
        /// </summary>
        /// <param name="minFrequency">The minimum duration (in seconds) between ticks.</param>
        /// <param name="maxFrequency">The maximum duration (in seconds) between ticks.</param>
        public SporadicTimer(float minFrequency, float maxFrequency) : base(0f)
        {
            _minFrequency = minFrequency;
            _maxFrequency = maxFrequency;
            CalculateRandomFrequency();
        }

        /// <summary>
        /// While the timer is running, repeatedly increments the timer by <see cref="Time.deltaTime"/> up to the current frequency,
        /// setting a new random current frequency and invoking <see cref="OnTick"/> each time the current frequency is reached.
        /// </summary>
        public override void Tick()
        {
            if (!IsRunning) return;

            CurrentTime += Time.deltaTime;

            if (CurrentTime >= CurrentFrequency)
            {
                CurrentTime = 0f;
                CalculateRandomFrequency();
                OnTick.Invoke();
            }
        }

        /// <summary>
        /// Resets the timer.
        /// </summary>
        public override void Reset()
        {
            CurrentTime = 0f;
            CalculateRandomFrequency();
            OnReset.Invoke();
        }

        /// <summary>
        /// Resets the timer with a new frequency range.
        /// </summary>
        /// <param name="minFrequency"></param>
        /// <param name="maxFrequency"></param>
        public void Reset(int minFrequency, int maxFrequency)
        {
            CurrentTime = 0f;
            _minFrequency = minFrequency;
            _maxFrequency = maxFrequency;
            CalculateRandomFrequency();
            OnReset.Invoke();
        }

        void CalculateRandomFrequency() => CurrentFrequency = Random.Range(_minFrequency, _maxFrequency);
    }
}
