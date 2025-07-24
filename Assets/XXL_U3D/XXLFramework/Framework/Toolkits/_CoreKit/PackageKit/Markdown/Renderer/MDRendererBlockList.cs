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
    internal class MDRendererBlockList : MarkdownObjectRenderer<MDRendererMarkdown, ListBlock>
    {
        protected override void Write(MDRendererMarkdown renderer, ListBlock block)
        {
            var layout = renderer.Layout;

            layout.Space();
            layout.Indent();

            var prevImplicit = renderer.ConsumeSpace;
            renderer.ConsumeSpace = true;

            var prefixStyle = renderer.Style;

            if (!block.IsOrdered)
            {
                prefixStyle.Bold = true;
            }

            for (var i = 0; i < block.Count; i++)
            {
                layout.Prefix(block.IsOrdered ? (i + 1).ToString() + "." : "\u2022", prefixStyle);
                renderer.WriteChildren(block[i] as ListItemBlock);
            }

            renderer.ConsumeSpace = prevImplicit;
            layout.Outdent();
            layout.Space();
        }
    }
}
#endif