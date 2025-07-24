/****************************************************************************
 * Copyright (c) 2015 ~ 2022  UNDER MIT LICENSE
 * 

 ****************************************************************************/

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using UnityEditor;
using UnityEditor.Callbacks;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace XXLFramework
{
	public class CodeGenKitPipeline : ScriptableObject
	{
		private static CodeGenKitPipeline mInstance;

		public static CodeGenKitPipeline Default
		{
			get
			{
				if (mInstance) return mInstance;

				var filePath = Dir.Value + FileName;

				if (File.Exists(filePath))
				{
					return mInstance = AssetDatabase.LoadAssetAtPath<CodeGenKitPipeline>(filePath);
				}

				return mInstance = CreateInstance<CodeGenKitPipeline>();
			}
		}

		public void Save()
		{
			var filePath = Dir.Value + FileName;

			if (!File.Exists(filePath))
			{
				AssetDatabase.CreateAsset(this, Dir.Value + FileName);
			}

			EditorUtility.SetDirty(this);
			AssetDatabase.SaveAssets();
			AssetDatabase.Refresh();
		}

		private static readonly Lazy<string> Dir =
			new Lazy<string>(() => "Assets/XXL_U3D/XXLFramework/Framework/FrameworkData/CodeGenKit/".CreateDirIfNotExists());

		private const string FileName = "Pipeline.asset";

		[SerializeField] public CodeGenTask CurrentTask;

		public void Generate(CodeGenTask task)
		{
			CurrentTask = task;

			CurrentTask.Status = CodeGenTaskStatus.Search;
			if (task.IsPanel)
			{
				BasePanel panel = task.GameObject.GetComponent<BasePanel>();
				Debug.Log("生成panel名字:"+panel.name);
				BindCodeInfo panelCodeInfo = CreatePanelCodeInfo(panel.transform, panel.BindDetails);
				CreateUIPanelCode(panel, panelCodeInfo);
				CurrentTask.BindInfos = panelCodeInfo.BindInfos;
				CurrentTask.BindDetails = panel.BindDetails;
			}
			else
			{
				ViewController viewController = task.GameObject.GetComponent<ViewController>();
				Debug.Log("生成viewController名字:"+viewController.name);
				BindCodeInfo panelCodeInfo = CreatePanelCodeInfo(viewController.transform, viewController.BindDetails);
				CreateViewControllerCode(viewController, panelCodeInfo);
				CurrentTask.BindInfos = panelCodeInfo.BindInfos;
				CurrentTask.BindDetails = viewController.BindDetails;
			}

			Save();

			CurrentTask.Status = CodeGenTaskStatus.Compile;
		}

		private void CreateViewControllerCode(ViewController viewController, BindCodeInfo panelCodeInfo)
		{
			if (null == viewController)
				return;

			var dir = viewController.ScriptsFolder;
			var generateFilePath = $"{dir}/{viewController.ScriptName}.cs";

			if (!File.Exists(generateFilePath))
			{
				generateFilePath.GetFolderPath().CreateDirIfNotExists();
				ViewConTemplate.Write(viewController.ScriptName, generateFilePath, viewController.Namespace, CodeGenKitSetting.Load());
				Debug.Log(">>>>>>>Success Create Prefab Code: " + viewController.ScriptName);
			}
			else
			{
				Debug.Log($"{viewController.ScriptName}已存在");
			}
			CreateViewControllerDesignerCode(viewController, panelCodeInfo);
		}

		private void CreateViewControllerDesignerCode(ViewController viewController, BindCodeInfo panelCodeInfo)
		{
			if (null == viewController)
				return;

			var dir = viewController.ScriptsFolder;

			if (!File.Exists(dir))
			{
				dir.GetFolderPath().CreateDirIfNotExists();
			}
			ViewControDesignerTemplate.Write(viewController.ScriptName, dir, viewController.Namespace, panelCodeInfo, CodeGenKitSetting.Load());
		}

		private BindCodeInfo CreatePanelCodeInfo(Transform root, List<BindDetail> bindDetails)
		{
			BindCodeInfo panelCodeInfo = new BindCodeInfo();
			panelCodeInfo.GameObjectName = root.gameObject.name;
			foreach (var item in bindDetails)
			{
				panelCodeInfo.BindInfos.Add(new BindInfo
				{
					TypeName = item.ComponentName,
					MemberName = item.BindObj.name,
					PathToRoot = CodeGenHelper.PathToParent(item.BindObj.transform, root.name),
					BindObj = item.BindObj
				}) ;
			}
			return panelCodeInfo;
		}

		private void CreateUIPanelCode(BasePanel panel, BindCodeInfo panelCodeInfo)
		{

			if (null == panel)
				return;

			var dir = panel.ScriptsFolder;
			var generateFilePath = $"{dir}/{panel.ScriptName}.cs";

			if (!File.Exists(generateFilePath))
			{
				generateFilePath.GetFolderPath().CreateDirIfNotExists();
				UIPanelTemplate.Write(panel.ScriptName, generateFilePath, panel.Namespace, UIKitSetting.Load());
				Debug.Log(">>>>>>>Success Create UIPrefab Code: " + panel.ScriptName);
			}
			else
			{
				Debug.Log($"{panel.ScriptName}已存在");
			}
			CreateUIPanelDesignerCode(panel, panelCodeInfo);
		}


		private void CreateUIPanelDesignerCode(BasePanel panel, BindCodeInfo panelCodeInfo)
		{
			if (null == panel)
				return;

			var dir = panel.ScriptsFolder;

			if (!File.Exists(dir))
			{
				dir.GetFolderPath().CreateDirIfNotExists();
			}
			UIPanelDesignerTemplate.Write(panel.ScriptName, dir, panel.Namespace, panelCodeInfo, UIKitSetting.Load());
		}


		private void OnCompile()
		{
			if (CurrentTask == null) return;
			if (CurrentTask.Status == CodeGenTaskStatus.Compile)
			{
				var generateClassName = CurrentTask.ScriptName;
				var generateNamespace = CurrentTask.Namespace;

				var assemblies = AppDomain.CurrentDomain.GetAssemblies().Where(assembly =>
					!assembly.FullName.StartsWith("Unity"));

				var typeName = generateNamespace + "." + generateClassName;

				var type = assemblies.Where(a => a.GetType(typeName) != null)
					.Select(a => a.GetType(typeName)).FirstOrDefault();

				if (type == null)
				{
					Debug.Log("编译失败");
					return;
				}

				Debug.Log(type);

				var gameObject = CurrentTask.GameObject;


				var scriptComponent = gameObject.GetComponent(type);

				if (!scriptComponent)
				{
					scriptComponent = gameObject.AddComponent(type);
				}

				var serializedObject = new SerializedObject(scriptComponent);

				var bindDetails = serializedObject.FindProperty("BindDetails");
				bindDetails.arraySize = CurrentTask.BindDetails.Count;
				for (int i = 0; i < CurrentTask.BindDetails.Count; i++)
				{
					int index = i;
					BindDetail bindDetail = CurrentTask.BindDetails[index];
					SerializedProperty bindDetailSer = bindDetails.GetArrayElementAtIndex(index);
					bindDetailSer.FindPropertyRelative("BindObj").objectReferenceValue = bindDetail.BindObj;

					var componentNames = bindDetailSer.FindPropertyRelative("ComponentNames");
					componentNames.arraySize = bindDetail.ComponentNames.Length;
					for (int j = 0; j < bindDetail.ComponentNames.Length; j++)
					{
						componentNames.GetArrayElementAtIndex(j).stringValue = bindDetail.ComponentNames[j];
					}
					bindDetailSer.FindPropertyRelative("ComponentNameIndex").intValue = bindDetail.ComponentNameIndex;
					bindDetailSer.FindPropertyRelative("ComponentName").stringValue = bindDetail.ComponentName;
				}
			
				foreach (var bindInfo in CurrentTask.BindInfos)
				{
					var componentName = bindInfo.TypeName.Split('.').Last();
					var serializedProperty = serializedObject.FindProperty(bindInfo.MemberName);
					var component = gameObject.transform.Find(bindInfo.PathToRoot).GetComponent(componentName);

					if (!component)
					{
						component = gameObject.transform.Find(bindInfo.PathToRoot).GetComponent(bindInfo.TypeName);
					}

					serializedProperty.objectReferenceValue = component;

					// Debug.Log(componentName + "@@@@" + serializedProperty + "@@@@" + component);
				}

				serializedObject.FindProperty("ScriptsFolder").stringValue = CurrentTask.ScriptsFolder;
				serializedObject.FindProperty("PrefabFolder").stringValue = CurrentTask.PrefabFolder;
				serializedObject.FindProperty("GeneratePrefab").boolValue = CurrentTask.GeneratePrefab;
				serializedObject.FindProperty("ScriptName").stringValue = CurrentTask.ScriptName;
				serializedObject.FindProperty("Namespace").stringValue = CurrentTask.Namespace;
				

				var generatePrefab = CurrentTask.GeneratePrefab;
				var prefabFolder = CurrentTask.PrefabFolder;

				if (CurrentTask.IsPanel)
				{
					BasePanel panel = gameObject.GetComponent<BasePanel>();
					if (panel.GetType() != type)
					{
						DestroyImmediate(panel, false);
					}
				}
				else
				{
					ViewController viewController = gameObject.GetComponent<ViewController>();
					if (viewController.GetType() != type)
					{
						DestroyImmediate(viewController, false);
					}
				}

				serializedObject.ApplyModifiedPropertiesWithoutUndo();

				if (generatePrefab)
				{
					prefabFolder.CreateDirIfNotExists();

					var generatePrefabPath = prefabFolder + "/" + gameObject.name + ".prefab";

					if (File.Exists(generatePrefabPath))
					{
						// PrefabUtility.SavePrefabAsset(gameObject);
					}
					else
					{
						PrefabUtils.SaveAndConnect(generatePrefabPath, gameObject);
					}
				}


				EditorSceneManager.MarkSceneDirty(SceneManager.GetActiveScene());


				CurrentTask.Status = CodeGenTaskStatus.Complete;
				CurrentTask = null;
			}
		}

		[DidReloadScripts]
		static void Compile()
		{
			Default.OnCompile();
		}
	}
}
