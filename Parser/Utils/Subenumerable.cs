using System;
using System.Collections;
using System.Collections.Generic;

namespace Parser.Utils
{
    public class Subenumerable<T> : IEnumerable<T>
    {
        private readonly IList<T> _source;
        private readonly int _start;
        private readonly int _end;
        public Subenumerable(IList<T> source, int start, int end)
        {
            _source = source;
            _start = start;
            _end = end;
        }

        public IEnumerator<T> GetEnumerator()
        {
            return new Enumerator(this);
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        public class Enumerator : IEnumerator<T>
        {
            private Subenumerable<T> _subenumerable;
            private int _currentPos;
            public Enumerator(Subenumerable<T> subenumerable)
            {
                _subenumerable = subenumerable;
                _currentPos = subenumerable._start-1;
            }

            public bool MoveNext()
            {
                if(_currentPos < _subenumerable._end)
                    _currentPos++;
                return _currentPos < _subenumerable._end;
            }

            public void Reset()
            {
                _currentPos = _subenumerable._start;
            }

            public T Current { get => _subenumerable._source[_currentPos]; }

            object IEnumerator.Current => Current;

            public void Dispose()
            {
            }
        }
    }
}