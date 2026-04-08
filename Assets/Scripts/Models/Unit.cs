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

    [Serializable]
    public class UnitInstance
    {
        public Unit data;
        public string id;
        public string Name => data.unitName;
        
        public float maxHP;
        public float currentHP;
        public float attack;
        public float defense;
        public float speed;

        public ElementType Element => data.element;
        public List<string> Tags => data.tags;
        
        public int buffDuration = 0; // +50% DMG
        public int debuffDuration = 0; // -50% Defense
        
        public List<SpellInstance> spells = new List<SpellInstance>();
        public bool IsAlive => currentHP > 0;
        public bool IsDead => currentHP <= 0;

        public UnitInstance(Unit unitData)
        {
            data = unitData;
            id = Guid.NewGuid().ToString();
            maxHP = unitData.baseHP;
            currentHP = maxHP;
            attack = unitData.baseAttack;
            defense = unitData.baseDefense;
            speed = unitData.baseSpeed;
            foreach (var s in unitData.spells) spells.Add(new SpellInstance(s));
        }

        public void TakeDamage(float amount)
        {
            float effectiveDefense = debuffDuration > 0 ? defense * 0.5f : defense;
            float finalDamage = Mathf.Max(1, amount - (effectiveDefense * 0.5f));
            currentHP -= finalDamage;
            if (currentHP < 0) currentHP = 0;
        }

        public void Heal(float amount)
        {
            currentHP += amount;
            if (currentHP > maxHP) currentHP = maxHP;
        }

        public float GetModifiedAttack()
        {
            return buffDuration > 0 ? attack * 1.5f : attack;
        }
    }
}
