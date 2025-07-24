using Sirenix.Utilities;
using UnityEditor;
using UnityEngine;

public class AddBoxCollider 
{
    [MenuItem("GameObject/Add BoxCollider", false, 0)]
    static void AddAdaptiveBoxCollider()
    {
        var selection = Selection.activeTransform;
        if (selection == null)
        {
            Debug.Log("请选择一个物体！");
            return;
        }
        AddAdaptiveBoxCollider(selection);
    }

    [MenuItem("GameObject/Add BoxColliders", false, 0)]
    static void AddAdaptiveBoxColliders()
    {
        var selection = Selection.transforms;
        if (selection == null)
        {
            Debug.Log("请选择一个物体！");
            return;
        }
		foreach (var item in selection)
		{
            AddAdaptiveBoxCollider(item);
        }
        
    }

    public static void AddAdaptiveBoxCollider(Transform selection)
    {
        if (selection.GetComponent<Collider>() != null)
        {
            Debug.Log("所选物体上已有碰撞体！");
            return;
        }

        var renderers = selection.GetComponentsInChildren<Renderer>();
        if (renderers.Length == 0)
        {
            Debug.Log("所选物体及其子物体上没有渲染器！无法计算碰撞体！");
            return;
        }

        // 记录位置、旋转、缩放
        Vector3 pos = selection.position;
        Vector3 angles = selection.eulerAngles;
        Vector3 scale = selection.lossyScale;

        selection.eulerAngles = Vector3.zero; // 重置旋转

        Bounds bound = renderers[0].bounds;
        renderers.ForEach(r => bound.Encapsulate(r.bounds));

        // 添加盒式碰撞体并处理位置、旋转、缩放
        var bc = selection.gameObject.AddComponent<BoxCollider>();
        bc.size = new Vector3(bound.size.x / scale.x, bound.size.y / scale.y, bound.size.z / scale.z);
        Vector3 tempCenter = bound.center - pos;
        bc.center = new Vector3(tempCenter.x / scale.x, tempCenter.y / scale.y, tempCenter.z / scale.z);

        selection.eulerAngles = angles; // 恢复旋转

        EditorUtility.SetDirty(selection);
        Debug.Log("添加碰撞体完成！");
    }

 
}

    

   


