/****************************************************************************
 * Copyright (c) 2015 - 2022  UNDER MIT License
 * 

 ****************************************************************************/

using System;

namespace XXLFramework
{
    public class APIDescriptionCNAttribute : Attribute
    {
        public string Description { get; private set; }

        public APIDescriptionCNAttribute(string description)
        {
            Description = description;
        }
    }
}