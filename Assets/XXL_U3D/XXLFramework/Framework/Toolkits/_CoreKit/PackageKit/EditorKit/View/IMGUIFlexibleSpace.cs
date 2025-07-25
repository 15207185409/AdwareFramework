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
    public interface IMGUIFlexibleSpace : IMGUIView, IXMLToObjectConverter
    {
    }

    internal class IMGUIFlexibleSpaceView : IMGUIAbstractView, IMGUIFlexibleSpace
    {
        protected override void OnGUI()
        {
            GUILayout.FlexibleSpace();
        }

        public T Convert<T>(XmlNode node) where T : class
        {
            var flexibleSpace = EasyIMGUI.FlexibleSpace();

            foreach (XmlAttribute childNodeAttribute in node.Attributes)
            {
                if (childNodeAttribute.Name == "Id")
                {
                    flexibleSpace.Id = childNodeAttribute.Value;
                }
            }

            return flexibleSpace as T;
        }
    }
}
#endif