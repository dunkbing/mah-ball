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
        public const float ExplosionLifeTime = 1f;
        // scoring
        public const int NormalScore = 10;
        public const int WhitePlatScore = 20;
        public const int GreenPlatScore = 30;
        public const int HeartScore = 50;
        // data file path
        public static readonly string DataFilePath = $"{Application.persistentDataPath}/save.txt";
    }
}