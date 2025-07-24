/****************************************************************************
 * Copyright (c) 2016 - 2022  UNDER MIT License
 * 

 ****************************************************************************/

#if UNITY_EDITOR
using UnityEditor;

namespace XXLFramework
{

    public class EditorPrefsBoolProperty : BindableProperty<bool>
    {

        public EditorPrefsBoolProperty(string key, bool initValue = false)
        {
            // 初始化
            mValue = EditorPrefs.GetBool(key, initValue);

            Register(value => { EditorPrefs.SetBool(key, value); });
        }
    }
}
#endif