using Common;
using UI;
using UnityEngine;
using Utilities;

namespace Entities
{
    public class Player : Entity, ISpawn, IDamageable
    {
        public Rigidbody2D rb2d;
        public LineRenderer lr;
        public ParticleSystem ps;
        public SpriteRenderer sr;

        private float _health;

        // weapons
        private GameObject _weapon;


        private Camera _cam;
        private Vector3 _startPoint;
        private Vector3 _endPoint;

        // on air time limit
        private float _energy = GameStats.MaxEnergy;
        private float _chargeTime;

        private bool _onAir = true;

        private Vector3 _originScale;

        private void Awake()
        {
            _cam = Camera.main;
            _originScale = transform.localScale;

            OnExplode += (() =>
            {
                AudioManager.Instance.Play("explosion");
                ObjectPool.Instance.Spawn("PlayerExplosion", transform.position, Quaternion.identity, go =>
                {
                    var particle = go.GetComponent<ParticleSystem>();
                    var psMain = particle.main;
                    psMain.startColor = GameStats.Instance.PlayerColor;
                    particle.Play();
                });
                GameStats.Instance.StopIncreasingScore();
            });
        }

        private void Start()
        {
            HUD.Instance.SetMaxEnergy(_energy);
            HUD.Instance.SetEnergy(_energy);
            transform.localScale = _originScale;
            SetPlayerColor(GameStats.Instance.PlayerColor);
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

        private void DragRelease()
        {
            AudioManager.Instance.Play("shoot");
            PpvUtils.Instance.Disable();

            TimeManager.StopSlowMotion();

            _endPoint = _cam.ScreenToWorldPoint(Input.mousePosition);
            _endPoint.z = 15;

            Vector2 velocity = (_startPoint - _endPoint) * GameStats.Instance.Power;

            transform.up = velocity.normalized;
            transform.localScale = _originScale;

            ReduceVel(ref velocity, GameStats.Instance.Power);

            rb2d.velocity = velocity;
            lr.positionCount = 0;

            _energy -= _chargeTime;
            _chargeTime = 0;
        }

        public void SetPlayerColor(Color color)
        {
            GameStats.Instance.PlayerColor = color;
            sr.color = color;
            // set particle color
            var psMain = ps.main;
            psMain.startColor = color;
            // line renderer color
            lr.startColor = color;
        }

        public void SelectWeapon(string wpName)
        {
            switch (wpName)
            {
                case WeaponType.Sword:
                    _weapon = ObjectPool.Instance.Spawn(WeaponType.Sword, transform.position, Quaternion.identity);
                    _weapon.transform.SetParent(gameObject.transform);
                    break;
                case WeaponType.Spike:
                    break;
                case WeaponType.None:
                    _weapon?.transform.SetParent(null);
                    break;
            }
        }

        private void Dragging()
        {
            _chargeTime += Time.deltaTime * 10;

            // use effects
            PpvUtils.Instance.Activate();

            // slow down
            TimeManager.DoSlowMotion();

            _endPoint = _cam.ScreenToWorldPoint(Input.mousePosition);
            _endPoint.z = 15;

            Vector2 velocity = (_startPoint - _endPoint) * GameStats.Instance.Power;

            // set rotation toward velocity and squeeze the ball by y
            transform.up = velocity.normalized;
            if (transform.localScale.y > 0.4f)
            {
                transform.localScale -= new Vector3(0, 0.01f);
            }

            ReduceVel(ref velocity, GameStats.Instance.Power);

            var trajectory = Plot(rb2d, transform.position, velocity, 400);
            lr.positionCount = trajectory.Length;
            lr.SetPositions(trajectory);
        }

        private void DragStart()
        {
            _startPoint = _cam.ScreenToWorldPoint(Input.mousePosition);
            _startPoint.z = 15;
        }

        private static Vector3[] Plot(Rigidbody2D rb, Vector2 pos, Vector2 velocity, int steps)
        {
            var result = new Vector3[steps];

            var timeStep = Time.fixedDeltaTime / Physics2D.velocityIterations;
            var gravityAccel = Physics2D.gravity * (rb.gravityScale * timeStep * timeStep);

            var drag = 1f - timeStep * rb.drag;
            var moveStep = velocity * timeStep;

            for (var i = 0; i < steps; i++)
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
            if (!(velocity.magnitude > maxMag)) return;

            velocity.Normalize();
            velocity *= maxMag;
        }

        private void OnCollisionStay2D(Collision2D other)
        {
            HandleCollision(other, true);
        }

        private void OnCollisionEnter2D(Collision2D other)
        {
            HandleCollision(other);
        }

        private void OnParticleCollision(GameObject other)
        {
            if (other.CompareTag("Virus"))
            {
                TakeDamage(Constants.VirusDamage/20f, GameStats.Instance.currentWeaponType == WeaponType.None ? 0 : GameStats.Instance.CurrentWeapon.Defence);
            }
        }

        private void HandleCollision(Collision2D other, bool stay = false)
        {
            if (!stay && !GameStats.GameIsPaused) ps.Play();

            switch (other.gameObject.tag)
            {
                case "PlatformSurface":
                    if (!stay && !GameStats.GameIsPaused) HUD.Instance.IncreaseScore(Constants.WhitePlatScore);
                    RegenKi(Constants.WhitePlatRegenRate);
                    _onAir = false;
                    break;
                case "BluePlatformSurface":
                    if (!stay && !GameStats.GameIsPaused) HUD.Instance.IncreaseScore(Constants.BluePlatScore);
                    RegenKi(Constants.BluePlatRegenRate);
                    _onAir = false;
                    break;
                case "GreenPlatformSurface":
                    if (!stay && !GameStats.GameIsPaused) HUD.Instance.IncreaseScore(Constants.GreenPlatScore);
                    RegenKi(Constants.WhitePlatRegenRate/2);
                    RegenHp(Constants.GreenPlatRegenRate);
                    _onAir = false;
                    break;
                case "Lava":
                    break;
                case "Bullet":
                    TakeDamage(Constants.BulletDamage, GameStats.Instance.CurrentWeapon.Defence);
                    break;
                case "Virus":
                case "Star":
                case "Square":
                    RegenHp(25);
                    RegenKi(3);
                    break;
            }
        }

        private void OnCollisionExit2D(Collision2D other)
        {
            if (other.gameObject.CompareTag("PlatformSurface") || other.gameObject.CompareTag("GreenPlatformSurface") || other.gameObject.CompareTag("BluePlatformSurface"))
            {
                _onAir = true;
            }
        }

        public void Spawn()
        {
            _health = GameStats.MaxHealth;
            HUD.Instance.healthBar.SetMaxHealth(_health);
            SelectWeapon(GameStats.Instance.currentWeaponType);

            ResetEnergy();
        }

        private void ResetEnergy()
        {
            _energy = GameStats.MaxEnergy;
        }

        public void RegenKi(float amount)
        {
            _energy += amount;

            if (_energy > GameStats.MaxEnergy)
            {
                _energy = GameStats.MaxEnergy;
            }
        }

        public void RegenHp(float amount)
        {
            _health += amount;

            if (_health > GameStats.MaxHealth)
            {
                _health = GameStats.MaxHealth;
            }

            HUD.Instance.healthBar.SetHealth(_health);
        }

        public void TakeDamage(float damage, float defense)
        {
            _health = _health - damage + defense;
            HUD.Instance.healthBar.SetHealth(_health);

            if (_health > 0) return;

            Explode();
            HUD.Instance.DecreaseHealth();
            if (!HUD.Instance.IsEmptyLife())
            {
                ObjectPool.Instance.Spawn(GameStats.Instance.currentWeaponType == WeaponType.Spike ? "SpikePlayer" : nameof(Player), new Vector3(0, 1.5f, 0), Quaternion.identity);
            }
            else
            {
                TimeManager.StopSlowMotion();
                lr.positionCount = 0;
                GameStats.Instance.SaveStatsToFile();
                PauseMenu.Instance.Pause();
            }
        }
    }
}
