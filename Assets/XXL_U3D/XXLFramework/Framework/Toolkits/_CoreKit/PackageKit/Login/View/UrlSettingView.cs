/****************************************************************************
 * Copyright (c) 2020.10 
 * 
 * https://xxlframework.cn
 * https://github.com//XXLFramework
 * https://gitee.com//XXLFramework
 * 
 * Permission is hereby granted, free of charge, to any person obtaining a copy
 * of this software and associated documentation files (the "Software"), to deal
 * in the Software without restriction, including without limitation the rights
 * to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
 * copies of the Software, and to permit persons to whom the Software is
 * furnished to do so, subject to the following conditions:
 * 
 * The above copyright notice and this permission notice shall be included in
 * all copies or substantial portions of the Software.
 * 
 * THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
 * IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
 * FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
 * AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
 * LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
 * OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN
 * THE SOFTWARE.
 ****************************************************************************/

#if UNITY_EDITOR
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using UnityEditor;
using UnityEngine;

namespace XXLFramework
{
    [DisplayName("服务器地址")]
    [DisplayNameCN("服务器地址")]
    [DisplayNameEN("Service(OnlyCN)")]
    [PackageKitGroup("XXLFramework")]
    [PackageKitRenderOrder(-1)]
    internal class UrlSettingView : VerticalLayout, IPackageKitView
    {
        public EditorWindow EditorWindow { get; set; }

        private  UrlSetting UrlSettingData;


        public void Init()
        {
            UrlSettingData = UrlSetting.Load();
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

                GUILayout.BeginHorizontal();
                {
                    GUILayout.Label(LocaleText.Url, mLabelBold12.Value, GUILayout.Width(200));

                    UrlSettingData.url = EditorGUILayout.TextField(UrlSettingData.url);

                }
                GUILayout.EndHorizontal();

                GUILayout.Space(6);

                if (GUILayout.Button(LocaleText.Apply))
                {
                    UrlSettingData.Save();
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
            public static string Url => IsCN ? " 服务器地址:" : "ServiceAddress:";
            public static string Apply => IsCN ? "保存" : "Apply";
        }
    }

    [Serializable]
    internal class UrlSetting
	{
        static string mConfigSavedDir => (Application.dataPath + "/XXL_U3D/XXLFramework/Framework/FrameworkData/").CreateDirIfNotExists() + "ProjectConfig/";

        private const string mConfigSavedFileName = "UrlSetting.json";

        private const string staticIP = "192.168.10.69";

        public string url = "192.168.10.69";

        public static string BaseUrl = $"http://{staticIP}:53362/api";

        public static UrlSetting Load()
        {
            mConfigSavedDir.CreateDirIfNotExists();

            if (!File.Exists(mConfigSavedDir + mConfigSavedFileName))
            {
                using (var fileStream = File.Create(mConfigSavedDir + mConfigSavedFileName))
                {
                    fileStream.Close();
                }
            }

            var frameworkConfigData =
                JsonUtility.FromJson<UrlSetting>(File.ReadAllText(mConfigSavedDir + mConfigSavedFileName));

            if (frameworkConfigData == null || string.IsNullOrEmpty(frameworkConfigData.url))
            {
                frameworkConfigData = new UrlSetting { url = "192.168.10.69" };
            }
			else
			{
                BaseUrl = $"http://{frameworkConfigData.url}:53362/api";
            }

            return frameworkConfigData;
        }

        public void Save()
        {
            BaseUrl = $"http://{this.url}:53362/api";
            File.WriteAllText(mConfigSavedDir + mConfigSavedFileName, JsonUtility.ToJson(this));
            AssetDatabase.Refresh();
        }
    }
}
#endif