/****************************************************************************
 * Copyright (c) 2015 - 2022  UNDER MIT License
 * 
 * http://XXLFramework.cn
 * https://github.com//XXLFramework
 * https://gitee.com//XXLFramework
 ****************************************************************************/

#if UNITY_EDITOR
using System.Xml;
using UnityEngine;

namespace XXLFramework
{
    public interface IMGUITextField : IMGUIView, IHasText<IMGUITextField>, IXMLToObjectConverter
    {
        BindableProperty<string> Content { get; }

        IMGUITextField PasswordMode();
    }

    internal class IMGUITextFieldView : IMGUIAbstractView, IMGUITextField
    {
        public IMGUITextFieldView()
        {
            Content = new BindableProperty<string>(string.Empty);

            mStyleProperty = new GUIStyleProperty(() => GUI.skin.textField);
        }

        public BindableProperty<string> Content { get; private set; }

        protected override void OnGUI()
        {
            if (mPasswordMode)
            {
                Content.Value = CrossPlatformGUILayout.PasswordField(Content.Value, Style.Value, LayoutStyles);
            }
            else
            {
                Content.Value = CrossPlatformGUILayout.TextField(Content.Value, Style.Value, LayoutStyles);
            }
        }


        private bool mPasswordMode = false;

        public IMGUITextField PasswordMode()
        {
            mPasswordMode = true;
            return this;
        }

        public IMGUITextField Text(string labelText)
        {
            Content.Value = labelText;
            return this;
        }

        public T Convert<T>(XmlNode node) where T : class
        {
            var textArea = EasyIMGUI.TextField();

            foreach (XmlAttribute nodeAttribute in node.Attributes)
            {
                if (nodeAttribute.Name == "Id")
                {
                    textArea.Id = nodeAttribute.Value;
                }
                else if (nodeAttribute.Name == "Text")
                {
                    textArea.Text(nodeAttribute.Value);
                }
            }

            return textArea as T;
        }
    }
}
#endif