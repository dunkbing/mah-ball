// Created by Binh Bui on 06, 22, 2021

using Common;
using UnityEngine;
using Utilities;

namespace UI
{
    public class Menu: MonoBehaviour
    {
        protected void Resume()
        {
            GameStats.GameIsPaused = false;
            HUD.Instance.IncreaseScore(-GameStats.Instance.Score);
            Spawner.Instance.StartGame();
            PpvUtils.Instance.Disable();
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

        public void PlaySound()
        {
            AudioManager.Instance.Play("tap");
        }
    }
}