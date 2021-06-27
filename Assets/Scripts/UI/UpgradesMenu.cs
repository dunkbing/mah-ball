// Created by Binh Bui on 06, 25, 2021

using Common;
using Entities;
using TMPro;
using UnityEngine;

namespace UI
{
    public class UpgradesMenu : Menu
    {
        public TextMeshProUGUI levelSwordTmp;
        public TextMeshProUGUI unlockSwordTmp;
        public TextMeshProUGUI levelGunTmp;
        public TextMeshProUGUI unlockGunTmp;
        public TextMeshProUGUI levelSpikeTmp;
        public TextMeshProUGUI unlockSpikeTmp;
        public TextMeshProUGUI coinTmp;

        private void Start()
        {
            gameObject.SetActive(false);
        }

        public void UpgradeWeapon(string type)
        {
            GameStats.Instance.UpgradeWeapon(type);
            LoadWeaponInfo();
        }

        public void UpgradeStats(string type)
        {
            GameStats.Instance.UpgradeStats(type);
            LoadWeaponInfo();
        }

        public void LoadWeaponInfo()
        {
            var weapons = GameStats.Instance.Weapons;
            if (weapons[WeaponType.Sword].Level > 0)
            {
                levelSwordTmp.SetText($"LEVEL: {weapons[WeaponType.Sword].Level}");
                unlockSwordTmp.SetText($"UPGRADE: ${weapons[WeaponType.Sword].Price}");
            } else
            {
                levelSwordTmp.SetText("LEVEL 0");
                unlockSwordTmp.SetText($"UNLOCK: ${weapons[WeaponType.Sword].Price}");
            }

            // if (weapons[WeaponType.Gun].Level > 0)
            // {
            //     levelGunTmp.SetText($"LEVEL: {weapons[WeaponType.Gun].Level}");
            //     unlockGunTmp.SetText($"UPGRADE: ${weapons[WeaponType.Gun].Price}");
            // } else
            // {
            //     levelGunTmp.SetText("LEVEL: 0");
            //     unlockGunTmp.SetText($"UNLOCK: ${weapons[WeaponType.Gun].Price}");
            // }

            if (weapons[WeaponType.Spike].Level > 0)
            {
                levelSpikeTmp.SetText($"LEVEL: {weapons[WeaponType.Spike].Level}");
                unlockSpikeTmp.SetText($"UPGRADE: ${weapons[WeaponType.Spike].Price}");
            } else
            {
                levelSpikeTmp.SetText("LEVEL: 0");
                unlockSpikeTmp.SetText($"UNLOCK: ${weapons[WeaponType.Spike].Price}");
            }

            // coin
            coinTmp.SetText($"${GameStats.Instance.Coin}");
        }
    }
}