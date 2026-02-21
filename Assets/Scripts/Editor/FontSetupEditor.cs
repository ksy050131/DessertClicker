using UnityEngine;
using UnityEditor;
using TMPro;
using TMPro.EditorUtilities;
using System.Linq;

public class FontSetupEditor : MonoBehaviour
{
    [MenuItem("Tools/Setup Galmuri11 Font")]
    public static void SetupFont()
    {
        // 1. Load the TTF source font
        Font sourceFont = AssetDatabase.LoadAssetAtPath<Font>("Assets/Fonts/Galmuri11.ttf");
        if (sourceFont == null)
        {
            Debug.LogError("Galmuri11.ttf not found at Assets/Fonts/Galmuri11.ttf");
            return;
        }

        // 2. Create a new Dynamic TMP FontAsset
        string savePath = "Assets/Fonts/Galmuri11 SDF Dynamic.asset";

        // Check if already exists
        TMP_FontAsset existingFont = AssetDatabase.LoadAssetAtPath<TMP_FontAsset>(savePath);
        if (existingFont != null)
        {
            // Already created, just assign to all texts
            AssignFontToAllTexts(existingFont);
            return;
        }

        // Create new Dynamic font asset
        TMP_FontAsset fontAsset = TMP_FontAsset.CreateFontAsset(
            sourceFont,    // source font
            42,            // sampling point size
            5,             // padding
            UnityEngine.TextCore.LowLevel.GlyphRenderMode.SDFAA, // render mode
            1024,          // atlas width
            1024           // atlas height
        );

        if (fontAsset == null)
        {
            Debug.LogError("Failed to create TMP_FontAsset from Galmuri11.ttf");
            return;
        }

        // Set to Dynamic mode so Korean characters are generated on-the-fly
        fontAsset.atlasPopulationMode = AtlasPopulationMode.Dynamic;
        fontAsset.name = "Galmuri11 SDF Dynamic";

        // Save as asset
        AssetDatabase.CreateAsset(fontAsset, savePath);

        // Save the material sub-asset
        if (fontAsset.material != null)
        {
            fontAsset.material.name = fontAsset.name + " Material";
            AssetDatabase.AddObjectToAsset(fontAsset.material, fontAsset);
        }

        // Save atlas texture sub-asset
        if (fontAsset.atlasTextures != null)
        {
            for (int i = 0; i < fontAsset.atlasTextures.Length; i++)
            {
                if (fontAsset.atlasTextures[i] != null)
                {
                    fontAsset.atlasTextures[i].name = fontAsset.name + " Atlas " + i;
                    AssetDatabase.AddObjectToAsset(fontAsset.atlasTextures[i], fontAsset);
                }
            }
        }

        AssetDatabase.SaveAssets();
        AssetDatabase.Refresh();

        Debug.Log($"Created Dynamic TMP Font Asset at: {savePath}");

        // 3. Assign to all TMP texts in scene
        AssignFontToAllTexts(fontAsset);
    }

    private static void AssignFontToAllTexts(TMP_FontAsset fontAsset)
    {
        var allTexts = Object.FindObjectsByType<TMP_Text>(FindObjectsSortMode.None);
        int count = 0;
        foreach (var text in allTexts)
        {
            text.font = fontAsset;
            EditorUtility.SetDirty(text);
            count++;
        }

        Debug.Log($"Galmuri11 SDF Dynamic font assigned to {count} TMP text elements.");
        
        // Mark scene dirty
        UnityEditor.SceneManagement.EditorSceneManager.MarkSceneDirty(
            UnityEditor.SceneManagement.EditorSceneManager.GetActiveScene()
        );
    }
}
