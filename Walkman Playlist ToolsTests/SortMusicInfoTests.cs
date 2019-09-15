using Microsoft.VisualStudio.TestTools.UnitTesting;
using Walkman_Playlist_Tools;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Walkman_Playlist_Tools.Tests
{
    [TestClass()]
    public class SortMusicInfoTests
    {
        [TestMethod()]
        public void ByTitleTest()
        {
            MusicInfo info1 = new MusicInfo();
            info1.Title = "A";
            MusicInfo info2 = new MusicInfo();
            info2.Title = "B";
            MusicInfo info3 = new MusicInfo();
            info3.Title = "阿";
            MusicInfo info4 = new MusicInfo();
            info4.Title = "珍";
            MusicInfo info5 = new MusicInfo();
            info5.Title = "5";
            MusicInfo info6 = new MusicInfo();
            info6.Title = "/";
            var testCollection = new ObservableCollection<MusicInfo>();
            testCollection.Add(info4);
            testCollection.Add(info1);
            testCollection.Add(info6);
            testCollection.Add(info2);
            testCollection.Add(info5);
            testCollection.Add(info3);
            //var resultCollection = SortMusicInfo.ByChineseFormat(testCollection, i => i.Title);
        }

        [TestMethod()]
        public void ByNumberFormatTest()
        {
            MusicInfo info1 = new MusicInfo();
            info1.Title = "2017/2/1 11:16";
            MusicInfo info2 = new MusicInfo();
            info2.Title = "2018/3/2 1:30";
            MusicInfo info3 = new MusicInfo();
            info3.Title = "2010/10/9 19:50";
            MusicInfo info4 = new MusicInfo();
            info4.Title = "2020/1/1 1:1";
            MusicInfo info5 = new MusicInfo();
            info5.Title = "2050/3/1 0:0";
            MusicInfo info6 = new MusicInfo();
            info6.Title = "1992/12/25/15:00";
            MusicInfo info7 = new MusicInfo();
            info7.Title = "752";
            MusicInfo info8 = new MusicInfo();
            info8.Title = "560";
            var testCollection = new ObservableCollection<MusicInfo>();
            /*
            testCollection.Add(info4);
            testCollection.Add(info1);
            testCollection.Add(info6);
            testCollection.Add(info2);
            testCollection.Add(info5);
            testCollection.Add(info3);*/
            testCollection.Add(info7);
            testCollection.Add(info8);
            //var resultCollection = SortMusicInfo.ByNumberFormat(testCollection, i => i.Title);
        }
    }
}