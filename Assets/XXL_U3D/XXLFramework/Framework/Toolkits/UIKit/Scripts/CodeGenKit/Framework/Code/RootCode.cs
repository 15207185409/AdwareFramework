/****************************************************************************
 * Copyright (c) 2015 ~ 2022  UNDER MIT LICENSE
 * 

 ****************************************************************************/

using System.Collections.Generic;

namespace XXLFramework
{
    public class RootCode : ICodeScope
    {
        private List<ICode> mCodes = new List<ICode>();

        public List<ICode> Codes
        {
            get { return mCodes; }
            set { mCodes = value; }
        }


        public void Gen(ICodeWriter writer)
        {
            foreach (var code in Codes)
            {
                code.Gen(writer);
            }
        }
    }
}