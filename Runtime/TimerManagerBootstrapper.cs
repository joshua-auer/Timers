#if UNITY_EDITOR
using UnityEditor;
#endif

using UnityEngine;
using UnityEngine.LowLevel;
using UnityEngine.PlayerLoop;
using UnityUtilities;

namespace Timers
{
    /// <summary>
    /// Class responsible for inserting the TimerManager into the PlayerLoop.
    /// </summary>
    internal static class TimerManagerBootstrapper
    {
        static PlayerLoopSystem s_timerSystem;

        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.AfterAssembliesLoaded)]
        internal static void Initialize()
        {
            var currentPlayerLoop = PlayerLoop.GetCurrentPlayerLoop();

            if (!InsertTimerManager<Update>(ref currentPlayerLoop, 0))
            {
                Debug.LogWarning("Timers not initialized, unable to register TimerManager into the Update loop.");
                return;
            }

            PlayerLoop.SetPlayerLoop(currentPlayerLoop);

#if UNITY_EDITOR
            EditorApplication.playModeStateChanged -= EditorApplication_OnPlayModeStateChanged;
            EditorApplication.playModeStateChanged += EditorApplication_OnPlayModeStateChanged;

            static void EditorApplication_OnPlayModeStateChanged(PlayModeStateChange playModeState)
            {
                if (playModeState == PlayModeStateChange.ExitingPlayMode)
                {
                    var currentPlayerLoop = PlayerLoop.GetCurrentPlayerLoop();
                    RemoveTimerManager<Update>(ref currentPlayerLoop);
                    PlayerLoop.SetPlayerLoop(currentPlayerLoop);

                    TimerManager.Clear();
                }
            }
#endif
        }

        static bool InsertTimerManager<T>(ref PlayerLoopSystem loop, int index)
        {
            s_timerSystem = new PlayerLoopSystem
            {
                type = typeof(TimerManager),
                updateDelegate = TimerManager.UpdateTimers,
                subSystemList = null
            };

            return PlayerLoopUtility.TryInsertSystem<T>(ref loop, in s_timerSystem, index);
        }

        static void RemoveTimerManager<T>(ref PlayerLoopSystem loop) => PlayerLoopUtility.RemoveSystem<T>(ref loop, in s_timerSystem);
    }
}
