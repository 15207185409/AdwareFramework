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
    internal class MDRendererInlineLink : MarkdownObjectRenderer<MDRendererMarkdown, LinkInline>
    {
        protected override void Write(MDRendererMarkdown renderer, LinkInline node)
        {
            var url = node.GetDynamicUrl != null ? node.GetDynamicUrl() : node.Url;

            if (node.IsImage)
            {
                renderer.Layout.Image(url, renderer.GetContents(node), node.Title);
            }
            else
            {
                renderer.Link = url;

                if (string.IsNullOrEmpty(node.Title) == false)
                {
                    renderer.ToolTip = node.Title;
                }

                renderer.WriteChildren(node);

                renderer.ToolTip = null;
                renderer.Link = null;
            }
        }
    }
}
#endif