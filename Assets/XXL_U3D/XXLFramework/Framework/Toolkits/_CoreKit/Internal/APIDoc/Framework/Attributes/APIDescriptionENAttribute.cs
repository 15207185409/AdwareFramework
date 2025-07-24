/****************************************************************************
 * Copyright (c) 2015 - 2022  UNDER MIT License
 * 

 ****************************************************************************/

using System;

namespace XXLFramework
{
    public class APIDescriptionENAttribute : Attribute
    {
        public string Description { get; private set; }

        public APIDescriptionENAttribute(string description)
        {
            Description = description;
        }
        
    }
}