/****************************************************************************
 * Copyright (c) 2015 - 2022  UNDER MIT License
 * 
 * http://XXLFramework.cn
 * https://github.com//XXLFramework
 * https://gitee.com//XXLFramework
 ****************************************************************************/

#if UNITY_EDITOR
using System;
using UnityEditor;

namespace XXLFramework
{
    public interface IPackageKitView
    {
        EditorWindow EditorWindow { get; set; }
        
        void Init();

        void OnUpdate();
        void OnGUI();

        void OnWindowGUIEnd();

        void OnDispose();
        void OnShow();
        void OnHide();
    }
}
#endif