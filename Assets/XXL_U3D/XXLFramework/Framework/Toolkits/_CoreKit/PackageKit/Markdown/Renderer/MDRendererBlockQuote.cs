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
    ////////////////////////////////////////////////////////////////////////////////
    // <blockquote>...</blockquote>
    /// <see cref="Markdig.Renderers.Html.QuoteBlockRenderer"/>
    internal class MDRendererBlockQuote : MarkdownObjectRenderer<MDRendererMarkdown, QuoteBlock>
    {
        protected override void Write(MDRendererMarkdown renderer, QuoteBlock block)
        {
            var prevImplicit = renderer.ConsumeSpace;
            renderer.ConsumeSpace = false;

            renderer.Layout.StartBlock(true);
            renderer.WriteChildren(block);
            renderer.Layout.EndBlock();

            renderer.ConsumeSpace = prevImplicit;

            renderer.FinishBlock(true);
        }
    }
}
#endif