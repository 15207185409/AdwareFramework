/****************************************************************************
 * Copyright (c) 2015 - 2022  UNDER MIT License
 * 
 ****************************************************************************/

namespace XXLFramework
{
    /// <summary>
    /// 单例接口
    /// </summary>
    public interface ISingleton
    {
        /// <summary>
        /// 单例初始化(继承当前接口的类都需要实现该方法)
        /// </summary>
        void OnSingletonInit();
    }
}