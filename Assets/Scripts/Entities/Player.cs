using Common;
using UI;
using UnityEngine;
using Utilities;

namespace Entities
{
    public class Player : Entity, ISpawn
    {
        public float power = 5f;
        public Rigidbody2D rb2d;
        public LineRenderer lr;
        public ParticleSystem ps;
        public SpriteRenderer sr;

        private Camera _cam;
        private Vector3 _startPoint;
        private Vector3 _endPoint;

        // on air time limit
        private float _energy = Constants.MaxEnergy;
        private float _chargeTime;

        private bool _onAir = true;

        private Vector3 _originScale;

        // to increase player score every 1s
        private float _elapsedTime;

        public static Player Instance { get; private set; }

        private void Awake()
        {
            Instance = this;
            _cam = Camera.main;
            _originScale = transform.localScale;

            OnExplode += (() =>
            {
                ObjectPool.Instance.Spawn("PlayerExplosion", transform.position, Quaternion.identity, go =>
                {
                    var particle = go.GetComponent<ParticleSystem>();
                    var psMain = particle.main;
                    psMain.startColor = GameStats.PlayerColor;
                    particle.Play();
                });
            });
        }

        private void Start()
        {
            HUD.Instance.SetMaxEnergy(_energy);
            HUD.Instance.SetEnergy(_energy);
            transform.localScale = _originScale;
            SetPlayerColor(GameStats.PlayerColor);
        }

        private void Update()
        {
            if (_onAir)
            {
                _energy -= Time.deltaTime;
            }

            if (GameStats.GameIsPaused)
            {
                return;
            }

            HUD.Instance.SetEnergy(_energy);

            if (_energy <= 0)
            {
                lr.positionCount = 0;
                TimeManager.StopSlowMotion();
                return;
            }

            if (Input.GetMouseButtonDown(0))
            {
                DragStart();
            } else if (Input.GetMouseButton(0))
            {
                Dragging();
            } else if (Input.GetMouseButtonUp(0))
            {
                DragRelease();
            }
            else
            {
                lr.positionCount = 0;
            }
        }

        private void LateUpdate()
        {
            if (GameStats.GameIsPaused) return;

            if (_elapsedTime < 1)
            {
                _elapsedTime += Time.deltaTime;
            }
            else
            {
                HUD.Instance.IncreaseScore(Constants.NormalScore);
                _elapsedTime = 0;
            }
        }

        private void DragRelease()
        {
            AudioManager.Instance.Play("shoot");
            PpvUtils.Instance.ExitSlowMo();

            TimeManager.StopSlowMotion();

            _endPoint = _cam.ScreenToWorldPoint(Input.mousePosition);
            _endPoint.z = 15;

            Vector2 velocity = (_startPoint - _endPoint) * power;

            transform.up = velocity.normalized;
            transform.localScale = _originScale;

            ReduceVel(ref velocity, 15);

            rb2d.velocity = velocity;
            lr.positionCount = 0;

            _energy -= _chargeTime;
            _chargeTime = 0;
        }

        public void SetPlayerColor(Color color)
        {
            GameStats.PlayerColor = color;
            sr.color = color;
            // set particle color
            var psMain = ps.main;
            psMain.startColor = color;
            // line renderer color
            lr.startColor = color;
            // set explosion color
            // var explosionMain = playerExplosion.main;
            // explosionMain.startColor = color;
        }

        private void Dragging()
        {
            _chargeTime += Time.deltaTime * 10;
            // vignette mode
            PpvUtils.Instance.EnterSlowMo();

            // slow down
            TimeManager.DoSlowMotion();

            _endPoint = _cam.ScreenToWorldPoint(Input.mousePosition);
            _endPoint.z = 15;

            Vector2 velocity = (_startPoint - _endPoint) * power;

            // set rotation toward velocity and squeeze the ball by y
            transform.up = velocity.normalized;
            if (transform.localScale.y > 0.4f)
            {
                transform.localScale -= new Vector3(0, 0.01f);
            }

            ReduceVel(ref velocity, 15);

            Vector2[] trajectory = Plot(rb2d, transform.position, velocity, 400);
            lr.positionCount = trajectory.Length;

            Vector3[] positions = new Vector3[trajectory.Length];
            for (int i = 0; i < trajectory.Length; i++)
            {
                positions[i] = trajectory[i];
            }
            lr.SetPositions(positions);
        }

        private void DragStart()
        {
            _startPoint = _cam.ScreenToWorldPoint(Input.mousePosition);
            _startPoint.z = 15;
        }

        private Vector2[] Plot(Rigidbody2D rb, Vector2 pos, Vector2 velocity, int steps)
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

        private void OnCollisionStay2D(Collision2D other)
        {
            HandleCollision(other, true);
        }

        private void OnCollisionEnter2D(Collision2D other)
        {
            HandleCollision(other);
        }

        private void HandleCollision(Collision2D other, bool stay = false)
        {
            if (!stay && !GameStats.GameIsPaused) ps.Play();

            switch (other.gameObject.tag)
            {
                case "PlatformSurface":
                    if (!stay && !GameStats.GameIsPaused) HUD.Instance.IncreaseScore(Constants.WhitePlatScore);

                    Regen(Constants.WhitePlatRegenRate);
                    _onAir = false;
                    break;
                case "GreenPlatformSurface":
                    if (!stay && !GameStats.GameIsPaused) HUD.Instance.IncreaseScore(Constants.GreenPlatScore);
                    Regen(Constants.GreenPlatRegenRate);
                    _onAir = false;
                    break;
                case "Lava":
                    Explode();
                    break;
            }
            // if (other.collider.CompareTag("PlatformSurface"))
            // {
            //     if (!stay && !GameStats.GameIsPaused) HUD.Instance.IncreaseScore(Constants.WhitePlatScore);
            //
            //     Regen(Constants.WhitePlatRegenRate);
            //     _onAir = false;
            // } else if (other.collider.CompareTag("GreenPlatformSurface"))
            // {
            //     if (!stay && !GameStats.GameIsPaused) HUD.Instance.IncreaseScore(Constants.GreenPlatScore);
            //     Regen(Constants.GreenPlatRegenRate);
            //     _onAir = false;
            // } else if (other.gameObject.CompareTag("Lava"))
            // {
            //     Explode();
            // }
        }

        private void OnCollisionExit2D(Collision2D other)
        {
            if (other.gameObject.CompareTag("PlatformSurface") || other.gameObject.CompareTag("GreenPlatformSurface"))
            {
                _onAir = true;
            }
        }

        public void Spawn()
        {
            transform.localScale = new Vector3(0.7f, 0.7f);
            ResetEnergy();
        }

        public void ResetEnergy()
        {
            _energy = Constants.MaxEnergy;
        }

        private void Regen(float amount)
        {
            if (_energy > Constants.MaxEnergy)
            {
                _energy = Constants.MaxEnergy;
                return;
            }

            _energy += amount;
        }
    }
}
