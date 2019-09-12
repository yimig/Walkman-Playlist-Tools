using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Globalization;
using System.Threading;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Media;

namespace Walkman_Playlist_Tools
{
    public class SortMusicInfo
    {

        private class ChineseStringComparer : IComparer<string>
        {
            private CompareInfo myComp;
            private CompareOptions myOptions = CompareOptions.None;

            // Constructs a comparer using the specified CompareOptions.
            public ChineseStringComparer(CompareInfo cmpi, CompareOptions options)
            {
                myComp = cmpi;
                this.myOptions = options;
            }

            // Compares strings with the CompareOptions specified in the constructor.
            public int Compare(string a, string b)
            {
                if (a == b) return 0;
                if (a == null) return -1;
                if (b == null) return 1;
                if (a != null && b != null)return myComp.Compare(a, b, myOptions);
                throw new ArgumentException("a and b should be strings.");

            }
        }

        private class NumberStringComparer : IComparer<string>
        {

            // Compares strings with the CompareOptions specified in the constructor.
            public int Compare(string a, string b)
            {
                int aInt=0, bInt=0;
                if (a == b) return 0;
                if (string.IsNullOrEmpty(a)) return -1;
                if (string.IsNullOrEmpty(b)) return 1;
                string[] aStrArr = a.Split(new char[] { '/', ' ', ':' });
                string[] bStrArr = b.Split(new char[] { '/', ' ', ':' });
                if (a != null && b != null)
                {
                    for(int i=aStrArr.Length-1;i>=0;i--)aInt+=(int)(Convert.ToInt32(aStrArr[i])
                    *Math.Pow(10, aStrArr.Length - i));
                    for (int i = bStrArr.Length - 1; i >= 0; i--) bInt += (int)(Convert.ToInt32(bStrArr[i])
                                                                                * Math.Pow(10, bStrArr.Length - i));
                    if (aInt > bInt) return 1;
                    else if (bInt > aInt) return -1;
                    else return 0;
                }
                throw new ArgumentException("a and b should be strings.");

            }
        }

        /// <summary>
        /// 通过中文读音顺序排序音乐信息
        /// </summary>
        /// <param name="infos">音乐列表数据</param>
        /// <param name="keySelecter">要排序的信息</param>
        /// <returns></returns>
        private static ObservableCollection<MusicInfo> ByChineseFormat (ObservableCollection<MusicInfo> infos,Func<MusicInfo,string> keySelecter)
        {
            ChineseStringComparer chineseComp = new ChineseStringComparer(CompareInfo.GetCompareInfo("zh-CN"), CompareOptions.None);
            return new ObservableCollection<MusicInfo>(infos.OrderBy(keySelecter,chineseComp));
        }

        /// <summary>
        /// 通过英文字母顺序排序音乐信息
        /// </summary>
        /// <param name="infos">音乐列表数据</param>
        /// <param name="keySelecter">要排序的信息</param>
        /// <returns></returns>
        public static ObservableCollection<MusicInfo> ByNormalFormat (ObservableCollection<MusicInfo> infos, Func<MusicInfo, string> keySelecter)
        {
            return new ObservableCollection<MusicInfo>(infos.OrderBy(keySelecter));
        }

        /// <summary>
        /// 通过数字大小排序音乐信息
        /// </summary>
        /// <param name="infos"></param>
        /// <param name="keySelecter"></param>
        /// <returns></returns>
        private static ObservableCollection<MusicInfo> ByNumberFormat(ObservableCollection<MusicInfo> infos,Func<MusicInfo,string> keySelecter)
        {
            NumberStringComparer numComp=new NumberStringComparer();
            return new ObservableCollection<MusicInfo>(infos.OrderBy(keySelecter,numComp));
        }

