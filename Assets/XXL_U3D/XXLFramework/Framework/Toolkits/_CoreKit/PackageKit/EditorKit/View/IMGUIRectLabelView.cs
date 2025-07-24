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
    public interface IMGUIRectLabel : IMGUIView, IHasText<IMGUIRectLabel>, IXMLToObjectConverter,
        IHasRect<IMGUIRectLabel>
    {
    }

    public class IMGUIRectLabelView : IMGUIAbstractView, IMGUIRectLabel
    {
        public IMGUIRectLabelView()
        {
            mStyleProperty = new GUIStyleProperty(() => new GUIStyle(GUI.skin.label));
        }

        private string mText = string.Empty;
        private Rect mRect = new Rect(0, 0, 200, 100);

        protected override void OnGUI()
        {
            GUI.Label(mRect, mText, mStyleProperty.Value);
        }

        public IMGUIRectLabel Text(string labelText)
        {
            mText = labelText;

            return this;
        }

        public T Convert<T>(XmlNode node) where T : class
        {
            throw new System.NotImplementedException();
        }

        public IMGUIRectLabel Rect(Rect rect)
        {
            mRect = rect;
            return this;
        }

        public IMGUIRectLabel Position(Vector2 position)
        {
            mRect.position = position;
            return this;
        }

        public IMGUIRectLabel Position(float x, float y)
        {
            mRect.x = x;
            mRect.y = y;
            return this;
        }

        public IMGUIRectLabel Size(float width, float height)
        {
            mRect.width = width;
            mRect.height = height;
            return this;
        }

        public IMGUIRectLabel Size(Vector2 size)
        {
            mRect.size = size;
            return this;
        }

        public IMGUIRectLabel Width(float width)
        {
            mRect.width = width;
            return this;
        }

        public IMGUIRectLabel Height(float height)
        {
            mRect.height = height;
            return this;
        }
    }
}
#endif