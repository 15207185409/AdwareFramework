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
using System;
using UnityEngine;

namespace XXLFramework
{
    internal abstract class MDBlock
    {
        public string ID = null;
        public Rect Rect = new Rect();
        public MDBlock Parent = null;
        public float Indent = 0.0f;

        public abstract void Arrange(MDContext context, Vector2 anchor, float maxWidth);
        public abstract void Draw(MDContext context);

        public MDBlock(float indent)
        {
            Indent = indent;
        }

        public virtual MDBlock Find(string id)
        {
            return id.Equals(ID, StringComparison.Ordinal) ? this : null;
        }
    }
}
#endif