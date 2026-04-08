using System.Collections.Generic;
using UnityEngine;
using SWWoW.Models;

namespace SWWoW.Engine
{
    public class CombatRunner : MonoBehaviour
    {
        [Header("Manual Setup (Leave empty to use Mock Data)")]
        public List<Unit> playerTeamData;
        public List<Unit> enemyTeamData;

        private void Start()
        {
            Debug.Log("--- INITIALIZING COMBAT SIMULATION ---");

            List<UnitInstance> players = new List<UnitInstance>();
            List<UnitInstance> enemies = new List<UnitInstance>();

            if (playerTeamData != null && playerTeamData.Count > 0)
            {
                foreach (var data in playerTeamData) players.Add(new UnitInstance(data));
            }
            else
            {
                players.Add(UnitDatabase.GetOrcWarrior());
                players.Add(UnitDatabase.GetNightElfPriest());
            }

            if (enemyTeamData != null && enemyTeamData.Count > 0)
            {
                foreach (var data in enemyTeamData) enemies.Add(new UnitInstance(data));
            }
            else
            {
                enemies.Add(UnitDatabase.GetUndeadWarlock());
                enemies.Add(UnitDatabase.GetTrollHunter());
            }

            CombatSimulation sim = new CombatSimulation();
            sim.Run(players, enemies);
        }
    }
}
