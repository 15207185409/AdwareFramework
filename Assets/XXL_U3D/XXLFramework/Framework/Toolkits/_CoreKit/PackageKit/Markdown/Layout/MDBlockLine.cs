/****************************************************************************
 * Copyright (c) 2019 Gwaredd Mountain UNDER MIT License
 * Copyright (c) 2022  UNDER MIT License
 *
 * https://github.com/gwaredd/UnityMarkdownViewer
 * http://XXLFramework.cn
 * https://github.com//XXLFramework
 * https://gitee.com//XXLFramework
 ****************************************************************************/

#if UNITY_EDITOR
using UnityEngine;

namespace XXLFramework
{
    internal class MDBlockLine : MDBlock
    {
        public MDBlockLine(float indent) : base(indent)
        {
        }

        public override void Draw(MDContext context)
        {
            var rect = new Rect(Rect.position.x, Rect.center.y, Rect.width, 1.0f);
            GUI.Label(rect, string.Empty, GUI.skin.GetStyle("hr"));
        }

        public override void Arrange(MDContext context, Vector2 pos, float maxWidth)
        {
            Rect.position = pos;
            Rect.width = maxWidth;
            Rect.height = 10.0f;
        }
    }
}
#endif