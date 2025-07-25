/****************************************************************************
 * Copyright (c) 2015 - 2022  UNDER MIT License

 ****************************************************************************/

using System.Collections.Generic;

namespace XXLFramework
{
#if UNITY_EDITOR
    // v1 No.171
    [ClassAPI("06.PoolKit", "ListPool<T>", 1, "ListPool<T>")]
    [APIDescriptionCN("存储 List 对象池，用于优化减少 new 调用次数。")]
    [APIDescriptionEN("Store a pool of List objects for optimization to reduce the number of new calls.")]
    [APIExampleCode(@"

var names = ListPool<string>.Get()
names.Add(""Hello"");

names.Release2Pool();
// or ListPool<string>.Release(names);
")]
#endif

    public static class ListPool<T>
    {
        /// <summary>
        /// 栈对象：存储多个List
        /// </summary>
        static Stack<List<T>> mListStack = new Stack<List<T>>(8);

        /// <summary>
        /// 出栈：获取某个List对象
        /// </summary>
        /// <returns></returns>
        public static List<T> Get()
        {
            if (mListStack.Count == 0)
            {
                return new List<T>(8);
            }

            return mListStack.Pop();
        }

        /// <summary>
        /// 入栈：将List对象添加到栈中
        /// </summary>
        /// <param name="toRelease"></param>
        public static void Release(List<T> toRelease)
        {
            toRelease.Clear();
            mListStack.Push(toRelease);
        }
    }

    public static class ListPoolExtensions

    {
        public static void Release2Pool<T>(this List<T> self)
        {
            ListPool<T>.Release(self);
        }
    }
}