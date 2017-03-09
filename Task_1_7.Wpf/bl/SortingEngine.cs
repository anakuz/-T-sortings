#region Imports (5)

using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;

#endregion Imports (5)

namespace Task_1_7.Wpf
{
    public enum ElemsType
    {
        Integer,
        Point2D
    }
    public enum MethodType
    {
        IComparable,
        Delegate,
        DelegateInverse
    }
    public enum SortType
    {
        Bubble,
        QuickSort,
        Insertion
    }

    public interface ICommonSorting
    {
        event Action ItemsGenerated;
        event Action Sorted;

        string OriginalItemsText { get; set; }

        string SortedItemsText { get; }

        void GenerateItems();

        void Sort(MethodType methodType, SortType sortType);
    }

    public static class Helper
    {
        private static readonly Random Rnd = new Random();

        public static IEnumerable GetValues(Type enumType)
        {
            return Enum.GetValues(enumType).OfType<object>().ToArray();
        }

        public static Comparison<T> Inverse<T>(Comparison<T> compare)
        {
            return (x, y) =>
            {
                if (compare(x, y) == -1)
                {
                    return 1;
                }
                if (compare(x, y) == 1)
                {
                    return -1;
                }
                return 0;
            };
        }

        public static int Random(int min, int max)
        {
            return Rnd.Next(min, max);
        }

        public static void SaveInvoke(Action action)
        {
            action?.Invoke();
        }
    }

    public class IntegerSorting : ICommonSorting
    {
        public event Action ItemsGenerated;
        public event Action Sorted;

        public string OriginalItemsText { get; set; }

        public string SortedItemsText
        {
            get
            {
                if (SortingException != null)
                {
                    throw SortingException;
                }
                return SortingText;
            }
        }

        private Exception SortingException { get; set; }

        private string SortingText { get; set; }

        public void GenerateItems()
        {
            OriginalItemsText = IntHelper.GenerateIntsRandomString();
            Helper.SaveInvoke(ItemsGenerated);
        }

        public void Sort(MethodType methodType, SortType sortType)
        {
            try
            {
                SortingException = null;
                SortingText = null;
                int[] points = IntHelper.ParseArray(OriginalItemsText);

                if (methodType == MethodType.IComparable)
                {
                    switch (sortType)
                    {
                        case SortType.Bubble:
                            SortUtils.BubbleSort(points);
                            break;
                        case SortType.QuickSort:
                            SortUtils.QuickSort(points);
                            break;
                        case SortType.Insertion:
                            SortUtils.InsertionSort(points);
                            break;
                        default:
                            throw new ArgumentOutOfRangeException("sortType", sortType, null);
                    }
                }
                else
                {
                    Comparison<int> compareFunc = Comparer<int>.Default.Compare;
                    if (methodType == MethodType.DelegateInverse)
                    {
                        compareFunc = Helper.Inverse(compareFunc);
                    }
                    switch (sortType)
                    {
                        case SortType.Bubble:
                            SortUtils.BubbleSort(points, compareFunc);
                            break;
                        case SortType.QuickSort:
                            SortUtils.QuickSort(points, compareFunc);
                            break;
                        case SortType.Insertion:
                            SortUtils.InsertionSort(points, compareFunc);
                            break;
                        default:
                            throw new ArgumentOutOfRangeException("sortType", sortType, null);
                    }
                }

                SortingText = IntHelper.ToArrayString(points);
            }
            catch (Exception e)
            {
                SortingException = e;
            }
            finally
            {
                Helper.SaveInvoke(Sorted);
            }
        }
    }

    public class IntHelper
    {
        public static string GenerateIntsRandomString()
        {
            return ToArrayString(Enumerable.Range(0, Helper.Random(2, 6)).Select(x => Helper.Random(0, 20)).ToArray());
        }

        public static int[] ParseArray(string text)
        {
            return text.Split(new[] { ' ' }, StringSplitOptions.RemoveEmptyEntries).Select(int.Parse).ToArray();
        }

        public static string ToArrayString(int[] ints)
        {
            return ints.Aggregate("", (a, b) => a + " " + b);
        }
    }

    internal class MathHelper
    {
        public static double Sqr(double x)
        {
            return x * x;
        }
    }

    public class Point2D : IComparable<Point2D>
    {
        private int _x;
        private int _y;

        private Point2D(int x, int y)
        {
            _x = x;
            _y = y;
        }

        private double CalcDistaceTo0()
        {
            return Math.Sqrt(MathHelper.Sqr(_x) + MathHelper.Sqr(_y));
        }

        public static int Compare(Point2D x, Point2D y)
        {
            var dX = x.CalcDistaceTo0();
            var dY = y.CalcDistaceTo0();
            if (dX < dY)
            {
                return -1;
            }
            if (dX > dY)
            {
                return 1;
            }
            return 0;
        }

        public int CompareTo(Point2D other)
        {
            return Compare(this, other);
        }

        private static Point2D Generate()
        {
            return new Point2D(Helper.Random(0, 20), Helper.Random(0, 20));
        }

        public static string GenerateArrayString()
        {
            var n = Helper.Random(3, 6);
            var sb = new StringBuilder();
            for (int i = 0; i < n; i++)
            {
                sb.AppendFormat("{0} ", Generate());
            }
            return sb.ToString();
        }

        private static Point2D Parse(string point)
        {
            var x = point.Remove(0, 1);
            x = x.Remove(x.Length - 1, 1);
            var xy = x.Split(',');
            return new Point2D(int.Parse(xy[0]), int.Parse(xy[1]));
        }

