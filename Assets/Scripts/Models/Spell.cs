using System;
using UnityEngine;

namespace SWWoW.Models
{
    public enum SpellType { BasicAttack, SpecialAttack, AoE, Heal, AoEHeal, Buff, AoEBuff, Debuff, AoEDebuff }

    [CreateAssetMenu(fileName = "NewSpell", menuName = "SWWoW/Spell")]
    public class Spell : ScriptableObject
    {
        public string spellName;
        public int cooldown;
        public SpellType type;
        public float multiplier = 1.0f;
    }

    /// <summary>
    /// Runtime instance of a spell to track cooldowns during combat.
    /// </summary>
    [Serializable]
    public class SpellInstance
    {
        public Spell data;
        public int currentCooldown;

        public bool IsReady => currentCooldown <= 0;

        public SpellInstance(Spell spellData)
        {
            data = spellData;
            currentCooldown = 0;
        }

        public void ReduceCooldown()
        {
            if (currentCooldown > 0) currentCooldown--;
        }

        public void ResetCooldown()
        {
            currentCooldown = data.cooldown;
        }
    }
}
