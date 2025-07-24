using UnityEditor;
using UnityEngine;

namespace XXLFramework
{
    public class RemoveAllCollider : MonoBehaviour
    {
        [MenuItem("GameObject/Remove Colliders", false, 0)]
        static void RemoveColliders()
        {
            GameObject[] selectedObjects = Selection.gameObjects;
            foreach (GameObject selectedObject in selectedObjects)
            {
                RemoveColliderFromObject(selectedObject);
            }
        }

        static void RemoveColliderFromObject(GameObject gameObject)
        {
            Collider[] colliders = gameObject.GetComponentsInChildren<Collider>();
            if (colliders != null)
            {
                foreach (var item in colliders)
                {
                    DestroyImmediate(item);
                }
            }
            foreach (Transform child in gameObject.transform)
            {
                RemoveColliderFromObject(child.gameObject);
            }
            Debug.Log("移除Collider完成");
        }
    }
}

