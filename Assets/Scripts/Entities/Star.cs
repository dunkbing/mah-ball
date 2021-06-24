// Created by Binh Bui on 06, 24, 2021

using Common;
using UnityEngine;
using Utilities;

namespace Entities
{
    public class Star : Entity, IFalling
    {
        private GameObject _target;
        public Rigidbody2D rigidBody;
        public float speed = 1.5f;
        public Gun gun;

        private void Awake()
        {
            InvokeRepeating(nameof(Shoot), 0.5f, 0.3f);
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
                gun.Shoot();
            }
        }

        public void Fall()
        {
            if (GameStats.GameIsPaused) return;
            rigidBody.MovePosition(Vector3.down * (speed * Time.fixedDeltaTime) + transform.position);
        }
    }
}