using UnityEngine;

public class Virus : Entity, IFalling
{
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
            AudioManager.Instance.Play("explosion");
            ObjectPool.Instance.Spawn("VirusExplosion", transform.position, Quaternion.identity, go =>
            {
                go.GetComponent<ParticleSystem>().Play();
            });
            other.gameObject.GetComponent<Player>().Explode();
            Explode();
            HeartCounter.Instance.DecreaseHealth();
            if (!HeartCounter.Instance.IsEmptyLife())
            {
                ObjectPool.Instance.Spawn(nameof(Player), new Vector3(0, 1, 0), Quaternion.identity);
            }
            else
            {
                ScoreMenu.Instance.SaveScore();
                PauseMenu.Instance.DelayPause(1f);
            }
        }
    }

    public void Fall()
    {
        if (GameStats.GameIsPaused) return;
        _rb.MovePosition(Vector3.down * (speed * Time.fixedDeltaTime) + transform.position);
    }
}
