/****************************************************************************
 * Copyright (c) 2015 - 2022  UNDER MIT License
 * 

 ****************************************************************************/

#if UNITY_EDITOR
using UnityEditor;
using UnityEngine;

namespace XXLFramework
{
    [PackageKitGroup("XXLFramework")]
    [PackageKitRenderOrder(7)]
    [DisplayNameCN("LocaleKit 设置")]
    [DisplayNameEN("LocaleKit Setting")]
    public class LocaleKitConfigView:IPackageKitView
    {
		public EditorWindow EditorWindow { get; set; }

		public void Init()
        {
        }

        public void OnGUI()
        {
            if (GUILayout.Button(Locale.Config))
            {
                Selection.activeObject = LanguageDefineConfig.Default;
            }
        }

        public void OnWindowGUIEnd()
        {
        }

        public void OnDestroy()
        {
        }

		public void OnUpdate()
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

		class Locale
        {
            public static string Config => LocaleKitEditor.IsCN.Value ? "配置" : "Config";
        }
    }
}
#endif