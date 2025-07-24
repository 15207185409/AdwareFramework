/****************************************************************************
 * Copyright (c) 2015 ~ 2022  UNDER MIT LICENSE
 * 

 ****************************************************************************/

#if UNITY_EDITOR
using System.IO;
using System.Linq;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEditorInternal;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace XXLFramework
{
    [CustomEditor(typeof(ViewController), true)]
    public class ViewControllerInspector : Editor
    {
        private ReorderableList bindDetails;

        [MenuItem("GameObject/CodeGenKit/@(Alt+V)Add View Controller &v", false, 0)]
        static void AddView()
        {
            var gameObject = Selection.objects.First() as GameObject;

            if (!gameObject)
            {
                Debug.LogWarning("需要选择 GameObject");
                return;
            }

            var view = gameObject.GetComponent<ViewController>();

            if (!view)
            {
                gameObject.AddComponent<ViewController>();
            }
        }
       
        private ViewControllerInspectorLocale mLocaleText = new ViewControllerInspectorLocale();


        public ViewController ViewController => target as ViewController;


        private void OnEnable()
        {
            if (ViewController == null)
            {
                return;
            }
            RemoveDestroyedBindDetail();
            RefreshComponents(ViewController);
            DrawReorderableList();
            if (string.IsNullOrEmpty(ViewController.ScriptsFolder))
            {
                var setting = CodeGenKitSetting.Load();
                ViewController.ScriptsFolder = setting.ScriptDir;
            }

            if (string.IsNullOrEmpty(ViewController.PrefabFolder))
            {
                var setting = CodeGenKitSetting.Load();
                ViewController.PrefabFolder = setting.PrefabDir;
            }

            if (string.IsNullOrEmpty(ViewController.ScriptName))
            {
                ViewController.ScriptName = ViewController.name;
            }

            if (string.IsNullOrEmpty(ViewController.Namespace))
            {
                var setting = CodeGenKitSetting.Load();
                ViewController.Namespace = setting.Namespace;
            }
        }

        //刷新组件
        private void RefreshComponents(BindGroup bindGroup)
        {
            foreach (var item in bindGroup.BindDetails)
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
            for (int i = ViewController.BindDetails.Count - 1; i >= 0; i--)
            {
                if (ViewController.BindDetails[i].BindObj == null)
                {
                    ViewController.BindDetails.RemoveAt(i);
                }
            }
        }

        private readonly ViewControllerInspectorStyle mStyle = new ViewControllerInspectorStyle();

        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();
            

            GUILayout.BeginVertical("box");

            GUILayout.BeginHorizontal();
            GUILayout.Label(mLocaleText.CodegenPart,mStyle.BigTitleStyle.Value);
            LocaleKitEditor.DrawSwitchToggle(GUI.skin.label.normal.textColor);
            GUILayout.EndHorizontal();

            CreateDragBindArea();

            GUILayout.BeginHorizontal();
            GUILayout.Label(mLocaleText.BindInfo, GUILayout.Width(150));
            GUILayout.FlexibleSpace();
            if (GUILayout.Button("清空"))
            {
                ViewController.BindDetails.Clear();
            }
            GUILayout.EndHorizontal();
            ShowBindInfo();

            GUILayout.BeginHorizontal();
            GUILayout.Label(mLocaleText.Namespace, GUILayout.Width(150));
            ViewController.Namespace = EditorGUILayout.TextArea(ViewController.Namespace);
            GUILayout.EndHorizontal();

            GUILayout.BeginHorizontal();
            GUILayout.Label(mLocaleText.ScriptName, GUILayout.Width(150));
            ViewController.ScriptName = EditorGUILayout.TextArea(ViewController.ScriptName);
            GUILayout.EndHorizontal();

            GUILayout.BeginHorizontal();
            GUILayout.Label(mLocaleText.ScriptsFolder, GUILayout.Width(150));

            string path = FindScriptPathByClassName();
            if (path.IsNotNullAndEmpty())
            {
                ViewController.ScriptsFolder = path;
                //Debug.Log("脚本路径：" + path);
                EditorGUILayout.LabelField(ViewController.ScriptsFolder, GUILayout.Height(30));

            }
            else
            {
                //Debug.Log("资源不存在");
                ViewController.ScriptsFolder =
                EditorGUILayout.TextField(ViewController.ScriptsFolder, GUILayout.Height(30));
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
                        ViewController.ScriptsFolder = newPath;
                        AssetDatabase.SaveAssets();
                        AssetDatabase.Refresh();
                        EditorSceneManager.MarkSceneDirty(SceneManager.GetActiveScene());
                    }
                }
            }


            GUILayout.BeginHorizontal();
            ViewController.GeneratePrefab =
                GUILayout.Toggle(ViewController.GeneratePrefab, mLocaleText.GeneratePrefab);
            GUILayout.EndHorizontal();

            if (ViewController.GeneratePrefab)
            {
                GUILayout.BeginHorizontal();
                GUILayout.Label(mLocaleText.PrefabGenerateFolder, GUILayout.Width(150));
                ViewController.PrefabFolder =
                    GUILayout.TextArea(ViewController.PrefabFolder, GUILayout.Height(30));
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
                            ViewController.PrefabFolder = newPath;
                            AssetDatabase.SaveAssets();
                            AssetDatabase.Refresh();
                            EditorSceneManager.MarkSceneDirty(SceneManager.GetActiveScene());
                        }
                    }
                }
            }

            var fileFullPath = ViewController.ScriptsFolder + "/" + ViewController.ScriptName + ".cs";
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
                CodeGenKit.Generate(ViewController);
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
            string className = ViewController.GetType().ToString();
            className = className.Substring(className.LastIndexOf('.') + 1);

            if (className == "ViewController")
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
                            ViewController.AddBindObj(new BindDetail(bindObj));
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