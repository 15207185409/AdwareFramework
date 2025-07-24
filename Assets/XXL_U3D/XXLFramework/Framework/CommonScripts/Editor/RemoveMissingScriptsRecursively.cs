using UnityEngine;
using UnityEditor;

namespace XXLFramework
{
    public class RemoveMissingScriptsRecursively : EditorWindow
    {
        [MenuItem("GameObject/XXLFramework/RemoveMissingScriptsRecursively")]
        public static void ShowWindow()
        {
            RemoveInSelected();
        }

        private static void RemoveInSelected()
        {
            GameObject[] go = Selection.gameObjects;
            foreach (GameObject g in go)
            {
                RemoveRecursively(g);
            }
            Debug.Log("移除丢失脚本完成");
        }

        private static void RemoveRecursively(GameObject g)
        {
            GameObjectUtility.RemoveMonoBehavioursWithMissingScript(g);

            foreach (Transform childT in g.transform)
            {
                RemoveRecursively(childT.gameObject);
            }
        }
    }
}