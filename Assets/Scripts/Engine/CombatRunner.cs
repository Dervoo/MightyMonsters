using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using SWWoW.Models;

namespace SWWoW.Engine
{
    public class CombatRunner : MonoBehaviour
    {
        private CombatVisualizer visualizer;
        private CombatUIManager uiManager;
        
        private string selectedTargetId = null;
        private int selectedSpellIndex = -1;

        private void Start()
        {
            visualizer = GetComponent<CombatVisualizer>();
            if (visualizer == null) visualizer = gameObject.AddComponent<CombatVisualizer>();

            uiManager = GetComponent<CombatUIManager>();
            if (uiManager == null) uiManager = gameObject.AddComponent<CombatUIManager>();

            StartCoroutine(RunCombatSequence());
        }

        private IEnumerator RunCombatSequence()
        {
            Debug.Log("--- INITIALIZING INTERACTIVE COMBAT V2 ---");

            List<UnitInstance> players = new List<UnitInstance>();
            List<UnitInstance> enemies = new List<UnitInstance>();

            players.Add(UnitDatabase.GetOrcWarrior());
            players.Add(UnitDatabase.GetNightElfPriest());
            players.Add(UnitDatabase.GetDwarfPaladin());
            players.Add(UnitDatabase.GetTrollHunter());

            enemies.Add(UnitDatabase.GetUndeadWarlock());
            enemies.Add(UnitDatabase.GetTrollHunter());
            enemies.Add(UnitDatabase.GetUndeadWarlock());
            enemies.Add(UnitDatabase.GetTrollHunter());

            for (int i = 0; i < players.Count; i++) visualizer.SpawnUnit(players[i], true, i);
            for (int i = 0; i < enemies.Count; i++) visualizer.SpawnUnit(enemies[i], false, i);

            yield return new WaitForSeconds(1.0f);

            bool battleOver = false;
            while (!battleOver)
            {
                foreach (var p in players)
                {
                    if (p.IsDead) continue;
                    visualizer.SetUnitHighlight(p.id, true);
                    
                    selectedSpellIndex = -1;
                    selectedTargetId = null;
                    bool actionConfirmed = false;

                    uiManager.ShowSpellSelection(p, (index) => selectedSpellIndex = index);
                    
                    while (!actionConfirmed)
                    {
                        if (Mouse.current != null && Mouse.current.leftButton.wasPressedThisFrame)
                        {
                            string clickedId = visualizer.GetUnitIdAtMouse();
                            
                            if (selectedSpellIndex == -1 && clickedId != null && enemies.Exists(u => u.id == clickedId && !u.IsDead))
                            {
                                selectedSpellIndex = 0;
                                selectedTargetId = clickedId;
                                actionConfirmed = true;
                            }
                            else if (selectedSpellIndex != -1 && clickedId != null)
                            {
                                var spell = p.spells[selectedSpellIndex];
                                bool isPositive = IsPositiveSpell(spell.data.type);
                                
                                if (isPositive && players.Exists(u => u.id == clickedId && !u.IsDead))
                                {
                                    selectedTargetId = clickedId;
                                    actionConfirmed = true;
                                }
                                else if (!isPositive && enemies.Exists(u => u.id == clickedId && !u.IsDead))
                                {
                                    selectedTargetId = clickedId;
                                    actionConfirmed = true;
                                }
                            }
                        }
                        
                        if (selectedSpellIndex != -1)
                        {
                            var spell = p.spells[selectedSpellIndex];
                            if (IsAoESpell(spell.data.type) && Mouse.current != null && Mouse.current.leftButton.wasPressedThisFrame)
                            {
                                string clickedId = visualizer.GetUnitIdAtMouse();
                                if (clickedId != null) actionConfirmed = true;
                            }
                        }
                        yield return null;
                    }

                    uiManager.SetPanelActive(false);
                    visualizer.SetUnitHighlight(p.id, false);
                    
                    var finalSpell = p.spells[selectedSpellIndex];
                    bool finalIsAoE = IsAoESpell(finalSpell.data.type);
                    bool finalIsPositive = IsPositiveSpell(finalSpell.data.type);
                    
                    List<UnitInstance> targetList = new List<UnitInstance>();
                    if (finalIsAoE) targetList = finalIsPositive ? players.FindAll(u => !u.IsDead) : enemies.FindAll(u => !u.IsDead);
                    else targetList.Add(finalIsPositive ? players.Find(u => u.id == selectedTargetId) : enemies.Find(u => u.id == selectedTargetId));

                    float baseDmg = p.GetModifiedAttack() * finalSpell.data.multiplier;
                    foreach (var target in targetList)
                    {
                        yield return StartCoroutine(visualizer.AnimateAction(p.id, target.id, baseDmg, GetActionType(finalSpell.data.type)));
                        ApplyEffect(target, finalSpell.data.type, baseDmg);
                        visualizer.UpdateUnitStatus(target);
                    }
                    
                    finalSpell.ResetCooldown();
                    if (IsTeamDead(enemies)) { battleOver = true; break; }
                    yield return new WaitForSeconds(0.2f);
                }

                if (battleOver) break;

                foreach (var e in enemies)
                {
                    if (e.IsDead) continue;
                    var readySpells = e.spells.FindAll(s => s.IsReady);
                    var spell = readySpells[Random.Range(0, readySpells.Count)];
                    bool enemyIsAoE = IsAoESpell(spell.data.type);
                    bool enemyIsPositive = IsPositiveSpell(spell.data.type);

                    List<UnitInstance> targetList = new List<UnitInstance>();
                    if (enemyIsAoE) targetList = enemyIsPositive ? enemies.FindAll(u => !u.IsDead) : players.FindAll(u => !u.IsDead);
                    else
                    {
                        var t = enemyIsPositive ? GetRandomTarget(enemies) : GetRandomTarget(players);
                        if (t != null) targetList.Add(t);
                    }

                    foreach (var target in targetList)
                    {
                        float baseDmg = e.GetModifiedAttack() * spell.data.multiplier;
                        yield return StartCoroutine(visualizer.AnimateAction(e.id, target.id, baseDmg, GetActionType(spell.data.type)));
                        ApplyEffect(target, spell.data.type, baseDmg);
                        visualizer.UpdateUnitStatus(target);
                    }
                    spell.ResetCooldown();
                    if (IsTeamDead(players)) { battleOver = true; break; }
                    yield return new WaitForSeconds(0.2f);
                }

                UpdateStatusDurations(players);
                UpdateStatusDurations(enemies);
            }
        }

