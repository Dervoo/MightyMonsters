---
noteId: "e51b5910332411f19906ef3e47744fbb"
tags: []

---

# 🛠 Unity Automation & Manual Commands

Plik ten zawiera instrukcje dotyczące zarządzania danymi w Unity oraz komendy, które możesz mi zlecić do automatyzacji projektu.

---

## 📋 Zarządzanie ScriptableObjects

System został oparty na **ScriptableObjects**, co pozwala na tworzenie jednostek i czarów bez dotykania kodu.

### Jak stworzyć nową jednostkę (Manualnie):
1.  W oknie **Project** kliknij prawym przyciskiem -> **Create** -> **SWWoW** -> **Spell**.
    *   Ustaw nazwę (np. `Fireball`), cooldown i mnożnik obrażeń.
2.  Kliknij prawym przyciskiem -> **Create** -> **SWWoW** -> **Unit**.
    *   Ustaw statystyki (HP, Attack, Defense, Speed).
    *   W sekcji **Spells** przeciągnij stworzone wcześniej czary.
3.  Przeciągnij nową jednostkę do obiektu **CombatTester** w polu `Player Team Data` lub `Enemy Team Data`.

---

## 🤖 Komendy Automatyzacji (Co możesz mi zlecić)

Możesz poprosić mnie o wykonanie następujących zadań poprzez czat:

### 1. Generowanie danych (Scripting)
*   *"Wygeneruj 10 zbalansowanych jednostek typu Water."* -> Przygotuję kod, który stworzy te assety automatycznie.
*   *"Zmień balans wszystkich czarów: zwiększ cooldowny o 1."* -> Napiszę skrypt edytorski, który przejdzie przez wszystkie Twoje assety i je zaktualizuje.

### 2. Rozbudowa Silnika
*   *"Dodaj system buffów/debuffów (np. Bleed, Stun)."* -> Zaktualizuję modele `UnitInstance` i `CombatEngine`.
*   *"Zaimplementuj system synergii w walce."* -> Dopiszę logikę sprawdzającą tagi (np. "Orc") podczas startu walki.

### 3. Skrypty Pomocnicze (Editor Tools)
*   *"Stwórz przycisk w edytorze do czyszczenia konsoli i resetu sceny."*
*   *"Zrób okno statystyk, które pokaże mi balans wszystkich jednostek w tabeli."*

---

## ⚠️ Ważne uwagi
*   **Assety:** Ja nie widzę fizycznych plików `.asset` w Twoim folderze Unity, dopóki mi o nich nie powiesz lub nie wkleisz ich zawartości (jeśli otworzysz je w Notatniku).
*   **Struktura:** Zawsze trzymaj skrypty w `Assets/Scripts`, aby moje ścieżki się zgadzały.
