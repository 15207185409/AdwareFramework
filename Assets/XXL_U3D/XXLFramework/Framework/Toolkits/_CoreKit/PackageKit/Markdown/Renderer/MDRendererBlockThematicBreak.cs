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
using Markdig.Renderers;
using Markdig.Syntax;

namespace XXLFramework
{
    internal class MDRendererBlockThematicBreak : MarkdownObjectRenderer<MDRendererMarkdown, ThematicBreakBlock>
    {
        protected override void Write(MDRendererMarkdown renderer, ThematicBreakBlock block)
        {
            renderer.Layout.HorizontalLine();
            renderer.FinishBlock();
        }
    }
}
#endif