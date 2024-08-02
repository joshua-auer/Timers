using System;

namespace Timers
{
    /// <summary>
    /// A base class used to create various types of timers.
    /// </summary>
    public abstract class Timer : IDisposable
    {
        /// <summary>
        /// The time the timer starts with.
        /// </summary>
        public float StartTime { get; protected set; }

        /// <summary>
        /// The current time (in seconds) of the timer.
        /// </summary>
        public float CurrentTime { get; protected set; }

        /// <summary>
        /// Is the timer currently running?
        /// </summary>
        public bool IsRunning { get; protected set; }

        /// <summary>
        /// Action that is invoked when the timer starts.
        /// </summary>
        public Action OnStart = delegate { };

        /// <summary>
        /// Action that is invoked when the timer stops.
        /// </summary>
        public Action OnStop = delegate { };

        /// <summary>
        /// Action that is invoked when the timer resumes.
        /// </summary>
        public Action OnResume = delegate { };

        /// <summary>
        /// Action that is invoked when the timer pauses.
        /// </summary>
        public Action OnPause = delegate { };

        /// <summary>
        /// Action that is invoked when the timer is reset.
        /// </summary>
        public Action OnReset = delegate { };

        /// <summary>
        /// Constructor to create a new timer.
        /// </summary>
        /// <param name="startTime">The time at which the timer should start.</param>
        protected Timer(float startTime)
        {
            StartTime = startTime;
        }

        /// <summary>
        /// If the timer is not currently running, starts running the timer using the currently set StartTime and registers it onto the TimerManager.
        /// </summary>
        public virtual void Start()
        {
            if (!IsRunning)
            {
                CurrentTime = StartTime;
                IsRunning = true;
                TimerManager.RegisterTimer(this);
                OnStart.Invoke();
            }
        }

        /// <summary>
        /// If the timer is currently running, stops running the timer and deregisters it from the TimerManager.
        /// </summary>
        public void Stop()
        {
            if (IsRunning)
            {
                IsRunning = false;
                TimerManager.DeregisterTimer(this);
                OnStop.Invoke();
            }
        }

        /// <summary>
        /// If the timer is not currently running, resumes running the timer from wherever it was paused.
        /// </summary>
        /// <remarks>
        /// Note that you can only resume a timer that has been paused using <see cref="Pause"/>.
        /// Resuming a timer that has been stopped using <see cref="Stop"/> will not work, as <see cref="Stop"/> deregisters the timer.
        /// </remarks>
        public void Resume()
        {
            if (!IsRunning)
            {
                IsRunning = true;
                OnResume.Invoke();
            }
        }

        /// <summary>
        /// If the timer is currently running, pauses the timer.
        /// </summary>
        public void Pause()
        {
            if (IsRunning)
            {
                IsRunning = false;
                OnPause.Invoke();
            }
        }

        /// <summary>
        /// Resets the timer using the currently set StartTime.
        /// </summary>
        public virtual void Reset()
        {
            CurrentTime = StartTime;
            OnReset.Invoke();
        }

        /// <summary>
        /// Implement this method to control how the CurrentTime is incremented/decremented.
        /// </summary>
        public abstract void Tick();

        #region Disposal

        bool _disposed;

        ~Timer()
        {
            Dispose(false);
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (_disposed) return;

            if (disposing)
                TimerManager.DeregisterTimer(this);

            _disposed = true;
        }

        #endregion
    }
}
