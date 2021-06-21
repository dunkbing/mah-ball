using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour, ISpawn
{
    private ObjectPool _objectPool;
    public GameObject particle;

    private void Awake()
    {
        _objectPool = ObjectPool.Instance;
    }

    private void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject.CompareTag("Lava"))
        {
            Instantiate(particle, transform.position, Quaternion.identity);
            _objectPool.Retrieve(nameof(Player));
        }
    }

    public void Spawn()
    {
        transform.localScale = new Vector3(0.7f, 0.7f);
    }
}
