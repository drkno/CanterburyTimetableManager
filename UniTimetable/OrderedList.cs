using System;
using System.Collections.Generic;
using System.Text;

namespace UniTimetable
{
    class OrderedList<T> : List<T>, ICloneable where T : IComparable<T>
    {
        public OrderedList()
            : base() { }

        public OrderedList(IEnumerable<T> collection)
            : base(collection) { }

        public OrderedList(int capacity)
            : base(capacity) { }

        public new void Add(T item)
        {
            // add the first item
            if (base.Count == 0)
                base.Add(item);
            // insert following items
            Add(item, 0, base.Count - 1);
        }

        private void Add(T item, int start, int end)
        {
            if (start >= end)
            {
                // go to the left or right?
                if (CompareItems(item, base[start]) < 0)
                {
                    base.Insert(start, item);
                }
                else
                {
                    base.Insert(start + 1, item);
                }
                return;
            }

            int middle = (start + end) / 2;
            // check which side of the middle to go to
            if (CompareItems(item, base[middle]) < 0)
            {
                // item belongs to the left of centre
                Add(item, start, middle - 1);
            }
            else
            {
                // item belongs to the right of centre
                Add(item, middle + 1, end);
            }
        }

        private int CompareItems(T item1, T item2)
        {
            return item1.CompareTo(item2);
        }

        #region ICloneable Members

        object ICloneable.Clone()
        {
            return this.Clone();
        }

        #endregion

        public virtual OrderedList<T> Clone()
        {
            return new OrderedList<T>(this);
        }
    }
}
