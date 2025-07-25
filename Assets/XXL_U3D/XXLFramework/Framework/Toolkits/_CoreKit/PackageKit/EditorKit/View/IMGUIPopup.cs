/****************************************************************************
 * Copyright (c) 2015 - 2022  UNDER MIT License
 * 
 * http://XXLFramework.cn
 * https://github.com//XXLFramework
 * https://gitee.com//XXLFramework
 ****************************************************************************/

#if UNITY_EDITOR
using System;
using System.Collections.Generic;
using UnityEditor;

namespace XXLFramework
{
    public interface IPopup : IMGUIView
    {
        IPopup WithIndexAndMenus(int index, params string[] menus);

        IPopup OnIndexChanged(Action<int> indexChanged);

        IPopup ToolbarStyle();

        BindableProperty<int> IndexProperty { get; }
        IPopup Menus(List<string> value);
    }

    public class PopupView : IMGUIAbstractView, IPopup
    {
        protected PopupView()
        {
            mStyleProperty = new GUIStyleProperty(() => EditorStyles.popup);
        }

        public static IPopup Create()
        {
            return new PopupView();
        }

        private BindableProperty<int> mIndexProperty = new BindableProperty<int>(0);

        public BindableProperty<int> IndexProperty
        {
            get { return mIndexProperty; }
        }

        public IPopup Menus(List<string> menus)
        {
            mMenus = menus.ToArray();
            return this;
        }

        private string[] mMenus = { };

        protected override void OnGUI()
        {
            IndexProperty.Value =
                EditorGUILayout.Popup(IndexProperty.Value, mMenus, mStyleProperty.Value, LayoutStyles);
        }

        public IPopup WithIndexAndMenus(int index, params string[] menus)
        {
            IndexProperty.Value = index;
            mMenus = menus;
            return this;
        }

        public IPopup OnIndexChanged(Action<int> indexChanged)
        {
            IndexProperty.Register(indexChanged);
            return this;
        }

        public IPopup ToolbarStyle()
        {
            mStyleProperty = new GUIStyleProperty(() => EditorStyles.toolbarPopup);
            return this;
        }
    }
}
#endif