/****************************************************************************
 * Copyright (c) 2015 - 2022  UNDER MIT License
 * 
 * http://XXLFramework.cn
 * https://github.com//XXLFramework
 * https://gitee.com//XXLFramework
 ****************************************************************************/

using System;

namespace XXLFramework
{
    public class PackageKitGroupAttribute : Attribute
    {
        public string GroupName { get; set; }

        public PackageKitGroupAttribute(string groupName)
        {
            GroupName = groupName;
        }
    }
}