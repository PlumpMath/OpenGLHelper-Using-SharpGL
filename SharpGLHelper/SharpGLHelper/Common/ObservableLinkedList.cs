using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Text;

namespace SharpGLHelper
{
    public class ObservableLinkedList<T> : INotifyCollectionChanged
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
                        CollectionChanged(this, new NotifyCollectionChangedEventArgs(action, newItems, null));
                        break;
                    case NotifyCollectionChangedAction.Remove:
                        CollectionChanged(this, new NotifyCollectionChangedEventArgs(action, null, oldItems));
                        break;
                    case NotifyCollectionChangedAction.Replace:
                        CollectionChanged(this, new NotifyCollectionChangedEventArgs(action, newItems, oldItems));
                        break;
                }
            }
        }
        #endregion events


        private LinkedList<T> _linkedList = new LinkedList<T>();

        public ObservableLinkedList(IEnumerable<T> items = null)
        {

        }

        public void AddLast(T item)
        {
            AddLast(new T[] { item });
        }
        public void AddLast(IEnumerable<T> items)
        {
            foreach (var item in items)
            {
                _linkedList.AddLast(item);
            }

            OnCollectionChanged(NotifyCollectionChangedAction.Add, items, null);
        }
        public void Remove(T item)
        {
            Remove(new T[] { item });
        }

        public void Remove(IEnumerable<T> items)
        {
            foreach (var item in items)
	        {
		        _linkedList.Remove(item);
	        }
            OnCollectionChanged(NotifyCollectionChangedAction.Remove, null, items);
        }
    }
}
