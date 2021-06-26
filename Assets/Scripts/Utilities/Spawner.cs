using Common;
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
            SpawnPlayer();
            _objectPool.Spawn(nameof(Platform), Vector3.up * 2, Quaternion.identity, go =>
            {
                go.GetComponent<Platform>().firstPlatform = true;
            });
            InvokeRepeating(nameof(SpawnObject), .1f, 2f);
        }

        public void PreStart()
        {
            _objectPool.RetrieveAll();
            SpawnPlayer();
            _objectPool.Spawn(nameof(Platform), new Vector3(0, 2.5f, 0), Quaternion.identity, go =>
            {
                go.GetComponent<Platform>().firstPlatform = true;
            }).GetComponent<Platform>().speed = 0;
        }

        private void SpawnPlayer()
        {
            GameStats.Instance.currentPlayer = _objectPool.Spawn(GameStats.Instance.currentWeaponType == WeaponType.Spike ? "SpikePlayer" : nameof(Player),
                new Vector3(0, 3.5f, 0), Quaternion.identity, (go =>
                {
                    var rb = go.GetComponent<Rigidbody2D>();
                    if (GameStats.Instance.currentWeaponType == WeaponType.Gun)
                    {
                        rb.constraints = RigidbodyConstraints2D.FreezeRotation;
                    }
                    else
                    {
                        rb.constraints = RigidbodyConstraints2D.None;
                    }
                })).GetComponent<Player>();
        }

        private void SpawnObject()
        {
            var random = Random.Range(0f, 1f);
            var x = Random.Range(0f, 1f) < .5f ? Random.Range(-7f, -2.5f) : Random.Range(2.5f, 7f);
            if (random < 0.2f)
            {
                _objectPool.Spawn("GreenPlatform", new Vector3(x, 6, 0), Quaternion.identity);
            } else if (random >= 0.2 && random < 0.4)
            {
                _objectPool.Spawn("Virus", new Vector3(x, 6, 0), Quaternion.identity);
            } else if (random >= 0.4 && random < 0.7)
            {
                _objectPool.Spawn("Coin", new Vector3(x, 6, 0), Quaternion.identity);
            } else if (random >= 0.7f && random < 0.8f)
            {
                _objectPool.Spawn(nameof(Star), new Vector3(x, 6, 0), Quaternion.identity);
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
