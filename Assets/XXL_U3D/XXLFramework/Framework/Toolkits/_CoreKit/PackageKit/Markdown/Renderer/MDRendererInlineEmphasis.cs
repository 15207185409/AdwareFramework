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
    internal class MDRendererInlineEmphasis : MarkdownObjectRenderer<MDRendererMarkdown, EmphasisInline>
    {
        protected override void Write(MDRendererMarkdown renderer, EmphasisInline node)
        {
            bool prev = false;

            if (node.IsDouble)
            {
                prev = renderer.Style.Bold;
                renderer.Style.Bold = true;
            }
            else
            {
                prev = renderer.Style.Italic;
                renderer.Style.Italic = true;
            }

            renderer.WriteChildren(node);

            if (node.IsDouble)
            {
                renderer.Style.Bold = prev;
            }
            else
            {
                renderer.Style.Italic = prev;
            }
        }
    }
}
#endif