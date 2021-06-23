// Created by Binh Bui on 06, 23, 2021

using Common;
using Entities;
using UnityEngine;
using UnityEngine.UI;

namespace UI
{
    public class CustomizeMenu : Menu
    {
        public GameObject startMenu;
        public GameObject customizeMenu;

        private void Start()
        {
            customizeMenu.SetActive(false);
        }

        public void Back()
        {
            customizeMenu.SetActive(false);
            startMenu.SetActive(true);
        }

        public void SelectColor(Image image)
        {
            Player.Instance.SetPlayerColor(image.color);
            GameStats.Instance.SaveToFile();
        }
    }
}