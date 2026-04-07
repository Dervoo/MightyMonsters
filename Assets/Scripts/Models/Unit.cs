using System;
using System.Collections.Generic;

namespace SWWoW.Models
{
    public enum ElementType { Fire, Water, Earth }

    [Serializable]
    public class Unit
    {
        public string Name;
        public int Level = 1;
        public float MaxHP;
        public float CurrentHP;
        public float Attack;
        public float Defense;
        public float Speed;
        public ElementType Element;
        public List<Spell> Spells = new List<Spell>();
        public List<string> Tags = new List<string>(); // e.g., "Orc", "Undead"

        public bool IsAlive => CurrentHP > 0;

        public Unit(string name, float hp, float attack, float defense, float speed, ElementType element)
        {
            Name = name;
            MaxHP = hp;
            CurrentHP = hp;
            Attack = attack;
            Defense = defense;
            Speed = speed;
            Element = element;
        }
    }
}
