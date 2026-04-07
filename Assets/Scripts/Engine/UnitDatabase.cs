using System.Collections.Generic;
using SWWoW.Models;

namespace SWWoW.Engine
{
    public static class UnitDatabase
    {
        public static Unit GetOrcWarrior()
        {
            var unit = new Unit("Orc Warrior", 1200, 80, 50, 95, ElementType.Fire);
            unit.Tags.Add("Orc");
            unit.Tags.Add("Warrior");
            unit.Spells.Add(new Spell("Smash", 0, SpellType.BasicAttack, 1.0f));
            unit.Spells.Add(new Spell("Execute", 4, SpellType.SpecialAttack, 2.5f));
            return unit;
        }

        public static Unit GetNightElfPriest()
        {
            var unit = new Unit("Night Elf Priest", 800, 60, 30, 110, ElementType.Earth);
            unit.Tags.Add("Elf");
            unit.Tags.Add("Priest");
            unit.Spells.Add(new Spell("Smite", 0, SpellType.BasicAttack, 0.8f));
            unit.Spells.Add(new Spell("Holy Light", 3, SpellType.Heal, 1.5f));
            return unit;
        }

        public static Unit GetUndeadWarlock()
        {
            var unit = new Unit("Undead Warlock", 900, 100, 35, 105, ElementType.Water);
            unit.Tags.Add("Undead");
            unit.Tags.Add("Warlock");
            unit.Spells.Add(new Spell("Shadow Bolt", 0, SpellType.BasicAttack, 1.1f));
            unit.Spells.Add(new Spell("Chaos Bolt", 5, SpellType.SpecialAttack, 3.0f));
            return unit;
        }

        public static Unit GetTrollHunter()
        {
            var unit = new Unit("Troll Hunter", 1000, 90, 40, 115, ElementType.Earth);
            unit.Tags.Add("Troll");
            unit.Tags.Add("Hunter");
            unit.Spells.Add(new Spell("Shot", 0, SpellType.BasicAttack, 1.0f));
            unit.Spells.Add(new Spell("Aimed Shot", 3, SpellType.SpecialAttack, 1.8f));
            return unit;
        }

        public static Unit GetDwarfPaladin()
        {
            var unit = new Unit("Dwarf Paladin", 1300, 70, 60, 90, ElementType.Fire);
            unit.Tags.Add("Dwarf");
            unit.Tags.Add("Paladin");
            unit.Spells.Add(new Spell("Hammer", 0, SpellType.BasicAttack, 0.9f));
            unit.Spells.Add(new Spell("Holy Shield", 4, SpellType.Buff, 0.0f)); // No dmg, utility
            return unit;
        }
    }
}
