using UnityEngine;

public class Platform : MonoBehaviour
{
    private Rigidbody2D _rb;
    private float _speed = 1.5f;

    private void Awake()
    {
        _rb = GetComponent<Rigidbody2D>();
    }

    private void FixedUpdate()
    {
        _rb.MovePosition(Vector3.down * (_speed * Time.fixedDeltaTime) + transform.position);
    }
}
