/****************************************************************************
 * Copyright (c) 2015 ~ 2022  UNDER MIT LICENSE
 * 

 ****************************************************************************/

namespace XXLFramework
{
    /// <summary>
    /// 后花括号
    /// </summary>
    public class CloseBraceCode : ICode
    {
        private readonly bool mSemicolon;

        public CloseBraceCode(bool semicolon)
        {
            mSemicolon = semicolon;
        }

        public void Gen(ICodeWriter writer)
        {
            var semicolonKey = mSemicolon ? ";" : string.Empty;
            writer.WriteLine("}" + semicolonKey);
        }
    }
}