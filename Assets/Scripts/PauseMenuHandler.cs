using UnityEngine;

public class PauseMenuHandler : MonoBehaviour
{
    public static PauseMenuHandler Instance { get; private set; }
    public GameObject pauseMenu;

    private void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
        pauseMenu.SetActive(true);
    }

    public void Resume()
    {
        AudioManager.Instance.Play("tap");
        GameStats.Paused = false;
        pauseMenu.SetActive(false);
        Spawner.Instance.StartGame();
        PpvUtils.Instance.DisablePp();
    }

    public void Pause()
    {
        pauseMenu.SetActive(true);
        Spawner.Instance.StopSpawning();
        GameStats.Paused = true;
    }
}
