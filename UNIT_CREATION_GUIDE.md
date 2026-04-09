# 📘 Instrukcja Tworzenia Nowej Jednostki (Unit Creation Standard - FULL 3D)

Niniejszy dokument określa standardy dla architektury 3D (Summoners War Style).

---

## 🎨 1. Standard Wizualny (Visual Anchor - 3D)
- **Styl:** Stylized Low-poly.
- **Rendering:** Toon Shader / Cel-shading.
- **Referencje:** Wykorzystujemy grafiki AI do określenia kolorystyki, zbroi i broni.

---

## 🛠 2. Specyfikacja Techniczna (Unity Integration)

### A. Assety 3D
- **Format:** FBX.
- **Szkielet:** Humanoid (dla ludzi) / Generic (dla potworów).
- **Lokalizacja:** `Assets/Models/Characters/`.

### B. Implementacja w Kodzie
- Jednostki są spawnowane jako **Prefaby 3D**.
- Skrypt `CombatVisualizer` zarządza Animatorami jednostek.

---

## 📝 3. Lista Referencji (Concept Art)
*(Tu znajdują się Twoje grafiki z folderu characters/ jako baza dla modeli)*
