# Changes Log

## [2026-04-09] - Unit Creation Standard & Roster Expansion
- **Standaryzacja Architektury Jednostek**:
    - Stworzono `UNIT_CREATION_GUIDE.md` – ścisły standard wizualny (Prompty AI) i techniczny.
    - Wprowadzono **Cykl Poprawek** dla dokumentacji, aby skrócić czas wdrażania nowych assetów.
- **Rozszerzenie Rosteru o 8 Jednostek**:
    - Pełna implementacja w `UnitDatabase.cs` na bazie grafik z folderu `characters/`.
    - Dodano jednostki: *Iron Crossbowman, Dawn Priestess, Bonebreaker, Demon Sorcerer, Forest Druid, Ancient Ent, Royal Protector, Void Assassin*.
    - Każda jednostka otrzymała unikalny zestaw umiejętności (S1 + Special/AoE) oraz system tagów pod synergie.
- **Usprawnienie Modelu Danych**:
    - Rozszerzono klasę `Unit` o listę `tags` dla obsługi frakcji i klas.
- **System Nadzoru i Logowania**:
    - Wprowadzono `errors_database.md` do śledzenia historii problemów technicznych.
    - Zautomatyzowano proces kopiowania assetów do folderu `Resources`.
- **Weryfikacja Architektury**:
    - **DECYZJA STRATEGICZNA:** Przejście na **Full 3D (Summoners War Style)**. 
    - Grafiki AI z folderu `characters/` zostają uznane za oficjalne referencje wizualne (Blueprints).
    - System Billboardingu zostaje oznaczony jako przestarzały (legacy).
- **Pierwszy Model 3D (Full Pipeline Test)**:
    - Pomyślnie wdrożono model **Royal Protector** (2000x skala).
    - Zautomatyzowano proces nakładania materiałów: `CombatVisualizer` automatycznie przypisuje `ToonShader` i teksturę.
    - Naprawiono pozycjonowanie sceny: Przesunięto jednostki i kamerę, aby cała walka była widoczna na ekranie.
    - Wdrożono system animacji Idle (płynne kołysanie).

## [2026-04-08] - Sprite-based Visuals & Character Architecture (Epic Fantasy Roster)
- **Implementacja Fazy 3: Architektura Unity**:
    - Przejście z prymitywnych brył 3D na system **SpriteRenderer (2.5D)**.
    - Dodano skrypt `CharacterVisuals.cs` obsługujący automatyczne podstawianie grafiki z `ScriptableObject`.
    - Wprowadzono system **Billboardingu** – postacie zawsze patrzą w stronę kamery.
    - Zaktualizowano model `Unit` o pole `visual` (Sprite).
- **Zatwierdzono Epic Fantasy Roster V1**: 9 unikalnych jednostek podzielonych na frakcje (Światło, Cień, Natura).
- **Poprawa Widoczności**: Powiększono modele jednostek i czcionki UI dla lepszej czytelności na urządzeniach mobilnych.

## [2026-04-08] - Stat Modifiers, UI Polish & Strategic Selection
- Skalowalne Statystyki (Buff/Debuff): +50% DMG, -50% DEF.
- Interaktywne Wybieranie Skilli: Podświetlanie wybranego przycisku, możliwość zmiany decyzji przed atakiem.
- Auto-Attack Logic: Szybkie kliknięcie we wroga bez wyboru skilla odpala S1.

## [2026-04-08] - Buffs, Debuffs & Auto-Attack
- System Statusów (Buff/Debuff) z licznikami tur.
- Warianty AoE dla wszystkich typów czarów.

## [2026-04-08] - AoE, Ikony i Interaktywność (SW Style)
- Ataki Obszarowe (AoE) i System Ikon/Kolorów UI.
- Logika Interakcji i Naprawa Błędów Input System.
