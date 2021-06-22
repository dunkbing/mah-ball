// Created by Binh Bui on 06, 22, 2021

using Common;
using UnityEngine;
using Utilities;

namespace UI
{
    public class Menu: MonoBehaviour
    {
        public virtual void Resume()
        {
            AudioManager.Instance.Play("tap");
            GameStats.GameIsPaused = false;
            HUD.Instance.IncreaseScore(-GameStats.Score);
            Spawner.Instance.StartGame();
            PpvUtils.Instance.ExitSlowMo();
        }

        public virtual void Pause()
        {
            Spawner.Instance.StopSpawning();
            GameStats.GameIsPaused = true;
        }

        public void Quit()
        {
            Application.Quit();
        }
    }
}