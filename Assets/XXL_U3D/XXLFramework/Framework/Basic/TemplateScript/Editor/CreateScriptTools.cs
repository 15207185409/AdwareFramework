using UnityEngine;
using UnityEditor;
using System.IO;
using UnityEditor.ProjectWindowCallback;
using System.Text.RegularExpressions;
using System.Text;
public class CreateScriptTools
{
    private const string BasicScriptsFolder = "Assets/XXL_U3D/XXLFramework/Framework/Basic/TemplateScript/ScriptsTemplates/";
    private const string CommandScriptDefalt = "NewCommand.cs.txt";
    private const string SystemScriptDefalt = "NewSystem.cs.txt";
    private const string ModelScriptDefalt = "NewModel.cs.txt";
    private const string InterfaceScriptDefalt = "NewInterface.cs.txt";
    private const string UIPanelScriptDefalt = "NewUIPanel.cs.txt";
    private const string MonoScriptDefalt = "NewMonoScript.cs.txt";
    private const string ArchitectureDefalt = "NewArchitecture.cs.txt";

    [MenuItem("Assets/Create/C# BasicScript/NewScript", false, 100)]
    public static void CreateScript()
    {
        string localPath = GetSelectedPathOrFallBack();
        ProjectWindowUtil.StartNameEditingIfProjectWindowExists(0, ScriptableObject.CreateInstance<MyDoCreateScriptAsset>(), localPath + "/NewMonoScript.cs", null, BasicScriptsFolder + MonoScriptDefalt);
    }

    [MenuItem("Assets/Create/C# BasicScript/NewCommand", false, 100)]
    public static void CreateCommand()
    {
        string localPath = GetSelectedPathOrFallBack();
        ProjectWindowUtil.StartNameEditingIfProjectWindowExists(0, ScriptableObject.CreateInstance<MyDoCreateScriptAsset>(), localPath + "/NewCommand.cs", null, BasicScriptsFolder+CommandScriptDefalt);
    }

    [MenuItem("Assets/Create/C# BasicScript/NewSystem", false, 100)]
    public static void CreateSystem()
    {
        string localPath = GetSelectedPathOrFallBack();
        ProjectWindowUtil.StartNameEditingIfProjectWindowExists(0, ScriptableObject.CreateInstance<MyDoCreateScriptAsset>(), localPath + "/NewSystem.cs", null, BasicScriptsFolder + SystemScriptDefalt);
    }

    [MenuItem("Assets/Create/C# BasicScript/NewModel", false, 100)]
    public static void CreateModel()
    {
        string localPath = GetSelectedPathOrFallBack();
        ProjectWindowUtil.StartNameEditingIfProjectWindowExists(0, ScriptableObject.CreateInstance<MyDoCreateScriptAsset>(), localPath + "/NewModel.cs", null, BasicScriptsFolder + ModelScriptDefalt);
    }

    [MenuItem("Assets/Create/C# BasicScript/NewArchitecture", false, 100)]
    public static void CreateArchitecture()
    {
        string localPath = GetSelectedPathOrFallBack();
        ProjectWindowUtil.StartNameEditingIfProjectWindowExists(0, ScriptableObject.CreateInstance<MyDoCreateScriptAsset>(), localPath + "/NewArchitecture.cs", null, BasicScriptsFolder + ArchitectureDefalt);
    }


    [MenuItem("Assets/Create/C# BasicScript/NewInterface", false, 100)]
    public static void CreateInterface()
    {
        string localPath = GetSelectedPathOrFallBack();
        ProjectWindowUtil.StartNameEditingIfProjectWindowExists(0, ScriptableObject.CreateInstance<MyDoCreateScriptAsset>(), localPath + "/NewInterface.cs", null, BasicScriptsFolder + InterfaceScriptDefalt);
    }

    [MenuItem("Assets/Create/C# BasicScript/NewUIPanel", false, 100)]
    public static void CreateUIPanel()
    {
        string localPath = GetSelectedPathOrFallBack();
        ProjectWindowUtil.StartNameEditingIfProjectWindowExists(0, ScriptableObject.CreateInstance<MyDoCreateScriptAsset>(), localPath + "/NewUIPanel.cs", null, BasicScriptsFolder + UIPanelScriptDefalt);
    }

    public static string GetSelectedPathOrFallBack()
    {
        string path = "Assets";
        foreach (UnityEngine.Object obj in Selection.GetFiltered(typeof(UnityEngine.Object), SelectionMode.Assets))
        {
            path = AssetDatabase.GetAssetPath(obj);
            if (!string.IsNullOrEmpty(path) && File.Exists(path))
            {
                path = Path.GetDirectoryName(path);
                break;
            }
        }
        return path;
    }
}
class MyDoCreateScriptAsset : EndNameEditAction
{
    public override void Action(int instanceId, string pathName, string resourceFile)
    {
        Debug.Log("EndNameAction");
        UnityEngine.Object o = CreateScriptsFromTemplate(pathName, resourceFile);
        ProjectWindowUtil.ShowCreatedAsset(o);
    }
    internal static UnityEngine.Object CreateScriptsFromTemplate(string pathName, string resourceFolder)
    {
        string fullPath = Path.GetFullPath(pathName);
        StreamReader streamReader = new StreamReader(resourceFolder);
        string text = streamReader.ReadToEnd();
        streamReader.Close();
        string fileNameWithOutExtension = Path.GetFileNameWithoutExtension(pathName);
        text = Regex.Replace(text, "#SCRIPTNAME#", fileNameWithOutExtension);
        //bool encoderShouldEmitUTF8Identifer = false;
        //bool throwOnInvalidBytes = false;
        //UTF8Encoding encoding = new UTF8Encoding(encoderShouldEmitUTF8Identifer, throwOnInvalidBytes);
        bool append = false;
        StreamWriter streamWriter = new StreamWriter(fullPath, append, new UTF8Encoding(false));
        streamWriter.Write(text);
        streamWriter.Close();
        AssetDatabase.ImportAsset(pathName);
        return AssetDatabase.LoadAssetAtPath(pathName, typeof(UnityEngine.Object));
    }


}