using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SharpGLHelper
{
    public class ObservableLinkedSet<T> : ObservableLinkedList<T>
    {
        public ObservableLinkedSet(IEnumerable<T> items = null)
            :base(items)
        {
            CollectionChanged += ObservableLinkedSet_CollectionChanged;
        }

        void ObservableLinkedSet_CollectionChanged(object sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
        {
            // If any of the e.NewItems are already in the linkedList, then remove it.
            if (e.NewItems != null)
            {
                foreach (var item in e.NewItems)
                {
                    var firstItem = FindFirst((T) item);
                    var lastItem = FindLast((T) item);

                    if (firstItem != lastItem)
                        Remove(firstItem);
                }
            }
        }
    }
}
