using Common;
using UnityEngine;
using Utilities;

namespace Entities
{
    public class Virus : Enemy, ISpawn
    {
        private void Awake()
        {
            Damage = 60;

            OnExplode += () =>
            {
                AudioManager.Instance.Play("explosion");
                ObjectPool.Instance.Spawn("VirusExplosion", transform.position, Quaternion.identity, go =>
                {
                    go.GetComponent<ParticleSystem>().Play();
                });
                GameStats.Instance.EnemyKilled += 1;
            };
        }

        private void FixedUpdate()
        {
            Fall();
        }

        public void Spawn()
        {
            Health = Constants.VirusHealth;
            healthBar.SetMaxHealth(Health);
        }
    }
}