        public static Point2D[] ParseArray(string text)
        {
            var points = text.Split(new[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);
            var list = new List<Point2D>();
            foreach (var point in points)
            {
                list.Add(Parse(point));
            }
            return list.ToArray();
        }

        public static string ToArrayString(Point2D[] points)
        {
            var sb = new StringBuilder();
            foreach (var point2D in points)
            {
                sb.AppendFormat("{0} ", point2D);
            }
            return sb.ToString();
        }

        public override string ToString()
        {
            return String.Format("({0},{1})", _x, _y);
        }
    }

    public class Point2DSorting : ICommonSorting
    {
        public event Action ItemsGenerated;
        public event Action Sorted;

        public string OriginalItemsText { get; set; }

        public string SortedItemsText
        {
            get
            {
                if (SortingException != null)
                {
                    throw SortingException;
                }
                return SortingText;
            }
        }

        private Exception SortingException { get; set; }

        private string SortingText { get; set; }

        public void GenerateItems()
        {
            OriginalItemsText = Point2D.GenerateArrayString();
            Helper.SaveInvoke(ItemsGenerated);
        }

        public void Sort(MethodType methodType, SortType sortType)
        {
            try
            {
                SortingException = null;
                SortingText = null;

                var points = Point2D.ParseArray(OriginalItemsText);
                if (methodType == MethodType.IComparable)
                {
                    switch (sortType)
                    {
                        case SortType.Bubble:
                            SortUtils.BubbleSort(points);
                            break;
                        case SortType.QuickSort:
                            SortUtils.QuickSort(points);
                            break;
                        case SortType.Insertion:
                            SortUtils.InsertionSort(points);
                            break;
                        default:
                            throw new ArgumentOutOfRangeException("sortType", sortType, null);
                    }
                }
                else
                {
                    Comparison<Point2D> compareFunc = Comparer<Point2D>.Default.Compare;
                    if (methodType == MethodType.DelegateInverse)
                    {
                        compareFunc = Helper.Inverse(compareFunc);
                    }
                    switch (sortType)
                    {
                        case SortType.Bubble:
                            SortUtils.BubbleSort(points, compareFunc);
                            break;
                        case SortType.QuickSort:
                            SortUtils.QuickSort(points, compareFunc);
                            break;
                        case SortType.Insertion:
                            SortUtils.InsertionSort(points, compareFunc);
                            break;
                        default:
                            throw new ArgumentOutOfRangeException("sortType", sortType, null);
                    }
                }
                SortingText = Point2D.ToArrayString(points);
            }
            catch (Exception e)
            {
                SortingException = e;
            }
            finally
            {
                Helper.SaveInvoke(Sorted);
            }
        }
    }

    public class SortingEngine
    {
        public event Action ItemsGenerated;
        public event Action Sorted;
        public event Action TypeOfElemsChanged;

        private ElemsType _typeOfElems = ElemsType.Integer;
        private ICommonSorting _sorting;
        private SortType _sortType;
        private MethodType _methodType;

        public MethodType MethodType
        {
            get { return _methodType; }
            set
            {
                _methodType = value;
                GenerateItems();
            }
        }

        public static IEnumerable MethodTypes { get { return Helper.GetValues(typeof(MethodType)); } }

        public string OriginalItemsText
        {
            get { return Sorting.OriginalItemsText; }
            set { Sorting.OriginalItemsText = value; }
        }

        public string SortedItemsText { get { return Sorting.SortedItemsText; } }

        private ICommonSorting Sorting
        {
            get { return _sorting; }
            set
            {
                TurnEvents(_sorting, false);
                _sorting = value;
                TurnEvents(value, true);
            }
        }

        public SortType SortType
        {
            get { return _sortType; }
            set
            {
                _sortType = value;
                GenerateItems();
            }
        }

        public static IEnumerable SortTypes { get { return Helper.GetValues(typeof(SortType)); } }

        public ElemsType TypeOfElems
        {
            get { return _typeOfElems; }
            set
            {
                if (_typeOfElems == value)
                    return;
                _typeOfElems = value;
                OnTypeOfElemsChanged();
            }
        }

        public static IEnumerable TypesOfElems { get { return Helper.GetValues(typeof(ElemsType)); } }

        public SortingEngine()
        {
            Sorting = new IntegerSorting();
            TypeOfElemsChanged += SortingEngine_TypeOfElemsChanged;
        }

        public void GenerateItems()
        {
            Sorting.GenerateItems();
        }

        private void OnTypeOfElemsChanged()
        {
            var ev = TypeOfElemsChanged;
            if (ev != null)
            {
                ev();
            }
        }

        public void Sort()
        {
            Sorting.Sort(MethodType, SortType);
        }

        private void sorting_ItemsGenerated()
        {
            Helper.SaveInvoke(ItemsGenerated);
        }

        private void sorting_Sorted()
        {
            Helper.SaveInvoke(Sorted);
        }

        private void SortingEngine_TypeOfElemsChanged()
        {
            switch (TypeOfElems)
            {
                case ElemsType.Point2D:
                    Sorting = new Point2DSorting();
                    break;
                case ElemsType.Integer:
                    Sorting = new IntegerSorting();
                    break;
            }
            GenerateItems();
        }

        private void TurnEvents(ICommonSorting sorting, bool turnOn)
        {
            if (sorting == null)
            {
                return;
            }
            if (turnOn)
            {
                sorting.Sorted += sorting_Sorted;
                sorting.ItemsGenerated += sorting_ItemsGenerated;
            }
            else
            {
                sorting.Sorted -= sorting_Sorted;
                sorting.ItemsGenerated -= sorting_ItemsGenerated;
            }
        }
    }
}
