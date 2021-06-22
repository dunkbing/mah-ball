// Created by Binh Bui on 06, 22, 2021

using System.IO;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class HUD : MonoBehaviour
{
    public Slider slider;
    public TextMeshProUGUI scoreTmp;

    public static HUD Instance { get; private set; }

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
        GameStats.Score += score;
        scoreTmp.SetText($"${GameStats.Score}");
    }

    public void SaveScore()
    {
        if (GameStats.Score > GameStats.HighScore)
        {
            GameStats.HighScore = GameStats.Score;
        }

        GameStats.Score = 0;
        File.WriteAllText(Constants.DataFilePath, GameStats.HighScore.ToString());
    }
    #endregion
}