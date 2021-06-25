// Created by Binh Bui on 06, 25, 2021

using Common;
using UnityEngine;

namespace Entities
{
    public abstract class Enemy : Entity, IFalling, IDamageable
    {
        public Rigidbody2D rigidBody;
        public float speed = 1.5f;
        public HealthBar healthBar;

        protected int Health { get; set; }

        public void Fall()
        {
            if (GameStats.GameIsPaused) return;
            rigidBody.MovePosition(Vector3.down * (speed * Time.fixedDeltaTime) + transform.position);
        }

        public void TakeDamage(int damage)
        {
            Health -= damage;
            healthBar.SetHealth(Health);
        }
    }
}