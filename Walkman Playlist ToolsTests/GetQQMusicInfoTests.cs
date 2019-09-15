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
    public class GetQQMusicInfoTests
    {
        [TestMethod()]
        public void GetLyricTest()
        {
            string result = GetQQMusicInfo.GetLyric("00255UlM3SljUh");
        }
    }
}