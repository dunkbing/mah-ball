// Created by Binh Bui on 06, 23, 2021

using UnityEngine;
using UnityEngine.Rendering.PostProcessing;
using Utilities;

namespace UI
{
    public class SettingMenu : Menu
    {
        public GameObject startMenu;
        public GameObject settingMenu;
        public PostProcessVolume volume;

        public void Setting()
        {
            startMenu.SetActive(false);
            settingMenu.SetActive(true);
        }

        public void SetMusicVolume(float volume)
        {
            AudioManager.Instance.SetVolume("bgmusic", volume);
        }

        public void SetSfxVolume(float volume)
        {
            AudioManager.Instance.SetVolume("tap", volume);
            AudioManager.Instance.SetVolume("explosion", volume);
            AudioManager.Instance.SetVolume("shoot", volume);
            AudioManager.Instance.SetVolume("powerup", volume);
        }

        public void UseEffect(bool use)
        {
            volume.enabled = use;
        }
    }
}