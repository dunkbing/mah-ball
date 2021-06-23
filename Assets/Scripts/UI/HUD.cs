// Created by Binh Bui on 06, 22, 2021

using System.Collections.Generic;
using System.Linq;
using Common;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace UI
{
    public class HUD : MonoBehaviour
    {
        public Slider slider;
        public TextMeshProUGUI scoreTmp;
        public List<GameObject> hearts;

        public Animator scoreAnim;

        private float _deltaTime;

        public static HUD Instance { get; private set; }

        private void Start()
        {
            Application.targetFrameRate = 60;
        }

        private void Update()
        {
            _deltaTime += (Time.unscaledDeltaTime - _deltaTime) * 0.1f;
        }

        // display fps
        private void OnGUI()
        {
            int w = Screen.width, h = Screen.height;

            var style = new GUIStyle();

            var rect = new Rect(0, 0, w, h * 2f / 100);
            style.alignment = TextAnchor.UpperRight;
            style.fontSize = h * 3 / 100;
            style.normal.textColor = Color.red;
            var msec = _deltaTime * 1000.0f;
            var fps = 1.0f / _deltaTime;
            var text = $"{msec:0.0} ms ({fps:0.} fps)";
            GUI.Label(rect, text, style);
        }

        private void Awake()
        {
            Instance = this;
        }

        #region energy bar
        public void SetMaxEnergy(float maxEnergy)
        {
            slider.maxValue = maxEnergy;
        }

        public void SetEnergy(float energy)
        {
            slider.value = energy;
        }
        #endregion

        #region score tmp
        public void IncreaseScore(int score)
        {
            GameStats.Instance.Score += score;
            if (score > Constants.NormalScore)
            {
                scoreAnim.Play("Score");
            }
            scoreTmp.SetText($"Score: {GameStats.Instance.Score}");
        }
        #endregion

        #region heart counter
        public void IncreaseHealth()
        {
            if (hearts.Last().activeSelf) return;
            foreach (var heart in hearts.Where(heart => !heart.activeSelf))
            {
                heart.SetActive(true);
                break;
            }
        }

        public void DecreaseHealth()
        {
            for (var i = hearts.Count - 1; i >= 0; i--)
            {
                if (hearts[i].activeSelf)
                {
                    hearts[i].SetActive(false);
                    break;
                }
            }
        }

        public void ResetLife()
        {
            hearts[0].SetActive(true);
        }

        public bool IsEmptyLife()
        {
            return !hearts.First().activeSelf;
        }
        #endregion

        #region Stats
        public void ShowStats(TextMeshProUGUI tmp)
        {
            int highScore = GameStats.Instance.HighScore;
            int coin = GameStats.Instance.Coin;
            int enemyKilled = GameStats.Instance.TotalEnemyKilled;
            tmp.SetText($"High score: {highScore}\nCoin: {coin}\nEnemy killed: {enemyKilled}");
        }
        #endregion
    }
}