using UnityEngine;

public class TimeManager : MonoBehaviour
{
    [RuntimeInitializeOnLoadMethod]
    public static void DoSlowMotion()
    {
        const float slowDownFactor = 0.2f;
        Time.timeScale = slowDownFactor;
        Time.fixedDeltaTime = Time.timeScale * 0.02f;
    }

    [RuntimeInitializeOnLoadMethod]
    public static void StopSlowMotion()
    {
        Time.timeScale = 1f;
    }

    // stop time
    [RuntimeInitializeOnLoadMethod]
    public static void DaWarudo()
    {
        Time.timeScale = 0f;
    }
}
