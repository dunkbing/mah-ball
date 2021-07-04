// Created by Binh Bui on 06, 24, 2021

using UnityEngine;
using Utilities;

namespace Entities
{
    public class Gun : MonoBehaviour
    {
        public GameObject gunPivot;
        public Transform shootPoint;
        private readonly float _bulletForce = 25;

        public void Aim(Transform target)
        {
            var difference = target.transform.position - gunPivot.transform.position;
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

        public void Shoot(string bulletName)
        {
            var rb = ObjectPool.Instance.Spawn(bulletName, shootPoint.position, Quaternion.identity).GetComponent<Rigidbody2D>();
            rb.AddForce(shootPoint.transform.up * _bulletForce, ForceMode2D.Impulse);
        }
    }
}