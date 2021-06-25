using Common;
using UI;
using UnityEngine;
using Utilities;

namespace Entities
{
    public class Virus : Enemy
    {
        private void Awake()
        {
            Health = 100;
            healthBar.SetMaxHealth(Health);
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

        private void OnCollisionEnter2D(Collision2D other)
        {
            if (other.gameObject.CompareTag("Player"))
            {

                HUD.Instance.DecreaseHealth();
                if (!HUD.Instance.IsEmptyLife())
                {
                    ObjectPool.Instance.Spawn(nameof(Player), new Vector3(0, 1, 0), Quaternion.identity);
                }
                else
                {
                    GameStats.Instance.SaveStatsToFile();
                    PauseMenu.Instance.Pause();
                }
                if (Health <= 0)
                {
                    Explode();
                }
            }
        }
    }
}
