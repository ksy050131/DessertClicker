using UnityEngine;
using UnityEditor;
using UnityEngine.UI;
using UnityEditor.SceneManagement;

public class NativeAspectFixer
{
    [MenuItem("Tools/DessertClicker/Fix Native Aspect")]
    public static void ApplyNativeAspect()
    {
        int changedCount = 0;
        
        // Find all Image components in the scene
        Image[] images = GameObject.FindObjectsOfType<Image>(true);
        foreach (var img in images)
        {
            if (img.gameObject.name.StartsWith("UpgradeSlot_") || img.gameObject.name.StartsWith("DecoSlot_") || (img.sprite != null && img.sprite.name.StartsWith("Slot"))) 
            {
                Undo.RecordObject(img, "Change Image Type");
                img.type = Image.Type.Simple;

                AspectRatioFitter fitter = img.GetComponent<AspectRatioFitter>();
                if (fitter == null)
                {
                    fitter = Undo.AddComponent<AspectRatioFitter>(img.gameObject);
                }
                
                Undo.RecordObject(fitter, "Change Aspect Ratio");
                fitter.aspectMode = AspectRatioFitter.AspectMode.WidthControlsHeight;
                
                // Calculate correct aspect ratio from the sprite native rect
                if (img.sprite != null)
                {
                    float nativeRatio = img.sprite.rect.width / img.sprite.rect.height;
                    fitter.aspectRatio = nativeRatio;
                }
                else
                {
                    fitter.aspectRatio = 2.0f; // Fallback for 64x32
                }

                EditorUtility.SetDirty(img.gameObject);
                changedCount++;
                Debug.Log($"[MCP] Fixed {img.gameObject.name} to Native Aspect Ratio: {fitter.aspectRatio}. Image type set to Simple.");
            }
        }

        if (changedCount > 0)
        {
            EditorSceneManager.SaveOpenScenes();
            Debug.Log($"[MCP] Successfully updated {changedCount} slots and SAVED the scene.");
        }
        else
        {
            Debug.LogWarning("[MCP] No slots found to fix.");
        }
    }
}
