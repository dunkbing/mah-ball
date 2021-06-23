using Common;
using UI;
using UnityEngine;
using Utilities;

namespace Entities
{
    public class Heart : Entity, IFalling
    {
        private Rigidbody2D _rb;
        public float speed = 1.5f;

        private void Awake()
        {
            _rb = GetComponent<Rigidbody2D>();
        }

        private void FixedUpdate()
        {
            Fall();
        }

        private void OnCollisionEnter2D(Collision2D other)
        {
            if (other.gameObject.CompareTag("Player"))
            {
                HUD.Instance.IncreaseScore(Constants.HeartScore);
                HUD.Instance.IncreaseHealth();
                AudioManager.Instance.Play("powerup");
                other.gameObject.GetComponent<Rigidbody2D>().AddForce(Vector2.up * 6, ForceMode2D.Impulse);
                ObjectPool.Instance.Spawn("HeartExplosion", transform.position, Quaternion.identity, go =>
                {
                    go.GetComponent<ParticleSystem>().Play();
                });
                other.gameObject.GetComponent<Player>().ResetEnergy();
                Explode();
            }
        }

        public void Fall()
        {
            if (GameStats.GameIsPaused) return;
            _rb.MovePosition(Vector3.down * (speed * Time.fixedDeltaTime) + transform.position);
        }
    }
}
