using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Globalization;
using System.Threading;

namespace Walkman_Playlist_Tools
{
    public static class SortMusicInfo
    {

        private class MyStringComparer : IComparer<string>
        {
            private CompareInfo myComp;
            private CompareOptions myOptions = CompareOptions.None;

            // Constructs a comparer using the specified CompareOptions.
            public MyStringComparer(CompareInfo cmpi, CompareOptions options)
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

        /// <summary>
        /// 通过中文读音顺序排序音乐信息
        /// </summary>
        /// <param name="infos">音乐列表数据</param>
        /// <param name="keySelecter">要排序的信息</param>
        /// <returns></returns>
        public static ObservableCollection<MusicInfo> ByChineseFormat (ObservableCollection<MusicInfo> infos,Func<MusicInfo,string> keySelecter)
        {
            MyStringComparer myComp = new MyStringComparer(CompareInfo.GetCompareInfo("zh-CN"), CompareOptions.None);
            return new ObservableCollection<MusicInfo>(infos.OrderBy(keySelecter,myComp));
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
    }
}
