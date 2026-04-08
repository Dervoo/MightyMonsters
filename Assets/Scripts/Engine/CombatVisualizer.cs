using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using TMPro;
using SWWoW.Models;

public class CombatVisualizer : MonoBehaviour
{
    public static CombatVisualizer Instance { get; private set; }

    [Header("Positioning Settings")]
    public Vector3 playerTeamStartPos = new Vector3(-12, 0, -6);
    public Vector3 enemyTeamStartPos = new Vector3(12, 0, 6);
    public Vector3 lineDirection = new Vector3(1, 0, -1);
    public float unitSpacing = 5.0f; // Większy odstęp dla większych jednostek

    private Dictionary<string, GameObject> unitObjects = new Dictionary<string, GameObject>();
    private Dictionary<GameObject, string> reverseUnitLookup = new Dictionary<GameObject, string>();
    private Dictionary<string, Transform> hpBarForegrounds = new Dictionary<string, Transform>();
    private Dictionary<string, TextMeshPro> statusTexts = new Dictionary<string, TextMeshPro>();

    private void Awake()
    {
        Instance = this;
        SetupCamera();
    }

    private void SetupCamera()
    {
        Camera mainCam = Camera.main;
        if (mainCam != null)
        {
            mainCam.transform.position = new Vector3(-25, 18, -25);
            mainCam.transform.rotation = Quaternion.Euler(30, 45, 0);
            mainCam.backgroundColor = new Color(0.1f, 0.1f, 0.1f);
            mainCam.clearFlags = CameraClearFlags.SolidColor;
        }
    }

    public void SpawnUnit(UnitInstance unit, bool isPlayerTeam, int index)
    {
        GameObject go = GameObject.CreatePrimitive(isPlayerTeam ? PrimitiveType.Capsule : PrimitiveType.Cube);
        go.name = unit.Name + "_" + unit.id;
        
        // WIĘKSZE JEDNOSTKI
        go.transform.localScale = new Vector3(2.5f, 2.5f, 2.5f);
        
        Vector3 basePos = isPlayerTeam ? playerTeamStartPos : enemyTeamStartPos;
        go.transform.position = basePos + (lineDirection * (index * unitSpacing));
        go.transform.position += Vector3.up * 1.25f; // Podniesienie ze względu na skalę
        
        go.transform.LookAt(isPlayerTeam ? enemyTeamStartPos : playerTeamStartPos);
        if (isPlayerTeam) go.GetComponent<Renderer>().material.color = Color.cyan;
        else go.GetComponent<Renderer>().material.color = Color.red;

        CreateHPBar(go, unit);
        unitObjects[unit.id] = go;
        reverseUnitLookup[go] = unit.id;
    }

    private void CreateHPBar(GameObject parent, UnitInstance unit)
    {
        GameObject canvasGo = new GameObject("UnitUI");
        canvasGo.transform.SetParent(parent.transform);
        canvasGo.transform.localPosition = new Vector3(0, 1.8f, 0); // Relatywnie do skali 2.5
        canvasGo.transform.localScale = Vector3.one * 0.4f; // Odwrócenie skali rodzica dla UI
        
        // HP Bar Background (WIĘKSZY)
        GameObject bg = GameObject.CreatePrimitive(PrimitiveType.Quad);
        Destroy(bg.GetComponent<Collider>());
        bg.transform.SetParent(canvasGo.transform, false);
        bg.transform.localScale = new Vector3(10f, 1f, 1f);
        bg.GetComponent<Renderer>().material.color = Color.black;

        // HP Bar Foreground
        GameObject fg = GameObject.CreatePrimitive(PrimitiveType.Quad);
        Destroy(fg.GetComponent<Collider>());
        fg.transform.SetParent(canvasGo.transform, false);
        fg.transform.localScale = new Vector3(10f, 1f, 1f);
        fg.transform.localPosition = new Vector3(0, 0, -0.01f);
        fg.GetComponent<Renderer>().material.color = Color.green;
        hpBarForegrounds[unit.id] = fg.transform;

        // Name (WIĘKSZY)
        GameObject nameGo = new GameObject("NameText");
        nameGo.transform.SetParent(canvasGo.transform, false);
        nameGo.transform.localPosition = new Vector3(0, 1.5f, 0);
        var nameTm = nameGo.AddComponent<TextMeshPro>();
        nameTm.text = unit.Name;
        nameTm.fontSize = 18;
        nameTm.alignment = TextAlignmentOptions.Center;

        // Status Effects Text
        GameObject statusGo = new GameObject("StatusText");
        statusGo.transform.SetParent(canvasGo.transform, false);
        statusGo.transform.localPosition = new Vector3(0, -1.5f, 0);
        var statusTm = statusGo.AddComponent<TextMeshPro>();
        statusTm.text = "";
        statusTm.fontSize = 15;
        statusTm.alignment = TextAlignmentOptions.Center;
        statusTexts[unit.id] = statusTm;
    }

    public void SetUnitHighlight(string unitId, bool highlight)
    {
        if (unitObjects.ContainsKey(unitId))
        {
            var r = unitObjects[unitId].GetComponent<Renderer>();
            r.material.color = highlight ? Color.yellow : (unitId.Contains("Enemy") ? Color.red : Color.cyan);
        }
    }

