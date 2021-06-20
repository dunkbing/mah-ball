using UnityEngine;
using Random = UnityEngine.Random;

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
        _objectPool.RetrieveAll();
        TimeManager.StopSlowMotion();
        _objectPool.Spawn(nameof(Player), new Vector3(0, 3, 0), Quaternion.identity);
        _objectPool.Spawn(nameof(Platform), Vector3.up * 2, Quaternion.identity);
        InvokeRepeating(nameof(SpawnPlatform), .1f, 2f);
    }

    private void PreStart()
    {
        _objectPool.Spawn(nameof(Player), new Vector3(0, 1, 0), Quaternion.identity);
        _objectPool.Spawn(nameof(Platform), Vector3.zero, Quaternion.identity).GetComponent<Platform>().speed = 0;
    }

    private void SpawnPlatform()
    {
        _objectPool.Spawn(nameof(Platform), new Vector3(Random.Range(-7, 7), 6, 0), Quaternion.identity);
    }

    public void StopSpawning()
    {
        CancelInvoke(nameof(SpawnPlatform));
    }
}
