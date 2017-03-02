using System;
using System.CodeDom;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Navigation;

namespace Task_1_7.Wpf
{
    public class Point2D : IComparable<Point2D>
    {
        private int _x;

        private int _y;

        private Point2D(int x, int y)
        {
            _x = x;
            _y = y;
        }

        public override string ToString()
        {
            return String.Format("({0},{1})", _x, _y);
        }

        public int CompareTo(Point2D other)
        {
            var p1 = CalcDistaceTo0();
            var p2 = other.CalcDistaceTo0();
            if (p1 < p2)
                return -1;
            if (p1 > p2)
                return 1;
            return 0;
        }

        private double CalcDistaceTo0()
        {
            return Math.Sqrt(MathHelper.Sqr(_x) + MathHelper.Sqr(_y));
        }

        public static Point2D[] ParseArray(string text)
        {
            var points = text.Split(new[] {' '}, StringSplitOptions.RemoveEmptyEntries);
            var list = new List<Point2D>();
            foreach (var point in points)
            {
                list.Add(Parse(point));
            }
            return list.ToArray();
        }

        private static Point2D Parse(string point)
        {
            var x = point.Remove(0, 1);
            x = x.Remove(x.Length - 1, 1);
            var xy = x.Split(',');
            return new Point2D(int.Parse(xy[0]), int.Parse(xy[1]));
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

        private static Point2D Generate()
        {
            return new Point2D(Helper.Random(0, 20), Helper.Random(0, 20));
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
    }

    public class IntegerHelper : IComparable<IntegerHelper>
    {
        private static string GenerateArrayInt()
        {
            var n = Helper.Random(4, 8);
            var sb = new StringBuilder();
            for (int i = 0; i < n; n++)
                sb.AppendFormat("{0} ", Helper.Random(0, 100));
            return sb.ToString();
        }

        private static int[] ParseIntArray(string text)
        {
            var ints = text.Split(new[] {' '}, StringSplitOptions.RemoveEmptyEntries);
            var list = new List<int>();
            foreach (var el in ints)
                list.Add(int.Parse(el));
            return list.ToArray();
        }

        private static string ToStringArray(int[] ints)
        {
            var sb = new StringBuilder();
            foreach (var el in ints)
                sb.AppendFormat("{0} ", el);
            return sb.ToString();
        }

        public int CompareTo(IntegerHelper other)
        {
            throw new NotImplementedException();
        }
    }

    internal class MathHelper
    {
        public static double Sqr(double x)
        {
            return x*x;
        }
    }

    public interface ICommonSorting
    {
        void GenerateItems();
        void Sort(MethodType methodType, SortType sortType);
        string SortedItemsText { get; }
        string OriginalItemsText { get; set; }

        event Action Sorted;

        event Action ItemsGenerated;
    }

    public class Point2DSorting : ICommonSorting
    {
        private Exception SortingException { get; set; }

        private string SortingText { get; set; }

        public void GenerateItems()
        {
            OriginalItemsText = Point2D.GenerateArrayString();
            Helper.SafeInvoke(ItemsGenerated);
        }

        public void Sort(MethodType methodType, SortType sortType)
        {
            try
            {
                SortingException = null;
                SortingText = null;
                /*if (methodType != MethodType.IComparable)
                {
                    throw new Exception("not supported method type");
                }*/
                var points = Point2D.ParseArray(OriginalItemsText);
                switch (sortType)
                {
                    case SortType.Bubble:
                        SortUtils.BubbleSort(points);
                        break;
                    case SortType.QuickSort:
                        SortUtils.QuickSort(points);
                        break;
                    case SortType.Insertion:
                        //SortUtils.InsertionSort(points);
                        break;
                    default:
                        throw new ArgumentOutOfRangeException("sortType", sortType, null);
                }
                SortingText = Point2D.ToArrayString(points);
            }
            catch (Exception e)
            {
                SortingException = e;
            }
            finally
            {
                Helper.SafeInvoke(Sorted);
            }
        }


        public string SortedItemsText
        {
            get
            {
                if (SortingException != null)
                    throw SortingException;
                return SortingText;
            }
        }

        public string OriginalItemsText { get; set; }

        public event Action Sorted;

        public event Action ItemsGenerated;
    }

    public class IntegerSorting : ICommonSorting
    {
        private Exception SortingException { get; set; }

        private string SortingText { get; set; }

        public void GenerateItems()
        {
            //OriginalItemsText = ;
            Helper.SafeInvoke(ItemsGenerated);
        }

        public void Sort(MethodType methodType, SortType sortType)
        {
            try
            {
                SortingException = null;
                SortingText = null;
                if (methodType != MethodType.IComparable)
                {
                    throw  new Exception("not supported method type");
                }

            }
            catch (Exception e)
            {
                {
                    SortingException = e;
                }
                throw;
            }
            finally
            {
                Helper.SafeInvoke(Sorted);
            }
        }

        public string SortedItemsText
        {
            get
            {
                if (SortingException != null)
                    throw SortingException;
                return SortingText;
            }
        }

        public string OriginalItemsText { get; set; }

        public event Action Sorted;

        public event Action ItemsGenerated;
    }




    public class SortingEngine
    {
        private ElemsType _typeOfElems = ElemsType.Integer;
        private ICommonSorting _sorting = new IntegerSorting();

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

        private void TurnEvents(ICommonSorting sorting, bool turnOn)
        {
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

        void sorting_ItemsGenerated()
        {
            Helper.SafeInvoke(ItemsGenerated);
        }

        void sorting_Sorted()
        {
            Helper.SafeInvoke(Sorted);
        }


        public SortingEngine()
        {
            TypeOfElemsChanged += SortingEngine_TypeOfElemsChanged;
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
        }

        public void GenerateItems()
        {
            Sorting.GenerateItems();
        }

        public void Sort()
        {
            Sorting.Sort(MethodType, SortType);
        }

        public string SortedItemsText { get { return Sorting.SortedItemsText; } }

        public string OriginalItemsText
        {
            get { return Sorting.OriginalItemsText; }
            set { Sorting.OriginalItemsText = value; }
        }

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

        private void OnTypeOfElemsChanged()
        {
            var ev = TypeOfElemsChanged;
            if (ev != null)
                ev();
        }

        public SortType SortType { get; set; }
        public MethodType MethodType { get; set; }

        public event Action ItemsGenerated;

        public event Action Sorted;

        public event Action TypeOfElemsChanged;

        public static IEnumerable TypesOfElems { get { return Helper.GetValues(typeof(ElemsType)); } }
        public static IEnumerable SortTypes { get { return Helper.GetValues(typeof(SortType)); } }
        public static IEnumerable MethodTypes { get { return Helper.GetValues(typeof(MethodType)); } }
    }

    public enum ElemsType
    {
        Integer,
        Point2D
    }

    public enum SortType
    {
        Bubble,
        QuickSort,
        Insertion
    }

    public enum MethodType
    {
        IComparable,
        Delegate,
        DelegateInverse
    }



    public static class Helper
    {
        public static IEnumerable GetValues(Type enumType)
        {
            return Enum.GetValues(enumType).OfType<object>().ToArray();
        }

        public static void SafeInvoke(Action action)
        {
            if (action != null)
                action();
        }

        private static readonly Random _r = new Random();

        public static int Random(int min, int max)
        {
            return _r.Next(min, max);
        }
    }



}