using System.Collections.Generic;
using UnityEngine;
using SWWoW.Models;

namespace SWWoW.Engine
{
    public static class UnitDatabase
    {
        private static Spell CreateSpell(string name, int cd, SpellType type, float mult)
        {
            var s = ScriptableObject.CreateInstance<Spell>();
            s.spellName = name;
            s.cooldown = cd;
            s.type = type;
            s.multiplier = mult;
            return s;
        }

        public static UnitInstance GetOrcWarrior()
        {
            var unit = ScriptableObject.CreateInstance<Unit>();
            unit.unitName = "Orc Warrior";
            unit.baseHP = 1200;
            unit.baseAttack = 80;
            unit.baseDefense = 50;
            unit.baseSpeed = 95;
            unit.element = ElementType.Fire;
            unit.spells.Add(CreateSpell("Smash", 0, SpellType.BasicAttack, 1.0f));
            unit.spells.Add(CreateSpell("Whirlwind", 3, SpellType.AoE, 0.7f)); // AOE ATTACK
            unit.spells.Add(CreateSpell("Enrage", 4, SpellType.Buff, 1.5f));
            return new UnitInstance(unit);
        }

        public static UnitInstance GetNightElfPriest()
        {
            var unit = ScriptableObject.CreateInstance<Unit>();
            unit.unitName = "Night Elf Priest";
            unit.baseHP = 800;
            unit.baseAttack = 60;
            unit.baseDefense = 30;
            unit.baseSpeed = 110;
            unit.element = ElementType.Earth;
            unit.spells.Add(CreateSpell("Smite", 0, SpellType.BasicAttack, 0.8f));
            unit.spells.Add(CreateSpell("Holy Light", 2, SpellType.Heal, 1.2f));
            unit.spells.Add(CreateSpell("Prayer of Mending", 4, SpellType.AoEHeal, 0.6f)); // AOE HEAL
            return new UnitInstance(unit);
        }

        public static UnitInstance GetUndeadWarlock()
        {
            var unit = ScriptableObject.CreateInstance<Unit>();
            unit.unitName = "Undead Warlock";
            unit.baseHP = 900;
            unit.baseAttack = 100;
            unit.baseDefense = 35;
            unit.baseSpeed = 105;
            unit.element = ElementType.Water;
            unit.spells.Add(CreateSpell("Shadow Bolt", 0, SpellType.BasicAttack, 1.1f));
            unit.spells.Add(CreateSpell("Rain of Fire", 5, SpellType.AoE, 0.8f)); // AOE ATTACK
            unit.spells.Add(CreateSpell("Corruption", 3, SpellType.Debuff, 0.5f));
            return new UnitInstance(unit);
        }

        public static UnitInstance GetTrollHunter()
        {
            var unit = ScriptableObject.CreateInstance<Unit>();
            unit.unitName = "Troll Hunter";
            unit.baseHP = 1000;
            unit.baseAttack = 90;
            unit.baseDefense = 40;
            unit.baseSpeed = 115;
            unit.element = ElementType.Earth;
            unit.spells.Add(CreateSpell("Shot", 0, SpellType.BasicAttack, 1.0f));
            unit.spells.Add(CreateSpell("Multi-Shot", 3, SpellType.AoE, 0.6f)); // AOE ATTACK
            unit.spells.Add(CreateSpell("Trap", 4, SpellType.Debuff, 0.7f));
            return new UnitInstance(unit);
        }

        public static UnitInstance GetDwarfPaladin()
        {
            var unit = ScriptableObject.CreateInstance<Unit>();
            unit.unitName = "Dwarf Paladin";
            unit.baseHP = 1300;
            unit.baseAttack = 70;
            unit.baseDefense = 60;
            unit.baseSpeed = 90;
            unit.element = ElementType.Fire;
            unit.spells.Add(CreateSpell("Hammer", 0, SpellType.BasicAttack, 0.9f));
            unit.spells.Add(CreateSpell("Holy Shield", 4, SpellType.Buff, 0.0f));
            unit.spells.Add(CreateSpell("Consecration", 5, SpellType.AoE, 0.6f)); // AOE ATTACK
            return new UnitInstance(unit);
        }
    }
}
