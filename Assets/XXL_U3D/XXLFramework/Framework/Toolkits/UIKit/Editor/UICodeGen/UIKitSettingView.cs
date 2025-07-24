/****************************************************************************

 ****************************************************************************/

#if UNITY_EDITOR
using Sirenix.OdinInspector;
using System;
using System.IO;
using System.Text;
using UnityEditor;
using UnityEngine;

// ReSharper disable once CheckNamespace
namespace XXLFramework
{
    [PackageKitGroup("XXLFramework")]
    [PackageKitRenderOrder(4)]
    [DisplayNameCN("UIKit 设置")]
    [DisplayNameEN("UIKit Setting")]
    public class UIKitEditorWindow : IPackageKitView
    {
        public EditorWindow EditorWindow { get; set; }

        private UIKitSetting mUiKitSettingData;

        public void Init()
        {
            mUiKitSettingData = UIKitSetting.Load();
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
                GUILayout.Label(LocaleText.UINamespace, mLabel12.Value, GUILayout.Width(200));

                GUILayout.Space(6);

                GUILayout.BeginHorizontal();
                {
                    GUILayout.Label(LocaleText.UINamespace, mLabelBold12.Value, GUILayout.Width(200));

                    mUiKitSettingData.Namespace = EditorGUILayout.TextField(mUiKitSettingData.Namespace);

                }
                GUILayout.EndHorizontal();

                GUILayout.Space(6);

                GUILayout.BeginHorizontal();
                {
                    GUILayout.Label(LocaleText.UIScriptGenerateDir, mLabelBold12.Value, GUILayout.Width(200));

                    mUiKitSettingData.UIScriptDir = EditorGUILayout.TextField(mUiKitSettingData.UIScriptDir);

					if (GUILayout.Button("选择文件夹"))
					{
                        // 弹出文件选择对话框，只允许选择文件夹
                        string selectedFolderPath = EditorUtility.OpenFolderPanel("Select Assets Folder", mUiKitSettingData.UIScriptDir, "");

                        if (!string.IsNullOrEmpty(selectedFolderPath))
                        {
                            // 将选择的文件夹路径转换为相对于Assets的路径
                            string relativePath = EditorIOUtility.GetAssetsPath(selectedFolderPath);

                            mUiKitSettingData.UIScriptDir = relativePath;
                        }
                    }

                }
                GUILayout.EndHorizontal();

                GUILayout.Space(6);

                GUILayout.BeginHorizontal();
                {
                    GUILayout.Label(LocaleText.UIPanelPrefabDir, mLabelBold12.Value, GUILayout.Width(200));

                    mUiKitSettingData.UIPrefabDir = EditorGUILayout.TextField(mUiKitSettingData.UIPrefabDir);

                    if (GUILayout.Button("选择文件夹"))
                    {
                        // 弹出文件选择对话框，只允许选择文件夹
                        string selectedFolderPath = EditorUtility.OpenFolderPanel("Select Assets Folder", mUiKitSettingData.UIPrefabDir, "");

                        if (!string.IsNullOrEmpty(selectedFolderPath))
                        {
                            // 将选择的文件夹路径转换为相对于Assets的路径
                            string relativePath = EditorIOUtility.GetAssetsPath(selectedFolderPath);

                            mUiKitSettingData.UIPrefabDir = relativePath;
                        }
                    }
                }
                GUILayout.EndHorizontal();

                GUILayout.Space(6);

                if (GUILayout.Button(LocaleText.Apply))
                {
                    mUiKitSettingData.Save();
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
            public static string UINamespace => IsCN ? " UI 命名空间:" : "UI Namespace:";

            public static string UIScriptGenerateDir => IsCN ? " UI 脚本生成路径:" : " UI Scripts Generate Dir:";

            public static string UIPanelPrefabDir => IsCN ? " UIPanel Prefab 路径:" : " UIPanel Prefab Dir:";

            public static string Apply => IsCN ? "保存" : "Apply";
        }
    }    
}
#endif