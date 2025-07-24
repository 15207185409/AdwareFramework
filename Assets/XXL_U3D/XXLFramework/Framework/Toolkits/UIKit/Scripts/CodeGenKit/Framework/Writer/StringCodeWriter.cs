/****************************************************************************
 * Copyright (c) 2015 ~ 2022  UNDER MIT LICENSE
 * 

 ****************************************************************************/

using System.Text;

namespace XXLFramework
{
    public class StringCodeWriter : ICodeWriter
    {
        private readonly StringBuilder mWriter;

        public StringCodeWriter(StringBuilder writer)
        {
            mWriter = writer;
        }

        public int IndentCount { get; set; }

        private string Indent
        {
            get
            {
                var builder = new StringBuilder();

                for (var i = 0; i < IndentCount; i++)
                {
                    builder.Append("\t");
                }

                return builder.ToString();
            }
        }

        public void WriteFormatLine(string format, params object[] args)
        {
            mWriter.AppendFormat(Indent + format, args).AppendLine();
        }

        public void WriteLine(string code = null)
        {
            mWriter.AppendLine(Indent + code);
        }

        public void Dispose()
        {
            if (mWriter != null) mWriter.Clear();
        }
    }
}