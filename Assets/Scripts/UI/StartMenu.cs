// Created by Binh Bui on 06, 22, 2021

using Common;
using UnityEngine;

namespace UI
{
    public class StartMenu : Menu
    {
        public GameObject startMenu;

        public GameObject customizeMenu;

        public GameObject hud;

        private void Start()
        {
            startMenu.SetActive(true);
        }

        public void Play()
        {
            base.Resume();
            startMenu.SetActive(false);
            hud.SetActive(true);
            GameStats.Instance.StartIncreasingScore();
        }

        public void Customize()
        {
            startMenu.SetActive(false);
            customizeMenu.SetActive(true);
        }

        public void Setting()
        {
            // TODO: implement later
        }
    }
}