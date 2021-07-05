using System;
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
            Instance ??= this;
        }

        private void Start()
        {
            _objectPool = ObjectPool.Instance;
            PreStartGame();
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
            InvokeRepeating(nameof(SpawnObject), .1f, 1.5f);
        }

        public void PreStartGame()
        {
            _objectPool.RetrieveAll();
            SpawnPlayer();
            _objectPool.Spawn("Platform", new Vector3(0, 2.5f, 0), Quaternion.identity, go =>
            {
                go.GetComponent<Platform>().firstPlatform = true;
            }).GetComponent<Platform>().speed = 0;
        }

        private void SpawnPlayer()
        {
            var playerType = GameStats.Instance.currentWeaponType switch
            {
                WeaponType.None => nameof(NormalBall),
                WeaponType.BLob => nameof(BlobBall),
                WeaponType.Spike => "SpikePlayer",
                _ => nameof(NormalBall),
            };

            GameStats.Instance.currentBall = _objectPool.Spawn(playerType,
                new Vector3(0, 3.5f, 0), Quaternion.identity, (go =>
                {
                    var currentWeapon = GameStats.Instance.currentWeaponType;
                    var rb = go.GetComponent<Rigidbody2D>();
                    var tf = go.transform;

                    switch (currentWeapon)
                    {
                        case WeaponType.BLob:
                            tf.localScale = new Vector3(0.2f, 0.2f);
                            break;
                        case WeaponType.Sword:
                            rb.mass = 22;
                            tf.localScale = new Vector3(0.7f, 0.7f);
                            break;
                        case WeaponType.None:
                            rb.mass = 16;
                            break;
                        case WeaponType.Spike:
                            tf.localScale = new Vector3(0.25f, 0.25f);
                            break;
                    }
                })).GetComponent<NormalBall>();
        }

        private void SpawnObject()
        {
            var random = Random.Range(0f, 1f);
            if (random < 0.4f)
            {
                SpawnPlatform();
            } else if (random >= 0.4 && random < 0.7)
            {
                SpawnReward();
            } else if (random >= 0.7 && random < 1)
            {
                SpawnEnemy();
            }
        }

        private void SpawnPlatform()
        {
            var random = Random.Range(0f, 1f);
            var x = Random.Range(0f, 1f) < .5f ? Random.Range(-7f, -2.5f) : Random.Range(2.5f, 7f);
            if (random < 0.3f)
            {
                _objectPool.Spawn("GreenPlatform", new Vector3(x, 6, 0), Quaternion.identity);
            } else if (random >= 0.3f && random < 0.66)
            {
                _objectPool.Spawn("BluePlatform", new Vector3(x, 6, 0), Quaternion.identity);
            }
            else
            {
                _objectPool.Spawn(nameof(Platform), new Vector3(x, 6, 0), Quaternion.identity);
            }
        }

        private void SpawnEnemy()
        {
            var random = Random.Range(0f, 1f);
            var x = Random.Range(0f, 1f) < .5f ? Random.Range(-7f, -2.5f) : Random.Range(2.5f, 7f);

            if (random < 0.3f)
            {
                _objectPool.Spawn("Virus", new Vector3(x, 6, 0), Quaternion.identity);
            } else if (random >= 0.3f && random < 0.6f)
            {
                _objectPool.Spawn("Star", new Vector3(x, 6, 0), Quaternion.identity);
            } else if (random >= 0.6f && random < 0.9)
            {
                _objectPool.Spawn("Square", new Vector3(x, 6, 0), Quaternion.identity);
            }
        }

        private void SpawnReward()
        {
            var random = Random.Range(0f, 1f);
            var x = Random.Range(0f, 1f) < .5f ? Random.Range(-7f, -2.5f) : Random.Range(2.5f, 7f);

            if (random < 0.3f)
            {
                _objectPool.Spawn("Circle", new Vector3(x, 6, 0), Quaternion.identity);
            } else if (random >= 0.3f && random < 0.6f)
            {
                _objectPool.Spawn("Coin", new Vector3(x, 6, 0), Quaternion.identity);
            } else if (random >= 0.6f && random < 0.9)
            {
                _objectPool.Spawn("Heart", new Vector3(x, 6, 0), Quaternion.identity);
            }
        }

        // ReSharper disable Unity.PerformanceAnalysis
        public void StopSpawning()
        {
            CancelInvoke(nameof(SpawnObject));
        }
    }
}
