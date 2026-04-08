using System;
using System.Collections.Generic;
using UnityEngine;

namespace SWWoW.Models
{
    public enum ElementType { Fire, Water, Earth }

    [CreateAssetMenu(fileName = "NewUnit", menuName = "SWWoW/Unit")]
    public class Unit : ScriptableObject
    {
        public string unitName;
        public float baseHP;
        public float baseAttack;
        public float baseDefense;
        public float baseSpeed;
        public ElementType element;
        public List<Spell> spells = new List<Spell>();
        public List<string> tags = new List<string>();
    }

    /// <summary>
    /// Runtime instance of a unit to track HP and dynamic stats during combat.
    /// </summary>
    [Serializable]
    public class UnitInstance
    {
        public Unit data;
        public string Name => data.unitName;
        
        public float maxHP;
        public float currentHP;
        public float attack;
        public float defense;
        public float speed;
        public ElementType Element => data.element;
        
        public List<SpellInstance> spells = new List<SpellInstance>();
        public List<string> Tags => data.tags;

        public bool IsAlive => currentHP > 0;

        public UnitInstance(Unit unitData)
        {
            data = unitData;
            maxHP = unitData.baseHP;
            currentHP = maxHP;
            attack = unitData.baseAttack;
            defense = unitData.baseDefense;
            speed = unitData.baseSpeed;

            foreach (var s in unitData.spells)
            {
                spells.Add(new SpellInstance(s));
            }
        }
    }
}
