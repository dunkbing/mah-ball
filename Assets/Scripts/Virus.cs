using UnityEngine;

public class Virus : Entity, IFalling
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
        Fall();
    }

    private void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            Destroy(Instantiate(explosion, transform.position, Quaternion.identity), Constants.ExplosionLifeTime);
            other.gameObject.GetComponent<Player>().Explode();
            PauseMenu.Instance.DelayPause(1f);
            Explode();
        }
    }

    public void Fall()
    {
        if (GameStats.GameIsPaused) return;
        _rb.MovePosition(Vector3.down * (speed * Time.fixedDeltaTime) + transform.position);
    }
}
