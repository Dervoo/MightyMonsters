using UnityEditor;
using UnityEngine;
using UnityEditor.SceneManagement;
using SWWoW.Engine;

[InitializeOnLoad]
public class SceneSetupManager
{
    static SceneSetupManager()
    {
        EditorApplication.hierarchyChanged += OnHierarchyChanged;
    }

    private static void OnHierarchyChanged()
    {
        // Sprawdzamy czy nie jesteśmy w trybie Play
        if (EditorApplication.isPlayingOrWillChangePlaymode) return;

        GameObject tester = GameObject.Find("CombatTester");

        if (tester == null)
        {
            Debug.Log("[SceneSetupManager] CombatTester not found. Creating it automatically...");
            tester = new GameObject("CombatTester");
            tester.AddComponent<CombatRunner>();
            
            // Oznaczamy scenę jako brudną (do zapisu)
            EditorSceneManager.MarkSceneDirty(EditorSceneManager.GetActiveScene());
        }
        else if (tester.GetComponent<CombatRunner>() == null)
        {
            Debug.Log("[SceneSetupManager] CombatTester found but missing CombatRunner. Adding it...");
            tester.AddComponent<CombatRunner>();
            EditorSceneManager.MarkSceneDirty(EditorSceneManager.GetActiveScene());
        }
    }
}
