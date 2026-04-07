using System;

namespace SWWoW.Models
{
    public enum SpellType { BasicAttack, SpecialAttack, AoE, Heal, Buff, Debuff }

    [Serializable]
    public class Spell
    {
        public string Name;
        public int Cooldown;
        public int CurrentCooldown;
        public SpellType Type;
        public float Multiplier = 1.0f; // Scaling factor for damage/heal

        public bool IsReady => CurrentCooldown <= 0;

        public Spell(string name, int cooldown, SpellType type, float multiplier = 1.0f)
        {
            Name = name;
            Cooldown = cooldown;
            CurrentCooldown = 0;
            Type = type;
            Multiplier = multiplier;
        }

        public void ReduceCooldown()
        {
            if (CurrentCooldown > 0) CurrentCooldown--;
        }

        public void ResetCooldown()
        {
            CurrentCooldown = Cooldown;
        }
    }
}
