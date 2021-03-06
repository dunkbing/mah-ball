using Common;
using UI;
using UnityEngine;
using Utilities;

namespace Entities
{
    public class NormalBall : Ball
    {
        private Vector3 _originScale;

        protected override void Awake()
        {
            base.Awake();

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
            HUD.Instance.SetMaxEnergy(Energy);
            HUD.Instance.SetEnergy(Energy);
            transform.localScale = _originScale;
            SetPlayerColor(GameStats.Instance.PlayerColor);
        }

        protected override void DragRelease()
        {
            base.DragRelease();
            transform.localScale = _originScale;
        }

        protected override void Dragging()
        {
            base.Dragging();

            // squeeze ball
            if (transform.localScale.y > 0.4f)
            {
                transform.localScale -= new Vector3(0, 0.01f);
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

        private void OnParticleCollision(GameObject other)
        {
            if (other.CompareTag("Virus"))
            {
                TakeDamage(Constants.VirusDamage/20f, GameStats.Instance.currentWeaponType == WeaponType.None ? 0 : GameStats.Instance.CurrentWeapon.Defence);
            }
        }

        private void OnCollisionExit2D(Collision2D other)
        {
            if (other.gameObject.CompareTag("PlatformSurface") || other.gameObject.CompareTag("GreenPlatformSurface") || other.gameObject.CompareTag("BluePlatformSurface"))
            {
                onAir = true;
            }
        }
    }
}
