using UnityEngine;

namespace Common
{
    public class Constants : MonoBehaviour
    {
        // energy
        public const float MaxEnergy = 10f;
        public const float WhitePlatRegenRate = 0.03f;
        public const float GreenPlatRegenRate = 0.08f;
        public const float RewardRegen = 2f;

        public const int CoinReward = 10;

        public const float SpawningHeartRate = 0.6f;

        // health
        public const int PlayerHealth = 200;
        public const int SpikePlayerHealth = 300;
        public const int StarHealth = 500;
        public const int VirusHealth = 450;

        // scoring
        public const int NormalScore = 10;
        public const int WhitePlatScore = 20;
        public const int GreenPlatScore = 30;
        public const int HeartScore = 50;
        // data file path
        public static readonly string StatFilePath = $"{Application.persistentDataPath}/stat.txt";
        public static readonly string WeaponFilePath = $"{Application.persistentDataPath}/weapon.txt";

        // weapon prices
        public const int SwordPrice = 1000;
        public const int GunPrice = 2000;
        public const int SpikePrice = 3500;

        // weapon damages
        public const int SwordDamage = 60;
        public const int GunDamage = 20;
        public const int SpikeDamage = 100;

        // enemy damages
        public const int VirusDamage = 60;
        public const int StarDamage = 30;
        public const int LavaDamage = 26;
        public const int BulletDamage = 30;
    }
}