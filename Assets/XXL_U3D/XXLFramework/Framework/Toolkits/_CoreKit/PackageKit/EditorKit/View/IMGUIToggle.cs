/****************************************************************************
 * Copyright (c) 2015 - 2022  UNDER MIT License
 * 
 * http://XXLFramework.cn
 * https://github.com//XXLFramework
 * https://gitee.com//XXLFramework
 ****************************************************************************/

#if UNITY_EDITOR
using System;
using UnityEngine;

namespace XXLFramework
{
    public interface IMGUIToggle : IMGUIView, IHasText<IMGUIToggle>
    {
        BindableProperty<bool> ValueProperty { get; }

        IMGUIToggle IsOn(bool isOn);
        IMGUIToggle IsOn(Func<bool> isOngetter);
    }

    internal class IMGUIIMGUIToggleView : IMGUIAbstractView, IMGUIToggle
    {
        private Func<bool> mIsOnGetter;
        private string mText { get; set; }

        public IMGUIIMGUIToggleView()
        {
            ValueProperty = new BindableProperty<bool>(false);

            Style = new GUIStyleProperty(() => GUI.skin.toggle);
        }

        public BindableProperty<bool> ValueProperty { get; private set; }

        public IMGUIToggle IsOn(bool isOn)
        {
            ValueProperty.Value = isOn;
            return this;
        }

        public IMGUIToggle IsOn(Func<bool> isOnGetter)
        {
            mIsOnGetter = isOnGetter;
            return this;
        }

        protected override void OnGUI()
        {
            ValueProperty.Value =
                GUILayout.Toggle(mIsOnGetter?.Invoke() ?? ValueProperty.Value, mText ?? string.Empty, Style.Value,
                    LayoutStyles);
        }

        public IMGUIToggle Text(string text)
        {
            mText = text;
            return this;
        }
    }
}
#endif