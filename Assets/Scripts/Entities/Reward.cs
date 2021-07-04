using Common;
using UI;
using UnityEngine;
using Utilities;

namespace Entities
{
    public class Reward : Entity, IFalling
    {
        private Rigidbody2D _rb;
        public float speed = 1.5f;

        private void Awake()
        {
            _rb = GetComponent<Rigidbody2D>();

            OnExplode += () =>
            {
                AudioManager.Instance.Play("powerup");
                switch (gameObject.tag)
                {
                    case "Heart":
                        HUD.Instance.IncreaseScore(Constants.HeartScore);
                        HUD.Instance.IncreaseHealth();
                        ObjectPool.Instance.Spawn("HeartExplosion", transform.position, Quaternion.identity, go =>
                        {
                            go.GetComponent<ParticleSystem>().Play();
                        });
                        break;
                    case "Coin":
                        GameStats.Instance.Coin += Constants.CoinReward;
                        ObjectPool.Instance.Spawn("CoinExplosion", transform.position, Quaternion.identity, go =>
                        {
                            go.GetComponent<ParticleSystem>().Play();
                        });
                        break;
                }
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
                other.gameObject.GetComponent<Rigidbody2D>().AddForce(Vector2.up * 6, ForceMode2D.Impulse);
                other.gameObject.GetComponent<NormalBall>().RegenKi(Constants.RewardRegen);
                Explode();
            }
        }

        public void Fall()
        {
            if (GameStats.GameIsPaused) return;

            _rb.MovePosition(Vector3.down * (speed * Time.fixedDeltaTime) + transform.position);

            if (transform.position.y <= -5.5)
            {
                gameObject.SetActive(false);
            }
        }
    }
}
