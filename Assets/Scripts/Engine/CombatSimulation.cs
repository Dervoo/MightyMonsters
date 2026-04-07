using System;
using System.Collections.Generic;
using System.Linq;
using SWWoW.Models;

namespace SWWoW.Engine
{
    public class CombatSimulation
    {
        private CombatEngine engine;

        public void Run(List<Unit> playerTeam, List<Unit> enemyTeam)
        {
            engine = new CombatEngine(playerTeam, enemyTeam);
            Console.WriteLine("--- COMBAT START ---");

            int turnCounter = 0;
            string winner;

            while (!engine.IsCombatOver(out winner) && turnCounter < 100)
            {
                Unit activeUnit = engine.GetNextTurnUnit();
                ProcessTurn(activeUnit);
                turnCounter++;
            }

            Console.WriteLine("--- COMBAT END ---");
            Console.WriteLine($"Winner: {winner}");
        }

        private void ProcessTurn(Unit unit)
        {
            // Simple AI: target the first alive unit from the opposite team
            List<Unit> enemies = engine.PlayerTeam.Contains(unit) ? engine.EnemyTeam : engine.PlayerTeam;
            Unit target = enemies.FirstOrDefault(e => e.IsAlive);

            if (target == null) return;

            // Simple AI: Use special spell if ready, otherwise basic
            Spell spell = unit.Spells.FindLast(s => s.IsReady);
            
            if (spell != null)
            {
                float damage = engine.CalculateDamage(unit, target, spell);
                engine.ApplyDamage(target, damage);
                
                Console.WriteLine($"{unit.Name} uses {spell.Name} on {target.Name} for {damage:F1} dmg. (Target HP: {target.CurrentHP:F1})");
                
                spell.ResetCooldown();
            }

            // Cooldown reduction for all spells
            foreach (var s in unit.Spells) s.ReduceCooldown();
            
            engine.ResetActionBar(unit);
        }
    }
}
