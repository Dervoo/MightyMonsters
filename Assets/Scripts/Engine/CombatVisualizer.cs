using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using TMPro;
using SWWoW.Models;

namespace SWWoW.Engine
{
    public class CombatVisualizer : MonoBehaviour
    {
        public static CombatVisualizer Instance { get; private set; }

        [Header("Positioning Settings")]
        // Przesunięcie wszystkiego bardziej "w lewo" i do tyłu, aby zmieściło się w kadrze
        public Vector3 playerTeamStartPos = new Vector3(-15, 0, 5); 
        public Vector3 enemyTeamStartPos = new Vector3(25, 0, 25);
        public Vector3 lineDirection = new Vector3(2.0f, 0, -3.0f);
        public float unitSpacing = 12.0f;

        private Dictionary<string, GameObject> unitObjects = new Dictionary<string, GameObject>();
        private Dictionary<GameObject, string> reverseUnitLookup = new Dictionary<GameObject, string>();
        private Dictionary<string, Transform> hpBarForegrounds = new Dictionary<string, Transform>();

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
                mainCam.transform.position = new Vector3(-70, 60, -70);
                mainCam.transform.rotation = Quaternion.Euler(35, 45, 0);
                mainCam.backgroundColor = new Color(0.02f, 0.02f, 0.05f);
                mainCam.clearFlags = CameraClearFlags.SolidColor;
                mainCam.farClipPlane = 3000f;
            }
        }

        public void SpawnUnit(UnitInstance unit, bool isPlayerTeam, int index)
        {
            GameObject go = new GameObject(unit.Name + "_" + unit.id);
            GameObject visualChild = null;

            if (unit.data.prefab3D != null)
            {
                visualChild = Instantiate(unit.data.prefab3D, go.transform);
                visualChild.name = "Visual";
                visualChild.transform.localPosition = Vector3.zero;
                
                // SKALA 2000x (Zgodnie z prośbą)
                visualChild.transform.localScale = Vector3.one * 2000.0f; 
                
                ApplyToonMaterial(visualChild, unit.data.unitName.Replace(" ", ""));

                // Auto-Centering Pivot
                var meshRenderer = visualChild.GetComponentInChildren<MeshRenderer>();
                if (meshRenderer != null)
                {
                    Vector3 centerOffset = visualChild.transform.InverseTransformPoint(meshRenderer.bounds.center);
                    visualChild.transform.localPosition = -centerOffset;
                }

                if (visualChild.GetComponent<Collider>() == null)
                {
                    var col = visualChild.AddComponent<BoxCollider>();
                    col.size = new Vector3(2, 5, 2);
                }
            }
            else
            {
                visualChild = new GameObject("Visual");
                visualChild.transform.SetParent(go.transform);
                visualChild.transform.localPosition = Vector3.up * 8f;
                visualChild.transform.localScale = new Vector3(15f, 15f, 15f); // POWIĘKSZONE SPRITE'Y
                
                var sr = visualChild.AddComponent<SpriteRenderer>();
                sr.sprite = unit.data.visual;
                
                var collider = visualChild.AddComponent<BoxCollider>();
                collider.size = new Vector3(1, 1, 0.1f);
                visualChild.AddComponent<CharacterVisuals>().Setup(unit);
            }

            Vector3 basePos = isPlayerTeam ? playerTeamStartPos : enemyTeamStartPos;
            go.transform.position = basePos + (lineDirection * (index * unitSpacing));
            go.transform.position += Vector3.up * 2.0f;

            if (!isPlayerTeam) go.transform.rotation = Quaternion.Euler(0, 180, 0);

            CreateHPBar(go, unit);
            unitObjects[unit.id] = go;
            
            foreach (var col in go.GetComponentsInChildren<Collider>())
            {
                reverseUnitLookup[col.gameObject] = unit.id;
            }

            StartCoroutine(AnimateIdle(go));
        }

        private IEnumerator AnimateIdle(GameObject go)
        {
            float randomOffset = Random.value * Mathf.PI * 2;
            while (go != null)
            {
                float y = Mathf.Sin(Time.time * 1.5f + randomOffset) * 0.5f;
                go.transform.position += Vector3.up * y * Time.deltaTime;
                yield return null;
            }
        }

        private void ApplyToonMaterial(GameObject model, string unitName)
        {
            var renderers = model.GetComponentsInChildren<Renderer>();
            Shader toonShader = Shader.Find("Custom/SimpleToon");
            Texture2D texture = Resources.Load<Texture2D>("Models/" + unitName + "_Tex");

            foreach (var r in renderers)
            {
                Material mat = new Material(toonShader != null ? toonShader : Shader.Find("Universal Render Pipeline/Lit"));
                if (texture != null) mat.SetTexture("_MainTex", texture);
                else mat.color = Color.magenta;
                r.material = mat;
            }
        }

        private void CreateHPBar(GameObject parent, UnitInstance unit)
        {
            GameObject canvasGo = new GameObject("UnitUI");
            canvasGo.transform.SetParent(parent.transform);
            canvasGo.transform.localPosition = new Vector3(0, 25.0f, 0); // Jeszcze wyżej dla skali 2000x
            canvasGo.transform.localScale = Vector3.one * 1.2f;
            
            GameObject bg = GameObject.CreatePrimitive(PrimitiveType.Quad);
            Destroy(bg.GetComponent<Collider>());
            bg.transform.SetParent(canvasGo.transform, false);
            bg.transform.localScale = new Vector3(12f, 1.5f, 1f);
            bg.GetComponent<Renderer>().material.color = Color.black;

            GameObject fg = GameObject.CreatePrimitive(PrimitiveType.Quad);
            Destroy(fg.GetComponent<Collider>());
            fg.transform.SetParent(canvasGo.transform, false);
            fg.transform.localScale = new Vector3(12f, 1.5f, 1f);
            fg.transform.localPosition = new Vector3(0, 0, -0.01f);
            fg.GetComponent<Renderer>().material.color = Color.green;
            hpBarForegrounds[unit.id] = fg.transform;

            GameObject nameGo = new GameObject("NameText");
            nameGo.transform.SetParent(canvasGo.transform, false);
            nameGo.transform.localPosition = new Vector3(0, 3.0f, 0);
            var nameTm = nameGo.AddComponent<TextMeshPro>();
            nameTm.text = unit.Name;
            nameTm.fontSize = 35;
            nameTm.alignment = TextAlignmentOptions.Center;
        }

        public void SetUnitHighlight(string unitId, bool highlight)
        {
            if (unitObjects.ContainsKey(unitId))
            {
                var renderers = unitObjects[unitId].GetComponentsInChildren<Renderer>();
                foreach (var r in renderers)
                {
                    foreach (var mat in r.materials)
                    {
                        mat.SetColor("_EmissionColor", highlight ? Color.yellow * 0.5f : Color.black);
                        if (r is SpriteRenderer sr) sr.color = highlight ? Color.yellow : Color.white;
                    }
                }
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

            Vector3 originalPos = actor.transform.position;
            Vector3 targetPos = target.transform.position + (originalPos - target.transform.position).normalized * 12.0f;

            float elapsed = 0;
            float duration = 0.25f;
            while (elapsed < duration)
            {
                actor.transform.position = Vector3.Lerp(originalPos, targetPos, elapsed / duration);
                elapsed += Time.deltaTime;
                yield return null;
            }

            ShowValueText(target.transform.position + Vector3.up * 15, value, Color.yellow);
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

        private void ShowValueText(Vector3 position, float value, Color color)
        {
            GameObject textGo = new GameObject("CombatText");
            textGo.transform.position = position;
            var tm = textGo.AddComponent<TextMeshPro>();
            tm.text = value.ToString("F0");
            tm.color = color;
            tm.fontSize = 50;
            tm.alignment = TextAlignmentOptions.Center;
            StartCoroutine(AnimateDamageText(textGo));
        }

        private IEnumerator AnimateDamageText(GameObject go)
        {
            float elapsed = 0;
            float duration = 1.2f;
            while (elapsed < duration)
            {
                if (go != null) go.transform.position += Vector3.up * Time.deltaTime * 8f;
                elapsed += Time.deltaTime;
                yield return null;
            }
            Destroy(go);
        }

        private IEnumerator ShakeObject(GameObject obj)
        {
            Vector3 pos = obj.transform.position;
            for (int i = 0; i < 6; i++)
            {
                if (obj != null) obj.transform.position = pos + Random.insideUnitSphere * 2.0f;
                yield return new WaitForSeconds(0.02f);
            }
            if (obj != null) obj.transform.position = pos;
        }

        public void UpdateUnitStatus(UnitInstance unit)
        {
            if (hpBarForegrounds.ContainsKey(unit.id))
            {
                float hpPercent = Mathf.Clamp01(unit.currentHP / unit.maxHP);
                hpBarForegrounds[unit.id].localScale = new Vector3(hpPercent * 12f, 1.5f, 1f);
                unitObjects[unit.id].SetActive(!unit.IsDead);
            }
        }
    }
}
