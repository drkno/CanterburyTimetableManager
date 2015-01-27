namespace UniTimetable.Model
{
    internal class History<T>
    {
        private readonly int _capacity;
        private int _count;
        private int _current;
        private T[] _items;

        public History(int capacity)
        {
            _capacity = capacity;
            Clear();
        }

        public void Clear()
        {
            _items = new T[_capacity];
            _count = 0;
            _current = -1;
        }

        public T Back()
        {
            // if we're at the start, return null
            if (_current <= 0)
                return default(T);
            // step the index back and return the item
            return _items[--_current];
        }

        public T Forward()
        {
            // if we're at the end already
            if (_current >= _count - 1 || _current < 0)
                return default(T);
            // return the next item and step up the index
            return _items[++_current];
        }

        public T Current()
        {
            // if there's nothing yet
            if (_current < 0)
                return default(T);
            // return the current item
            return _items[_current];
        }

        public T PeekBack()
        {
            // if we're at the start, return null
            if (_current <= 0)
                return default(T);
            // return the previous item
            return _items[_current - 1];
        }

        public T PeekForward()
        {
            // if we're at the end already
            if (_current >= _count - 1 || _current < 0)
                return default(T);
            // return the next item
            return _items[_current + 1];
        }

        public void Add(T item)
        {
            // if we're full to capacity
            if (_current == _capacity - 1)
            {
                // shift all items in the array back 1
                for (int i = 0; i < _capacity - 1; i++)
                    _items[i] = _items[i + 1];
            }
            else
            {
                _current++;
            }

            // insert the item at the next location
            _items[_current] = item;
            // fix the item count
            _count = _current + 1;
        }
    }
}