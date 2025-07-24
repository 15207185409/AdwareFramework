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
    public interface IMGUILayout : IMGUIView
    {
        IMGUILayout AddChild(IMGUIView view);

        void RemoveChild(IMGUIView view);

        void Clear();
    }
    
    public static class LayoutExtension
    {
        public static T Parent<T>(this T view, IMGUILayout parent) where T : IMGUIView
        {
            parent.AddChild(view);
            return view;
        }
    }
    
   
}