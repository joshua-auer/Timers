using UnityEngine;

namespace Timers
{
    /// <summary>
    /// A timer that ticks at a specific frequency.
    /// </summary>
    public class FrequencyTimer : Timer
    {
        /// <summary>
        /// The duration (in seconds) between ticks.
        /// </summary>
        public float Frequency { get; private set; }
        
        /// <summary>
        /// Action that is invoked each tick.
        /// </summary>
        public System.Action OnTick = delegate { };

        /// <summary>
        /// Constructor to create a new frequency timer.
        /// </summary>
        /// <param name="frequency">The duration (in seconds) between ticks.</param>
        public FrequencyTimer(float frequency) : base(0f)
        {
            Frequency = frequency;
        }

        /// <summary>
        /// Constructor to create a new frequency timer, with a random frequency between <paramref name="minFrequency"/> and <paramref name="maxFrequency"/>.
        /// </summary>
        /// <param name="minFrequency">The minimum duration (in seconds) between ticks.</param>
        /// <param name="maxFrequency">The maximum duration (in seconds) between ticks.</param>
        public FrequencyTimer(float minFrequency, float maxFrequency) : base(0f)
        {
            Frequency = Random.Range(minFrequency, maxFrequency);
        }

        /// <summary>
        /// While the timer is running, repeatedly increments the timer by <see cref="Time.deltaTime"/> up to the frequency, invoking <see cref="OnTick"/> each time the frequency is reached.
        /// </summary>
        public override void Tick()
        {
            if (!IsRunning) return;
            
            CurrentTime += Time.deltaTime;

            if (CurrentTime >= Frequency)
            {
                CurrentTime = 0f;
                OnTick.Invoke();
            }
        }

        /// <summary>
        /// Resets the timer to zero.
        /// </summary>
        public override void Reset()
        {
            CurrentTime = 0f;
            OnReset.Invoke();
        }

        /// <summary>
        /// Resets the timer to zero, while setting a new frequency.
        /// </summary>
        /// <param name="frequency"></param>
        public void Reset(float frequency)
        {
            CurrentTime = 0f;
            Frequency = frequency;
            OnReset.Invoke();
        }

        /// <summary>
        /// Resets the timer to zero, while setting a new random frequency between <paramref name="minFrequency"/> and <paramref name="maxFrequency"/>.
        /// </summary>
        /// <param name="minFrequency">The minimum duration between ticks.</param>
        /// <param name="maxFrequency">The maximum duration between ticks.</param>
        public void Reset(float minFrequency, float maxFrequency)
        {
            CurrentTime = 0f;
            Frequency = Random.Range(minFrequency, maxFrequency);
            OnReset.Invoke();
        }
    }
}
