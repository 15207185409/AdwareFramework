/****************************************************************************
 * Copyright (c) 2015 ~ 2022  UNDER MIT LICENSE
 * 

 ****************************************************************************/

#if UNITY_EDITOR
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.SceneManagement;
using System;
using UnityEditorInternal;
using System.Text.RegularExpressions;

namespace XXLFramework
{
	[CustomEditor(typeof(BasePanel), true)]
	public class BasePanelInspector : Editor
	{
		private ReorderableList bindDetails;


		[MenuItem("GameObject/CodeGenKit/@(Alt+Q)Add BasePanel &q", false, 0)]
		public static void AddView()
		{
			var gameObject = Selection.objects.First() as GameObject;

			if (!gameObject)
			{
				Debug.LogWarning("需要选择 GameObject");
				return;
			}

			var view = gameObject.GetComponent<BasePanel>();

			if (!view)
			{
				gameObject.AddComponent<BasePanel>();
			}
		}



		[MenuItem("GameObject/CodeGenKit/@(Alt+C)Create Code &c", false, 1)]
		static void CreateCode()
		{
			var gameObject = Selection.objects.First() as GameObject;
			CodeGenKit.Generate(gameObject.GetComponent<BindGroup>());
		}

		[MenuItem("GameObject/CodeGenKit/修正绑定物体名称", false, 2)]
		static void RevieseName()
		{
			var bindGroup = (Selection.objects.First() as GameObject).GetComponent<BindGroup>();

			if (!bindGroup)
			{
				Debug.LogWarning("选中物体不是BindGroup");
				return;
			}
			List<BindDetail> bindDetails = bindGroup.BindDetails;
			for (int i = 0; i < bindDetails.Count; i++)
			{
				var item = bindDetails[i].BindObj;
				item.name = RemoveSpecialCharacter(item.name);
			}
			Debug.Log("修正名称完成");
		}

		[MenuItem("GameObject/CodeGenKit/规范绑定物体名字", false, 3)]
		static void NormalizationName()
		{
			var bindGroup = (Selection.objects.First() as GameObject).GetComponent<BindGroup>();

			if (!bindGroup)
			{
				Debug.LogWarning("选中物体不是BindGroup");
				return;
			}
			List<BindDetail> bindDetails = bindGroup.BindDetails;
			for (int i = 0; i < bindDetails.Count; i++)
			{
				var item = bindDetails[i].BindObj;
				string reviseName = RemoveSpecialCharacter(item.name);
				item.name = GetAppendByComponentName(item, bindDetails[i].ComponentName) + reviseName;
			}
			Debug.Log("规范名称完成");
		}

		public static string RemoveSpecialCharacter(String hexData)
		{
			return Regex.Replace(hexData, "[ \\[ \\] \\^ \\-*×――(^)$%~!@#$…&%￥—+=<>《》!！??？:：•`·、。，；,.;\"‘’“”-]", "");
		}

		/// <summary>
		/// 根据组件获取名称前缀
		/// </summary>
		/// <returns></returns>
		static string GetAppendByComponentName(GameObject bindObj, string componentName)
		{
			if ((componentName == "TMP.TextMeshProUGUI" || componentName == "TMPro.TextMeshProUGUI" ||
				componentName == "TMPro.TextMeshPro" || componentName == "UnityEngine.UI.Text") &&
				!bindObj.name.Contains("Txt"))
			{
				return "Txt_";
			}
			else if ((componentName == "UnityEngine.UI.InputField" || componentName == "TMPro.TMP_InputField")
				&& !bindObj.name.Contains("Input"))
			{
				return "Input_";
			}
			else if ((componentName == "UnityEngine.UI.Dropdown" || componentName == "TMPro.TMP_Dropdown")
				&& !bindObj.name.Contains("Drop"))
			{
				return "Drop_";
			}
			else if (componentName == "UnityEngine.UI.Button" && !bindObj.name.Contains("Btn"))
			{
				return "Btn_";
			}
			else if (componentName == "UnityEngine.UI.RawImage" && !bindObj.name.Contains("Img"))
			{
				return "RImg_";
			}
			else if (componentName == "UnityEngine.UI.Image" && !bindObj.name.Contains("Img"))
			{
				return "Img_";
			}
			else if (componentName == "UnityEngine.UI.Toggle" && !bindObj.name.Contains("Tog"))
			{
				return "Tog_";
			}
			else if (componentName == "UnityEngine.UI.Slider" && !bindObj.name.Contains("Sli"))
			{
				return "Sli_";
			}
			else if (componentName == "UnityEngine.UI.Scrollbar" && !bindObj.name.Contains("Scr"))
			{
				return "Scr_";
			}
			else if (componentName == "Animator" && !bindObj.name.Contains("Ani"))
			{
				return "Ani_";
			}
			else
			{
				return "";
			}
		}





		private BasePanelInspectorLocale mLocaleText = new BasePanelInspectorLocale();


		public BasePanel CurrentPanel => target as BasePanel;


