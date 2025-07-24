/****************************************************************************
 * Copyright (c) 2015 ~ 2022  UNDER MIT LICENSE
 * 

 ****************************************************************************/

#if UNITY_EDITOR
using System;
using UnityEngine;

namespace XXLFramework
{
    /// <summary>
    /// 存储一些Mark相关的信息
    /// </summary>
    [Serializable]
    public class BindInfo
    {
        public string TypeName;

        public string PathToRoot;

        public GameObject BindObj;
        //public Component BindScript;
        
        public string MemberName;
    }

}
#endif