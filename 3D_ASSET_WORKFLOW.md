---
noteId: "54af1aa033f211f1b2dd85643de1fb52"
tags: []

---

# 🗿 3D Model Creation & Integration (User + AI Agent)

Ten dokument opisuje podział pracy przy wdrażaniu modeli 3D do projektu SWWoW. Cel: Styl *Summoners War: Sky Arena*.

---

## 🛠 1. Twoje Zadanie: Generowanie Modelu (The Artist)

Użyj jednego z poniższych narzędzi, aby zamienić swoje grafiki AI (`characters/`) na pliki 3D:

### Rekomendowane Narzędzia AI 3D:
1. **[Meshy.ai](https://www.meshy.ai/)** (Najlepsze efekty "Text-to-3D" i "Image-to-3D").
2. **[Rodin (Deemos)](https://hyperhuman.deemos.com/rodin)** (Bardzo wysoka jakość detali).
3. **[CSM.ai](https://3d.csm.ai/)** (Dobre do szybkich prototypów).
4. **[Luma AI (Genie)](https://lumalabs.ai/genie)** (Darmowe, dobre do prostych kształtów).

### Co musisz zrobić:
1. Wgraj obrazek postaci do wybranego narzędzia.
2. Pobierz wynik jako plik **.fbx** oraz **teksturę (PNG/JPG)**.
3. Wrzuć pobrane pliki do folderu: `Assets/Resources/Models/`.
4. **Poinformuj mnie:** "Dodałem model dla [Nazwa Postaci]".

---

## 🤖 2. Moje Zadanie: Implementacja (The Architect)

Gdy tylko dodasz model do folderu, ja wykonuję następujące kroki:
1. **Konfiguracja Importu:** Ustawiam skalę modelu i parametry riggowania (Humanoid/Generic).
2. **Tworzenie Prefaba:** Tworzę `Prefab` jednostki w Unity z odpowiednim komponentem `Animator`.
3. **Update UnitDatabase:** Przypisuję nowy Prefab 3D do odpowiedniej jednostki w kodzie C#.
4. **Shader Setup:** Nakładam nasz dedykowany **Toon Shader** na model, aby uzyskać obrys (Outline) i rysunkowy wygląd.

---

## 🎨 3. Standard Wyglądu (Summoners War Look)

Każdy model po moim przetworzeniu będzie posiadał:
- **Cel-Shading:** 2-stopniowe cieniowanie (jasne/ciemne bez gradientów).
- **Bold Outline:** Wyraźny czarny kontur wokół sylwetki.
- **Emission:** Świecące runy i oczy (jeśli postać je posiada).

---

## ✅ Checklist dla Ciebie
1. [ ] Wygenerowano model 3D z obrazka.
2. [ ] Plik .fbx jest w `Assets/Resources/Models/`.
3. [ ] Tekstura jest w tym samym folderze.
4. [ ] Napisałeś do mnie: "Model [Nazwa] gotowy".
