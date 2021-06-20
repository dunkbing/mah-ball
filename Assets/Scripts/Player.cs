using UnityEngine;

public class Player : MonoBehaviour
{
    private ObjectPool _objectPool;

    private void Awake()
    {
        _objectPool = ObjectPool.Instance;
    }

    private void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject.CompareTag("Lava"))
        {
            Pause();
        }
    }

    private void Pause()
    {
        PauseMenuHandler.Instance.Pause();
        _objectPool.Retrieve(nameof(Player));
        // _objectPool.RetrieveAll();
        TimeManager.StopSlowMotion();
        GameStats.Paused = true;
    }
}
