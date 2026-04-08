using System;

namespace SWWoW.Models
{
    public enum SynergyType { AttackBoost, SpeedBoost, HealthRegen, BurnEffect }

    [Serializable]
    public class Synergy
    {
        public string Name;
        public string RequiredTag; // e.g., "Orc", "Fire"
        public int RequiredCount;
        public SynergyType Effect;
        public float Value; // e.g., 0.15 for 15%

        public Synergy(string name, string tag, int count, SynergyType effect, float value)
        {
            Name = name;
            RequiredTag = tag;
            RequiredCount = count;
            Effect = effect;
            Value = value;
        }
    }
}
