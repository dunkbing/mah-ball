using Entities;
using UI;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Utilities
{
    public class Spawner : MonoBehaviour
    {
        public static Spawner Instance { get; private set; }

        private ObjectPool _objectPool;

        private void Awake()
        {
            Instance = this;
            _objectPool = ObjectPool.Instance;
        }

        private void Start()
        {
            PreStart();
        }

        public void StartGame()
        {
            HUD.Instance.ResetLife();
            _objectPool.RetrieveAll();
            TimeManager.StopSlowMotion();
            _objectPool.Spawn(nameof(Player), new Vector3(0, 3, 0), Quaternion.identity);
            _objectPool.Spawn(nameof(Platform), Vector3.up * 2, Quaternion.identity, go =>
            {
                go.GetComponent<Platform>().firstPlatform = true;
            });
            InvokeRepeating(nameof(SpawnObject), .1f, 2f);
        }

        private void PreStart()
        {
            _objectPool.Spawn(nameof(Player), new Vector3(0, 1, 0), Quaternion.identity);
            _objectPool.Spawn(nameof(Platform), Vector3.zero, Quaternion.identity, go =>
            {
                go.GetComponent<Platform>().firstPlatform = true;
            }).GetComponent<Platform>().speed = 0;
        }

        private void SpawnObject()
        {
            var random = Random.Range(0f, 1f);
            var x = Random.Range(0f, 1f) < .5f ? Random.Range(-7f, -2.5f) : Random.Range(2.5f, 7f);
            if (random < 0.2f)
            {
                _objectPool.Spawn("GreenPlatform", new Vector3(x, 6, 0), Quaternion.identity);
            } else if (random >= 0.2 && random < 0.35)
            {
                _objectPool.Spawn("Virus", new Vector3(x, 6, 0), Quaternion.identity);
            }
            else
            {
                _objectPool.Spawn(nameof(Platform), new Vector3(x, 6, 0), Quaternion.identity);
            }
        }

        // ReSharper disable Unity.PerformanceAnalysis
        public void StopSpawning()
        {
            CancelInvoke(nameof(SpawnObject));
        }
    }
}
