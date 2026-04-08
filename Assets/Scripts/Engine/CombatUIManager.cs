using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using SWWoW.Models;
using System;

namespace SWWoW.Engine
{
    public class CombatUIManager : MonoBehaviour
    {
        public static CombatUIManager Instance { get; private set; }

        private Transform spellButtonsParent;
        private List<GameObject> activeButtons = new List<GameObject>();
        private int currentSelectedIndex = -1;

        private void Awake()
        {
            Instance = this;
            CreateUIOverlay();
        }

        private void CreateUIOverlay()
        {
            if (FindObjectOfType<UnityEngine.EventSystems.EventSystem>() == null)
            {
                GameObject eventSystem = new GameObject("EventSystem");
                eventSystem.AddComponent<UnityEngine.EventSystems.EventSystem>();
                eventSystem.AddComponent<UnityEngine.InputSystem.UI.InputSystemUIInputModule>();
            }

            GameObject canvasGo = new GameObject("CombatCanvas");
            Canvas canvas = canvasGo.AddComponent<Canvas>();
            canvas.renderMode = RenderMode.ScreenSpaceOverlay;
            canvasGo.AddComponent<CanvasScaler>();
            canvasGo.AddComponent<GraphicRaycaster>();

            GameObject panelGo = new GameObject("SpellPanel");
            panelGo.transform.SetParent(canvasGo.transform);
            RectTransform panelRect = panelGo.AddComponent<RectTransform>();
            panelRect.anchorMin = new Vector2(0.5f, 0);
            panelRect.anchorMax = new Vector2(0.5f, 0);
            panelRect.pivot = new Vector2(0.5f, 0);
            panelRect.sizeDelta = new Vector2(800, 150);
            panelRect.anchoredPosition = new Vector2(0, 50);

            spellButtonsParent = panelGo.transform;
            panelGo.SetActive(false);
        }

        public void SetPanelActive(bool active)
        {
            spellButtonsParent.gameObject.SetActive(active);
        }

        public void ShowSpellSelection(UnitInstance unit, Action<int> onSelect)
        {
            ClearButtons();
            currentSelectedIndex = -1;

            for (int i = 0; i < unit.spells.Count && i < 3; i++)
            {
                int index = i;
                var spell = unit.spells[i];
                GameObject btnGo = CreateButton(spell, i);
                Button btn = btnGo.GetComponent<Button>();
                
                btn.interactable = spell.IsReady;
                btn.onClick.AddListener(() => {
                    currentSelectedIndex = index;
                    HighlightButton(index);
                    onSelect?.Invoke(index);
                });
                activeButtons.Add(btnGo);
            }
            SetPanelActive(true);
        }

        private void HighlightButton(int selectedIndex)
        {
            for (int i = 0; i < activeButtons.Count; i++)
            {
                var outline = activeButtons[i].GetComponent<Outline>();
                if (outline == null) outline = activeButtons[i].AddComponent<Outline>();
                outline.effectDistance = new Vector2(5, 5);
                outline.effectColor = (i == selectedIndex) ? Color.yellow : Color.clear;
            }
        }

        private GameObject CreateButton(SpellInstance spell, int index)
        {
            GameObject btnGo = new GameObject("SpellButton_" + index);
            btnGo.transform.SetParent(spellButtonsParent);
            
            Image img = btnGo.AddComponent<Image>();
            Color baseColor = GetColorForType(spell.data.type);
            if (!spell.IsReady) baseColor *= 0.5f;
            img.color = baseColor;
            
            btnGo.AddComponent<Button>();
            RectTransform rect = btnGo.GetComponent<RectTransform>();
            rect.sizeDelta = new Vector2(160, 120);
            rect.anchoredPosition = new Vector2((index - 1) * 180, 0);

            // Icon
            GameObject iconGo = new GameObject("Icon");
            iconGo.transform.SetParent(btnGo.transform);
            var iconTm = iconGo.AddComponent<TextMeshProUGUI>();
            iconTm.text = GetIconText(spell.data.type);
            iconTm.fontSize = 40;
            iconTm.alignment = TextAlignmentOptions.Top;
            RectTransform iconRect = iconGo.GetComponent<RectTransform>();
            iconRect.anchorMin = Vector2.zero;
            iconRect.anchorMax = Vector2.one;
            iconRect.sizeDelta = new Vector2(0, -20);
            iconRect.anchoredPosition = new Vector2(0, 10);

            // Text
            GameObject textGo = new GameObject("Text");
            textGo.transform.SetParent(btnGo.transform);
            var tm = textGo.AddComponent<TextMeshProUGUI>();
            tm.text = $"\n\n<b>{spell.data.spellName}</b>\n<size=12>x{spell.data.multiplier}</size>";
            if (!spell.IsReady) tm.text += $"\n<color=red>CD: {spell.currentCooldown}</color>";
            tm.fontSize = 18;
            tm.alignment = TextAlignmentOptions.Center;
            tm.color = Color.white;
            
            RectTransform textRect = textGo.GetComponent<RectTransform>();
            textRect.anchorMin = Vector2.zero;
            textRect.anchorMax = Vector2.one;
            textRect.sizeDelta = Vector2.zero;

            return btnGo;
        }

        private Color GetColorForType(SpellType type)
        {
            return type switch
            {
                SpellType.Heal => new Color(0.1f, 0.6f, 0.1f, 0.9f),
                SpellType.AoEHeal => new Color(0.1f, 0.8f, 0.4f, 0.9f),
                SpellType.SpecialAttack => new Color(0.6f, 0.1f, 0.1f, 0.9f),
                SpellType.AoE => new Color(0.8f, 0.4f, 0.1f, 0.9f),
                SpellType.Buff => new Color(0.1f, 0.4f, 0.8f, 0.9f),
                SpellType.AoEBuff => new Color(0.1f, 0.4f, 0.8f, 0.9f),
                SpellType.Debuff => new Color(0.4f, 0.1f, 0.6f, 0.9f),
                SpellType.AoEDebuff => new Color(0.4f, 0.1f, 0.6f, 0.9f),
                _ => Color.gray
            };
        }

        private string GetIconText(SpellType type)
        {
            return type switch
            {
                SpellType.Heal => "HP+",
                SpellType.AoEHeal => "ALL+",
                SpellType.SpecialAttack => "PWR",
                SpellType.AoE => "AOE",
                SpellType.Buff => "BUF",
                SpellType.AoEBuff => "BUF",
                SpellType.Debuff => "DBF",
                SpellType.AoEDebuff => "DBF",
                _ => "ATK"
            };
        }

        private void ClearButtons()
        {
            foreach (var btn in activeButtons) Destroy(btn);
            activeButtons.Clear();
        }
    }
}
