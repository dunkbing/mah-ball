using Common;
using UI;
using UnityEngine;
using Utilities;

namespace Entities
{
    public class Player : Entity, ISpawn
    {
        private PlayerController _playerController;

        private float _score;

        private void Awake()
        {
            _playerController = GetComponent<PlayerController>();
            OnExplode += (() =>
            {
                ObjectPool.Instance.Spawn("PlayerExplosion", transform.position, Quaternion.identity, go =>
                {
                    go.GetComponent<ParticleSystem>().Play();
                });
            });
        }

        private void Update()
        {
            if (GameStats.GameIsPaused) return;

            if (_score < 1)
            {
                _score += Time.deltaTime;
            }
            else
            {
                HUD.Instance.IncreaseScore(Constants.NormalScore);
                _score = 0;
            }
        }

        private void OnCollisionEnter2D(Collision2D other)
        {
            if (other.gameObject.CompareTag("Lava"))
            {
                Explode();
            }
        }

        public void Spawn()
        {
            transform.localScale = new Vector3(0.7f, 0.7f);
            _playerController.ResetEnergy();
        }
    }
}
