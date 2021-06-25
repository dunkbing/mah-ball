// Created by Binh Bui on 06, 25, 2021

using Common;
using UnityEngine;
using UnityEngine.UI;

namespace Entities
{
    public class HealthBar : MonoBehaviour
    {
        public Slider slider;
        public float speed;

        public void SetMaxHealth(int health)
        {
            slider.maxValue = health;
            slider.value = health;
        }

        public void SetHealth(int health)
        {
            slider.value = health;
        }

        private void FixedUpdate()
        {
            Fall();
        }

        private void Fall()
        {
            if (GameStats.GameIsPaused) return;
            transform.Translate(Vector3.down * (speed * Time.fixedDeltaTime));
            // _rb.MovePosition(Vector3.down * (speed * Time.fixedDeltaTime) + transform.position);
        }
    }
}
