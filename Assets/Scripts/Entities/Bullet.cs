// Created by Binh Bui on 06, 24, 2021

using UnityEngine;
using Utilities;

namespace Entities
{
    public class Bullet : Entity
    {
        private void OnCollisionEnter2D(Collision2D other)
        {
            ObjectPool.Instance.Spawn("BulletExplosion", other.transform.position, Quaternion.identity);
            Explode();
        }
    }
}