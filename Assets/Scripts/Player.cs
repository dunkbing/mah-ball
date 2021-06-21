using UnityEngine;

public class Player : Entity, ISpawn
{
    public GameObject explosion;

    private PlayerController _playerController;

    private float _score;

    private void Awake()
    {
        _playerController = GetComponent<PlayerController>();
        OnExplode += (() =>
        {
            Destroy(Instantiate(explosion, transform.position, Quaternion.identity), Constants.ExplosionLifeTime);
        });
    }

    private void Update()
    {
        if (GameStats.GameIsPaused) return;

        if (_score < 1)
        {
            _score += Time.deltaTime;
        }
        else
        {
            ScoreMenu.Instance.IncreaseScore(Constants.NormalScore);
            _score = 0;
        }
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
