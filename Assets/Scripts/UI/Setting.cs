// Created by Binh Bui on 06, 23, 2021

using Common;
using TMPro;
using UnityEngine;
using Utilities;

namespace UI
{
    public class Setting : Menu
    {
        public GameObject startMenu;
        public TextMeshProUGUI playTmp;

        public override void Pause()
        {
            if (GameStats.GameIsPaused)
            {
                startMenu.SetActive(false);
                GameStats.GameIsPaused = false;
            }
            else
            {
                playTmp.SetText("RESUME");
                startMenu.SetActive(true);
                GameStats.GameIsPaused = true;
                TimeManager.DaWarudo();
            }
        }
    }
}