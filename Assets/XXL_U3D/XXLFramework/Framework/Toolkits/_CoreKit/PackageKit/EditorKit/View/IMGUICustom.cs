/****************************************************************************
 * Copyright (c) 2015 - 2022  UNDER MIT License
 * 
 * http://XXLFramework.cn
 * https://github.com//XXLFramework
 * https://gitee.com//XXLFramework
 ****************************************************************************/

#if UNITY_EDITOR
using System;
using System.Xml;

namespace XXLFramework
{
    public interface IMGUICustom : IMGUIView, IXMLToObjectConverter
    {
        IMGUICustom OnGUI(Action onGUI);
    }

    internal class IMGUICustomView : IMGUIAbstractView, IMGUICustom
    {
        private Action mOnGUIAction { get; set; }

        protected override void OnGUI()
        {
            mOnGUIAction.Invoke();
        }

        public IMGUICustom OnGUI(Action onGUI)
        {
            mOnGUIAction = onGUI;
            return this;
        }

        public T Convert<T>(XmlNode node) where T : class
        {
            var custom = EasyIMGUI.Custom();

            foreach (XmlAttribute nodeAttribute in node.Attributes)
            {
                if (nodeAttribute.Name == "Id")
                {
                    custom.Id = nodeAttribute.Value;
                }
            }

            return custom as T;
        }
    }
}
#endif