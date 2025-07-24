/****************************************************************************
 * Copyright (c) 2015 ~ 2022  UNDER MIT LICENSE
 * 

 ****************************************************************************/

namespace XXLFramework
{
    // 花括号
    public class OpenBraceCode : ICode
    {
        public void Gen(ICodeWriter writer)
        {
            writer.WriteLine("{");
        }
    }
}