using System;
using System.Collections;
using System.IO;
using TMPro;
using UnityEngine;

public class PauseMenu : MonoBehaviour
{
    public static PauseMenu Instance { get; private set; }
    public GameObject pauseMenu;

    // score ui
    public GameObject highScoreTmpGo;
    private TextMeshProUGUI _highScoreTmp;

    private void Awake()
    {
        Instance = this;

        _highScoreTmp = highScoreTmpGo.GetComponent<TextMeshProUGUI>();

        // load high score
        try
        {
            var highScoreTxt = File.ReadAllText(Constants.DataFilePath);
            int.TryParse(highScoreTxt, out GameStats.HighScore);
            _highScoreTmp.SetText($"High score: {highScoreTxt}");
        }
        catch (Exception e) when(e is FileNotFoundException || e is DirectoryNotFoundException)
        {
            Debug.Log(e.Message);
        }

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
        ScoreMenu.Instance.IncreaseScore(-GameStats.Score);
        Spawner.Instance.StartGame();
        PpvUtils.Instance.ExitSlowMo();
    }

    public void Pause()
    {
        pauseMenu.SetActive(true);
        _highScoreTmp.SetText($"High score: {GameStats.HighScore}");
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
        Pause();
    }
}
