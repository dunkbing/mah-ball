using UnityEngine;

public class Heart : Entity
{
    public GameObject explosion;
    private Rigidbody2D _rb;
    public float speed = 1.5f;

    private void Awake()
    {
        _rb = GetComponent<Rigidbody2D>();
    }

    private void FixedUpdate()
    {
        if (GameStats.Paused) return;
        _rb.MovePosition(Vector3.down * (speed * Time.fixedDeltaTime) + transform.position);
    }

    private void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            AudioManager.Instance.Play("powerup");
            Instantiate(explosion, transform.position, Quaternion.identity);
            Explode();
        }
    }
}