        private bool IsPositiveSpell(SpellType type) => type == SpellType.Heal || type == SpellType.AoEHeal || type == SpellType.Buff || type == SpellType.AoEBuff;
        private bool IsAoESpell(SpellType type) => type == SpellType.AoE || type == SpellType.AoEHeal || type == SpellType.AoEBuff || type == SpellType.AoEDebuff;

        private string GetActionType(SpellType type)
        {
            if (type == SpellType.Heal || type == SpellType.AoEHeal) return "Heal";
            if (type == SpellType.Buff || type == SpellType.AoEBuff) return "Buff";
            if (type == SpellType.Debuff || type == SpellType.AoEDebuff) return "Debuff";
            return "Attack";
        }

        private void ApplyEffect(UnitInstance target, SpellType type, float value)
        {
            switch (type)
            {
                case SpellType.Heal:
                case SpellType.AoEHeal: target.Heal(value); break;
                case SpellType.Buff:
                case SpellType.AoEBuff: target.buffDuration = 2; break;
                case SpellType.Debuff:
                case SpellType.AoEDebuff: target.debuffDuration = 2; break;
                default: target.TakeDamage(value); break;
            }
        }

        private void UpdateStatusDurations(List<UnitInstance> team)
        {
            foreach (var unit in team)
            {
                if (unit.buffDuration > 0) unit.buffDuration--;
                if (unit.debuffDuration > 0) unit.debuffDuration--;
                foreach (var spell in unit.spells) spell.ReduceCooldown();
                visualizer.UpdateUnitStatus(unit);
            }
        }

        private UnitInstance GetRandomTarget(List<UnitInstance> targets)
        {
            var alive = targets.FindAll(t => !t.IsDead);
            if (alive.Count == 0) return null;
            return alive[Random.Range(0, alive.Count)];
        }

        private bool IsTeamDead(List<UnitInstance> team)
        {
            return team.TrueForAll(t => t.IsDead);
        }
    }
}
