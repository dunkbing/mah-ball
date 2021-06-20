using UnityEngine;

public class Platform : MonoBehaviour
{
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
}
