using System;
using System.IO;
using UnityEngine;

namespace Common
{
    public class GameStats : MonoBehaviour
    {
        public static bool GameIsPaused = true;
        public static int Score;
        public static int HighScore;
        public static Color PlayerColor = new Color(98, 238, 164);

        private void Awake()
        {
            ReadFromFile();
        }

        public static void ReadFromFile()
        {
            try
            {
                var highScoreTxt = File.ReadAllText(Constants.DataFilePath);
                Debug.Log(Constants.DataFilePath);
                var strings = Array.ConvertAll(highScoreTxt.Split('|'), float.Parse);
                HighScore = (int) strings[0];
                PlayerColor = new Color(strings[1], strings[2], strings[3]);
            }
            catch (Exception e) when(e is FileNotFoundException || e is DirectoryNotFoundException || e is IndexOutOfRangeException || e is FormatException)
            {
                Debug.Log(e.Message);
            }
        }

        public static void SaveToFile()
        {
            if (Score > HighScore)
            {
                HighScore = Score;
            }

            Score = 0;
            var r = PlayerColor.r;
            var g = PlayerColor.g;
            var b = PlayerColor.b;
            File.WriteAllText(Constants.DataFilePath, $"{HighScore}|{r}|{g}|{b}");
        }
    }
}
