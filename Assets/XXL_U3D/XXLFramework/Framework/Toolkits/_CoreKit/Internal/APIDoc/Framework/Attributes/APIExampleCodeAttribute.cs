/****************************************************************************
 * Copyright (c) 2015 - 2022  UNDER MIT License
 * 

 ****************************************************************************/

using System;

namespace XXLFramework
{
    public class APIExampleCodeAttribute : Attribute
    {
        public string Code { get; private set; }

        public APIExampleCodeAttribute(string code)
        {
            Code = code;
        }
    }
}