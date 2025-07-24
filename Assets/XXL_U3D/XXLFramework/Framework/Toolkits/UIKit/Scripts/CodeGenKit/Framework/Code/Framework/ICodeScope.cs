/****************************************************************************
 * Copyright (c) 2015 ~ 2022  UNDER MIT LICENSE
 * 

 ****************************************************************************/

namespace XXLFramework
{
    using System.Collections.Generic;

    public interface ICodeScope : ICode
    {
        List<ICode> Codes { get; set; }
    }
}