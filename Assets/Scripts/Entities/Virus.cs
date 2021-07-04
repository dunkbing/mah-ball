using Common;
using UnityEngine;
using Utilities;

namespace Entities
{
    public class Virus : Enemy, ISpawn
    {
        public SpriteRenderer sr;

        private void Awake()
        {
            Damage = 60;

            OnExplode += () =>
            {
                AudioManager.Instance.Play("explosion");
                ObjectPool.Instance.Spawn("VirusExplosion", transform.position, Quaternion.identity, go =>
                {
                    var ps = go.GetComponent<ParticleSystem>();
                    var psMain = ps.main;
                    psMain.startColor = sr.color;
                    ps.Play();
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
