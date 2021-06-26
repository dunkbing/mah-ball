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
            PreStartGame();
        }

        public void StartGame()
        {
            HUD.Instance.ResetLife();
            _objectPool.RetrieveAll();
            TimeManager.StopSlowMotion();
            SpawnPlayer();
            SpawnPlatform();
            InvokeRepeating(nameof(SpawnObject), .1f, 2f);
        }

        private void SpawnPlatform()
        {
            // bottom
            var bottom = Random.Range(1, 10) < 5;
            _objectPool.Spawn(nameof(Platform),
                bottom
                    ? new Vector3(Random.Range(-7f, -3f), Random.Range(-1.5f, -1f))
                    : new Vector3(Random.Range(3f, 7f), Random.Range(-1.5f, -1f)), Quaternion.identity, go =>
                {
                    go.GetComponent<Platform>().canSpin = Random.Range(1, 10) < 5;
                });
            // middle
            _objectPool.Spawn("BluePlatform", new Vector3(Random.Range(-1.2f, 1.2f), Random.Range(0.5f, 1f)), Quaternion.identity);
            // top
            var top = Random.Range(1, 10) < 5;
            _objectPool.Spawn("GreenPlatform",
                top
                    ? new Vector3(Random.Range(-7f, -3f), Random.Range(2.7f, 2.8f))
                    : new Vector3(Random.Range(3f, 7f), Random.Range(2.7f, 2.8f)), Quaternion.identity, go =>
                {
                    go.GetComponent<Platform>().canSpin = Random.Range(1, 10) < 5;
                });
        }

        public void PreStartGame()
        {
            _objectPool.RetrieveAll();
            SpawnPlayer();
            _objectPool.Spawn(nameof(Platform), new Vector3(0, 2.5f, 0), Quaternion.identity, go =>
            {
                var platform = go.GetComponent<Platform>();
                platform.firstPlatform = true;
                platform.canSpin = false;
            }).GetComponent<Platform>().speed = 0;
        }

        private void SpawnPlayer()
        {
            GameStats.Instance.currentPlayer = _objectPool.Spawn(GameStats.Instance.currentWeaponType == WeaponType.Spike ? "SpikePlayer" : nameof(Player),
                new Vector3(0, 3.5f, 0), Quaternion.identity, (go =>
                {
                    var currentWeapon = GameStats.Instance.currentWeaponType;
                    var rb = go.GetComponent<Rigidbody2D>();
                    if (currentWeapon == WeaponType.Gun || currentWeapon == WeaponType.Sword)
                    {
                        rb.mass = 10;
                    }
                    rb.constraints = currentWeapon == WeaponType.Gun ? RigidbodyConstraints2D.FreezeRotation : RigidbodyConstraints2D.None;

                    var tf = go.transform;
                    tf.localScale = currentWeapon == WeaponType.Spike ? new Vector3(0.25f, 0.25f) : new Vector3(0.7f, 0.7f);
                })).GetComponent<Player>();
        }

        private void SpawnObject()
        {
            var random = Random.Range(0f, 1f);
            var x = Random.Range(0f, 1f) < .5f ? Random.Range(-7f, -2.5f) : Random.Range(2.5f, 7f);
            if (random < 0.3f)
            {
                _objectPool.Spawn("Circle", new Vector3(x, 6, 0), Quaternion.identity);
            } else if (random >= 0.3 && random < 0.5)
            {
                _objectPool.Spawn("Virus", new Vector3(x, 6, 0), Quaternion.identity);
            } else if (random >= 0.5 && random < 0.7)
            {
                _objectPool.Spawn("Coin", new Vector3(x, 6, 0), Quaternion.identity);
            } else if (random >= 0.7f && random < 0.8f)
            {
                _objectPool.Spawn(nameof(Shooter), new Vector3(x, 6, 0), Quaternion.identity);
            }
            else
            {
                _objectPool.Spawn("Square", new Vector3(x, 6, 0), Quaternion.identity);
            }
        }

        // ReSharper disable Unity.PerformanceAnalysis
        public void StopSpawning()
        {
            CancelInvoke(nameof(SpawnObject));
        }
    }
}
