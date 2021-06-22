using System;
using System.Collections;
using System.IO;
using TMPro;
using UnityEngine;

public class PauseMenu : Menu
{
    public GameObject pauseMenu;

    public static PauseMenu Instance { get; private set; }

    // score ui
    public GameObject highScoreTmpGo;
    private TextMeshProUGUI _highScoreTmp;

    private void Awake()
    {
        Instance = this;

        _highScoreTmp = highScoreTmpGo.GetComponent<TextMeshProUGUI>();
    }

    private void Start()
    {
        pauseMenu.SetActive(false);

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

    public override void Pause()
    {
        base.Pause();
        pauseMenu.SetActive(true);
        _highScoreTmp.SetText($"High score: {GameStats.HighScore}");
    }

    public void Play()
    {
        base.Resume();
        pauseMenu.SetActive(false);
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
