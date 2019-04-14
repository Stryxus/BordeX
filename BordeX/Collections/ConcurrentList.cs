using System.Collections.Generic;

namespace System.Collections.Concurrent
{
    public class ConcurrentList<T> : IList<T>
    {
        private List<T> publicList;

        private readonly object lockList = new object();

        public ConcurrentList()
        {
            publicList = new List<T>();
        }

        public IEnumerator<T> GetEnumerator()
        {
            return Clone().GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return Clone().GetEnumerator();
        }

        public List<T> Clone()
        {
            lock (lockList)
            {
                return new List<T>(publicList);
            }
        }

        public void Add(T item)
        {
            lock (lockList)
            {
                publicList.Add(item);
            }
        }

        public bool Remove(T item)
        {
            bool isRemoved;

            lock (lockList)
            {
                isRemoved = publicList.Remove(item);
            }

            return (isRemoved);
        }

        public void Clear()
        {
            lock (lockList)
            {
                publicList.Clear();
            }
        }

        public bool Contains(T item)
        {
            bool containsItem;

            lock (lockList)
            {
                containsItem = publicList.Contains(item);
            }

            return (containsItem);
        }

        public void CopyTo(T[] array, int arrayIndex)
        {
            lock (lockList)
            {
                publicList.CopyTo(array, arrayIndex);
            }
        }

        public int Count
        {
            get
            {
                int count;

                lock ((lockList))
                {
                    count = publicList.Count;
                }

                return (count);
            }
        }

        public bool IsReadOnly
        {
            get { return false; }
        }

        public int IndexOf(T item)
        {
            int itemIndex;

            lock ((lockList))
            {
                itemIndex = publicList.IndexOf(item);
            }

            return (itemIndex);
        }

        public void Insert(int index, T item)
        {
            lock ((lockList))
            {
                publicList.Insert(index, item);
            }
        }

        public void RemoveAt(int index)
        {
            lock ((lockList))
            {
                publicList.RemoveAt(index);
            }
        }

        public T this[int index]
        {
            get
            {
                lock ((lockList))
                {
                    return publicList[index];
                }
            }
            set
            {
                lock ((lockList))
                {
                    publicList[index] = value;
                }
            }
        }
    }
}