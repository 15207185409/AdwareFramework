using System;
using System.Collections.Generic;
using UnityEngine;

namespace XXLFramework
{
    public class UIKitTableIndex<TKeyType, TDataItem> : IDisposable
    {
        private Dictionary<TKeyType, TDataItem> dictionary = new Dictionary<TKeyType, TDataItem>();
        private Func<TDataItem, TKeyType> mGetKeyByDataItem = null;

        public UIKitTableIndex(Func<TDataItem, TKeyType> keyGetter)
        {
            mGetKeyByDataItem = keyGetter;
        }

        public int GetCount() 
        {
            return dictionary.Count;
        }

        public IDictionary<TKeyType, TDataItem> Dictionary
        {
            get { return dictionary; }
        }

        public bool Add(TDataItem dataItem)
        {
            var key = mGetKeyByDataItem(dataItem);
            if (!dictionary.ContainsKey(key))
            {
                dictionary.Add(key,dataItem);
                return true;
            }
            else
            {
                Debug.Log($"已存在{key}");
                return false;
            }
        }

        public bool Remove(TDataItem dataItem)
        {
            var key = mGetKeyByDataItem(dataItem);
            if (dictionary.ContainsKey(key))
            {
                dictionary.Remove(key);
                return true;
            }
            else
            {
                Debug.Log($"未找到{key.ToString()}");
                return false;
            }
        }

        public TDataItem Get(TKeyType key)
        {
            if (dictionary.ContainsKey(key))
            {
              return dictionary[key];
            }
            else
            {
                Debug.Log($"未找到{key.ToString()}");
                return default(TDataItem);
            }
        }

        public void Clear()
        {
            dictionary.Clear();
        }


        public void Dispose()
        {
            dictionary.Release2Pool();

            dictionary = null;
        }
    }
}