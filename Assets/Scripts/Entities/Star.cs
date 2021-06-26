// Created by Binh Bui on 06, 24, 2021

using Common;
using UnityEngine;
using Utilities;

namespace Entities
{
    public class Star : Enemy, ISpawn
    {
        private GameObject _target;

        public Gun gun;

        private void Awake()
        {
            Damage = 30;
            InvokeRepeating(nameof(Shoot), 0.5f, 0.3f);

            OnExplode += () =>
            {
                AudioManager.Instance.Play("explosion");
                ObjectPool.Instance.Spawn("StarExplosion", transform.position, Quaternion.identity, go =>
                {
                    go.GetComponent<ParticleSystem>().Play();
                });
                GameStats.Instance.EnemyKilled += 1;
            };
        }

        private void FixedUpdate()
        {
            if (_target)
            {
                gun.Aim(_target.transform);
            }
            Fall();
        }

        private void OnTriggerEnter2D(Collider2D other)
        {
            HandleTrigger(other);
        }

        private void OnTriggerStay2D(Collider2D other)
        {
            HandleTrigger(other);
        }

        private void OnTriggerExit2D(Collider2D other)
        {
            if (_target == other.gameObject)
            {
                _target = null;
            }
        }

        private void HandleTrigger(Component other)
        {
            if (_target == null && other.CompareTag("Player"))
            {
                _target = other.gameObject;
            }
        }

        private void Shoot()
        {
            if (_target)
            {
                gun.Shoot("Bullet");
            }
        }

        public void Spawn()
        {
            Health = Constants.StarHealth;
            healthBar.SetMaxHealth(Health);
        }
    }
}