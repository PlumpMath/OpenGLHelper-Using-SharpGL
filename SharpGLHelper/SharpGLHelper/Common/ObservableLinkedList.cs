using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Text;

namespace SharpGLHelper
{
    public class ObservableLinkedList<T> : INotifyCollectionChanged, IEnumerable<T>
    {
        #region events
        public event NotifyCollectionChangedEventHandler CollectionChanged;

        private void OnCollectionChanged(NotifyCollectionChangedAction action, T newItem, T oldItem)
        {
            if (CollectionChanged != null)
            {
                switch (action)
                {
                    case NotifyCollectionChangedAction.Add:
                        CollectionChanged(this, new NotifyCollectionChangedEventArgs(action, newItem, null));
                        break;
                    case NotifyCollectionChangedAction.Remove:
                        CollectionChanged(this, new NotifyCollectionChangedEventArgs(action, null, oldItem));
                        break;
                    case NotifyCollectionChangedAction.Replace:
                        CollectionChanged(this, new NotifyCollectionChangedEventArgs(action, newItem, oldItem));
                        break;
                }
            }
        }
        private void OnCollectionChanged(NotifyCollectionChangedAction action, IEnumerable<T> newItems, IEnumerable<T> oldItems)
        {   
            if (CollectionChanged != null)
            {
                switch (action)
                {
                    case NotifyCollectionChangedAction.Add:
                        CollectionChanged(this, new NotifyCollectionChangedEventArgs(action, newItems.ToList<T>()));
                        break;
                    case NotifyCollectionChangedAction.Remove:
                        CollectionChanged(this, new NotifyCollectionChangedEventArgs(action, oldItems.ToList<T>()));
                        break;
                    case NotifyCollectionChangedAction.Replace:
                        CollectionChanged(this, new NotifyCollectionChangedEventArgs(action, newItems.ToList<T>(), oldItems.ToList<T>()));
                        break;
                }
            }
        }
        #endregion events

        #region fields
        private LinkedList<T> _linkedList = new LinkedList<T>();
        #endregion fields

        #region properties
        public int Count { get { return _linkedList.Count; } }
        #endregion properties

        public ObservableLinkedList(IEnumerable<T> items = null, bool notify = true)
        {
            AddLast(items, notify);
        }

        public void AddLast(T item, bool notify = true)
        {
            AddLast(new T[] { item }, notify);
        }
        public void AddLast(IEnumerable<T> items, bool notify = true)
        {
            if (items == null)
                return;

            foreach (var item in items)
            {
                if (item != null)
                    _linkedList.AddLast(item);
            }
            
            if (notify)
                OnCollectionChanged(NotifyCollectionChangedAction.Add, items, null);
        }
        public void Remove(T item, bool notify = true)
        {
            Remove(new T[] { item }, notify);
        }

        public void Remove(IEnumerable<T> items, bool notify = true)
        {
            foreach (var item in items)
	        {
		        _linkedList.Remove(item);
	        }

            if (notify)
                OnCollectionChanged(NotifyCollectionChangedAction.Remove, null, items);
        }

        public void Remove(LinkedListNode<T> item, bool notify = true)
        {
            Remove(new LinkedListNode<T>[] { item }, notify);
        }

        public void Remove(IEnumerable<LinkedListNode<T>> items, bool notify = true)
        {
            foreach (var item in items)
            {
                _linkedList.Remove(item);
            }

            if (notify)
                OnCollectionChanged(NotifyCollectionChangedAction.Remove, null, items.Select<LinkedListNode<T>, T>(x => x.Value));
        }



        public void RemoveAt(int idx, bool notify = true)
        {
            RemoveAt(new int[] { idx }, notify);
        }

        public void RemoveAt(IEnumerable<int> idxs, bool notify = true)
        {
            var removedValues = new List<T>();

            var sortedIdxs = idxs.OrderBy(x=>x);

            var curIdxsPos = 0;
            var curIdx = idxs.ElementAt(curIdxsPos);

            var curNode = _linkedList.First;

            int i = 0;
            while(curNode != null)
            {
                if (curIdx == i)
                {
                    var node = curNode;

                    curNode = curNode.Next;

                    removedValues.Add(node.Value);
                    _linkedList.Remove(node);

                    
                    if (++curIdxsPos >= idxs.Count())
                        break; // Test if there are more idxs.
                    else
                    {
                        curIdx = idxs.ElementAt(curIdxsPos);
                    }
                }
                else
                {
                    curNode = curNode.Next;
                }

                i++;
            }

            if (notify)
                OnCollectionChanged(NotifyCollectionChangedAction.Remove, null, removedValues);
        }


        public void Replace(T currentItem, T newItem, bool notify = true)
        {
            var node = _linkedList.First;
            while (node == null)
            {
                if (node.Value == null && node.Value.Equals(currentItem))
                {
                    node.Value = newItem;
                    if (notify)
                        OnCollectionChanged(NotifyCollectionChangedAction.Replace, newItem, currentItem);
                    break;
                }

                node = node.Next;
	        }
        }

        public T ElementAt(int idx)
        {
            return _linkedList.ElementAt(idx);
        }

        public LinkedListNode<T> FindFirst(T element)
        {
            return _linkedList.Find(element);
        }
        public LinkedListNode<T> FindLast(T element)
        {
            return _linkedList.FindLast(element);
        }

        public void Clear(bool notify = true)
        {
            var allValues = _linkedList.ToList();

            _linkedList.Clear();

            if (notify)
                OnCollectionChanged(NotifyCollectionChangedAction.Remove, null, allValues);
        }

        public T this[int idx]
        {
            get { return ElementAt(idx); }
            set { Replace(this[idx], value); }
        }


        public IEnumerator<T> GetEnumerator()
        {
            return _linkedList.GetEnumerator();
        }


        System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
        {
            return _linkedList.GetEnumerator();
        }
    }
}
