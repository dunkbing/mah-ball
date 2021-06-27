// Created by Binh Bui on 06, 26, 2021

using Common;
using UnityEngine;
using Utilities;
using Random = UnityEngine.Random;

namespace Entities
{
    public class Circle : Enemy, ISpawn
    {
        public SpriteRenderer sr;
        private void Awake()
        {
            Health = 20;
            Damage = 0;

            OnExplode += () =>
            {
                AudioManager.Instance.Play("explosion");
                ObjectPool.Instance.Spawn("VirusExplosion", transform.position, Quaternion.identity, go =>
                {
                    var ps = go.GetComponent<ParticleSystem>();
                    var psMain = ps.main;
                    psMain.startColor = sr.color;
                    ps.Play();
                });
                GameStats.Instance.EnemyKilled += 1;
            };
        }

        public void Spawn()
        {
            sr.color = new Color(Random.Range(0, 1f), Random.Range(0, 1f), Random.Range(0, 1f));
        }

        private void FixedUpdate()
        {
            Fall();
        }

        private void OnCollisionExit2D(Collision2D other)
        {
            if (!other.gameObject.CompareTag("Player")) return;

            GameStats.Instance.Coin += Constants.CoinReward/2;
            GameStats.Instance.currentPlayer.RegenHp(Health);
            GameStats.Instance.currentPlayer.RegenKi(Health/3);
            Explode();
        }
    }
}