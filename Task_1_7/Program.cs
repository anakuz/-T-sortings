using System;
using System.Collections.Generic;
using System.Xml.Serialization;

namespace Task_1_7
{
    class Program
    {
        static void Main()
        {

            while (true)
            {
                
            }

            
            var x = new int[] { 1, 6, 3 };
            Comparison<int> t0 = Comparer<int>.Default.Compare;
            var t = Inverse(t0);
            SortUtils.InsertionSort(x, t);


            
        }

        private static Comparison<T> Inverse<T>(Comparison<T> compare)
        {
            return (x, y) =>
            {
                if (compare(x, y) == -1)
                    return 1;
                if (compare(x, y) == 1)
                    return -1;
                return 0;
            };
        }
    }


   
}