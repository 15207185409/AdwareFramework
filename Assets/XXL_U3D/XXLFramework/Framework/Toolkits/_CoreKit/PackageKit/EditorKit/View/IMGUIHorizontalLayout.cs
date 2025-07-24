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
    public interface IMGUIHorizontalLayout : IMGUILayout, IXMLToObjectConverter
    {
        IMGUIHorizontalLayout Box();
    }

    internal class HorizontalLayout : IMGUIAbstractLayout, IMGUIHorizontalLayout
    {
        public string HorizontalStyle { get; set; }


        protected override void OnGUIBegin()
        {
            if (string.IsNullOrEmpty(HorizontalStyle))
            {
                GUILayout.BeginHorizontal();
            }
            else
            {
                GUILayout.BeginHorizontal(HorizontalStyle);
            }
        }

        protected override void OnGUIEnd()
        {
            GUILayout.EndHorizontal();
        }

        public IMGUIHorizontalLayout Box()
        {
            HorizontalStyle = "box";
            return this;
        }

        public T Convert<T>(XmlNode node) where T : class
        {
            var horizontal = EasyIMGUI.Horizontal();

            foreach (XmlAttribute childNodeAttribute in node.Attributes)
            {
                if (childNodeAttribute.Name == "Id")
                {
                    horizontal.Id = childNodeAttribute.Value;
                }
            }

            return horizontal as T;
        }
    }
}
#endif