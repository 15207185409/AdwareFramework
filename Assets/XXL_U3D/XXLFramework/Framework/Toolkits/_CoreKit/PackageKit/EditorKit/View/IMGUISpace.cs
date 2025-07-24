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
    public interface IMGUISpace : IMGUIView, IXMLToObjectConverter
    {
        IMGUISpace Pixel(int pixel);
    }

    internal class IMGUISpaceView : IMGUIAbstractView, IMGUISpace
    {
        private int mPixel = 10;

        protected override void OnGUI()
        {
            GUILayout.Space(mPixel);
        }

        public IMGUISpace Pixel(int pixel)
        {
            mPixel = pixel;
            return this;
        }

        public T Convert<T>(XmlNode node) where T : class
        {
            var space = EasyIMGUI.Space();

            foreach (XmlAttribute nodeAttribute in node.Attributes)
            {
                if (nodeAttribute.Name == "Id")
                {
                    space.Id = nodeAttribute.Value;
                }
                else if (nodeAttribute.Name == "Pixel")
                {
                    space.Pixel(int.Parse(nodeAttribute.Value));
                }
            }

            return space as T;
        }
    }
}
#endif