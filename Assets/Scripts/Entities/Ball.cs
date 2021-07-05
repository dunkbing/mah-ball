// Created by Binh Bui on 07, 03, 2021

using Common;
using UI;
using UnityEngine;
using Utilities;

namespace Entities
{
    public class Ball : Entity, IDamageable, ISpawn
    {
        public LineRenderer lr;
        public ParticleSystem ps;
        public SpriteRenderer sr;

        public Rigidbody2D rb2d;

        private Camera _cam;
        private Vector3 _startPoint;
        private Vector3 _endPoint;

        private float _chargeTime;

        protected bool onAir = true;

        private float _health;

        // weapons
        private GameObject _weapon;

        // on air time limit
        protected float Energy = GameStats.MaxEnergy;

        protected virtual void Awake()
        {
            _cam = Camera.main;
        }

        protected virtual void Update()
        {
            if (onAir)
            {
                Energy -= Time.deltaTime;
            }

            if (GameStats.GameIsPaused)
            {
                return;
            }

            HUD.Instance.SetEnergy(Energy);

            if (Energy <= 0)
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

        protected virtual void DragRelease()
        {
            AudioManager.Instance.Play("shoot");
            PpvUtils.Instance.Disable();

            TimeManager.StopSlowMotion();

            _endPoint = _cam.ScreenToWorldPoint(Input.mousePosition);
            _endPoint.z = 15;

            Vector2 velocity = (_startPoint - _endPoint) * GameStats.Instance.Power;

            transform.up = velocity.normalized;

            ReduceVel(ref velocity, GameStats.Instance.Power);

            rb2d.velocity = velocity;
            lr.positionCount = 0;

            Energy -= _chargeTime;
            _chargeTime = 0;
        }

        protected virtual void Dragging()
        {
            _chargeTime += Time.deltaTime * 15;

            // use effects
            PpvUtils.Instance.Activate();

            // slow down
            TimeManager.DoSlowMotion();

            _endPoint = _cam.ScreenToWorldPoint(Input.mousePosition);
            _endPoint.z = 15;

            Vector2 velocity = (_startPoint - _endPoint) * GameStats.Instance.Power;

            // set rotation toward velocity
            transform.up = velocity.normalized;

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
                    if (_weapon)
                    {
                        _weapon.transform.SetParent(null);
                    }
                    break;
            }
        }

        private void ResetEnergy()
        {
            Energy = GameStats.MaxEnergy;
        }

        public void Spawn()
        {
            _health = GameStats.MaxHealth;
            HUD.Instance.healthBar.SetMaxHealth(_health);
            SelectWeapon(GameStats.Instance.currentWeaponType);

            ResetEnergy();
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
                ObjectPool.Instance.Spawn(GameStats.Instance.currentWeaponType == WeaponType.Spike ? "SpikePlayer" : nameof(NormalBall), new Vector3(0, 1.5f, 0), Quaternion.identity);
            }
            else
            {
                TimeManager.StopSlowMotion();
                lr.positionCount = 0;
                GameStats.Instance.SaveStatsToFile();
                PauseMenu.Instance.Pause();
            }
        }

        public void RegenKi(float amount)
        {
            Energy += amount;

            if (Energy > GameStats.MaxEnergy)
            {
                Energy = GameStats.MaxEnergy;
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

        protected void HandleCollision(Collision2D other, bool stay = false)
        {
            if (!stay && !GameStats.GameIsPaused) ps.Play();

            switch (other.gameObject.tag)
            {
                case "PlatformSurface":
                    if (!stay && !GameStats.GameIsPaused) HUD.Instance.IncreaseScore(Constants.WhitePlatScore);
                    RegenKi(Constants.WhitePlatRegenRate);
                    onAir = false;
                    break;
                case "BluePlatformSurface":
                    if (!stay && !GameStats.GameIsPaused) HUD.Instance.IncreaseScore(Constants.BluePlatScore);
                    RegenKi(Constants.BluePlatRegenRate);
                    onAir = false;
                    break;
                case "GreenPlatformSurface":
                    if (!stay && !GameStats.GameIsPaused) HUD.Instance.IncreaseScore(Constants.GreenPlatScore);
                    RegenKi(Constants.WhitePlatRegenRate / 2);
                    RegenHp(Constants.GreenPlatRegenRate);
                    onAir = false;
                    break;
                case "Lava":
                    break;
                case "Bound":
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
    }
}