/****************************************************************************
 * Copyright (c) 2015 ~ 2022  UNDER MIT LICENSE
 * 

 ****************************************************************************/

using System.IO;
using System.Text;

namespace XXLFramework
{
    public class FileCodeWriter : ICodeWriter
    {
        private readonly StreamWriter mWriter;

        public FileCodeWriter(StreamWriter writer)
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
            mWriter.WriteLine(Indent + format, args);
        }

        public void WriteLine(string code = null)
        {
            mWriter.WriteLine(Indent + code);
        }

        public void Dispose()
        {
            if (mWriter != null) mWriter.Dispose();
        }
    }
}