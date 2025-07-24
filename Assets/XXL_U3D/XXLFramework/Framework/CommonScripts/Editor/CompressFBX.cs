using UnityEngine;
using UnityEditor;
using System.IO;
using System.Collections.Generic;

public class CompressFBX : EditorWindow
{
	ModelImporterMeshCompression compression = ModelImporterMeshCompression.Off; //压缩等级
	bool isReadable = false; //设置模型是否可读


	[MenuItem("XXLFramework/CompressFBX")]
	static void Init()
	{
		Random.InitState(System.DateTime.Now.Millisecond);
		EditorWindow window = EditorWindow.GetWindow<CompressFBX>("压缩模型");
		window.position = new Rect(200, 150, 500, 200);
	}

	void OnGUI()
	{
		EditorGUILayout.Space();
		EditorGUILayout.HelpBox("设置选中图片或选中路径下的图片属性", MessageType.Info);
		EditorGUILayout.Space();

		GUILayout.BeginVertical();
		{
			isReadable = GUILayout.Toggle(isReadable, "isReadable");
			if (GUILayout.Button("设置读写"))
			{
				SetReadable(GetSelectedFbxs());
			}

			compression = (ModelImporterMeshCompression)EditorGUILayout.EnumPopup("压缩等级", compression);
			if (GUILayout.Button("压缩模型"))
			{
				Compress(GetSelectedFbxs());
			}
		}
		GUILayout.EndVertical();

	}



	static List<ModelImporter> GetSelectedFbxs()
	{
		//return Selection.GetFiltered(typeof(ModelImporter), SelectionMode.DeepAssets);
		Object[] objs = Selection.GetFiltered(typeof(object), SelectionMode.DeepAssets);
		List<ModelImporter> selectedFbxs = new List<ModelImporter>();
		foreach (var o in objs)
		{
			//非对象不继续
			if (!(o is GameObject))
				continue;
			GameObject mod = o as GameObject;
			//将mod模型路径存储在path中
			string path = AssetDatabase.GetAssetPath(mod);
			ModelImporter modelimporter = ModelImporter.GetAtPath(path) as ModelImporter;
			if (!modelimporter)
			{
				//Debug.LogError(string.Format("path-->{0}<---不是ModelImporter", path));
				continue;
			}
			else
			{
				selectedFbxs.Add(modelimporter);
			}
		}
		return selectedFbxs;
	}

	private void SetReadable(List<ModelImporter> fbxs)
	{
		if (fbxs != null)
		{
			if (fbxs.Count < 1)
			{
				ShowNotification(new GUIContent("找不到模型!"));
				return;
			}
		}
		else
		{
			ShowNotification(new GUIContent("请选中图片或路径!"));
			return;
		}

		int i = 0;
		foreach (var imp in fbxs)
		{
			imp.isReadable = isReadable;
			ShowProgress((float)i / (float)fbxs.Count, fbxs.Count, i);
			i++;
			imp.SaveAndReimport();
		}
		AssetDatabase.Refresh();
		EditorUtility.ClearProgressBar();
		fbxs = null;
		Debug.Log("设置可读完成");
	}

	void Compress(List<ModelImporter> fbxs)
	{
		if (fbxs != null)
		{
			if (fbxs.Count < 1)
			{
				ShowNotification(new GUIContent("找不到模型!"));
				return;
			}
		}
		else
		{
			ShowNotification(new GUIContent("请选中图片或路径!"));
			return;
		}

		int i = 0;
		foreach (var imp in fbxs)
		{
			imp.meshCompression = compression;
			ShowProgress((float)i / (float)fbxs.Count, fbxs.Count, i);
			i++;
			imp.SaveAndReimport();
		}
		AssetDatabase.Refresh();
		EditorUtility.ClearProgressBar();
		fbxs = null;
		Debug.Log("压缩模型完成");
	}

	public static void ShowProgress(float val, int total, int cur)
	{
		EditorUtility.DisplayProgressBar("设置模型中...", string.Format("请稍等({0}/{1}) ", cur, total), val);
	}
}
