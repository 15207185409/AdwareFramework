using System;
using System.Collections;
using System.Collections.Generic;

namespace XXLFramework
{
    public abstract class UIKitTable<TDataItem> : IEnumerable<TDataItem>, IDisposable
    {
        public bool Add(TDataItem item)
        {
          return OnAdd(item);
        }

        public bool Remove(TDataItem item)
        {
          return OnRemove(item);
        }

        public void Clear()
        {
            OnClear();
        }

        protected abstract bool OnAdd(TDataItem item);
        protected abstract bool OnRemove(TDataItem item);

        protected abstract void OnClear();

        public abstract IEnumerator<TDataItem> GetEnumerator();

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        public void Dispose()
        {
            OnDispose();
        }

        protected abstract void OnDispose();
    }
}