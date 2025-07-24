/****************************************************************************
 * Copyright (c) 2015 - 2022  UNDER MIT License
 * 
 * http://XXLFramework.cn
 * https://github.com//XXLFramework
 * https://gitee.com//XXLFramework
 ****************************************************************************/

#if UNITY_EDITOR
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace XXLFramework
{
    public class EasyInspectorEditor : Editor, IMGUILayoutRoot
    {
        VerticalLayout IMGUILayoutRoot.Layout { get; set; }
        RenderEndCommandExecutor IMGUILayoutRoot.RenderEndCommandExecutor { get; set; }

        protected void Save()
        {
            EditorUtility.SetDirty(target);
            UnityEditor.SceneManagement.EditorSceneManager
                .MarkSceneDirty(SceneManager.GetActiveScene());
            GUIUtility.ExitGUI();
        }
    }
}
#endif