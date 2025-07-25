/****************************************************************************
 * Copyright (c) 2015 ~ 2022  UNDER MIT LICENSE
 * 

 ****************************************************************************/

namespace XXLFramework
{
    using System;

    public class CustomCodeScope : CodeScope
    {
        private string mFirstLine { get; set; }

        public CustomCodeScope(string firstLine)
        {
            mFirstLine = firstLine;
        }
        
        protected override void GenFirstLine(ICodeWriter codeWriter)
        {
            codeWriter.WriteLine(mFirstLine);
        }
    }
    
    public static partial class ICodeScopeExtensions
    {
        public static ICodeScope CustomScope(this ICodeScope self,string firstLine,bool semicolon, Action<CustomCodeScope> customCodeScopeSetting)
        {
            var custom = new CustomCodeScope(firstLine);
            custom.Semicolon = semicolon;
            customCodeScopeSetting(custom);
            self.Codes.Add(custom);
            return self;
        }
    }
}