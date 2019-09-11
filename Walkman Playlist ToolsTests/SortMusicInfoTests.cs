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
            MusicInfo info1=new MusicInfo();
            info1.Title = "A";
            MusicInfo info2=new MusicInfo();
            info2.Title = "B";
            MusicInfo info3=new MusicInfo();
            info3.Title = "阿";
            MusicInfo info4=new MusicInfo();
            info4.Title = "珍";
            MusicInfo info5=new MusicInfo();
            info5.Title = "5";
            MusicInfo info6=new MusicInfo();
            info6.Title = "/";
            var testCollection= new ObservableCollection<MusicInfo>();
            testCollection.Add(info4);
            testCollection.Add(info1);
            testCollection.Add(info6);
            testCollection.Add(info2);
            testCollection.Add(info5);
            testCollection.Add(info3);
            var resultCollection = SortMusicInfo.ByChineseFormat(testCollection,i=>i.Title);
        }
    }
}