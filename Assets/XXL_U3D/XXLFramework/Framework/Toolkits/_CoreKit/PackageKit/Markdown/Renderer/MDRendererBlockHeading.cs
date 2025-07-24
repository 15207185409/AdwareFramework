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
    internal class MDRendererBlockHeading : MarkdownObjectRenderer<MDRendererMarkdown, HeadingBlock>
    {
        protected override void Write(MDRendererMarkdown renderer, HeadingBlock block)
        {
            var prevStyle = renderer.Style.Size;
            renderer.Style.Size = block.Level;
            renderer.WriteLeafBlockInline(block);
            renderer.Style.Size = prevStyle;

            if (block.Level == 1)
            {
                renderer.Layout.HorizontalLine();
                renderer.FinishBlock(true);
            }
            else
            {
                renderer.FinishBlock();
            }
        }
    }
}
#endif