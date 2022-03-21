using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace Parser.Utils
{
    public class BufferedEnumerable<T> : IEnumerable<T>
    {
        private readonly IEnumerator<T> _enumerator;
        private readonly List<T> _alreadyReadItems = new List<T>();
        public BufferedEnumerable(IEnumerable<T> enumerable)
        {
            _enumerator = enumerable.GetEnumerator();
        }

        private bool ReadMore(int count)
        {
            var result = true;
            var nextItems = new List<T>(count);
            for (var i = 0; i < count; i++)
            {
                if (!_enumerator.MoveNext())
                {
                    result = false;
                    break;
                }
                nextItems.Add(_enumerator.Current);
            }
            _alreadyReadItems.AddRange(nextItems);
            return result;
        }

        public IEnumerator<T> GetEnumerator()
        {
            return new Enumerator(this);
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        public IEnumerable<T> this[Range range]
        {
            get
            {
                if (range.Start.IsFromEnd || range.End.IsFromEnd)
                    throw new Exception("Can't read buffered enumerable from end");
                var (offset, length) = range.GetOffsetAndLength(int.MaxValue);
                if (_alreadyReadItems.Count < offset + length + 1)
                    ReadMore(offset + length + 1 - _alreadyReadItems.Count);
                return new Subenumerable<T>(_alreadyReadItems, range.Start.Value, range.End.Value + 1);
            }
        }

        public class Enumerator : IEnumerator<T>
        {
            private BufferedEnumerable<T> _bufferedEnumerable;
            private int _currentPos;
            public Enumerator(BufferedEnumerable<T> bufferedEnumerable)
            {
                _bufferedEnumerable = bufferedEnumerable;
                _currentPos = -1;
            }

            public bool MoveNext()
            {
                if (_currentPos < _bufferedEnumerable._alreadyReadItems.Count)
                {
                    _currentPos++;
                    return _currentPos < _bufferedEnumerable._alreadyReadItems.Count || _bufferedEnumerable.ReadMore(1);
                }
                var hadRead = _bufferedEnumerable.ReadMore(1);
                if (hadRead)
                    _currentPos++;
                return hadRead;
            }

            public void Reset()
            {
                _currentPos = -1;
            }

            public T Current { get => _bufferedEnumerable._alreadyReadItems[_currentPos]; }

            object IEnumerator.Current => Current;

            public void Dispose()
            {
            }
        }
    }
}