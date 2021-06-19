using UnityEngine;

public class Global : MonoBehaviour
{
    private void Start()
    {
        ObjectPool.Instance.Spawn(nameof(Player), new Vector3(0, 3, 0), Quaternion.identity);
        ObjectPool.Instance.Spawn(nameof(Platform), Vector3.up * 2, Quaternion.identity);
        InvokeRepeating(nameof(SpawnPlatform), .1f, 1.5f);
    }

    private void SpawnPlatform()
    {
        ObjectPool.Instance.Spawn(nameof(Platform), new Vector3(Random.Range(-7, 7), 6, 0), Quaternion.identity);
    }
}
