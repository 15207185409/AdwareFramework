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
    public class DisplayNameENAttribute : Attribute
    {
        public string DisplayName { get; }

        public DisplayNameENAttribute(string displayName)
        {
            DisplayName = displayName;
        }
    }
}