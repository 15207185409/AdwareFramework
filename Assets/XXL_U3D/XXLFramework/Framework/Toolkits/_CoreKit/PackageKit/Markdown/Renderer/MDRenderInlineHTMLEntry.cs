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
    internal class MDRendererInlineHtmlEntity : MarkdownObjectRenderer<MDRendererMarkdown, HtmlEntityInline>
    {
        protected override void Write(MDRendererMarkdown renderer, HtmlEntityInline node)
        {
            renderer.Text(node.Transcoded.ToString());
        }
    }
}
#endif