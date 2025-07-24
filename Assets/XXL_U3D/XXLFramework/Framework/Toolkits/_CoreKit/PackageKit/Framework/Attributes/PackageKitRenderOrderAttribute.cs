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
    [AttributeUsage(AttributeTargets.Class)]
    public class PackageKitRenderOrderAttribute : Attribute
    {
        public int Order { get; private set; }
        
        public PackageKitRenderOrderAttribute(int order)
        {
            Order = order;
        }
    }
}