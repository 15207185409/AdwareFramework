/****************************************************************************
 * Copyright (c) 2015 - 2022  UNDER MIT License
 * 
 * http://XXLFramework.cn
 * https://github.com//XXLFramework
 * https://gitee.com//XXLFramework
 ****************************************************************************/

using System.Xml;
using UnityEngine;

namespace XXLFramework
{
    public interface IMGUIRectBox : IMGUIView, IHasRect<IMGUIRectBox>
    {
        IMGUIRectBox Text(string text);
    }

    public class IMGUIRectBoxView : IMGUIAbstractView, IMGUIRectBox
    {
        public IMGUIRectBoxView()
        {
            mStyleProperty = new GUIStyleProperty(() => GUI.skin.box);
        }

        private Rect mRect = new Rect(0, 0, 200, 100);

        private string mText = string.Empty;

        protected override void OnGUI()
        {
            GUI.Box(mRect, mText, mStyleProperty.Value);
        }

        public IMGUIRectBox Text(string labelText)
        {
            mText = labelText;

            return this;
        }

        public T Convert<T>(XmlNode node) where T : class
        {
            throw new System.NotImplementedException();
        }

        public IMGUIRectBox Rect(Rect rect)
        {
            mRect = rect;
            return this;
        }

        public IMGUIRectBox Position(Vector2 position)
        {
            mRect.position = position;
            return this;
        }

        public IMGUIRectBox Position(float x, float y)
        {
            mRect.x = x;
            mRect.y = y;
            return this;
        }

        public IMGUIRectBox Size(float width, float height)
        {
            mRect.width = width;
            mRect.height = height;
            return this;
        }

        public IMGUIRectBox Size(Vector2 size)
        {
            mRect.size = size;
            return this;
        }

        public IMGUIRectBox Width(float width)
        {
            mRect.width = width;
            return this;
        }

        public IMGUIRectBox Height(float height)
        {
            mRect.height = height;
            return this;
        }
    }
}