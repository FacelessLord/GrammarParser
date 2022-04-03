using System;
using System.Collections.Generic;
using System.Linq;

namespace Parser
{
    public static class EnumerableExtensions
    {
        public static void ForEach<T>(this IEnumerable<T> enumerable, Action<T> action)
        {
            foreach (var item in enumerable)
            {
                action(item);
            }
        }
        public static IEnumerable<T> Peek<T>(this IEnumerable<T> enumerable, Action<T> action)
        {
            var list = enumerable.ToList();
            foreach (var item in list)
            {
                action(item);
            }

            return list;
        }

        public static bool None<T>(this IEnumerable<T> enumerable, Func<T, bool> predicate)
        {
            return !enumerable.Any(predicate);
        }

        public static string Join<T>(this IEnumerable<T> enumerable, string separator)
        {
            return string.Join(separator, enumerable);
        }

        public static List<List<T>> Split<T>(this IEnumerable<T> enumerable, Predicate<T> splitterSelector)
        {
            var result = new List<List<T>>();
            var collector = new List<T>();
            foreach (var item in enumerable)
            {
                if (splitterSelector(item))
                {
                    result.Add(collector);
                    result.Add(new List<T>{item});
                    collector = new List<T>();
                }
                else
                {
                    collector.Add(item);
                }
            }
            result.Add(collector);
            return result;
        }
    }
}