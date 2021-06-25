// Created by Binh Bui on 06, 25, 2021

using System;
using UnityEngine;

namespace Entities
{
    public struct WeaponType
    {
        public const string Sword = "Sword";
        public const string Gun = "Gun";
        public const string Spike = "Spike";
    }

    [Serializable]
    public struct Weapon
    {
        public string Name { get; set; }
        [SerializeField] private int level;
        public int Level
        {
            get => level;
            set
            {
                level = value;
            }
        }

        public int Price { get; set; }
        public int Damage { get; set; }
        public override string ToString()
        {
            return $"{Name}|{Level}|{Price}|{Damage}";
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
            };
        }
    }
}