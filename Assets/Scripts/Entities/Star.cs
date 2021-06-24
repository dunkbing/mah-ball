// Created by Binh Bui on 06, 24, 2021

using UnityEngine;

namespace Entities
{
    public class Star : Entity
    {
        public GameObject gunPivot;
        public GameObject bullet;
        public Transform shootPoint;
        private GameObject _target;

        private void Awake()
        {
            InvokeRepeating(nameof(Shoot), 0.5f, 1f);
        }

        private void FixedUpdate()
        {
            Aim();
        }

        private void Aim()
        {
            if (_target)
            {
                var difference = _target.transform.position - gunPivot.transform.position;
                difference.Normalize();
                var rotationZ = Mathf.Atan2(difference.y, difference.x) * Mathf.Rad2Deg;
                gunPivot.transform.rotation = Quaternion.Euler(0f, 0f, rotationZ);

                // gun on the other side of the body
                if (rotationZ < -90 || rotationZ > 90)
                {
                    // facing right
                    if (transform.eulerAngles.y == 0)
                    {
                        gunPivot.transform.localRotation = Quaternion.Euler(180, 0f, -rotationZ);
                    }
                } else if (transform.eulerAngles.y == 180)
                {
                    gunPivot.transform.localRotation = Quaternion.Euler(180, 180, -rotationZ);
                }
            }
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
            if (_target == null && other.CompareTag("Virus"))
            {
                _target = other.gameObject;
            }
        }

        private void Shoot()
        {
            if (_target)
            {
                var rb = Instantiate(bullet, shootPoint.position, Quaternion.identity).GetComponent<Rigidbody2D>();
                rb.AddForce(shootPoint.transform.up * 20, ForceMode2D.Impulse);
            }
        }
    }
}