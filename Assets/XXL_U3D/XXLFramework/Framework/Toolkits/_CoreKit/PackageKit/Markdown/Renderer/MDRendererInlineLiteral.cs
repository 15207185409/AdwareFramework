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
using Markdig.Syntax.Inlines;

namespace XXLFramework
{
    internal class MDRendererInlineLiteral : MarkdownObjectRenderer<MDRendererMarkdown, LiteralInline>
    {
        protected override void Write(MDRendererMarkdown renderer, LiteralInline node)
        {
            renderer.Text(node.Content.ToString());
        }
    }
}
#endif