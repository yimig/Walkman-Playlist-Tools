using Microsoft.VisualStudio.TestTools.UnitTesting;
using Walkman_Playlist_Tools;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Walkman_Playlist_Tools.Tests
{
    [TestClass()]
    public class LoadWindowTests
    {
        [TestMethod()]
        public void DistinctTest()
        {
            List<MusicInfo> orginInfos=new List<MusicInfo>();
            MusicInfo info1=new MusicInfo();
            info1.Path = "AAA";
            MusicInfo info2=new MusicInfo();
            info2.Path = "BBB";
            MusicInfo info3=new MusicInfo();
            info3.Path = "CCC";
            orginInfos.Add(info1);
            orginInfos.Add(info2);
            orginInfos.Add(info3);
            List<string>delPaths=new List<string>();
            //delPaths.Add("BBB");
            delPaths.Add("CCC");
            List<MusicInfo> resultInfos=new List<MusicInfo>();
            resultInfos.Add(info1);
            Assert.IsTrue(LoadWindow.Distinct(orginInfos,delPaths)[1].Path=="BBB");
        }
    }
}