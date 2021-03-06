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
        public GameObject adsButton;
        public Animator animator;

        public static PauseMenu Instance { get; private set; }

        // score ui
        public TextMeshProUGUI highScoreTmp;
        public TextMeshProUGUI coinTmp;

        private void Awake()
        {
            Instance ??= this;
        }

        private void Start()
        {
            pauseMenu.SetActive(false);
        }

        public override void Pause()
        {
            base.Pause();
            pauseMenu.SetActive(true);
            // randomly show ads button
            adsButton.SetActive(Random.Range(0, 1f) < 0.8);
            highScoreTmp.SetText($"High score: {GameStats.Instance.HighScore}");
            coinTmp.SetText($"Coin: {GameStats.Instance.Coin}");
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
            Spawner.Instance.PreStartGame();
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

        public void RequestAdsReward()
        {
            //AdsManager.Instance.ShowReward();
            //AdsManager.Instance.OnEarnedReward += () =>
            //{
            //    animator.Play("RewardAd");
            //    GameStats.Instance.Coin += 2000;
            //    coinTmp.SetText($"Coin: {GameStats.Instance.Coin}");
            //    GameStats.Instance.SaveStatsToFile();
            //};
        }
    }
}
