using Common;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Entities
{
    public class Lava : MonoBehaviour
    {
        // User Inputs
        public float amplitude;
        public float frequency = 1f;


        // Position Storage Variables
        private Vector3 _posOffset;
        private Vector3 _tempPos;

        private void Awake () {
            // Store the starting position & rotation of the object
            _posOffset = transform.position;
            amplitude = Random.Range(.4f, 1f);
        }

        // Update is called once per frame
        private void FixedUpdate ()
        {
            Float();
        }

        private void Float()
        {
            // Float up/down with a Sin()
            _tempPos = _posOffset;
            _tempPos.y += Mathf.Sin (Time.fixedTime * Mathf.PI * frequency) * amplitude;

            transform.position = _tempPos;
        }

        private void OnCollisionStay2D(Collision2D other)
        {
            HandleCollision(other);
        }

        private void OnCollisionEnter2D(Collision2D other)
        {
            HandleCollision(other);
        }

        private static void HandleCollision(Collision2D other)
        {
            if (!other.collider.CompareTag("Player")) return;

            GameStats.Instance.currentBall.TakeDamage(Constants.LavaDamage, 0);
        }
    }
}
