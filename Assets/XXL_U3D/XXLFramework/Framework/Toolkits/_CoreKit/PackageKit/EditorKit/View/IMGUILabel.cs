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
using UnityEngine;

namespace XXLFramework
{
    public interface IMGUILabel : IMGUIView, IHasText<IMGUILabel>, IHasTextGetter<IMGUILabel>, IXMLToObjectConverter
    {
    }

    internal class IMGUILabelView : IMGUIAbstractView, IMGUILabel
    {
        public string Content { get; set; }

        public IMGUILabelView()
        {
            mStyleProperty = new GUIStyleProperty(() => new GUIStyle(GUI.skin.label));
        }

        protected override void OnGUI()
        {
            GUILayout.Label(mTextGetter == null ? Content : mTextGetter(), Style.Value, LayoutStyles);
        }

        public IMGUILabel Text(string labelText)
        {
            Content = labelText;
            return this;
        }

        public IMGUILabel Text(Func<string> textGetter)
        {
            mTextGetter = textGetter;
            return this;
        }

        private Func<string> mTextGetter;

        public T Convert<T>(XmlNode node) where T : class
        {
            var label = EasyIMGUI.Label();

            foreach (XmlAttribute childNodeAttribute in node.Attributes)
            {
                if (childNodeAttribute.Name == "Id")
                {
                    label.Id = childNodeAttribute.Value;
                }
                else if (childNodeAttribute.Name == "Text")
                {
                    label.Text(childNodeAttribute.Value);
                }
                else if (childNodeAttribute.Name == "FontBold")
                {
                    label.FontBold();
                }
                else if (childNodeAttribute.Name == "FontSize")
                {
                    label.FontSize(int.Parse(childNodeAttribute.Value));
                }
            }

            return label as T;
        }
    }
}
#endif