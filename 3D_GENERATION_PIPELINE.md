---
noteId: "ecd75a6033f011f1b2dd85643de1fb52"
tags: []

---

# 💎 3D Generation Pipeline (Summoners War Style)

Niniejsza instrukcja określa proces transformacji grafik 2D (Concept Art) w pełni funkcjonalne modele 3D zoptymalizowane pod mobilny silnik Unity, zachowując estetykę *Summoners War: Sky Arena*.

---

## 🎨 1. Estetyka Wizualna (Stylistic Constraints)

Aby uzyskać efekt "SW Style", modele muszą spełniać poniższe kryteria:
- **Low-Poly Heroic:** Wyraźne, uproszczone bryły. Unikamy mikro-detali na siatce (zastępujemy je teksturą).
- **Big Proportions:** Powiększone dłonie, stopy i głowy (lepsza czytelność na ekranie komórki).
- **Vibrant Colors:** Nasycone barwy, które "świecą" nawet bez dynamicznego oświetlenia.
- **Cel-Shaded Outline:** Każdy model MUSI mieć czarny obrys (Inverted Hull Method).

---

## 🛠 2. Proces Generowania (The 4-Step Method)

### Krok A: Przygotowanie Referencji (AI Refinement)
Zanim użyjesz narzędzia 3D, Twoja grafika musi być "czysta":
1. **Remove Background:** Usuń białe tło (zrobione w `Assets/Resources/Characters/`).
2. **Orthographic Projection:** Jeśli to możliwe, wygeneruj widok z przodu (Front View) i z boku (Side View) dla tej samej postaci, aby zachować skalę.

### Krok B: Generowanie Siatki (Mesh Generation)
Użyj narzędzi AI do generowania 3D (np. *Meshy.ai, Rodin, CSM.ai*):
- **Parametr "Target Polycount":** 3,000 - 5,000 trójkątów (Triangles) na postać.
- **Parametr "Topology":** Quad-dominant (ułatwia animację).
- **Zadanie:** Przekonwertuj `Char_[Nazwa].png` na plik `.obj` lub `.fbx`.

### Krok C: Teksturowanie (Stylized Texturing)
- **Texture Size:** 1024x1024 px.
- **Baking:** Wypalenie *Ambient Occlusion* bezpośrednio w teksturze (SW nie używa ciężkich cieni w czasie rzeczywistym).
- **Emission Map:** Stwórz maskę dla świecących run (`Glowing Runes`), aby świeciły w ciemności/podczas ataku.

### Krok D: Rigging & Skinning (System Kości)
1. **Mixamo Integration:** Prześlij model do Adobe Mixamo dla automatycznego riggingu (Standard Humanoid).
2. **Custom Bones:** Dla jednostek takich jak *Ancient Ent* (drzewo), wymagany jest prosty, dedykowany rig dla gałęzi.

---

## 📐 3. Standardy Implementacji w Unity

### 1. Toon Shader Setup
Każdy model 3D w projekcie musi używać materiału z **URP Toon Shader**:
- **Base Map:** Twoja tekstura.
- **Shade Steps:** 2 (twarde przejście między światłem a cieniem).
- **Outline Thickness:** 0.02 - 0.05 (zależnie od skali).

### 2. Animator Controller
Każda jednostka musi posiadać stany:
- `Idle`: Spokojne oddychanie/lekkie kołysanie.
- `Attack_S1`: Szybki atak podstawowy.
- `Attack_S2`: Widowiskowa animacja czaru specjalnego.
- `Hit`: Reakcja na otrzymanie obrażeń.
- `Die`: Animacja upadku.

---

## 📝 4. Checklist dla Nowego Modelu 3D

- [ ] Model wyeksportowany jako `.fbx` (Scale: 1.0, Forward: -Z).
- [ ] Tekstura przypisana do Toon Shadera.
- [ ] Animator przypisany do Prefaba.
- [ ] Punkt `VFX_Spawn` ustawiony na dłoniach/broni (dla efektów czarów).
- [ ] Collider ustawiony wokół tułowia.

---

## 🔄 5. Cykl Aktualizacji (Errors & Changes)
Wszelkie problemy z importem (np. "odwrócone ściany" - Backface Culling) należy wpisać do `errors_database.md`.
Poprawne wdrożenie modelu 3D odnotowujemy w `Changes.md`.
