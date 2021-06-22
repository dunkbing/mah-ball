// Created by Binh Bui on 06, 22, 2021

using UnityEngine;

namespace UI
{
    public class StartMenu : Menu
    {
        public GameObject startMenu;

        private void Start()
        {
            startMenu.SetActive(true);
        }

        public void Play()
        {
            base.Resume();
            startMenu.SetActive(false);
        }

        public void Shop()
        {
            // TODO: implement later
        }

        public void Setting()
        {
            // TODO: implement later
        }
    }
}