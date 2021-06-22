using UI;
using UnityEngine;
using Utilities;
using Random = UnityEngine.Random;

namespace Entities
{
    public class Lava : MonoBehaviour
    {
        // User Inputs
        public float amplitude;
        public float frequency = 1f;

        private bool _floatDir;

        // Position Storage Variables
        private Vector3 _posOffset;
        private Vector3 _tempPos;

        private void Awake () {
            // Store the starting position & rotation of the object
            _posOffset = transform.position;
            amplitude = Random.Range(.4f, 1f);
            _floatDir = Random.Range(.5f, 1f) > .5f;
        }

        // Update is called once per frame
        private void Update () {
            // Float up/down with a Sin()
            _tempPos = _posOffset;
            _tempPos.y += Mathf.Sin (Time.fixedTime * Mathf.PI * frequency) * amplitude;

            transform.position = _floatDir ? _tempPos : -_tempPos;
        }

        private void OnCollisionEnter2D(Collision2D other)
        {
            if (other.collider.CompareTag("Player"))
            {
                AudioManager.Instance.Play("explosion");
                HUD.Instance.DecreaseHealth();
                if (!HUD.Instance.IsEmptyLife())
                {
                    ObjectPool.Instance.Spawn(nameof(Player), new Vector3(0, 1, 0), Quaternion.identity);
                }
                else
                {
                    HUD.Instance.SaveScore();
                    PauseMenu.Instance.Pause();
                }
            }
        }

    }
}