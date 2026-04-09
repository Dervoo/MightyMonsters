---
noteId: "24678e1033f011f1b2dd85643de1fb52"
tags: []

---

# 🛠 Database Błędów i Problemów (Error Log)

Oto historia problemów napotkanych podczas tworzenia projektu SWWoW oraz ich rozwiązania.

| Data | Problem | Przyczyna | Rozwiązanie | Status |
| :--- | :--- | :--- | :--- | :--- |
| 2026-04-09 | Jednostki wyświetlają się jako "migające kropki" / brak grafik. | Unity importuje PNG jako 'Texture' zamiast 'Sprite (2D and UI)', przez co Resources.Load<Sprite> zwraca null. | Ręczna zmiana Texture Type w Inspectorze Unity na Sprite (2D and UI). | 🟠 Oczekiwanie na akcję użytkownika |
| 2026-04-09 | Zmiana wizji: Przejście z 2.5D na Full 3D. | Obecna architektura oparta na SpriteRenderer nie pozwala na obracanie kamery i animacje 3D (SW Style). | Rezygnacja z Billboardingu na rzecz prefabów 3D. Grafiki AI stają się referencjami. | ✅ Zaktualizowano strategię |
| 2026-04-09 | Brak widoczności Royal Protectora na scenie. | Modele z Meshy.ai mogą mieć mikroskopijną skalę i znajdować się pod ziemią (Pivot w środku modelu). | Zwiększono skalę do 5.0, dodano podniesienie o 1 unit (Y-Offset) i debug logi. | ✅ Zaktualizowano skalę |
| 2026-04-09 | Missing Sprite for Bonebreaker / inne. | UnitDatabase szukał nazw z spacjami, a pliki Resources mają nazwy CamelCase (np. 'Bonebreaker'). | Poprawiono UnitDatabase, aby używał 'internalName' do ładowania assetów. | ✅ Naprawiono ścieżki |
| 2026-04-09 | Royal Protector wciąż niewidoczny (mimo logów). | Skala 5.0 wciąż może być zbyt mała dla modeli AI, które często mają jednostki w milimetrach. | Zwiększono skalę do 50.0, podniesiono kamerę i dodano kolor Magenta dla błędów tekstur. | 🟠 Oczekiwanie na test |