        /// <summary>
        /// 倒转音乐列表信息
        /// </summary>
        /// <param name="infos"></param>
        /// <returns></returns>
        public static ObservableCollection<MusicInfo> Reverse(ObservableCollection<MusicInfo> infos)
        {
            ObservableCollection<MusicInfo> resultCollection=new ObservableCollection<MusicInfo>();
            for (int i = infos.Count - 1; i >= 0; i--)
            {
                resultCollection.Add(infos[i]);
            }

            return resultCollection;
        }

        /// <summary>
        /// 返回对某个列应用的排序类型函数
        /// </summary>
        /// <param name="label"></param>
        /// <param name="isNumber"></param>
        /// <returns></returns>
        public static Func<MusicInfo, string> GetMusicStrFunc(Label label,out bool isNumber)
        {
            Func<MusicInfo, string> resultFunc;
            switch (label.Content.ToString())
            {
                case "标题":
                {
                    resultFunc = i => i.Title;
                    isNumber = false;
                }
                    break;
                case "格式":
                {
                    resultFunc = i => i.Format;
                    isNumber = false;
                }
                    break;
                case "艺术家":
                {
                    resultFunc = i => i.Artist;
                    isNumber = false;
                }
                    break;
                case "专辑名称":
                {
                    isNumber = false;
                    resultFunc = i => i.Album;
                }
                    break;
                case "曲长":
                {
                    resultFunc = i => i.Length.ToString();
                    isNumber = true;
                }
                    break;
                case "年代":
                {
                    resultFunc = i => i.Year;
                    isNumber = true;
                }
                    break;
                case "同步时间":
                {
                    resultFunc = i => i.Buildtime;
                    isNumber = true;
                }
                    break;
                default:
                {
                    resultFunc = i => i.Path;
                    isNumber = false;
                    break;
                }
            }

            return resultFunc;
        }

        /// <summary>
        /// 开始排序音乐列表
        /// </summary>
        /// <param name="dataCollection"></param>
        /// <param name="label"></param>
        /// <returns></returns>
        public static ObservableCollection<MusicInfo> StartSort(ObservableCollection<MusicInfo> dataCollection,
            Label label)
        {
            bool isNumber;
            Func<MusicInfo, string> func = GetMusicStrFunc(label, out isNumber);
            if (isNumber) return ByNumberFormat(dataCollection, func);
            else return ByChineseFormat(dataCollection, func);
        }

        /// <summary>
        /// 得到某标签对应的ListView
        /// </summary>
        /// <param name="label"></param>
        /// <returns></returns>
        private static ListView GetListView(object label)
        {
            var rawlabel = label as Label;
            var header = rawlabel.Parent as GridViewColumnHeader;
            var rowPresenter = header.Parent as GridViewHeaderRowPresenter;
            var scrollViewer1 = rowPresenter.Parent as ScrollViewer;
            var dockPanel = scrollViewer1.Parent as DockPanel;
            var grid = dockPanel.Parent as Grid;
            var scrollViewer2 = VisualTreeHelper.GetParent(grid) as ScrollViewer;
            var listBoxChrome = scrollViewer2.Parent as DependencyObject;
            return VisualTreeHelper.GetParent(listBoxChrome) as ListView;
        }

        public  static void Label_MouseLeftButtonDown(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            ListView targetLV = GetListView(sender);
            ObservableCollection<MusicInfo> infobase = targetLV.ItemsSource as ObservableCollection<MusicInfo>;
            try
            {
                if (!StaticData.isListReverse)
                    targetLV.ItemsSource = SortMusicInfo.StartSort(infobase, sender as Label);
                else targetLV.ItemsSource = SortMusicInfo.Reverse(SortMusicInfo.StartSort(infobase, sender as Label));
                StaticData.isListReverse = !StaticData.isListReverse;
            }
            catch (System.ArgumentNullException)
            {
                System.Console.WriteLine("出现异常字符，请检查相关信息是否有空值");
            }
            catch (System.FormatException)
            {
                System.Console.WriteLine("字符串错误，请手动清空并刷新音乐库");
            }
            catch (System.OverflowException)
            {
                System.Console.WriteLine("存在异常字符串：转换溢出");
            }
        }
    }
}
