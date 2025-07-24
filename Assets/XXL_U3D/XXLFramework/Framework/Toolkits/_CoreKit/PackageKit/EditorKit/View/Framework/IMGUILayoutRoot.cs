/****************************************************************************
 * Copyright (c) 2015 - 2022  UNDER MIT License
 * 
 * http://XXLFramework.cn
 * https://github.com//XXLFramework
 * https://gitee.com//XXLFramework
 ****************************************************************************/

#if UNITY_EDITOR
using System;

namespace XXLFramework
{
    public interface IMGUILayoutRoot
    {
        VerticalLayout Layout { get; set; }

        RenderEndCommandExecutor RenderEndCommandExecutor { get; set; }
    }
    
    public static class IMGUILayoutRootExtension
    {
        public static IMGUILayout GetLayout(this IMGUILayoutRoot self)
        {
            if (self.Layout == null)
            {
                self.Layout = new VerticalLayout();
            }

            return self.Layout;
        }

        public static void AddChild(this IMGUILayoutRoot self, IMGUIView child)
        {
            self.GetLayout().AddChild(child);
        }

        public static void RemoveChild(this IMGUILayoutRoot self, IMGUIView child)
        {
            self.GetLayout().RemoveChild(child);
        }


        public static RenderEndCommandExecutor GetCommandExecutor(this IMGUILayoutRoot self)
        {
            if (self.RenderEndCommandExecutor == null)
            {
                self.RenderEndCommandExecutor = new RenderEndCommandExecutor();
            }

            return self.RenderEndCommandExecutor;
        }

        public static void PushRenderEndCommand(this IMGUILayoutRoot self, Action command)
        {
            self.GetCommandExecutor().Push(command);
        }

        public static void ExecuteRenderEndCommand(this IMGUILayoutRoot self)
        {
            self.GetCommandExecutor().Execute();
        }
    }
}
#endif