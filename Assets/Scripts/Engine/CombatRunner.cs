using System.Collections.Generic;
using UnityEngine;
using SWWoW.Models;

namespace SWWoW.Engine
{
    public class CombatRunner : MonoBehaviour
    {
        private void Start()
        {
            Debug.Log("--- INITIALIZING COMBAT SIMULATION ---");

            // Setup Players
            List<Unit> players = new List<Unit>
            {
                UnitDatabase.GetOrcWarrior(),
                UnitDatabase.GetNightElfPriest()
            };

            // Setup Enemies
            List<Unit> enemies = new List<Unit>
            {
                UnitDatabase.GetUndeadWarlock(),
                UnitDatabase.GetTrollHunter()
            };

            CombatSimulation sim = new CombatSimulation();
            
            // Redirect simulation's Console.WriteLine would be hard, 
            // but for now, we just run it and hope the user sees Debug.Log 
            // if we update CombatSimulation to use a Logger.
            
            // For this prototype, I'll update CombatSimulation to use Debug.Log if it's in Unity.
            sim.Run(players, enemies);
        }
    }
}
