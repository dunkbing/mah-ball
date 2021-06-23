// Created by Binh Bui on 06, 22, 2021

using UnityEngine;

namespace UI
{
    public class StartMenu : Menu
    {
        public GameObject startMenu;

        public GameObject customizeMenu;

        private void Start()
        {
            startMenu.SetActive(true);
        }

        public void Play()
        {
            base.Resume();
            startMenu.SetActive(false);
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