using System;
using System.Collections.Generic;
using System.Text;

namespace Features
{
    public static class MyLinq
    {
        public static int Count<T>(this IEnumerable<T> sequance)
        {
            int count = 0;
            foreach(var item in sequance)
            {
                count++;
            }

            return count;
        }

        public static IEnumerable<T> Filter<T>(this IEnumerable<T> source, Func<T, bool> predicate)
        {

            foreach (var item in source)
            {
                if (predicate(item))
                {
                    yield return item;
                };
            }
        }

        public static IEnumerable<double> Random()
        {
            var random = new Random();
            while (true)
            {
                yield return random.NextDouble();
            }
        }
    }
}
