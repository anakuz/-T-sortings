using System;

namespace Task_1_7
{
    /*
        7.​ Класс Sorts, реализующий различные виды сортировок (1 студент – максимум 80 баллов; 2 студента, максимум – 40 баллов)

        Разработать обобщенный класс для сортировки массивов.

        *​ SortUtils

            o​ методы

                *​ QuickSort<T>(T[] array, CompareDelegate<T> comparator);

                *​ QuickSort<T>(T[] array) where T: Comparable<T>;

                *​ Методы, реализующие другие типы сортировок.
     */
    public static class SortUtils
    {
        public static void QuickSort<T>(T[] array, Comparison<T> comparator)
        {
            DelegateQuicksort.Sort(array, comparator);
        }

        public static void QuickSort<T>(T[] array) where T : IComparable<T>
        {
            RestrictionQuicksort.Sort(array);
        }

        public static void BubbleSort<T>(T[] array) where T : IComparable<T>
        {
            int i, j;
            for (i = array.Length - 1; i > 0; i--)
            {
                for (j = 0; j < i; j++)
                {
                    if (array[j].CompareTo(array[j + 1]) > 0)
                    {
                        DelegateQuicksort.Swap(array, j, j + 1);
                    }
                }
            }
        }

        public static void BubbleSort<T>(T[] array, Comparison<T> comparator)
        {
            int i, j;
            for (i = array.Length - 1; i > 0; i--)
            {
                for (j = 0; j < i; j++)
                {
                    if (comparator(array[j], (array[j + 1])) > 0)
                    {
                        DelegateQuicksort.Swap(array, j, j + 1);
                    }
                }
            }
        }

        public static void InsertionSort<T>(T[] array, Comparison<T> comparator)
        {
            for (int i = 1; i < array.Length; i++)
            {
                T cur = array[i];
                var j = i;
                while (j > 0 && comparator(cur, array[j - 1]) == -1)
                {
                    array[j] = array[j - 1];
                    j--;
                }
                array[j] = cur;
            }
        }

        public static void InsertionSort<T>(T[] array) where T : IComparable<T>
        {
            for (int i = 1; i < array.Length; i++)
            {
                T cur = array[i];
                var j = i;
                while (j > 0 && cur.CompareTo(array[j - 1]) == -1)
                {
                    array[j] = array[j - 1];
                    j--;
                }
                array[j] = cur;
            }
        }
    }

    static class DelegateQuicksort
    {
        public static void Swap<T>(T[] array, int left, int right)
        {
            var temp = array[right];
            array[right] = array[left];
            array[left] = temp;
        }

        public static void Sort<T>(T[] array, Comparison<T> comparer)
        {
            Sort(array, 0, array.Length - 1, comparer);
        }

        public static void Sort<T>(T[] array, int startIndex, int endIndex, Comparison<T> comparer)
        {
            int left = startIndex;
            int right = endIndex;
            int pivot = startIndex;
            startIndex++;

            while (endIndex >= startIndex)
            {
                if (comparer(array[startIndex], array[pivot]) >= 0 && comparer(array[endIndex], array[pivot]) < 0)
                {
                    Swap(array, startIndex, endIndex);
                }
                else if (comparer(array[startIndex], array[pivot]) >= 0)
                {
                    endIndex--;
                }
                else if (comparer(array[endIndex], array[pivot]) < 0)
                {
                    startIndex++;
                }
                else
                {
                    endIndex--;
                    startIndex++;
                }
            }

            Swap(array, pivot, endIndex);
            pivot = endIndex;
            if (pivot > left)
            {
                Sort(array, left, pivot, comparer);
            }
            if (right > pivot + 1)
            {
                Sort(array, pivot + 1, right, comparer);
            }
        }
    }

    static class RestrictionQuicksort
    {
        private static void Swap<T>(T[] array, int left, int right) where T : IComparable<T>
        {
            var temp = array[right];
            array[right] = array[left];
            array[left] = temp;
        }

        public static void Sort<T>(T[] array) where T : IComparable<T>
        {
            Sort(array, 0, array.Length - 1);
        }

        public static void Sort<T>(T[] array, int startIndex, int endIndex) where T : IComparable<T>
        {
            int left = startIndex;
            int right = endIndex;
            int pivot = startIndex;
            startIndex++;

            while (endIndex >= startIndex)
            {
                if (array[startIndex].CompareTo(array[pivot]) >= 0 && array[endIndex].CompareTo(array[pivot]) < 0)
                {
                    Swap(array, startIndex, endIndex);
                }
                else if (array[startIndex].CompareTo(array[pivot]) >= 0)
                {
                    endIndex--;
                }
                else if (array[endIndex].CompareTo(array[pivot]) < 0)
                {
                    startIndex++;
                }
                else
                {
                    endIndex--;
                    startIndex++;
                }
            }

            Swap(array, pivot, endIndex);
            pivot = endIndex;

            if (pivot > left)
            {
                Sort(array, left, pivot);
            }
            if (right > pivot + 1)
            {
                Sort(array, pivot + 1, right);
            }
        }
    }
}