/****************************************************************************
 * Copyright (c) 2015 ~ 2022  UNDER MIT LICENSE
 * 

 ****************************************************************************/

#if UNITY_EDITOR
using System.Collections.Generic;
using UnityEngine;

namespace XXLFramework
{
    public enum CodeGenTaskStatus
    {
        Search,
        Gen,
        Compile,
        Complete
    }

    public enum GameObjectFrom
    {
        Scene,
        Prefab
    }
    
    [System.Serializable]
    public class CodeGenTask
    { 
        public bool ShowLog = false;

        public bool IsPanel = false; //是否面板

        // state
        public CodeGenTaskStatus Status;

        // input
        public GameObject GameObject;
        public GameObjectFrom From = GameObjectFrom.Scene;

        // search
        public List<StringPair> NameToFullName = new List<StringPair>();
        public List<BindInfo> BindInfos = new List<BindInfo>();

        public List<BindDetail> BindDetails;
        
        // info
        public string ScriptsFolder;
        public string PrefabFolder;
        public bool GeneratePrefab;
        public string ScriptName;
        public string Namespace;
        
        // result
        public string MainCode;
        public string DesignerCode;
    }


	[System.Serializable]
    public class StringPair
    {
        public StringPair(string key, string value)
        {
            Key = key;
            Value = value;
        }
        
        public string Key;
        public string Value;
    }
}
#endif