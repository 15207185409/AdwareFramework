/****************************************************************************
 * Copyright (c) 2015 ~ 2022  UNDER MIT LICENSE
 * 

 ****************************************************************************/

#if UNITY_EDITOR
using UnityEditor;
using UnityEngine;

namespace XXLFramework
{
    public static class PrefabUtils
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="assetPrefabPath">Assets/xxx/yyy.prefab</param>
        /// <param name="gameObject"></param>
        public static Object SaveAndConnect(string assetPrefabPath, GameObject gameObject)
        {
            return PrefabUtility.SaveAsPrefabAssetAndConnect(gameObject,
                assetPrefabPath,
                InteractionMode.AutomatedAction);
        }
    }
}
#endif