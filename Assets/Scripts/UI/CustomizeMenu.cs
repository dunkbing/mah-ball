// Created by Binh Bui on 06, 23, 2021

using Common;
using Entities;
using UnityEngine;
using UnityEngine.UI;
using Utilities;

namespace UI
{
    public class CustomizeMenu : Menu
    {
        public GameObject startMenu;
        public GameObject customizeMenu;
        public Button swordButton;
        public Button gunButton;
        public Button spikeButton;

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
            GameStats.Instance.currentPlayer.SetPlayerColor(image.color);
            GameStats.Instance.SaveStatsToFile();
        }

        public void LoadWeapons()
        {
            var weapons = GameStats.Instance.Weapons;

            if (weapons[WeaponType.Sword].Level > 0)
            {
                swordButton.interactable = true;
            }
            if (weapons[WeaponType.Gun].Level > 0)
            {
                gunButton.interactable = true;
            }
            if (weapons[WeaponType.Spike].Level > 0)
            {
                spikeButton.interactable = true;
            }
        }

        public void SelectWeapon(string wpName)
        {
            ObjectPool.Instance.Retrieve(GameStats.Instance.currentWeaponName);
            GameStats.Instance.currentWeaponName = wpName;
            if (wpName == WeaponType.Spike)
            {
                Spawner.Instance.PreStart();
            }
            else
            {
                GameStats.Instance.currentPlayer.SelectWeapon(wpName);
            }
        }
    }
}