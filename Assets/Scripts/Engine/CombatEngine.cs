using System;
using System.Collections.Generic;
using System.Linq;
using SWWoW.Models;

namespace SWWoW.Engine
{
    public class CombatEngine
    {
        public List<UnitInstance> PlayerTeam;
        public List<UnitInstance> EnemyTeam;
        
        private Dictionary<UnitInstance, float> actionBars = new Dictionary<UnitInstance, float>();
        private const float TurnThreshold = 100f;

        public CombatEngine(List<UnitInstance> playerTeam, List<UnitInstance> enemyTeam)
        {
            PlayerTeam = playerTeam;
            EnemyTeam = enemyTeam;
            
            foreach (var unit in PlayerTeam.Concat(EnemyTeam))
            {
                actionBars[unit] = 0f;
            }
        }

        public UnitInstance GetNextTurnUnit()
        {
            while (true)
            {
                // Increment action bars based on speed
                foreach (var unit in actionBars.Keys.ToList())
                {
                    if (unit.IsAlive)
                    {
                        actionBars[unit] += unit.speed * 0.1f; // Speed scaling
                        if (actionBars[unit] >= TurnThreshold)
                        {
                            return unit;
                        }
                    }
                }
            }
        }

        public void ResetActionBar(UnitInstance unit)
        {
            if (actionBars.ContainsKey(unit))
                actionBars[unit] = 0f;
        }

        public float CalculateDamage(UnitInstance attacker, UnitInstance defender, SpellInstance spell)
        {
            float elementalMultiplier = GetElementalMultiplier(attacker.Element, defender.Element);
            float baseDamage = attacker.attack * spell.data.multiplier * elementalMultiplier;
            
            // Damage mitigation formula: Damage * (100 / (100 + Defense))
            float mitigation = 100f / (100f + defender.defense);
            return baseDamage * mitigation;
        }

        private float GetElementalMultiplier(ElementType attacker, ElementType defender)
        {
            if (attacker == ElementType.Fire && defender == ElementType.Earth) return 1.5f;
            if (attacker == ElementType.Earth && defender == ElementType.Water) return 1.5f;
            if (attacker == ElementType.Water && defender == ElementType.Fire) return 1.5f;

            if (attacker == ElementType.Fire && defender == ElementType.Water) return 0.5f;
            if (attacker == ElementType.Water && defender == ElementType.Earth) return 0.5f;
            if (attacker == ElementType.Earth && defender == ElementType.Fire) return 0.5f;

            return 1.0f;
        }

        public void ApplyDamage(UnitInstance target, float amount)
        {
            target.currentHP -= amount;
            if (target.currentHP < 0) target.currentHP = 0;
        }

        public bool IsCombatOver(out string winner)
        {
            if (PlayerTeam.All(u => !u.IsAlive))
            {
                winner = "Enemies";
                return true;
            }
            if (EnemyTeam.All(u => !u.IsAlive))
            {
                winner = "Players";
                return true;
            }
            winner = null;
            return false;
        }
    }
}
