using Common;
using UnityEngine;
using Utilities;

namespace Entities
{
    public class Virus : Enemy, ISpawn
    {
        private void Awake()
        {
            Name = nameof(Virus);
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
            // Spin();
            Fall();
        }

        private void Spin()
        {
            transform.Rotate(new Vector3(0, 0, 100f) * Time.fixedDeltaTime);
        }

        public void Spawn()
        {
            Health = Constants.VirusHealth;
            healthBar.SetMaxHealth(Health);
        }
    }
}
