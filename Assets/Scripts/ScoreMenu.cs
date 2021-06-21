using System;
using TMPro;
using UnityEngine;

public class ScoreMenu : MonoBehaviour
{
    public GameObject scoreTmpGo;
    private TextMeshProUGUI _scoreTmp;

    public static ScoreMenu Instance { get; private set; }

    private void Awake()
    {
        Instance = this;
        _scoreTmp = scoreTmpGo.GetComponent<TextMeshProUGUI>();
    }

    public void IncreaseScore(int score)
    {
        GameStats.Score += score;
        _scoreTmp.SetText($"Score: {GameStats.Score}");
    }
}