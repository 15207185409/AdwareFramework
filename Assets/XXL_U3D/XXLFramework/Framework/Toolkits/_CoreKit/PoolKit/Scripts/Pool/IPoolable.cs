/****************************************************************************
 * Copyright (c) 2015 - 2022  UNDER MIT License
 * 

 ****************************************************************************/

namespace XXLFramework
{
    /// <summary>
    /// I pool able.
    /// </summary>
    public interface IPoolable
    {
        void OnRecycled();
        bool IsRecycled { get; set; }
    }
}