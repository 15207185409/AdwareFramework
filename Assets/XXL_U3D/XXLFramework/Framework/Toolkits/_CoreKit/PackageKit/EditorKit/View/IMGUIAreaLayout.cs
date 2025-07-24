/****************************************************************************
 * Copyright (c) 2015 - 2022  UNDER MIT License
 * 
 * http://XXLFramework.cn
 * https://github.com//XXLFramework
 * https://gitee.com//XXLFramework
 ****************************************************************************/

using System;
using UnityEngine;

namespace XXLFramework
{
    public interface IMGUIAreaLayout : IMGUILayout
    {
        IMGUIAreaLayout WithRect(Rect rect);
        IMGUIAreaLayout WithRectGetter(Func<Rect> rectGetter);
    }

    public class IMGUIAreaLayoutView : IMGUIAbstractLayout, IMGUIAreaLayout
    {
        private Rect mRect;
        private Func<Rect> mRectGetter;

        public IMGUIAreaLayout WithRect(Rect rect)
        {
            mRect = rect;
            return this;
        }

        public IMGUIAreaLayout WithRectGetter(Func<Rect> rectGetter)
        {
            mRectGetter = rectGetter;
            return this;
        }

        protected override void OnGUIBegin()
        {
            GUILayout.BeginArea(mRectGetter == null ? mRect : mRectGetter());
        }

        protected override void OnGUIEnd()
        {
            GUILayout.EndArea();
        }
    }
}