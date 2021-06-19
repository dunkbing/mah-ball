using UnityEngine;

public class BallController : MonoBehaviour
{
    public float power = 5f;
    private Rigidbody2D _rb;
    private LineRenderer _lr;

    private Camera _cam;
    private Vector3 _startPoint;
    private Vector3 _endPoint;

    private void Start()
    {
        _cam = Camera.main;
        _rb = GetComponent<Rigidbody2D>();
        _lr = GetComponent<LineRenderer>();
    }

    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            _startPoint = _cam.ScreenToWorldPoint(Input.mousePosition);
            _startPoint.z = 15;
        }

        if (Input.GetMouseButton(0))
        {
            _endPoint = _cam.ScreenToWorldPoint(Input.mousePosition);
            _endPoint.z = 15;

            Vector2 velocity = (_startPoint - _endPoint) * power;
            ReduceVel(ref velocity, 15);

            Vector2[] trajectory = Plot(_rb, transform.position, velocity, 200);
            _lr.positionCount = trajectory.Length;

            Vector3[] positions = new Vector3[trajectory.Length];
            for (int i = 0; i < trajectory.Length; i++)
            {
                positions[i] = trajectory[i];
            }
            _lr.SetPositions(positions);
        }

        if (Input.GetMouseButtonUp(0))
        {
            _endPoint = _cam.ScreenToWorldPoint(Input.mousePosition);
            _endPoint.z = 15;

            Vector2 velocity = (_startPoint - _endPoint) * power;
            ReduceVel(ref velocity, 15);

            _rb.velocity = velocity;
            _lr.positionCount = 0;
            Debug.Log(velocity.magnitude);
        }
    }

    public Vector2[] Plot(Rigidbody2D rb, Vector2 pos, Vector2 velocity, int steps)
    {
        Vector2[] result = new Vector2[steps];

        float timeStep = Time.fixedDeltaTime / Physics2D.velocityIterations;
        Vector2 gravityAccel = Physics2D.gravity * (rb.gravityScale * timeStep * timeStep);

        float drag = 1f - timeStep * rb.drag;
        Vector2 moveStep = velocity * timeStep;

        for (int i = 0; i < steps; i++)
        {
            moveStep += gravityAccel;
            moveStep *= drag;
            pos += moveStep;
            result[i] = pos;
        }

        return result;
    }

    /// <summary>
    /// reduce the velocity magnitude if it is too large
    /// </summary>
    /// <param name="velocity"></param>
    /// <param name="maxMag"></param>
    private void ReduceVel(ref Vector2 velocity, float maxMag)
    {
        if (velocity.magnitude > maxMag)
        {
            velocity.Normalize();
            velocity *= maxMag;
        }
    }
}
