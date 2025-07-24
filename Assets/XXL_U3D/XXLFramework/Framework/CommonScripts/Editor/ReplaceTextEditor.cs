using UnityEngine;
using UnityEditor;
using TMPro;
using UnityEngine.UI;

public class ReplaceTextEditor : EditorWindow
{
    private string searchText = "";
    private string replaceText = "";

    [MenuItem("GameObject/ReplaceText", false, 0)]
    private static void ReplaceText()
    {
        var selectedObject = Selection.activeGameObject;
        if (selectedObject != null)
        {
            var window = GetWindow<ReplaceTextEditor>();
            window.titleContent = new GUIContent("Replace Text");
            window.Show();
        }
        else
        {
            Debug.LogWarning("Please select a GameObject first.");
        }
    }

    private void OnGUI()
    {
        EditorGUILayout.LabelField("Replace Text", EditorStyles.boldLabel);

        searchText = EditorGUILayout.TextField("Search Text", searchText);
        replaceText = EditorGUILayout.TextField("Replace Text", replaceText);

        if (GUILayout.Button("Replace"))
        {
            var selectedObject = Selection.activeGameObject;
            ReplaceTextInTextMeshProUGUI(selectedObject);
        }
    }

    private void ReplaceTextInTextMeshProUGUI(GameObject gameObject)
    {
        var textComponents = gameObject.GetComponentsInChildren<Text>(true);
        var tmpComponents = gameObject.GetComponentsInChildren<TextMeshProUGUI>(true);
        int count = 0;
        foreach (var textComponent in textComponents)
        {
            if (textComponent.text.Contains(searchText))
            {
                count++;
                textComponent.text = textComponent.text.Replace(searchText, replaceText);
            }
        }

        foreach (var tmpComponent in tmpComponents)
        {
            if (tmpComponent.text.Contains(searchText))
            {
                count++;
                tmpComponent.text = tmpComponent.text.Replace(searchText, replaceText);
            }
        }
        
        Debug.Log($"共替换{count}处,Text replaced successfully.");
    }
}