    public string GetUnitIdAtMouse()
    {
        if (Mouse.current == null) return null;
        Vector2 mousePos = Mouse.current.position.ReadValue();
        Ray ray = Camera.main.ScreenPointToRay(mousePos);
        if (Physics.Raycast(ray, out RaycastHit hit))
        {
            if (reverseUnitLookup.ContainsKey(hit.collider.gameObject))
                return reverseUnitLookup[hit.collider.gameObject];
        }
        return null;
    }

    public IEnumerator AnimateAction(string actorId, string targetId, float value, string actionType)
    {
        if (!unitObjects.ContainsKey(actorId) || !unitObjects.ContainsKey(targetId)) yield break;

        GameObject actor = unitObjects[actorId];
        GameObject target = unitObjects[targetId];

        if (actionType == "Heal")
        {
            ShowValueText(target.transform.position + Vector3.up * 4, value, Color.green);
            StartCoroutine(FlashTarget(target, Color.green));
            yield return StartCoroutine(BounceActor(actor));
        }
        else if (actionType == "Buff" || actionType == "Debuff")
        {
            Color c = actionType == "Buff" ? Color.blue : Color.magenta;
            ShowValueText(target.transform.position + Vector3.up * 5f, 0, c, actionType);
            StartCoroutine(FlashTarget(target, c));
            yield return StartCoroutine(BounceActor(actor));
        }
        else // Attack
        {
            Vector3 originalPos = actor.transform.position;
            Vector3 targetPos = target.transform.position + (originalPos - target.transform.position).normalized * 3.0f;

            float elapsed = 0;
            float duration = 0.15f;
            while (elapsed < duration)
            {
                actor.transform.position = Vector3.Lerp(originalPos, targetPos, elapsed / duration);
                elapsed += Time.deltaTime;
                yield return null;
            }

            ShowValueText(target.transform.position + Vector3.up * 4, value, Color.yellow);
            StartCoroutine(FlashTarget(target, Color.white));
            StartCoroutine(ShakeObject(target));

            elapsed = 0;
            while (elapsed < duration)
            {
                actor.transform.position = Vector3.Lerp(targetPos, originalPos, elapsed / duration);
                elapsed += Time.deltaTime;
                yield return null;
            }
            actor.transform.position = originalPos;
        }
    }

    private IEnumerator BounceActor(GameObject actor)
    {
        float elapsed = 0;
        Vector3 originalPos = actor.transform.position;
        while (elapsed < 0.3f)
        {
            actor.transform.position = originalPos + Vector3.up * Mathf.Sin(elapsed * Mathf.PI * 3) * 1.0f;
            elapsed += Time.deltaTime;
            yield return null;
        }
        actor.transform.position = originalPos;
    }

    private void ShowValueText(Vector3 position, float value, Color color, string customText = "")
    {
        GameObject textGo = new GameObject("CombatText");
        textGo.transform.position = position;
        var tm = textGo.AddComponent<TextMeshPro>();
        if (!string.IsNullOrEmpty(customText)) tm.text = customText;
        else tm.text = (color == Color.green ? "+" : "") + value.ToString("F0");
        
        tm.color = color;
        tm.fontSize = 15; // WIĘKSZE OBRAŻENIA
        tm.alignment = TextAlignmentOptions.Center;
        StartCoroutine(AnimateDamageText(textGo));
    }

    private IEnumerator AnimateDamageText(GameObject go)
    {
        float elapsed = 0;
        float duration = 1.2f;
        Vector3 startPos = go.transform.position;
        while (elapsed < duration)
        {
            go.transform.position = startPos + Vector3.up * (elapsed * 4f);
            var tm = go.GetComponent<TextMeshPro>();
            tm.alpha = 1f - (elapsed / duration);
            elapsed += Time.deltaTime;
            yield return null;
        }
        Destroy(go);
    }

    private IEnumerator FlashTarget(GameObject target, Color flashColor)
    {
        var renderer = target.GetComponent<Renderer>();
        Color originalColor = renderer.material.color;
        renderer.material.color = flashColor;
        yield return new WaitForSeconds(0.15f);
        renderer.material.color = originalColor;
    }

    private IEnumerator ShakeObject(GameObject obj)
    {
        Vector3 pos = obj.transform.position;
        for (int i = 0; i < 5; i++)
        {
            obj.transform.position = pos + Random.insideUnitSphere * 0.5f;
            yield return new WaitForSeconds(0.02f);
        }
        obj.transform.position = pos;
    }

    public void UpdateUnitStatus(UnitInstance unit)
    {
        if (hpBarForegrounds.ContainsKey(unit.id))
        {
            float hpPercent = Mathf.Clamp01(unit.currentHP / unit.maxHP);
            Transform fg = hpBarForegrounds[unit.id];
            fg.localScale = new Vector3(hpPercent * 10f, 1f, 1f);
            fg.localPosition = new Vector3((hpPercent - 1f) * 5f, 0, -0.01f);

            string status = "";
            if (unit.buffDuration > 0) status += "<color=blue>[BUF:" + unit.buffDuration + "]</color> ";
            if (unit.debuffDuration > 0) status += "<color=purple>[DBF:" + unit.debuffDuration + "]</color>";
            statusTexts[unit.id].text = status;

            unitObjects[unit.id].SetActive(!unit.IsDead);
        }
    }
}
