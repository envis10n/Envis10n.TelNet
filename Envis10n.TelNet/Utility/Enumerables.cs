using System;
using System.Collections.Generic;

namespace Envis10n.TelNet.Utility
{
    public static class Enumerables
    {
        public static T[] Slice<T>(T[] src, int startOffset, int endOffset)
        {
            T[] temp = new T[endOffset - startOffset];
            Buffer.BlockCopy(src, startOffset, temp, 0, temp.Length);
            return temp;
        }

        public static T[] Concat<T>(params IEnumerable<T>[] input)
        {
            List<T> list = new  List<T>();
            foreach (IEnumerable<T> item in input)
            {
                list.AddRange(item);
            }

            return list.ToArray();
        }
    }
}