/****************************************************************************
 * Copyright (c) 2015 ~ 2022  UNDER MIT LICENSE
 * 

 ****************************************************************************/

namespace XXLFramework
{
    using System.Collections.Generic;

    public abstract class CodeScope : ICodeScope
    {
        public bool Semicolon { get; set; }

        public void Gen(ICodeWriter writer)
        {
            GenFirstLine(writer);

            new OpenBraceCode().Gen(writer);

            writer.IndentCount++;

            foreach (var code in Codes)
            {
                code.Gen(writer);
            }

            writer.IndentCount--;

            new CloseBraceCode(Semicolon).Gen(writer);
        }

        protected abstract void GenFirstLine(ICodeWriter codeWriter);

        private List<ICode> mCodes = new List<ICode>();

        public List<ICode> Codes
        {
            get { return mCodes; }
            set { mCodes = value; }
        }
    }
}