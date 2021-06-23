using System.Collections;
using Common;
using TMPro;
using UnityEngine;
using Utilities;

namespace UI
{
    public class PauseMenu : Menu
    {
        public GameObject pauseMenu;
        public GameObject startMenu;

        public static PauseMenu Instance { get; private set; }

        // score ui
        public TextMeshProUGUI highScoreTmp;

        private void Awake()
        {
            Instance = this;
        }

        private void Start()
        {
            pauseMenu.SetActive(false);
        }

        public override void Pause()
        {
            base.Pause();
            pauseMenu.SetActive(true);
            highScoreTmp.SetText($"High score: {GameStats.HighScore}");
        }

        public void Play()
        {
            base.Resume();
            pauseMenu.SetActive(false);
        }

        public void MainMenu()
        {
            pauseMenu.SetActive(false);
            startMenu.SetActive(true);
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
}
