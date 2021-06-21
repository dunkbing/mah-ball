using System.Collections;
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
        GameStats.GameIsPaused = false;
        pauseMenu.SetActive(false);
        Spawner.Instance.StartGame();
        PpvUtils.Instance.ExitSlowMo();
    }

    public void Pause()
    {
        pauseMenu.SetActive(true);
        Spawner.Instance.StopSpawning();
        GameStats.GameIsPaused = true;
    }

    public void DelayPause(float time)
    {
        StartCoroutine(WaitAndPause(time));
    }

    private IEnumerator WaitAndPause(float time)
    {
        TimeManager.StopSlowMotion();
        GameStats.GameIsPaused = true;

        yield return new WaitForSeconds(time);
        PauseMenuHandler.Instance.Pause();
    }
}
