/****************************************************************************
 * Copyright (c) 2015 - 2022  UNDER MIT License
 * 

 ****************************************************************************/

using System.Collections.Generic;

namespace XXLFramework
{
    public abstract class PoolableObject<T> where T : PoolableObject<T>, new()
    {
        private static Stack<T> mPool = new Stack<T>(10);

        protected bool mInPool = false;

        public static T Allocate()
        {
            var node = mPool.Count == 0 ? new T() : mPool.Pop();
            node.mInPool = false;
            return node;
        }

        public void Recycle2Cache()
        {
            OnRecycle();
            mInPool = true;
            mPool.Push(this as T);
        }

        protected abstract void OnRecycle();
    }
}