/****************************************************************************
 * Copyright (c) 2015 ~ 2022  UNDER MIT License
 *
 * https://github.com//XXLFramework
 * https://gitee.com//XXLFramework
 ****************************************************************************/

#if UNITY_2018_4
using UnityEngine.Experimental.UIElements;
#else
using UnityEngine.UIElements;
#endif

#if UNITY_2018_4_OR_NEWER
namespace XXLFramework
{
    public static class UIElementExtensions
    {
        public static T Padding<T>(this T self, float top, float right, float bottom,
            float left) where T : VisualElement
        {
            self.style.paddingTop = top;
            self.style.paddingRight = right;
            self.style.paddingBottom = bottom;
            self.style.paddingLeft = left;
            return self;
        }

        public static T AddChild<T>(this T self, VisualElement child) where T : VisualElement
        {
            self.Add(child);
            return self;
        }

        public static T AddToParent<T>(this T self, VisualElement parent) where T : VisualElement
        {
            parent.AddChild(self);
            return self;
        }
    }
}
#endif