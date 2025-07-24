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
    public interface IMGUIVerticalLayout : IMGUILayout, IXMLToObjectConverter
    {
        IMGUIVerticalLayout Box();
    }

    public class VerticalLayout : IMGUIAbstractLayout, IMGUIVerticalLayout
    {
        public VerticalLayout()
        {
        }

        public string VerticalStyle { get; set; }

        public VerticalLayout(string verticalStyle = null)
        {
            VerticalStyle = verticalStyle;
        }

        protected override void OnGUIBegin()
        {
            if (string.IsNullOrEmpty(VerticalStyle))
            {
                GUILayout.BeginVertical(LayoutStyles);
            }
            else
            {
                GUILayout.BeginVertical(VerticalStyle, LayoutStyles);
            }
        }

        protected override void OnGUIEnd()
        {
            GUILayout.EndVertical();
        }

        public IMGUIVerticalLayout Box()
        {
            VerticalStyle = "box";
            return this;
        }


        public T Convert<T>(XmlNode node) where T : class
        {
            var scroll = EasyIMGUI.Vertical();

            foreach (XmlAttribute childNodeAttribute in node.Attributes)
            {
                if (childNodeAttribute.Name == "Id")
                {
                    scroll.Id = childNodeAttribute.Value;
                }
            }

            return scroll as T;
        }
    }
}
#endif