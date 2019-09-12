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
    public class MusicInfoTests
    {
        [TestMethod()]
        public void ConvertTimeTest()
        {
            Assert.IsTrue(MusicInfo.ConvertTime("11/9/2018 3:16 PM")=="2018/9/11 15:16");
        }
    }
}