		private void OnEnable()
		{
			if (CurrentPanel == null)
			{
				return;
			}
			RemoveDestroyedBindDetail();
			RefreshComponents(CurrentPanel);
			DrawReorderableList();
			if (string.IsNullOrEmpty(CurrentPanel.ScriptsFolder))
			{
				var setting = UIKitSetting.Load();
				CurrentPanel.ScriptsFolder = setting.UIScriptDir;
			}

			if (string.IsNullOrEmpty(CurrentPanel.PrefabFolder))
			{
				var setting = UIKitSetting.Load();
				CurrentPanel.PrefabFolder = setting.UIPrefabDir;
			}

			if (string.IsNullOrEmpty(CurrentPanel.ScriptName))
			{
				CurrentPanel.ScriptName = CurrentPanel.name;
			}

			if (string.IsNullOrEmpty(CurrentPanel.Namespace))
			{
				var setting = UIKitSetting.Load();
				CurrentPanel.Namespace = setting.Namespace;
			}
		}

		//刷新组件
		private void RefreshComponents(BindGroup panel)
		{
			foreach (var item in panel.BindDetails)
			{
				item.Refresh();
			}
		}

		private void DrawReorderableList()
		{
			SerializedProperty prop = serializedObject.FindProperty("BindDetails");

			bindDetails = new ReorderableList(serializedObject, prop, true, true, false, true);

			bindDetails.elementHeight = 20;
			//绘制元素
			bindDetails.drawElementCallback = (rect, index, isActive, focused) =>
			{
				SerializedProperty element = prop.GetArrayElementAtIndex(index);
				EditorGUI.PropertyField(rect, element);
			};

			//头部
			bindDetails.drawHeaderCallback = (rect) =>
				EditorGUI.LabelField(rect, "绑定物体列表");

			//当移除元素时回调
			bindDetails.onRemoveCallback = (ReorderableList list) =>
			{
				ReorderableList.defaultBehaviours.DoRemoveButton(list);
			};

		}



		private void RemoveDestroyedBindDetail()
		{
			for (int i = CurrentPanel.BindDetails.Count - 1; i >= 0; i--)
			{
				if (CurrentPanel.BindDetails[i].BindObj == null)
				{
					CurrentPanel.BindDetails.RemoveAt(i);
				}
			}
		}

		private readonly BasePanelInspectorStyle mStyle = new BasePanelInspectorStyle();

		public override void OnInspectorGUI()
		{
			base.OnInspectorGUI();
			
			GUILayout.BeginVertical("box");

			GUILayout.BeginHorizontal();
			GUILayout.Label(mLocaleText.CodegenPart, mStyle.BigTitleStyle.Value);

			LocaleKitEditor.DrawSwitchToggle(GUI.skin.label.normal.textColor);
			GUILayout.EndHorizontal();

			CreateDragBindArea();

			GUILayout.BeginHorizontal();
			GUILayout.Label(mLocaleText.BindInfo, GUILayout.Width(150));
			GUILayout.FlexibleSpace();
			if (GUILayout.Button("清空"))
			{
				CurrentPanel.BindDetails.Clear();
			}
			GUILayout.EndHorizontal();
			ShowBindInfo();

			GUILayout.BeginHorizontal();
			GUILayout.Label(mLocaleText.Namespace, GUILayout.Width(150));
			CurrentPanel.Namespace = EditorGUILayout.TextArea(CurrentPanel.Namespace);
			GUILayout.EndHorizontal();

			GUILayout.BeginHorizontal();
			GUILayout.Label(mLocaleText.ScriptName, GUILayout.Width(150));
			CurrentPanel.ScriptName = EditorGUILayout.TextArea(CurrentPanel.ScriptName);
			GUILayout.EndHorizontal();

			GUILayout.BeginHorizontal();
			GUILayout.Label(mLocaleText.ScriptsFolder, GUILayout.Width(150));

			string path = FindScriptPathByClassName();
			if (path.IsNotNullAndEmpty())
			{
				CurrentPanel.ScriptsFolder = path;
				//Debug.Log("脚本路径：" + path);
				EditorGUILayout.LabelField(CurrentPanel.ScriptsFolder, GUILayout.Height(30));
				
			}
			else
			{
				//Debug.Log("资源不存在");
				CurrentPanel.ScriptsFolder =
				EditorGUILayout.TextField(CurrentPanel.ScriptsFolder, GUILayout.Height(30));
			}
			

			GUILayout.EndHorizontal();


			EditorGUILayout.Space();
			EditorGUILayout.LabelField(mLocaleText.DragDescription);
			var sfxPathRect = EditorGUILayout.GetControlRect();
			sfxPathRect.height = 50;
			GUI.Box(sfxPathRect, string.Empty);
			EditorGUILayout.LabelField(string.Empty, GUILayout.Height(20));
			if (
				Event.current.type == EventType.DragUpdated
				&& sfxPathRect.Contains(Event.current.mousePosition)
			)
			{
				//改变鼠标的外表  
				DragAndDrop.visualMode = DragAndDropVisualMode.Generic;
				if (DragAndDrop.paths != null && DragAndDrop.paths.Length > 0)
				{
					if (DragAndDrop.paths[0] != "")
					{
						var newPath = DragAndDrop.paths[0];
						CurrentPanel.ScriptsFolder = newPath;
						AssetDatabase.SaveAssets();
						AssetDatabase.Refresh();
						EditorSceneManager.MarkSceneDirty(SceneManager.GetActiveScene());
					}
				}
			}

			GUILayout.BeginHorizontal();
			CurrentPanel.GeneratePrefab =
				GUILayout.Toggle(CurrentPanel.GeneratePrefab, mLocaleText.GeneratePrefab);
			GUILayout.EndHorizontal();

			if (CurrentPanel.GeneratePrefab)
			{
				GUILayout.BeginHorizontal();
				GUILayout.Label(mLocaleText.PrefabGenerateFolder, GUILayout.Width(150));
				CurrentPanel.PrefabFolder =
					GUILayout.TextArea(CurrentPanel.PrefabFolder, GUILayout.Height(30));
				GUILayout.EndHorizontal();

				EditorGUILayout.Space();
				EditorGUILayout.LabelField(mLocaleText.DragDescription);

				var dragRect = EditorGUILayout.GetControlRect();
				dragRect.height = 50;
				GUI.Box(dragRect, string.Empty);
				EditorGUILayout.LabelField(string.Empty, GUILayout.Height(20));
				if (
					Event.current.type == EventType.DragUpdated
					&& dragRect.Contains(Event.current.mousePosition)
				)
				{
					//改变鼠标的外表  
					DragAndDrop.visualMode = DragAndDropVisualMode.Generic;
					if (DragAndDrop.paths != null && DragAndDrop.paths.Length > 0)
					{
						if (DragAndDrop.paths[0] != "")
						{
							var newPath = DragAndDrop.paths[0];
							CurrentPanel.PrefabFolder = newPath;
							AssetDatabase.SaveAssets();
							AssetDatabase.Refresh();
							EditorSceneManager.MarkSceneDirty(SceneManager.GetActiveScene());
						}
					}
				}
			}

			var fileFullPath = CurrentPanel.ScriptsFolder + "/" + CurrentPanel.ScriptName + ".cs";
			if (File.Exists(fileFullPath))
			{
				var scriptObject = AssetDatabase.LoadAssetAtPath<MonoScript>(fileFullPath);
				if (GUILayout.Button(mLocaleText.OpenScript, GUILayout.Height(30)))
				{
					AssetDatabase.OpenAsset(scriptObject);
				}

				if (GUILayout.Button(mLocaleText.SelectScript, GUILayout.Height(30)))
				{
					Selection.activeObject = scriptObject;
				}
			}

			if (GUILayout.Button(mLocaleText.Generate, GUILayout.Height(30)))
			{
				CodeGenKit.Generate(CurrentPanel);
				GUIUtility.ExitGUI();
			}

			GUILayout.EndVertical();
		}

