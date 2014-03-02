using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;

namespace UniTimetable
{
    class History<T>
    {
        int Capacity_;
        T[] Items_;
        int Count_;
        int Current_;

        public History(int capacity)
        {
            Capacity_ = capacity;
            Clear();
        }

        public void Clear()
        {
            Items_ = new T[Capacity_];
            Count_ = 0;
            Current_ = -1;
        }

        public T Back()
        {
            // if we're at the start, return null
            if (Current_ <= 0)
                return default(T);
            // step the index back and return the item
            return Items_[--Current_];
        }

        public T Forward()
        {
            // if we're at the end already
            if (Current_ >= Count_ - 1 || Current_ < 0)
                return default(T);
            // return the next item and step up the index
            return Items_[++Current_];
        }

        public T Current()
        {
            // if there's nothing yet
            if (Current_ < 0)
                return default(T);
            // return the current item
            return Items_[Current_];
        }

        public T PeekBack()
        {
            // if we're at the start, return null
            if (Current_ <= 0)
                return default(T);
            // return the previous item
            return Items_[Current_ - 1];
        }

        public T PeekForward()
        {
            // if we're at the end already
            if (Current_ >= Count_ - 1 || Current_ < 0)
                return default(T);
            // return the next item
            return Items_[Current_ + 1];
        }

        public void Add(T item)
        {
            // if we're full to capacity
            if (Current_ == Capacity_ - 1)
            {
                // shift all items in the array back 1
                for (int i = 0; i < Capacity_ - 1; i++)
                    Items_[i] = Items_[i + 1];
            }
            else
            {
                Current_++;
            }

            // insert the item at the next location
            Items_[Current_] = item;
            // fix the item count
            Count_ = Current_ + 1;
        }
    }
}
