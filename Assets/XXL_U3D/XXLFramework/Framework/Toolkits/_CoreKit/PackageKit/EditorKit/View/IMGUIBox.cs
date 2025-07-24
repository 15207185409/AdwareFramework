/****************************************************************************
 * Copyright (c) 2015 - 2022  UNDER MIT License
 * 
 * http://XXLFramework.cn
 * https://github.com//XXLFramework
 * https://gitee.com//XXLFramework
 ****************************************************************************/

#if UNITY_EDITOR
using UnityEngine;

namespace XXLFramework
{
    public interface IMGUIBox : IMGUIView, IHasText<IMGUIBox>
    {
    }

    public class IMGUIBoxView : IMGUIAbstractView, IMGUIBox
    {
        public IMGUIBoxView()
        {
            mStyleProperty = new GUIStyleProperty(() =>
            {
                // Box 的颜色保持和文本的颜色一致
                var boxStyle = new GUIStyle(GUI.skin.box) { normal = { textColor = GUI.skin.label.normal.textColor } };
                return boxStyle;
            });
        }

        protected override void OnGUI()
        {
            GUILayout.Box(mText, mStyleProperty.Value, LayoutStyles);
        }

        private string mText = string.Empty;

        public IMGUIBox Text(string labelText)
        {
            mText = labelText;
            return this;
        }
    }
}
#endif