using UnityEngine;

public class Constants : MonoBehaviour
{
    public const float MaxEnergy = 10f;
    public const float WhitePlatRegenRate = 0.03f;
    public const float GreenPlatRegenRate = 0.08f;
    public const float SpawningHeartRate = 0.6f;
    public const float ExplosionLifeTime = 1f;
    // scoring
    public const int NormalScore = 1;
    public const int WhitePlatScore = 5;
    public const int GreenPlatScore = 5;
    public const int HeartScore = 20;
    // data file path
    public static readonly string DataFilePath = $"{Application.persistentDataPath}/save.txt";
}