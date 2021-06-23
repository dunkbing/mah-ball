using Common;
using UnityEngine;
using Utilities;

namespace Entities
{
    public class Platform : Entity, ISpawn
    {
        private Rigidbody2D _rb;
        public float speed = 1.5f;

        public bool firstPlatform;

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

        private void Awake()
        {
            _rb = GetComponent<Rigidbody2D>();
        }

        private void FixedUpdate()
        {
            if (GameStats.GameIsPaused) return;
            _rb.MovePosition(Vector3.down * (speed * Time.fixedDeltaTime) + transform.position);
        }
    }
}
