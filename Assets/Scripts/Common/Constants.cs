using UnityEngine;

namespace Common
{
    public class Constants : MonoBehaviour
    {
        // energy
        public const float WhitePlatRegenRate = 0.03f;
        public const float BluePlatRegenRate = 0.06f;
        public const float GreenPlatRegenRate = 0.5f;
        public const float RewardRegen = 2f;

        public const int CoinReward = 30;

        public const float SpawningHeartRate = 0.6f;

        // health

        public const int StarHealth = 350;
        public const int VirusHealth = 400;

        // scoring
        public const int NormalScore = 10;
        public const int WhitePlatScore = 20;
        public const int BluePlatScore = 30;
        public const int GreenPlatScore = 40;
        public const int HeartScore = 50;
        // data file path
        public static readonly string StatFilePath = $"{Application.persistentDataPath}/stat.txt";
        public static readonly string WeaponFilePath = $"{Application.persistentDataPath}/weapon.txt";

        // weapon prices
        public const int SwordPrice = 1000;
        public const int GunPrice = 2000;
        public const int SpikePrice = 3500;

        // stats prices
        public const int HpPrice = 1000;
        public const int KiPrice = 1000;
        public const int PowerPrice = 1000;

        // weapon damages
        public const int NoneDamage = 50;
        public const int SwordDamage = 60;
        public const int GunDamage = 40;
        public const int SpikeDamage = 100;

        // weapon defence
        public const int NoneDefence = 50;
        public const int SwordDefence = 55;
        public const int GunDefence = 10;
        public const int SpikeDefence = 60;

        public const int WeaponMaxLevel = 5;

        // enemy damages
        public const int VirusDamage = 60;
        public const int StarDamage = 30;
        public const int LavaDamage = 62;
        public const int BulletDamage = 70;
    }
}