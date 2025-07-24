/****************************************************************************
 * Copyright (c) 2015 ~ 2022  UNDER MIT LICENSE
 * 

 ****************************************************************************/

using System;
using System.IO;
using UnityEditor;
using UnityEngine;

namespace XXLFramework
{
    [Serializable]
    public class CodeGenKitSetting
    {
        static string mConfigSavedDir => (Application.dataPath + "/XXL_U3D/XXLFramework/Framework/FrameworkData/").CreateDirIfNotExists() + "ProjectConfig/";

        private const string mConfigSavedFileName = "ViewControllerSetting.json";

        public string Namespace = "XXLFramework";

        public string ScriptDir = "Assets/XXL_U3D/Game/Scripts/NotPanel";

        public string PrefabDir = "Assets/XXL_U3D/Game/Prefabs";



        public static CodeGenKitSetting Load()
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
                JsonUtility.FromJson<CodeGenKitSetting>(File.ReadAllText(mConfigSavedDir + mConfigSavedFileName));

            if (frameworkConfigData == null || string.IsNullOrEmpty(frameworkConfigData.Namespace))
            {
                frameworkConfigData = new CodeGenKitSetting();
            }

            return frameworkConfigData;
        }

        public void Save()
        {
            File.WriteAllText(mConfigSavedDir + mConfigSavedFileName, JsonUtility.ToJson(this));
            AssetDatabase.Refresh();
        }
    }
}
