// Created by Binh Bui on 06, 22, 2021

using System.Collections.Generic;
using System.Linq;
using Common;
using Entities;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace UI
{
    public class HUD : MonoBehaviour
    {
        public Slider energyBar;
        public HealthBar healthBar;
        public TextMeshProUGUI scoreTmp;
        public List<GameObject> hearts;

        public Animator scoreAnim;

        private float _deltaTime;

        public static HUD Instance { get; private set; }

        private void Start()
        {
            Application.targetFrameRate = 60;
            gameObject.SetActive(false);
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
            Instance ??= this;
        }

        #region energy bar
        public void SetMaxEnergy(float maxEnergy)
        {
            energyBar.maxValue = maxEnergy;
        }

        public void SetEnergy(float energy)
        {
            energyBar.value = energy;
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
            var gameStats = GameStats.Instance;
            var highScore = gameStats.HighScore;
            var coin = gameStats.Coin;
            var enemyKilled = gameStats.TotalEnemyKilled;
            var hp = GameStats.MaxHealth;
            var ki = GameStats.MaxEnergy;
            var power = gameStats.Power;
            tmp.SetText($"Hp: {hp}\nKi: {ki}\nPower: {power}\nHigh score: {highScore}\nCoin: {coin}\nEnemy killed: {enemyKilled}");
        }
        #endregion
    }
}