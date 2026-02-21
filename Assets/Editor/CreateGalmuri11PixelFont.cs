using UnityEngine;
using UnityEditor;
using TMPro;
using UnityEngine.TextCore.LowLevel;

public static class CreateGalmuri11PixelFont
{
    [MenuItem("Tools/Create Galmuri11 Pixel Font")]
    public static void Execute()
    {
        // Load source font
        Font sourceFont = AssetDatabase.LoadAssetAtPath<Font>("Assets/Fonts/Galmuri11.ttf");
        if (sourceFont == null)
        {
            Debug.LogError("Cannot find Assets/Fonts/Galmuri11.ttf");
            return;
        }

        string savePath = "Assets/Fonts/Galmuri11_Pixel.asset";

        // Delete existing if present
        var existing = AssetDatabase.LoadAssetAtPath<TMP_FontAsset>(savePath);
        if (existing != null)
        {
            AssetDatabase.DeleteAsset(savePath);
        }

        // Create font asset: Raster Hinted, 11pt (Galmuri11 native size), Dynamic for Korean
        TMP_FontAsset fontAsset = TMP_FontAsset.CreateFontAsset(
            sourceFont,
            11,                              // samplingPointSize = native Galmuri11 size
            1,                               // atlasPadding
            GlyphRenderMode.RASTER_HINTED,   // crisp pixels, no anti-aliasing
            1024,                            // atlas width
            1024,                            // atlas height
            AtlasPopulationMode.Dynamic,     // dynamic Korean support
            true                             // multi-atlas support
        );

        if (fontAsset == null)
        {
            Debug.LogError("Failed to create TMP_FontAsset");
            return;
        }

        // Atlas texture: Point filter for crisp pixels
        if (fontAsset.atlasTexture != null)
        {
            fontAsset.atlasTexture.filterMode = FilterMode.Point;
            fontAsset.atlasTexture.name = "Galmuri11_Pixel Atlas";
        }

        // Material: zero smoothness/sharpness
        if (fontAsset.material != null)
        {
            fontAsset.material.name = "Galmuri11_Pixel Material";
            if (fontAsset.material.HasProperty("_Sharpness"))
                fontAsset.material.SetFloat("_Sharpness", 0f);
            if (fontAsset.material.HasProperty("_Smoothness"))
                fontAsset.material.SetFloat("_Smoothness", 0f);
            if (fontAsset.material.HasProperty("_TextureWidth"))
                fontAsset.material.SetFloat("_TextureWidth", 1024f);
            if (fontAsset.material.HasProperty("_TextureHeight"))
                fontAsset.material.SetFloat("_TextureHeight", 1024f);
        }

        // Save as asset with sub-objects
        AssetDatabase.CreateAsset(fontAsset, savePath);

        if (fontAsset.atlasTexture != null)
            AssetDatabase.AddObjectToAsset(fontAsset.atlasTexture, fontAsset);

        if (fontAsset.material != null)
            AssetDatabase.AddObjectToAsset(fontAsset.material, fontAsset);

        EditorUtility.SetDirty(fontAsset);
        AssetDatabase.SaveAssets();
        AssetDatabase.Refresh();

        Debug.Log("[Galmuri11 Pixel] Font asset created at: " + savePath);
        Debug.Log("[Galmuri11 Pixel] Render Mode: RASTER_HINTED, Atlas: Dynamic, Filter: Point, Size: 11pt");
    }
}
