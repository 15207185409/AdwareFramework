/****************************************************************************
 * Copyright (c) 2015 ~ 2022  UNDER MIT LICENSE
 * 

 ****************************************************************************/

using System;

namespace XXLFramework
{
    public interface ICodeWriter : IDisposable
    {
        /// <summary>
        /// 缩进数量
        /// </summary>
        int IndentCount { get; set; }


        void WriteFormatLine(string format, params object[] args);
        void WriteLine(string code = null);
    }
}