using UnityEngine;

public class Player : Entity, ISpawn
{
    private ObjectPool _objectPool;
    public GameObject explosion;

    private PlayerController _playerController;

    private void Awake()
    {
        _objectPool = ObjectPool.Instance;
        _playerController = GetComponent<PlayerController>();
        OnExplode += (() =>
        {
            Instantiate(explosion, transform.position, Quaternion.identity);
        });
    }

    private void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject.CompareTag("Lava"))
        {
            Explode();
        }
    }

    public void Spawn()
    {
        transform.localScale = new Vector3(0.7f, 0.7f);
        _playerController.ResetEnergy();
    }
}
