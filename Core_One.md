# Project Scope: Fantasy Turn-Based RPG (Mobile)

## 1. Project Overview
**Role:** You are an expert Mobile Game Developer and Software Architect. 
**Goal:** Create a prototype of a mobile turn-based RPG inspired by "Summoner's War: Sky Arena", but heavily simplified. 
**Theme/Art Style:** High Fantasy, stylistically similar to "World of Warcraft" (bold silhouettes, stylized textures, vibrant colors, classic fantasy races/classes like Orcs, Elves, Undead).
**Target Platform:** Mobile (Touchscreen UI).
**Current Phase:** Single-player MVP (Minimum Viable Product). No multiplayer, no complex economy, no gear/runes system yet.

---

## 2. Core Gameplay Mechanics

### A. Entities (Monsters/Heroes)
Every unit in the game has the following basic structure:
* **Name:** display name.
* **Level:** 1 to Max (e.g., 100). Increases base stats.
* **Attributes (Simplified):**
    * `HP` (Health Points)
    * `Attack` (Damage scaling)
    * `Defense` (Damage mitigation)
    * `Speed` (Determines turn order in combat)
* **Element (Rock-Paper-Scissors system):** Fire, Water, Earth. (Light/Dark reserved for future).

### B. Combat System (Turn-Based)
* **Turn Order:** Decided by the `Speed` attribute. An "Action Bar" fills up; the first unit to reach 100% takes a turn.
* **Teams:** Player Team (max 4 units) vs. Enemy Team (max 4 units).
* **Win Condition:** Reduce all enemy units' HP to 0.

### C. Spells & Skills
Each unit has exactly 2 or 3 skills:
1.  **Basic Attack:** No cooldown, deals basic elemental damage.
2.  **Special Spell:** Has a cooldown (e.g., 3 turns). Can be a strong attack, an AoE (Area of Effect) spell, a heal, or a buff/debuff.
3.  **Passive/Leader Skill (Optional for MVP):** A constant effect (e.g., "Increases speed of all allied Orcs by 10%").

### D. Synergies
Simple synergy logic based on tags (e.g., Race or Element).
* *Example Synergy:* If 3 units with the "Undead" tag are in the team, all Undead units get +15% Attack.
* *Example Synergy:* If 2 "Fire" units are present, apply a burn effect on basic attacks.

---

## 3. Data Structure & Architecture
Keep the logic modular and decoupled from the UI.
* **Game Manager:** Handles the state of the game (Main Menu, Team Selection, Combat).
* **Combat Engine:** Calculates damage, manages the action queue (Speed), and tracks cooldowns.
* **Unit Database:** A JSON or ScriptableObject (if Unity) / Resource (if Godot) structure storing base stats for all available monsters.

---

## 4. Phase 1 Milestones (Your Tasks as an AI Agent)

Please execute these steps sequentially when asked:

* **Step 1: Data Models.** Define the basic data structures/classes for `Unit`, `Spell`, and `Synergy`.
* **Step 2: Combat Engine Logic.** Write the core loop for the Action Bar (Speed tracking) and turn resolution.
* **Step 3: Damage Calculation.** Create the math formulas for damage dealing (taking into account Attack, Defense, and Elemental advantage/disadvantage).
* **Step 4: Mock Database.** Generate 5 distinct WoW-style fantasy units (e.g., Orc Warrior, Night Elf Priest, Undead Warlock) with their base stats and spells.
* **Step 5: Combat Simulation.** Create a basic text/console based combat log to simulate a fight between two teams to test the logic before implementing the graphical UI.

## 5. Coding Guidelines
* Write clean, self-documenting code.
* Use standard design patterns (e.g., State Machine for combat phases, Observer pattern for health updates).
* If writing logic, please ask me which Game Engine or Framework we are using before providing engine-specific code (e.g., MonoBehaviour for Unity vs. Node for Godot).

## 6. Machine Learning Integration (Future Feature)
**Feature Name:** Tactical Oracle (Strategic Assistant)
* **Goal:** Use ML-Agents to analyze battlefield state and provide real-time suggestions.
* **Architecture Requirement:** The Combat Engine must be "headless" (pure logic, no visual dependencies). This allows us to run thousands of simulations per second in the background to train an ML model.
* **Data Logging:** Every turn (state of HP, Cooldowns, Buffs) and the final outcome (Win/Loss) must be loggable to a JSON format to create a training dataset.

(Pomysł na Feature ML: "Strategic Insight Engine"
Zamiast zwykłego przycisku "Auto-battle", który po prostu klika losowe skille, możesz wdrożyć system oparty na Reinforcement Learning (RL) lub prostszej sieci neuronowej, która analizuje aktualny stan pola bitwy.

Jak by to działało?
Analiza Synergii i Kontr: Model ML analizuje skład drużyny przeciwnika (atrybuty, typy jednostek) i podpowiada graczowi przed walką: "Twoja drużyna ma 15% mniejszą szansę na wygraną z tym ustawieniem. Spróbuj dodać jednostkę typu Fire, aby aktywować synergię przeciwko temu Bossowi".

Podpowiedzi w trakcie tury: Podczas walki, system podświetla skill, który statystycznie daje największą szansę na zwycięstwo w danej konfiguracji HP i cooldownów.

Dynamiczny Poziom Trudności (DDA): Jeśli gra wykryje, że gracz nudzi się zbyt łatwymi walkami, ML może "w locie" lekko zmodyfikować zachowanie AI przeciwników, aby zaczęli grać bardziej agresywnie lub lepiej targetować Twoich medyków.

Dlaczego to ma sens?
Dla gracza: To nie jest tylko "bot". To narzędzie edukacyjne, które uczy gracza głębi Twojego systemu synergii.

Technologicznie: W Unity masz pakiet ML-Agents. Możesz wytrenować model na tysiącach symulacji walk "bot vs bot" w chmurze, a potem wyeksportować gotowy model (plik .onnx), który działa lokalnie na telefonie niemal bez obciążenia procesora.)

Podsumowanie technologii:
Silnik: Unity (ze względu na ML-Agents i Firebase SDK).

Backend: Firebase (Firestore dla statystyk potworów, Auth dla kont graczy).

ML: Unity ML-Agents (Reinforcement Learning).

Wzoruj sie na grach Summoners War sky arena, Age of Magic, raid shadow legends, idle heroes