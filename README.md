# Summoners War World of Warcraft (Unity Prototype)

## Opis projektu
Prototyp mobilnej turówki RPG inspirowanej Summoners War: Sky Arena w klimacie World of Warcraft.

## Wymagania
*   Unity Editor (wersja 2022.3 LTS lub nowsza)
*   Visual Studio / VS Code

## 🚀 Instrukcja dla początkujących (Jak to odpalić?)

Ponieważ repozytorium zawiera same skrypty (bez ustawień projektu), wykonaj poniższe kroki, aby zobaczyć walkę w konsoli Unity:

### 1. Przygotowanie projektu
1.  Otwórz **Unity Hub**.
2.  Stwórz **Nowy Projekt** (New Project).
3.  **BARDZO WAŻNE:** Wybierz szablon **Universal 3D**. (To jest właśnie URP).
    *   *Uwaga: Nie wybieraj "High Definition 3D" ani "3D (Built-in)".*
4.  Nazwij projekt np. `SWWoW_Prototype`.
4.  Gdy projekt się otworzy, przejdź do folderu, w którym go stworzyłeś, i skopiuj folder `Assets/Scripts` z tego repozytorium do folderu `Assets` Twojego nowego projektu.

### 2. Uruchomienie symulacji
1.  W Unity, w oknie **Project**, powinieneś widzieć folder `Scripts`.
2.  Kliknij prawym przyciskiem myszy w oknie **Hierarchy** (lewa strona) i wybierz **Create Empty**. Nazwij go `CombatTester`.
3.  Przeciągnij skrypt `CombatRunner` (znajdziesz go w `Scripts/Engine/`) na obiekt `CombatTester` w hierarchii.
4.  Otwórz okno **Console** (na dole lub `Window -> General -> Console`).
5.  Naciśnij przycisk **Play** na górze ekranu.

### 3. Wynik
W oknie **Console** zobaczysz logi z przebiegu walki: kto kogo zaatakował, ile zadał obrażeń i kto wygrał.

## Struktura folderów
*   `Assets/Scripts/Models/`: Klasy danych (Unit, Spell, Synergy).
*   `Assets/Scripts/Engine/`: Logika walki i system tur.