		/// <summary>
		/// 根据类名查找脚本文件的相对路径
		/// </summary>
		/// <returns>相对于Assets目录的脚本文件路径</returns>
		private  string FindScriptPathByClassName()
		{
			string className = CurrentPanel.GetType().ToString();
			className = className.Substring(className.LastIndexOf('.') + 1);

			if (className == "BasePanel")
			{
				return string.Empty;
			}
			
			string[] guids = AssetDatabase.FindAssets($"t:MonoScript {className}");
			if (guids.Length == 0)
			{
				Debug.LogWarning($"No script found with name: {className}");
				return string.Empty;
			}
			
			foreach (string guid in guids)
			{
				string path = AssetDatabase.GUIDToAssetPath(guid);
				
				// 加载脚本
				MonoScript script = AssetDatabase.LoadAssetAtPath<MonoScript>(path);
				if (script != null && script.GetClass()?.Name == className)
				{
					//Debug.Log($"找到脚本: {path}", script);
					string result = Path.GetDirectoryName(path);
					return result; 
				}
			}
			//Debug.Log($"未找到{className}");
			return string.Empty;
		}
		


		//生成绑定拖拽区域
		private void CreateDragBindArea()
		{
			GUILayout.Label(mLocaleText.DragBindDescription);

			var sfxPathRect = EditorGUILayout.GetControlRect(false,50);
			GUI.Box(sfxPathRect, string.Empty);

			if (Event.current.type == EventType.DragUpdated && sfxPathRect.Contains(Event.current.mousePosition))
			{
				//改变鼠标的外表  
				DragAndDrop.visualMode = DragAndDropVisualMode.Generic;
			}

			if (Event.current.type == EventType.DragExited && sfxPathRect.Contains(Event.current.mousePosition))
			{
				if (DragAndDrop.objectReferences != null && DragAndDrop.objectReferences.Length > 0)
				{
					foreach (var item in DragAndDrop.objectReferences)
					{
						if (item is GameObject && (item as GameObject).GetComponent<Component>() != null)
						{
							GameObject bindObj = item as GameObject;
							CurrentPanel.AddBindObj(new BindDetail(bindObj));
						}
					}
				}
			}
		}

		private void ShowBindInfo()
		{
			serializedObject.Update();
			bindDetails.DoLayoutList();
			serializedObject.ApplyModifiedProperties();
		}

	}
}
#endif