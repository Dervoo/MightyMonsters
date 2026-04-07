# Changes Log

## [2026-04-07] - Initial Project Setup & Core Logic
-   Dodano `README.md` z instrukcjami.
-   Zaimplementowano **Modele Danych** (`Assets/Scripts/Models/`): `Unit`, `Spell`, `Synergy`.
-   Zaimplementowano **Silnik Walki** (`Assets/Scripts/Engine/CombatEngine.cs`):
    -   System Action Bar (inicjatywa oparta na szybkości).
    -   Formuła obliczania obrażeń z uwzględnieniem obrony i żywiołów.
-   Utworzono **Mock Database** (`UnitDatabase.cs`): 5 jednostek WoW-style.
-   Stworzono **Symulację Walki** (`CombatSimulation.cs`): Tekstowy log przebiegu starcia.
