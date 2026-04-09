---
noteId: "7fa33d3033f311f1b2dd85643de1fb52"
tags: []

---

# 🎨 Toon Shader Guide (Summoners War Style)

Ten dokument opisuje konfigurację wizualną modeli 3D w projekcie SWWoW.

---

## 🛠 1. Parametry Shadera (Custom/SimpleToon)

Każdy model 3D powinien używać materiału z shaderem `Custom/SimpleToon`. Oto kluczowe suwaki:

| Parametr | Opis | Rekomendowana Wartość |
| :--- | :--- | :--- |
| **Base Map** | Główna tekstura z kolorem postaci. | Plik .png z Meshy.ai |
| **Shadow Threshold** | Punkt odcięcia cienia (twardy cień). | `0.4 - 0.6` |
| **Shadow Smoothness** | Rozmycie krawędzi cienia. | `0.02` (dla stylu komiksowego) |
| **Emission Map** | Tekstura określająca co ma świecić. | Opcjonalna (np. runy) |
| **Emission Color** | Kolor i siła blasku. | Dowolny (np. złoty/niebieski) |

---

## 🖋 2. System Obrysu (Outline)

Obecny shader obsługuje podstawowe cieniowanie. Aby uzyskać czarny obrys (Outline):
1. Dodaj drugi komponent **Renderer** do modelu (Inverted Hull Method).
2. Nałóż na niego materiał czarny z wyłączonymi cieniami.
*(Agent zautomatyzuje to w skrypcie implementacyjnym).*

---

## 🔄 3. Aktualizacja Materiałów
Gdy wrzucisz model FBX, Agent automatycznie stworzy materiał `.mat`, przypisze teksturę i ustawi shader na `Custom/SimpleToon`.
