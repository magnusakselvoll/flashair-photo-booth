using System;

namespace flashair_slideshow
{
    public class HistoryQueue<T>
    {
        public int Capacity { get; }
        private T[] _buffer;
        private int _firstElement;
        private int _lastElement;

        public HistoryQueue(int capacity)
        {
            if (capacity < 1)
            {
                throw new ArgumentOutOfRangeException(nameof(capacity), capacity, @"Capacity must be a postivive number");
            }

            Capacity = capacity;

            Clear();
        }

        public void Clear()
        {
            _buffer = new T[Capacity];
            _firstElement = -1;
            _lastElement = -1;
        }

        public void Enqueue(T item)
        {
            lock (this)
            {
                if (_firstElement < 0)
                {
                    _firstElement = _lastElement = 0;
                }
                else
                {
                    _lastElement = (_lastElement + 1) % Capacity;

                    if (_lastElement == _firstElement) //buffer full
                    {
                        _firstElement = (_firstElement + 1) % Capacity;
                    }
                }

                _buffer[_lastElement] = item;
            }
        }

        public int Count
        {
            get
            {
                if (_firstElement < 0)
                {
                    return 0;
                }

                if (_firstElement == _lastElement)
                {
                    return 1;
                }

                if (_lastElement > _firstElement)
                {
                    return _lastElement + 1 - _firstElement;
                }

                return Capacity + _lastElement + 1 - _firstElement;
            }
        }

        public T this[int index]
        {
            get
            {
                if (index < 0 || index > Count)
                {
                    throw new ArgumentOutOfRangeException(nameof(index), index, $@"Index must be between 0 and current count({Count})");
                }

                return _buffer[(_firstElement + index) % Capacity];
            }
        }
    }
}
