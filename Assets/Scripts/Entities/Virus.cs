using Common;
using UI;
using UnityEngine;
using Utilities;

namespace Entities
{
    public class Virus : Entity, IFalling
    {
        private Rigidbody2D _rb;
        public float speed = 1.5f;

        private void Awake()
        {
            _rb = GetComponent<Rigidbody2D>();

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

        private void OnCollisionEnter2D(Collision2D other)
        {
            if (other.gameObject.CompareTag("Player"))
            {
                other.gameObject.GetComponent<Player>().Explode();
                Explode();
                HUD.Instance.DecreaseHealth();
                if (!HUD.Instance.IsEmptyLife())
                {
                    ObjectPool.Instance.Spawn(nameof(Player), new Vector3(0, 1, 0), Quaternion.identity);
                }
                else
                {
                    GameStats.Instance.SaveToFile();
                    PauseMenu.Instance.Pause();
                    Spawner.Instance.PreStart();
                }
            }
        }

        public void Fall()
        {
            if (GameStats.GameIsPaused) return;
            _rb.MovePosition(Vector3.down * (speed * Time.fixedDeltaTime) + transform.position);
        }
    }
}
