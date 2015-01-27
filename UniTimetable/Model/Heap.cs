#region

using System;
using System.Collections.Generic;

#endregion

namespace UniTimetable.Model
{
    // Heap implementation. Concepts/code based on
    // http://en.wikipedia.org/wiki/Heapsort

    internal class HeapBase<T>
    {
        private readonly T[] Heap_;
        private int MaxLength_;
        private int N_;

        protected virtual int CompareItems(T item1, T item2)
        {
            return 0;
        }

        public override string ToString()
        {
            string text = "";
            int nextLine = 1;
            for (int i = 1; i <= N_; i++)
            {
                text += Heap_[i] + " ";
                if (i == nextLine)
                {
                    text += "\n";
                    nextLine = nextLine*2 + 1;
                }
            }
            return text;
        }

        #region Heap operations

        /// <summary>
        ///     Constructs an empty heap.
        /// </summary>
        /// <param name="maxLength"></param>
        public HeapBase(int maxLength)
        {
            MaxLength_ = maxLength;
            Heap_ = new T[maxLength + 1];
            N_ = 0;
        }

        /// <summary>
        ///     Builds a heap from an existing list in time O(N).
        /// </summary>
        /// <param name="maxLength"></param>
        /// <param name="unordered"></param>
        public HeapBase(int maxLength, IEnumerable<T> unordered)
        {
            MaxLength_ = maxLength;
            Heap_ = new T[maxLength + 1];
            N_ = 0;

            // copy the elements into the array
            foreach (T item in unordered)
            {
                if (item == null)
                    break;
                Heap_[++N_] = item;
            }
        }

        /// <summary>
        ///     Finds the maximum element in the heap in time O(1).
        /// </summary>
        /// <returns>The greatest element.</returns>
        public T FindMaximum()
        {
            // get the top value
            return Heap_[1];
        }

        /// <summary>
        ///     Replaces the maximum element with a new one in time O(lg N).
        /// </summary>
        /// <param name="item"></param>
        /// <returns>The greatest element.</returns>
        public T RemoveMaximumAndAdd(T item)
        {
            // get the top value
            T max = Heap_[1];
            // insert the new value
            Heap_[1] = item;
            // and send it to its place
            FixDown(1);

            return max;
        }

        /// <summary>
        ///     Replaces the maximum element if the new element is less. Takes time O(lg N).
        /// </summary>
        /// <param name="item"></param>
        /// <returns>The larger element.</returns>
        public T CompareReplaceMaximum(T item)
        {
            // get the top value
            T max = Heap_[1];
            // if new item is less than the max
            if (CompareItems(item, max) < 0)
            {
                // insert the new item and fix the heap
                Heap_[1] = item;
                FixDown(1);
                // return the old max
                return max;
            }
            // keep the old max and return the new item
            return item;
        }

        /// <summary>
        ///     Removes the maximum element from the heap in time O(lg N).
        /// </summary>
        /// <param name="item"></param>
        /// <returns>The greatest element.</returns>
        public T RemoveMaximum()
        {
            // get the top value
            T max = Heap_[1];
            // bring in the bottom value, then decrease count
            Heap_[1] = Heap_[N_--];
            // and take it back down to the bottom
            FixDown(1);

            return max;
        }

        /// <summary>
        ///     Adds a new element to the heap in time O(lg N).
        /// </summary>
        /// <param name="item"></param>
        public void Add(T item)
        {
            // insert the new value at the end
            Heap_[++N_] = item;
            // and send it to its place
            FixUp(N_);
        }

        /// <summary>
        ///     Sorts the elements in the heap in ascending order. Worst case time O(N lg N).
        /// </summary>
        /// <returns></returns>
        public T[] GetSorted()
        {
            // copy the heap to the output array
            T[] ordered = new T[N_];
            Array.Copy(Heap_, 1, ordered, 0, N_);

            // keep sending the max to the back and reducing the size
            int end = N_ - 1;
            while (end > 0)
            {
                // take the greatest element to the back
                Exch(ref ordered[0], ref ordered[end]);
                // reduce the index for the end element
                end--;
                // bring up the next max and send back the end element
                FixDown(ordered, 0, 0, end);
            }

            return ordered;
        }

        #endregion

        #region Private manipulation methods

