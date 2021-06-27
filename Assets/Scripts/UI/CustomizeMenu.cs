// Created by Binh Bui on 06, 23, 2021

using Common;
using Entities;
using TMPro;
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
        public TextMeshProUGUI weaponInfoTmp;

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
            // if (weapons[WeaponType.Gun].Level > 0)
            // {
            //     gunButton.interactable = true;
            // }
            if (weapons[WeaponType.Spike].Level > 0)
            {
                spikeButton.interactable = true;
            }

            weaponInfoTmp.SetText(GameStats.Instance.currentWeaponType != WeaponType.None
                ? GameStats.Instance.CurrentWeapon.Info()
                : string.Empty);
        }

        public void SelectWeapon(string wpName)
        {
            if (wpName == GameStats.Instance.currentWeaponType) return;

            ObjectPool.Instance.Retrieve(GameStats.Instance.currentWeaponType);
            if (GameStats.Instance.currentWeaponType == WeaponType.Spike && GameStats.Instance.currentWeaponType != wpName)
            {
                GameStats.Instance.currentWeaponType = wpName;
                Spawner.Instance.PreStartGame();
            }
            GameStats.Instance.currentWeaponType = wpName;
            if (wpName == WeaponType.Spike)
            {
                Spawner.Instance.PreStartGame();
            }
            else
            {
                GameStats.Instance.currentPlayer.SelectWeapon(wpName);
            }

            weaponInfoTmp.SetText(GameStats.Instance.currentWeaponType != WeaponType.None
                ? GameStats.Instance.CurrentWeapon.Info()
                : string.Empty);
        }
    }
}