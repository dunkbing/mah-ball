using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
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

}
