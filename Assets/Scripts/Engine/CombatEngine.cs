using System;
using System.Collections.Generic;
using System.Linq;
using SWWoW.Models;

namespace SWWoW.Engine
{
    public class CombatEngine
    {
        public List<Unit> PlayerTeam;
        public List<Unit> EnemyTeam;
        
        private Dictionary<Unit, float> actionBars = new Dictionary<Unit, float>();
        private const float TurnThreshold = 100f;

        public CombatEngine(List<Unit> playerTeam, List<Unit> enemyTeam)
        {
            PlayerTeam = playerTeam;
            EnemyTeam = enemyTeam;
            
            foreach (var unit in PlayerTeam.Concat(EnemyTeam))
            {
                actionBars[unit] = 0f;
            }
        }

        public Unit GetNextTurnUnit()
        {
            while (true)
            {
                // Increment action bars based on speed
                foreach (var unit in actionBars.Keys.ToList())
                {
                    if (unit.IsAlive)
                    {
                        actionBars[unit] += unit.Speed * 0.1f; // Speed scaling
                        if (actionBars[unit] >= TurnThreshold)
                        {
                            return unit;
                        }
                    }
                }
            }
        }

        public void ResetActionBar(Unit unit)
        {
            if (actionBars.ContainsKey(unit))
                actionBars[unit] = 0f;
        }

        public float CalculateDamage(Unit attacker, Unit defender, Spell spell)
        {
            float elementalMultiplier = GetElementalMultiplier(attacker.Element, defender.Element);
            float baseDamage = attacker.Attack * spell.Multiplier * elementalMultiplier;
            
            // Damage mitigation formula: Damage * (100 / (100 + Defense))
            float mitigation = 100f / (100f + defender.Defense);
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

        public void ApplyDamage(Unit target, float amount)
        {
            target.CurrentHP -= amount;
            if (target.CurrentHP < 0) target.CurrentHP = 0;
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
