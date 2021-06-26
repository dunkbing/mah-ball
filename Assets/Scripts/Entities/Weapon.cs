// Created by Binh Bui on 06, 25, 2021

using System;

namespace Entities
{
    public struct WeaponType
    {
        public const string Sword = "Sword";
        public const string Gun = "Gun";
        public const string Spike = "Spike";
        public const string None = "None";
    }

    [Serializable]
    public struct Weapon
    {
        public string Name { get; set; }

        public int Level { get; set; }
        public int Price { get; set; }
        public int Damage { get; set; }
        public int Defence { get; set; }
        public override string ToString()
        {
            return $"{Name}|{Level}|{Price}|{Damage}|{Defence}";
        }

        public static Weapon Parse(string weaponStr)
        {
            string[] props = weaponStr.Split('|');
            return new Weapon()
            {
                Name = props[0],
                Level = Convert.ToInt32(props[1]),
                Price = Convert.ToInt32(props[2]),
                Damage = Convert.ToInt32(props[3]),
                Defence = Convert.ToInt32(props[4]),
            };
        }

        public static Weapon None => new Weapon();
    }
}