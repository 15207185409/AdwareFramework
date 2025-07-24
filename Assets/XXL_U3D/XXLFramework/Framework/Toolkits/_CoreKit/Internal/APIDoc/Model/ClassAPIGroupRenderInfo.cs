/****************************************************************************
 * Copyright (c) 2015 - 2022  UNDER MIT License
 * 

 ****************************************************************************/

#if UNITY_EDITOR
using System.Collections.Generic;

namespace XXLFramework
{
    internal class ClassAPIGroupRenderInfo
    {
        public List<ClassAPIRenderInfo> ClassAPIRenderInfos { get; set; }

        public string GroupName { get; private set; }

        public ClassAPIGroupRenderInfo(string groupName)
        {
            GroupName = groupName;
            Open = new EditorPrefsBoolProperty(groupName);
        }

        public EditorPrefsBoolProperty Open { get; private set; }
    }
}
#endif