using System;
using System.Collections.Generic;
using System.Linq;
using SWWoW.Models;
using UnityEngine;

namespace SWWoW.Engine
{
    public class CombatSimulation
    {
        private CombatEngine engine;

        public void Run(List<UnitInstance> playerTeam, List<UnitInstance> enemyTeam)
        {
            engine = new CombatEngine(playerTeam, enemyTeam);
            Debug.Log("--- COMBAT START ---");

            int turnCounter = 0;
            string winner;

            while (!engine.IsCombatOver(out winner) && turnCounter < 100)
            {
                UnitInstance activeUnit = engine.GetNextTurnUnit();
                ProcessTurn(activeUnit);
                turnCounter++;
            }

            Debug.Log("--- COMBAT END ---");
            Debug.Log($"Winner: {winner}");
        }

        private void ProcessTurn(UnitInstance unit)
        {
            // Simple AI: target the first alive unit from the opposite team
            List<UnitInstance> enemies = engine.PlayerTeam.Contains(unit) ? engine.EnemyTeam : engine.PlayerTeam;
            UnitInstance target = enemies.FirstOrDefault(e => e.IsAlive);

            if (target == null) return;

            // Simple AI: Use special spell if ready, otherwise basic
            SpellInstance spell = unit.spells.FindLast(s => s.IsReady);
            
            if (spell != null)
            {
                float damage = engine.CalculateDamage(unit, target, spell);
                engine.ApplyDamage(target, damage);
                
                Debug.Log($"{unit.Name} uses {spell.data.spellName} on {target.Name} for {damage:F1} dmg. (Target HP: {target.currentHP:F1})");
                
                spell.ResetCooldown();
            }

            // Cooldown reduction for all spells
            foreach (var s in unit.spells) s.ReduceCooldown();
            
            engine.ResetActionBar(unit);
        }
    }
}
