using UnityEngine;
using UnityEditor;
using TMPro;
using UnityEngine.UI;
using UnityEditor.SceneManagement;

public class SlotLayoutApplyEditor
{
    [MenuItem("Tools/DessertClicker/Apply Slot Layout")]
    public static void ApplySlotLayout()
    {
        int changedCount = 0;
        
        // Find all Image components in the scene
        Image[] images = GameObject.FindObjectsOfType<Image>(true);
        foreach (var img in images)
        {
            if (img.gameObject.name.StartsWith("UpgradeSlot_") || img.gameObject.name.StartsWith("DecoSlot_")) 
            {
                Transform slotT = img.transform;

                // 1. Get or Create UpgradeDesc
                Transform descT = slotT.Find("UpgradeDesc");
                TMP_Text descText = null;
                
                TMP_Text nameText = slotT.Find("UpgradeName")?.GetComponent<TMP_Text>();
                
                if (descT == null && nameText != null)
                {
                    // Create description text by duplicating name text to keep font settings
                    GameObject descObj = GameObject.Instantiate(nameText.gameObject, slotT);
                    descObj.name = "UpgradeDesc";
                    descT = descObj.transform;
                    descText = descObj.GetComponent<TMP_Text>();
                    Undo.RegisterCreatedObjectUndo(descObj, "Create UpgradeDesc");
                }
                else if (descT != null)
                {
                    descText = descT.GetComponent<TMP_Text>();
                }

                // 2. Apply RectTransform Anchors & Offsets for all elements
                
                // --- Title (UpgradeName) ---
                Transform titleT = slotT.Find("UpgradeName");
                if (titleT != null)
                {
                    Undo.RecordObject(titleT, "Layout Title");
                    RectTransform rt = titleT.GetComponent<RectTransform>();
                    rt.anchorMin = new Vector2(0.05f, 0.65f);
                    rt.anchorMax = new Vector2(0.60f, 0.95f);
                    rt.offsetMin = Vector2.zero;
                    rt.offsetMax = Vector2.zero;
                    
                    TMP_Text tmpro = titleT.GetComponent<TMP_Text>();
                    if (tmpro != null) {
                        Undo.RecordObject(tmpro, "Title Align");
                        tmpro.alignment = TextAlignmentOptions.TopLeft;
                        tmpro.fontSize = 28;
                        EditorUtility.SetDirty(tmpro);
                    }
                    EditorUtility.SetDirty(rt);
                }

                // --- Description (UpgradeDesc) ---
                if (descT != null)
                {
                    Undo.RecordObject(descT, "Layout Desc");
                    RectTransform rt = descT.GetComponent<RectTransform>();
                    rt.anchorMin = new Vector2(0.05f, 0.35f);
                    rt.anchorMax = new Vector2(0.60f, 0.60f);
                    rt.offsetMin = Vector2.zero;
                    rt.offsetMax = Vector2.zero;

                    if (descText != null) {
                         Undo.RecordObject(descText, "Desc Align");
                         descText.alignment = TextAlignmentOptions.MidlineLeft;
                         descText.fontSize = 20; // Slightly smaller
                         descText.text = "Description Here"; // Default preview
                         EditorUtility.SetDirty(descText);
                    }
                    EditorUtility.SetDirty(rt);
                }

                // --- Cost (UpgradeCost) ---
                Transform costT = slotT.Find("UpgradeCost");
                if (costT != null)
                {
                    Undo.RecordObject(costT, "Layout Cost");
                    RectTransform rt = costT.GetComponent<RectTransform>();
                    rt.anchorMin = new Vector2(0.05f, 0.05f);
                    rt.anchorMax = new Vector2(0.60f, 0.30f);
                    rt.offsetMin = Vector2.zero;
                    rt.offsetMax = Vector2.zero;

                    TMP_Text tmpro = costT.GetComponent<TMP_Text>();
                    if (tmpro != null) {
                         Undo.RecordObject(tmpro, "Cost Align");
                         tmpro.alignment = TextAlignmentOptions.BottomLeft;
                         tmpro.fontSize = 24;
                         EditorUtility.SetDirty(tmpro);
                    }
                    EditorUtility.SetDirty(rt);
                }

                // --- Buy Button (BuyButton_X) ---
                // Find button by starting with name since it has index
                Transform buyT = null;
                foreach (Transform child in slotT)
                {
                    if (child.name.StartsWith("BuyButton_"))
                    {
                        buyT = child;
                        break;
                    }
                }

                if (buyT != null)
                {
                    Undo.RecordObject(buyT, "Layout Buy Button");
                    RectTransform rt = buyT.GetComponent<RectTransform>();
                    rt.anchorMin = new Vector2(0.65f, 0.10f);
                    rt.anchorMax = new Vector2(0.95f, 0.90f); // Make button slightly taller
                    rt.offsetMin = Vector2.zero;
                    rt.offsetMax = Vector2.zero;
                    
                    // Center the text inside the buy button
                    Transform btnTextT = buyT.Find("Text (TMP)");
                    if (btnTextT != null)
                    {
                        Undo.RecordObject(btnTextT, "Buy Button Text Align");
                        RectTransform textRt = btnTextT.GetComponent<RectTransform>();
                        textRt.anchorMin = Vector2.zero;
                        textRt.anchorMax = Vector2.one;
                        textRt.offsetMin = Vector2.zero;
                        textRt.offsetMax = Vector2.zero;
                        
                        TMP_Text btnText = btnTextT.GetComponent<TMP_Text>();
                        if (btnText != null)
                        {
                            btnText.alignment = TextAlignmentOptions.Center;
                            btnText.fontSize = 28;
                            EditorUtility.SetDirty(btnText);
                        }
                        EditorUtility.SetDirty(textRt);
                    }

                    EditorUtility.SetDirty(rt);
                }

                changedCount++;
                Debug.Log($"[MCP] Applied Layout to {slotT.name}");
            }
        }

        if (changedCount > 0)
        {
            EditorSceneManager.SaveOpenScenes();
            Debug.Log($"[MCP] Successfully applied layout to {changedCount} slots and SAVED the scene.");
        }
        else
        {
            Debug.LogWarning("[MCP] No slots found to layout.");
        }
    }
}
