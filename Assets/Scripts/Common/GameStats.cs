using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Entities;
using UI;
using Unity.IO.LowLevel.Unsafe;
using UnityEngine;

namespace Common
{
    public class GameStats : MonoBehaviour
    {
        public static bool GameIsPaused = true;
        public int Score { get; set; }
        public static int MaxHealth { get; set; } = 200;
        public static float MaxEnergy { get; set; } = 10f;
        public int HighScore { get; private set; }
        public Color PlayerColor { get; set; }
        public int Coin { get; set; }
        public int EnemyKilled { get; set; }
        public int TotalEnemyKilled { get; set; }
        public float Power { get; private set; } = 10;

        public readonly Dictionary<string, Weapon> Weapons = new Dictionary<string, Weapon>();
        public string currentWeaponType = WeaponType.None;
        public Weapon CurrentWeapon => Weapons.ContainsKey(currentWeaponType) ? Weapons[currentWeaponType] : Weapon.None;

        public Ball currentBall;

        public static GameStats Instance { get; private set; }

        private void Awake()
        {
            Instance = this;
            PlayerColor  = new Color(98, 238, 164);

            LoadStatsFromFile();
            LoadWeaponsFromFile();
        }

        public void LoadStatsFromFile()
        {
            try
            {
                // load stats
                var highScoreTxt = File.ReadAllText(Constants.StatFilePath);
                var stats = highScoreTxt.Split('|');
                HighScore = Convert.ToInt32(stats[0]);
                PlayerColor = new Color(Convert.ToSingle(stats[1]), Convert.ToSingle(stats[2]),
                    Convert.ToSingle(stats[3]));
                Coin = Convert.ToInt32(stats[4]);
                TotalEnemyKilled = Convert.ToInt32(stats[5]);
                currentWeaponType = stats[6];
                MaxHealth = Convert.ToInt32(stats[7]);
                MaxEnergy = Convert.ToSingle(stats[8]);
                Power = Convert.ToSingle(stats[9]);
            }
            catch (Exception e) when (e is FileNotFoundException || e is DirectoryNotFoundException ||
                                      e is IndexOutOfRangeException || e is FormatException)
            {
                Debug.Log(e.Message);
            }
        }

        public void LoadWeaponsFromFile()
        {
            try
            {
                // load weapons
                var weaponsText = File.ReadAllText(Constants.WeaponFilePath);
                var weapons = weaponsText.Split('\n');
                if (weapons.Length == 2)
                {
                    foreach (var wStr in weapons)
                    {
                        var weapon = Weapon.Parse(wStr);
                        Weapons[weapon.Name] = weapon;
                    }
                }
                else
                {
                    Weapons[WeaponType.Sword] = new Weapon()
                        { Name = WeaponType.Sword, Level = 0, Price = Constants.SwordPrice, Damage = 0 };
                    // Weapons[WeaponType.Gun] = new Weapon(){Name = WeaponType.Gun, Level = 0, Price = Constants.GunPrice, Damage = 0};
                    Weapons[WeaponType.Spike] = new Weapon()
                        { Name = WeaponType.Spike, Level = 0, Price = Constants.SpikeDamage, Damage = 0 };
                }
            }
            catch (Exception e) when (e is FileNotFoundException || e is DirectoryNotFoundException ||
                                      e is IndexOutOfRangeException || e is FormatException)
            {
                Debug.Log(e.Message);
                Weapons[WeaponType.Sword] = new Weapon()
                    { Name = WeaponType.Sword, Level = 0, Price = Constants.SwordPrice, Damage = 0 };
                // Weapons[WeaponType.Gun] = new Weapon(){Name = WeaponType.Gun, Level = 0, Price = Constants.GunPrice, Damage = 0};
                Weapons[WeaponType.Spike] = new Weapon()
                    { Name = WeaponType.Spike, Level = 0, Price = Constants.SpikePrice, Damage = 0 };
            }
        }

        public void SaveStatsToFile()
        {
            if (Score > HighScore)
            {
                HighScore = Score;
            }

            Coin += Score / 10;
            TotalEnemyKilled += EnemyKilled;

            Score = 0;
            EnemyKilled = 0;

            var r = PlayerColor.r;
            var g = PlayerColor.g;
            var b = PlayerColor.b;
            File.WriteAllText(Constants.StatFilePath,
                $"{HighScore}|{r}|{g}|{b}|{Coin}|{TotalEnemyKilled}|{currentWeaponType}|{MaxHealth}|{MaxEnergy}|{Power}");
        }

        public void SaveWeaponsToFile()
        {
            File.WriteAllText(Constants.WeaponFilePath, string.Join("\n", Weapons.Values.ToArray()));
        }

        public void ResetStats()
        {
            Score = Coin = EnemyKilled = 0;
        }

        public void UpgradeWeapon(string wName)
        {
            // get copy of a weapon to upgrade
            var weapon = Weapons[wName];

            if (weapon.Level >= Constants.WeaponMaxLevel) return;

            if (Coin > weapon.Price)
            {
                Coin -= weapon.Price;
                switch (wName)
                {
                    case WeaponType.Sword:
                        weapon.Price += Constants.SwordPrice / 2;
                        weapon.Damage += weapon.Level < 1 ? Constants.SwordDamage : Constants.SwordDamage / 2;
                        weapon.Defence = Constants.SwordDefence;
                        break;
                    case WeaponType.Spike:
                        weapon.Price += Constants.SpikePrice / 2;
                        weapon.Damage += weapon.Level == 0 ? Constants.SpikeDamage : Constants.SpikeDamage / 3;
                        weapon.Defence = Constants.SpikeDefence;
                        break;
                }
                weapon.Level++;
            }
            Weapons[wName] = weapon;

            SaveWeaponsToFile();
        }

        public void UpgradeStats(string type)
        {
            switch (type)
            {
                case "HP" when Coin >= Constants.HpPrice:
                    MaxHealth += 50;
                    Coin -= Constants.HpPrice;
                    break;
                case "KI" when Coin >= Constants.KiPrice:
                    MaxEnergy += 5;
                    Coin -= Constants.KiPrice;
                    break;
                case "Power" when Coin >= Constants.PowerPrice:
                    if (Power < 25)
                    {
                        Power += 2;
                        Coin -= Constants.PowerPrice;
                    }
                    break;
            }

            Instance.SaveStatsToFile();
        }

        public void StartIncreasingScore()
        {
            StartCoroutine(nameof(IncreaseScore));
        }

        public void StopIncreasingScore()
        {
            StopCoroutine(nameof(IncreaseScore));
        }

        private IEnumerator IncreaseScore()
        {
            while (true)
            {
                yield return new WaitForSecondsRealtime(1f);
                HUD.Instance.IncreaseScore(Constants.NormalScore);
            }
        }
    }
}
