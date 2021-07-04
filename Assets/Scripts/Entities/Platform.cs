using Common;
using UnityEngine;
using Utilities;

namespace Entities
{
    public class Platform : Entity, ISpawn, IFalling
    {
        public Rigidbody2D rb;
        public float speed = 1.5f;

        public bool firstPlatform;

        public bool canSpin;

        private readonly Vector3 _spinSpeed = new Vector3(0, 0, 20f);

        public void Spawn()
        {
            speed = 1.5f;

            // spawning heart
            if (!firstPlatform && Random.Range(0, 1f) < Constants.SpawningHeartRate)
            {
                var position = transform.position;
                float x = Random.Range(position.x - 1.5f, position.x + 1.5f);
                ObjectPool.Instance.Spawn(nameof(Reward), new Vector3(x, position.y + 0.8f), Quaternion.identity);
            }
        }

        private void FixedUpdate()
        {
            Fall();
            // if (canSpin)
            // {
            //     Spin();
            // }
        }

        private void Spin()
        {
            transform.Rotate(_spinSpeed * Time.fixedDeltaTime);
        }

        public void Fall()
        {
            if (GameStats.GameIsPaused) return;
            rb.MovePosition(Vector3.down * (speed * Time.fixedDeltaTime) + transform.position);

            if (transform.position.y <= -5.5)
            {
                gameObject.SetActive(false);
            }
        }
    }
}
