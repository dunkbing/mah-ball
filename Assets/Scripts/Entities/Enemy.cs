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

            if (transform.position.y <= -4.5)
            {
                Explode();
            }
        }

        public void TakeDamage(int damage)
        {
            Health -= damage;
            healthBar.SetHealth(Health);

            if (Health <= 0)
            {
                Explode();
            }
        }

        private void OnCollisionEnter2D(Collision2D other)
        {
            var gameStats = GameStats.Instance;
            var dmg = gameStats.CurrentWeapon.Damage;

            switch (other.gameObject.tag)
            {
                case "Player":
                    var player = other.gameObject.GetComponent<Player>();
                    switch (gameStats.currentWeaponType)
                    {
                        case WeaponType.Sword:
                            TakeDamage(dmg);
                            break;
                        case WeaponType.Gun:
                        {
                            player.TakeDamage(Constants.VirusDamage);
                            player.CheckLife();

                            TakeDamage(dmg);
                            if (Health <= 0)
                            {
                                Explode();
                            }

                            break;
                        }
                        case WeaponType.None:
                            player.TakeDamage(Constants.SpikePlayerHealth);
                            player.CheckLife();
                            break;
                    }
                    break;
                case "Bullet":
                    if (other.gameObject.name.Contains("PlayerBullet"))
                    {
                        TakeDamage(dmg);
                    }
                    break;
            }
        }
    }
}