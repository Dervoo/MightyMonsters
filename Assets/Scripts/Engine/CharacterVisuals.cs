using UnityEngine;
using SWWoW.Models;

namespace SWWoW.Engine
{
    public class CharacterVisuals : MonoBehaviour
    {
        public SpriteRenderer spriteRenderer;
        public SpriteRenderer shadowRenderer;
        
        [Header("Animation Settings")]
        public float idleFloatAmplitude = 0.15f;
        public float idleFloatSpeed = 2.5f;
        
        private UnitInstance unitInstance;
        private Vector3 startPos;

        public void Setup(UnitInstance unit)
        {
            unitInstance = unit;
            startPos = transform.localPosition;
            
            if (spriteRenderer == null) spriteRenderer = GetComponent<SpriteRenderer>();
            spriteRenderer.sortingOrder = 10; // Upewniamy się, że są nad tłem

            // Tworzenie cienia
            if (shadowRenderer == null) CreateShadow();

            if (unit.data.visual != null)
            {
                spriteRenderer.sprite = unit.data.visual;
                spriteRenderer.color = Color.white;
            }
            else
            {
                // FALLBACK: Tworzymy prosty biały kwadrat programowo, jeśli nic nie ma
                Texture2D tex = new Texture2D(1, 1);
                tex.SetPixel(0, 0, Color.white);
                tex.Apply();
                spriteRenderer.sprite = Sprite.Create(tex, new Rect(0, 0, 1, 1), new Vector2(0.5f, 0.5f));
                
                spriteRenderer.color = unit.Element == ElementType.Fire ? Color.red : 
                                     unit.Element == ElementType.Water ? Color.blue : Color.green;
            }
        }

        private void CreateShadow()
        {
            GameObject shadowGo = new GameObject("Shadow");
            shadowGo.transform.SetParent(transform.parent);
            shadowGo.transform.localPosition = new Vector3(0, -1.8f, 0);
            shadowGo.transform.localScale = new Vector3(3f, 0.8f, 1f);
            shadowGo.transform.localRotation = Quaternion.Euler(90, 0, 0);
            
            shadowRenderer = shadowGo.AddComponent<SpriteRenderer>();
            shadowRenderer.sortingOrder = 5; // Pod postacią
            
            // Prosta tekstura cienia
            Texture2D tex = new Texture2D(1, 1);
            tex.SetPixel(0, 0, Color.black);
            tex.Apply();
            shadowRenderer.sprite = Sprite.Create(tex, new Rect(0, 0, 1, 1), new Vector2(0.5f, 0.5f));
            shadowRenderer.color = new Color(0, 0, 0, 0.4f);
        }

        private void Update()
        {
            if (Camera.main != null)
            {
                transform.rotation = Quaternion.Euler(0, Camera.main.transform.rotation.eulerAngles.y, 0);
            }

            float newY = startPos.y + Mathf.Sin(Time.time * idleFloatSpeed) * idleFloatAmplitude;
            transform.localPosition = new Vector3(startPos.x, newY, startPos.z);
        }
    }
}
