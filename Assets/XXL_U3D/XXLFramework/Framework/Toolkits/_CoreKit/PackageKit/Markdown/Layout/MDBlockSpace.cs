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
    internal class MDBlockSpace : MDBlock
    {
        public MDBlockSpace(float indent) : base(indent)
        {
        }

        public override void Draw(MDContext context)
        {
        }

        public override void Arrange(MDContext context, Vector2 pos, float maxWidth)
        {
            Rect.position = pos;
            Rect.width = 1.0f;
            Rect.height = context.LineHeight * 0.75f;
        }
    }
}
#endif