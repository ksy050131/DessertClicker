using UnityEngine;
using UnityEditor;
using UnityEngine.UI;

public class FixSlotUIEditor : EditorWindow
{
    [MenuItem("Tools/DessertClicker/Fix Slot UIs")]
    public static void FixSlots()
    {
        var scrollArea = GameObject.Find("ScrollArea")?.GetComponent<RectTransform>();
        if (scrollArea != null)
        {
            Undo.RecordObject(scrollArea, "Fix ScrollArea padding");
            scrollArea.offsetMin = new Vector2(10f, scrollArea.offsetMin.y);
            scrollArea.offsetMax = new Vector2(-10f, scrollArea.offsetMax.y);
        }

        var contents = new[] { GameObject.Find("UpgradeContent")?.GetComponent<RectTransform>(), GameObject.Find("DecoContent")?.GetComponent<RectTransform>() };
        int count = 0;
        
        foreach (var content in contents)
        {
            if (content == null) continue;
            foreach (RectTransform slot in content)
            {
                if (slot.name.StartsWith("UpgradeSlot") || slot.name.StartsWith("DecoSlot"))
                {
                    Undo.RecordObject(slot, "Fix Slot Dimensions");
                    
                    var fitter = slot.gameObject.GetComponent<AspectRatioFitter>();
                    if (fitter == null)
                    {
                        fitter = Undo.AddComponent<AspectRatioFitter>(slot.gameObject);
                    }
                    else
                    {
                        Undo.RecordObject(fitter, "Fix Fitter");
                    }
                    fitter.aspectMode = AspectRatioFitter.AspectMode.WidthControlsHeight;
                    fitter.aspectRatio = 490f / 127.6799f;
                    
                    var img = slot.GetComponent<Image>();
                    if (img != null)
                    {
                        Undo.RecordObject(img, "Fix Slot Image");
                        img.type = Image.Type.Simple;
                        img.preserveAspect = false;
                    }
                    
                    var title = slot.Find("UpgradeName") as RectTransform;
                    if (title != null)
                    {
                        Undo.RecordObject(title, "Fix Title");
                        title.anchorMin = new Vector2(0f, 0.5f);
                        title.anchorMax = new Vector2(0.7f, 1f);
                        title.offsetMin = new Vector2(10f, 0f);
                        title.offsetMax = new Vector2(0f, -10f);
                    }
                    
                    var cost = slot.Find("UpgradeCost") as RectTransform;
                    if (cost != null)
                    {
                        Undo.RecordObject(cost, "Fix Cost");
                        cost.anchorMin = new Vector2(0f, 0f);
                        cost.anchorMax = new Vector2(0.7f, 0.5f);
                        cost.offsetMin = new Vector2(10f, 10f);
                        cost.offsetMax = new Vector2(0f, 0f);
                    }
                    
                    Button buyBtnComp = slot.GetComponentInChildren<Button>();
                    if (buyBtnComp != null)
                    {
                        RectTransform buyBtnRect = buyBtnComp.GetComponent<RectTransform>();
                        Undo.RecordObject(buyBtnRect, "Fix Button");
                        buyBtnRect.anchorMin = new Vector2(0.7f, 0f);
                        buyBtnRect.anchorMax = new Vector2(1f, 1f);
                        buyBtnRect.offsetMin = new Vector2(10f, 10f);
                        buyBtnRect.offsetMax = new Vector2(-10f, -10f);
                    }
                    
                    EditorUtility.SetDirty(slot.gameObject);
                    count++;
                }
            }
        }
        Debug.Log($"AspectRatioFitter applied. ScrollArea padded 10px from edges. Count: {count}");
    }
}
