using UnityEngine;

public class Global : MonoBehaviour
{
    void Start()
    {
        InvokeRepeating(nameof(SpawnPlatform), 2f, 2f);
    }

    private void SpawnPlatform()
    {
        ObjectPool.Instance.Spawn(nameof(Platform), new Vector3(Random.Range(-7, 7), 6, 0), Quaternion.identity);
    }
}
