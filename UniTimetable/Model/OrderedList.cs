#region

using System;
using System.Collections.Generic;

#endregion

namespace UniTimetable.Model
{
    internal class OrderedList<T> : List<T>, ICloneable where T : IComparable<T>
    {
        public OrderedList()
        {
        }

        public OrderedList(IEnumerable<T> collection)
            : base(collection)
        {
        }

        public OrderedList(int capacity)
            : base(capacity)
        {
        }

        #region ICloneable Members

        object ICloneable.Clone()
        {
            return Clone();
        }

        #endregion

        public new void Add(T item)
        {
            // add the first item
            if (Count == 0)
                base.Add(item);
            // insert following items
            Add(item, 0, Count - 1);
        }

        private void Add(T item, int start, int end)
        {
            if (start >= end)
            {
                // go to the left or right?
                if (CompareItems(item, base[start]) < 0)
                {
                    Insert(start, item);
                }
                else
                {
                    Insert(start + 1, item);
                }
                return;
            }

            int middle = (start + end)/2;
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

        public virtual OrderedList<T> Clone()
        {
            return new OrderedList<T>(this);
        }
    }
}