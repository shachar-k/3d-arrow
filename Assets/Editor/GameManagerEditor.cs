using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
#endif

#if UNITY_EDITOR
[CustomEditor(typeof(GameManager))]
public class GameManagerEditor : Editor
{
    private Editor cachedSOEditor;

    public override void OnInspectorGUI()
    {
        // Draw the default component fields (like the targetAsset slot itself)
        DrawDefaultInspector();

        GameManager gameManager = (GameManager)target;

        if (gameManager.GameSettings != null)
        {
            EditorGUILayout.Space(10); // adds space in editor
            EditorGUILayout.LabelField("Game Settings Data", EditorStyles.boldLabel);

            // Create or reuse a cached editor for the ScriptableObject
            if (cachedSOEditor == null)
            {
                cachedSOEditor = CreateEditor(gameManager.GameSettings);
            }

            // Draw the ScriptableObject's inspector inline
            cachedSOEditor.OnInspectorGUI();
        }
    }
}
#endif