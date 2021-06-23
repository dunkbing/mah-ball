using System;
using System.IO;
using UnityEngine;

namespace Common
{
    public class GameStats : MonoBehaviour
    {
        public static bool GameIsPaused = true;
        public int Score { get; set; }
        public int HighScore { get; private set; }
        public Color PlayerColor { get; set; }
        public int Coin { get; set; }
        public int EnemyKilled { get; set; }
        public int TotalEnemyKilled { get; set; }

        public static GameStats Instance { get; private set; }

        private void Awake()
        {
            Instance = this;
            PlayerColor  = new Color(98, 238, 164);
            ReadFromFile();
        }

        public void ReadFromFile()
        {
            try
            {
                var highScoreTxt = File.ReadAllText(Constants.DataFilePath);
                var stats = Array.ConvertAll(highScoreTxt.Split('|'), float.Parse);
                HighScore = (int) stats[0];
                PlayerColor = new Color(stats[1], stats[2], stats[3]);
                Coin = (int) stats[4];
                TotalEnemyKilled = (int) stats[5];
            }
            catch (Exception e) when (e is FileNotFoundException || e is DirectoryNotFoundException ||
                                      e is IndexOutOfRangeException || e is FormatException)
            {
                Debug.Log(e.Message);
            }
        }

        public void SaveToFile()
        {
            if (Score > HighScore)
            {
                HighScore = Score;
            }

            Coin += Score / 10;
            TotalEnemyKilled += EnemyKilled;

            Score = 0;
            EnemyKilled = 0;

            var r = PlayerColor.r;
            var g = PlayerColor.g;
            var b = PlayerColor.b;
            File.WriteAllText(Constants.DataFilePath, $"{HighScore}|{r}|{g}|{b}|{Coin}|{TotalEnemyKilled}");
        }

        public void ResetStats()
        {
            Score = Coin = EnemyKilled = 0;
        }
    }
}
