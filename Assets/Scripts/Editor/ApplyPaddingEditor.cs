using UnityEngine;
using UnityEditor;

public class ApplyPaddingEditor
{
    [MenuItem("Tools/DessertClicker/Apply Padding")]
    public static void Apply()
    {
        GameObject scrollArea = GameObject.Find("ScrollArea");
        if (scrollArea != null)
        {
            RectTransform rt = scrollArea.GetComponent<RectTransform>();
            Undo.RecordObject(rt, "Apply Padding");
            rt.offsetMin = new Vector2(10, rt.offsetMin.y);
            rt.offsetMax = new Vector2(-10, rt.offsetMax.y);
            Debug.Log($"[MCP] ScrollArea Padding Applied: {rt.offsetMin} / {rt.offsetMax}");
            EditorUtility.SetDirty(rt);
        }
        else
        {
            Debug.LogWarning("[MCP] ScrollArea not found.");
        }
    }
}
