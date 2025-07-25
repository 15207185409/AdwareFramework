/****************************************************************************
 * Copyright (c) 2015 ~ 2022  UNDER MIT LICENSE
 * 

 ****************************************************************************/

#if UNITY_EDITOR
using System;
using UnityEditor;
using UnityEngine;

namespace XXLFramework
{
    [PackageKitGroup("XXLFramework")]
    [PackageKitRenderOrder(5)]
    [DisplayNameCN("CodeGenKit 设置")]
    [DisplayNameEN("CodegenKit Setting")]
    internal class CodeGenKitSettingEditor : IPackageKitView
    {
        public EditorWindow EditorWindow { get; set; }

        private CodeGenKitSetting mCodeGenKitSetting;

        public void Init()
        {
            mCodeGenKitSetting = CodeGenKit.Setting;
        }

        public void OnUpdate()
        {

        }

        private Lazy<GUIStyle> mLabelBold12 = new Lazy<GUIStyle>(() =>
        {
            return new GUIStyle(GUI.skin.label)
            {
                fontSize = 12,
                fontStyle = FontStyle.Bold
            };
        });

        private Lazy<GUIStyle> mLabel12 = new Lazy<GUIStyle>(() =>
        {
            return new GUIStyle(GUI.skin.label)
            {
                fontSize = 12,
            };
        });

        public void OnGUI()
        {

            GUILayout.BeginVertical("box");
            {
                GUILayout.Label(LocaleText.ViewControllerNamespace, mLabel12.Value, GUILayout.Width(200));

                GUILayout.Space(6);

                GUILayout.BeginHorizontal();
                {
                    GUILayout.Label(LocaleText.ViewControllerNamespace, mLabelBold12.Value, GUILayout.Width(200));

                    mCodeGenKitSetting.Namespace = EditorGUILayout.TextField(mCodeGenKitSetting.Namespace);

                }
                GUILayout.EndHorizontal();

                GUILayout.Space(6);

                GUILayout.BeginHorizontal();
                {
                    GUILayout.Label(LocaleText.ViewControllerScriptGenerateDir, mLabelBold12.Value,
                        GUILayout.Width(220));

                    mCodeGenKitSetting.ScriptDir = EditorGUILayout.TextField(CodeGenKit.Setting.ScriptDir);

                    if (GUILayout.Button("选择文件夹"))
                    {
                        // 弹出文件选择对话框，只允许选择文件夹
                        string selectedFolderPath = EditorUtility.OpenFolderPanel("Select Assets Folder", mCodeGenKitSetting.ScriptDir, "");

                        if (!string.IsNullOrEmpty(selectedFolderPath))
                        {
                            // 将选择的文件夹路径转换为相对于Assets的路径
                            string relativePath = EditorIOUtility.GetAssetsPath(selectedFolderPath);

                            mCodeGenKitSetting.ScriptDir = relativePath;
                            Debug.Log(CodeGenKit.Setting.ScriptDir);
                        }
                    }

                }
                GUILayout.EndHorizontal();

                GUILayout.Space(6);


                GUILayout.BeginHorizontal();
                {

                    GUILayout.Label(LocaleText.ViewControllerPrefabGenerateDir, mLabelBold12.Value,
                        GUILayout.Width(220));
                    mCodeGenKitSetting.PrefabDir =
                        EditorGUILayout.TextField(mCodeGenKitSetting.PrefabDir);

                    if (GUILayout.Button("选择文件夹"))
                    {
                        // 弹出文件选择对话框，只允许选择文件夹
                        string selectedFolderPath = EditorUtility.OpenFolderPanel("Select Assets Folder", mCodeGenKitSetting.PrefabDir, "");

                        if (!string.IsNullOrEmpty(selectedFolderPath))
                        {
                            // 将选择的文件夹路径转换为相对于Assets的路径
                            string relativePath = EditorIOUtility.GetAssetsPath(selectedFolderPath);

                            mCodeGenKitSetting.PrefabDir = relativePath;
                        }
                    }
                }
                GUILayout.EndHorizontal();

                GUILayout.Space(6);

                if (GUILayout.Button(LocaleText.Apply))
                {
                    CodeGenKit.Setting.Save();
                }
            }
            GUILayout.EndVertical();
        }

        public void OnWindowGUIEnd()
        {
        }

        public void OnDispose()
        {
        }

        public void OnShow()
        {
        }

        public void OnHide()
        {
        }


        class LocaleText
        {
            public static bool IsCN => LocaleKitEditor.IsCN.Value;

            public static string ViewControllerNamespace =>
                IsCN ? " ViewController 命名空间:" : "ViewController Namespace:";


            public static string ViewControllerScriptGenerateDir =>
                IsCN ? " ViewController 脚本生成路径:" : " ViewController Code Generate Dir:";

            public static string ViewControllerPrefabGenerateDir =>
                IsCN
                    ? " ViewController Prefab 生成路径:"
                    : " ViewController Prefab Generate Dir:";

            public static string Apply => IsCN ? "保存" : "Apply";
        }
    }
}
#endif