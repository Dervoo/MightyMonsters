using System.Collections.Generic;
using UnityEngine;
using SWWoW.Models;

namespace SWWoW.Engine
{
    public static class UnitDatabase
    {
        private static Sprite LoadSprite(string fileName)
        {
            // Próba ładowania bez rozszerzenia (standard Resources)
            Sprite s = Resources.Load<Sprite>("Characters/" + fileName);
            
            // Jeśli nie znaleziono, spróbuj wymusić odświeżenie lub sprawdź małe litery
            if (s == null) s = Resources.Load<Sprite>("Characters/" + fileName.ToLower());
            
            if (s == null) Debug.LogError($"[UnitDatabase] Sprite CRITICAL MISSING: Resources/Characters/{fileName}. Sprawdź Texture Type w Unity!");
            return s;
        }

        private static GameObject LoadPrefab(string fileName)
        {
            GameObject p = Resources.Load<GameObject>("Models/" + fileName);
            return p;
        }

        private static Spell CreateSpell(string name, int cd, SpellType type, float mult)
        {
            var s = ScriptableObject.CreateInstance<Spell>();
            s.spellName = name;
            s.cooldown = cd;
            s.type = type;
            s.multiplier = mult;
            return s;
        }

        public static UnitInstance GetIronCrossbowman() => CreateUnit("Iron Crossbowman", "IronCrossbowman", 1000, 110, 45, 105, ElementType.Fire);
        public static UnitInstance GetDawnPriestess() => CreateUnit("Dawn Priestess", "DawnPriestess", 850, 75, 35, 110, ElementType.Earth);
        public static UnitInstance GetBonebreaker() => CreateUnit("Bonebreaker", "Bonebreaker", 1400, 95, 55, 85, ElementType.Earth);
        public static UnitInstance GetDemonSorcerer() => CreateUnit("Demon Sorcerer", "DemonSorcerer", 900, 130, 30, 100, ElementType.Fire);
        public static UnitInstance GetForestDruid() => CreateUnit("Forest Druid", "ForestDruid", 950, 80, 40, 115, ElementType.Earth);
        public static UnitInstance GetAncientEnt() => CreateUnit("Ancient Ent", "AncientEnt", 1800, 70, 70, 70, ElementType.Earth);
        public static UnitInstance GetRoyalProtector() => CreateUnit("Royal Protector", "RoyalProtector", 1500, 85, 65, 90, ElementType.Fire);
        public static UnitInstance GetVoidAssassin() => CreateUnit("Void Assassin", "VoidAssassin", 800, 145, 25, 130, ElementType.Water);

        private static UnitInstance CreateUnit(string displayName, string internalName, float hp, float atk, float def, float spd, ElementType elm)
        {
            var unit = ScriptableObject.CreateInstance<Unit>();
            unit.unitName = displayName;
            unit.visual = LoadSprite(internalName);
            unit.prefab3D = LoadPrefab(internalName);
            
            unit.baseHP = hp;
            unit.baseAttack = atk;
            unit.baseDefense = def;
            unit.baseSpeed = spd;
            unit.element = elm;

            unit.spells.Add(CreateSpell("Attack", 0, SpellType.BasicAttack, 1.0f));
            unit.spells.Add(CreateSpell("Special", 3, SpellType.SpecialAttack, 1.5f));

            return new UnitInstance(unit);
        }

        public static UnitInstance GetOrcWarrior() => GetBonebreaker();
        public static UnitInstance GetNightElfPriest() => GetDawnPriestess();
        public static UnitInstance GetUndeadWarlock() => GetDemonSorcerer();
        public static UnitInstance GetTrollHunter() => GetIronCrossbowman();
        public static UnitInstance GetDwarfPaladin() => GetRoyalProtector();
    }
}
