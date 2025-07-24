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

namespace XXLFramework
{
    public interface IGenericMenu
    {
    }

    internal class GenericMenuView : IGenericMenu
    {
        protected GenericMenuView()
        {
        }

        public static GenericMenuView Create()
        {
            return new GenericMenuView();
        }

        private GenericMenu mMenu = new GenericMenu();

        public GenericMenuView Separator()
        {
            mMenu.AddSeparator(string.Empty);
            return this;
        }

        public GenericMenuView AddMenu(string menuPath, GenericMenu.MenuFunction click)
        {
            mMenu.AddItem(new GUIContent(menuPath), false, click);
            return this;
        }

        public void Show()
        {
            mMenu.ShowAsContext();
        }
    }
}
#endif