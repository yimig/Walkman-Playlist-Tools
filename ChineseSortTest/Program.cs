using System;
using System.Globalization;
using System.Threading;

namespace ChineseSortTest
{
    class Program
    {
        static void Main(string[] args)
        {
            string[] arr = { "趙（ZHAO）", "錢（QIAN）", "孫（SUN）", "李（LI）", "周（ZHOU）", "吳（WU）", "鄭（ZHENG）", "王（WANG）" };

            //发音 LCID：0x00000804
            CultureInfo PronoCi = new CultureInfo(2052);
            //Thread.CurrentThread.CurrentCulture = new CultureInfo("en-us"); 
            Array.Sort(arr);
            Console.WriteLine("按发音排序:");
            for (int i = arr.GetLowerBound(0); i <= arr.GetUpperBound(0); i++)
                Console.WriteLine("[{0}]:\t{1}", i, arr.GetValue(i));

            Console.WriteLine();

            //笔画数 LCID：0x00020804
            CultureInfo StrokCi = new CultureInfo(133124);
            Thread.CurrentThread.CurrentCulture = StrokCi;
            Array.Sort(arr);
            Console.WriteLine("按笔划数排序:");
            for (int i = arr.GetLowerBound(0); i <= arr.GetUpperBound(0); i++)
                Console.WriteLine("[{0}]:\t{1}", i, arr.GetValue(i));

            Console.WriteLine();

            //zh-cn （拼音：简中）
            Thread.CurrentThread.CurrentCulture = new CultureInfo("zh-cn");
            Array.Sort(arr);
            Console.WriteLine("zh-cn:");
            for (int i = arr.GetLowerBound(0); i <= arr.GetUpperBound(0); i++)
                Console.WriteLine("[{0}]:\t{1}", i, arr.GetValue(i));

            Console.WriteLine();

            //zh-tw （笔划数:繁中）
            Thread.CurrentThread.CurrentCulture = new CultureInfo("zh-tw");
            Array.Sort(arr);
            Console.WriteLine("zh-tw:");
            for (int i = arr.GetLowerBound(0); i <= arr.GetUpperBound(0); i++)
                Console.WriteLine("[{0}]:\t{1}", i, arr.GetValue(i));

            Console.ReadKey();

        }
    }
}