        protected void Heapify()
        {
            // FixDown all nodes, bottom-up
            // start at the last node which has any children
            for (int start = N_/2; start >= 1; start--)
            {
                FixDown(start);
            }
        }

        private void FixDown(int node)
        {
            int child1 = node*2;
            int child2 = child1 + 1;

            // already reached end
            if (child1 > N_)
                return;

            // child1 is the only child
            if (child2 > N_)
            {
                // if node is less than only child, switch them
                if (CompareItems(Heap_[node], Heap_[child1]) < 0)
                    Exch(ref Heap_[node], ref Heap_[child1]);
                return;
            }

            // node has 2 children - find largest child
            int largest;
            if (CompareItems(Heap_[child1], Heap_[child2]) > 0)
                largest = child1;
            else
                largest = child2;
            // check if the node is less than that child
            if (CompareItems(Heap_[node], Heap_[largest]) < 0)
            {
                // switch the node for its child
                Exch(ref Heap_[node], ref Heap_[largest]);
                // and apply recursively
                FixDown(largest);
            }
        }

        private void FixDown(T[] heap, int node, int start, int end)
        {
            int child1 = (node - start + 1)*2 + start - 1;
            int child2 = child1 + 1;

            // already reached end
            if (child1 > end)
                return;

            // child1 is the only child
            if (child2 > end)
            {
                // if node is less than only child, switch them
                if (CompareItems(heap[node], heap[child1]) < 0)
                    Exch(ref heap[node], ref heap[child1]);
                return;
            }

            // node has 2 children - find largest child
            int largest;
            if (CompareItems(heap[child1], heap[child2]) > 0)
                largest = child1;
            else
                largest = child2;
            // check if the node is less than that child
            if (CompareItems(heap[node], heap[largest]) < 0)
            {
                // switch the node for its child
                Exch(ref heap[node], ref heap[largest]);
                // and apply recursively
                FixDown(heap, largest, start, end);
            }
        }

        private void FixUp(int node)
        {
            // if the node is at the top already, finished
            if (node <= 1)
                return;
            // find parent
            int parent = node/2;
            // if the parent is less than the node
            //if (Heap_[parent] < Heap_[node])
            if (CompareItems(Heap_[parent], Heap_[node]) < 0)
            {
                // switch the node for its parent
                Exch(ref Heap_[parent], ref Heap_[node]);
                // fix recursively
                FixUp(parent);
            }
        }

        private void FixUp(T[] heap, int node, int start, int end)
        {
            // if the node is at the top already, finished
            if (node <= start)
                return;
            // find parent
            int parent = (node - start + 1)/2 + start - 1;
            // if the parent is less than the node
            //if (Heap_[parent] < Heap_[node])
            if (CompareItems(heap[parent], heap[node]) < 0)
            {
                // switch the node for its parent
                Exch(ref heap[parent], ref heap[node]);
                // fix recursively
                FixUp(heap, parent, start, end);
            }
        }

        // swap two items
        private void Exch(ref T item1, ref T item2)
        {
            T temp = item1;
            item1 = item2;
            item2 = temp;
        }

        // swap two items if the first is less than the second
        /*private bool CompExch(T item1, T item2)
        {
            bool swap = CompareItems(item1, item2) < 0;
            if (swap)
            {
                Exch(item1, item2);
            }
            return swap;
        }*/

        #endregion
    }

    internal class HeapOfComparable<T> : HeapBase<T> where T : IComparable
    {
        public HeapOfComparable(int maxLength)
            : base(maxLength)
        {
        }

        public HeapOfComparable(int maxLength, IEnumerable<T> unordered)
            : base(maxLength, unordered)
        {
            Heapify();
        }

        protected override int CompareItems(T item1, T item2)
        {
            return item1.CompareTo(item2);
        }
    }

    internal class HeapWithComparer<T> : HeapBase<T>
    {
        private readonly IComparer<T> Comparer_;

        public HeapWithComparer(int maxLength, IComparer<T> comparer)
            : base(maxLength)
        {
            Comparer_ = comparer;
        }

        public HeapWithComparer(int maxLength, IEnumerable<T> unordered, IComparer<T> comparer)
            : base(maxLength, unordered)
        {
            Comparer_ = comparer;
            Heapify();
        }

        protected override int CompareItems(T item1, T item2)
        {
            return Comparer_.Compare(item1, item2);
        }
    }
}