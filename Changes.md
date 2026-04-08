# Changes Log

## [2026-04-08] - ScriptableObject Refactoring & Automation Setup
-   **Refaktoryzacja Modeli:** `Unit` i `Spell` dziedziczą teraz po `ScriptableObject`.
-   **Rozdzielenie Danych:** Wprowadzono `UnitInstance` i `SpellInstance` do obsługi logiki w czasie rzeczywistym (HP, Cooldowny), co pozwala na wielokrotne użycie tych samych assetów danych.
-   **Aktualizacja Silnika:** `CombatEngine` i `CombatSimulation` obsługują teraz instancje obiektów.
-   **Unity Integration:** `CombatRunner` pozwala teraz na przeciąganie assetów jednostek prosto w edytorze Unity (Inspector).
-   **Nowy dokument:** Dodano `UnityCommands.md` z instrukcją automatyzacji i zarządzania danymi.
-   **Noob-Friendly Setup:** Dodano `CombatRunner.cs` i zaktualizowano `README.md`.

## [2026-04-07] - Initial Project Setup & Core Logic
-   Dodano `README.md` z instrukcjami.
-   Zaimplementowano **Modele Danych** (`Assets/Scripts/Models/`): `Unit`, `Spell`, `Synergy`.
-   Zaimplementowano **Silnik Walki** (`Assets/Scripts/Engine/CombatEngine.cs`):
    -   System Action Bar (inicjatywa oparta na szybkości).
    -   Formuła obliczania obrażeń z uwzględnieniem obrony i żywiołów.
-   Utworzono **Mock Database** (`UnitDatabase.cs`): 5 jednostek WoW-style.
-   Stworzono **Symulację Walki** (`CombatSimulation.cs`): Tekstowy log przebiegu starcia.
