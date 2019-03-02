using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Walkman_Playlist_Tools
{
    public static class Format
    {
        public static string LyricDelTime(string rawLyric)
        {
            if (rawLyric == null) return null;
            string[] lyricSplit = rawLyric.Split(new char[] {'\n'});
            if (lyricSplit.Length == 1) return rawLyric;
            else
            {
                string newLyric = null;
                foreach (string row in lyricSplit)
                {
                    string[] rowSplit = row.Split(new char[] {'[',']'});
                    newLyric += rowSplit[rowSplit.Length - 1] + '\n';
                }

                return newLyric;
            }
        }

        class CheckMultiArtist
        {
            private string artist1, artist2;
            private bool isAllEqual, isMultiArtist1 = false, isMutiArtist2 = false;
            private string[] artist1Split, artist2Split;

            public CheckMultiArtist(string artist1, string artist2, bool isAllEqual)
            {
                this.artist1 = artist1.Replace(" ","");
                this.artist2 = artist2.Replace(" ","");
                this.isAllEqual = isAllEqual;
                artist1Split = this.artist1.Split(new char[] { ';', '；' });
                artist2Split = this.artist2.Split(new char[] { ';', '；' });
                if (artist1Split.Length != 1) isMultiArtist1 = true;
                if (artist2Split.Length != 1) isMutiArtist2 = true;
            }

            private bool Contain()
            {
                foreach (var a1 in artist1Split)
                {
                    foreach (var a2 in artist2Split)
                    {
                        if (a1 == a2) return true;
                    }
                }

                return false;
            }

            private bool OneMulti(string one,string[] multi)
            {
                if (isAllEqual) return false;
                else
                {
                    foreach (string artists in multi)
                    {
                        if (artists == one) return true;
                    }

                    return false;
                }
            }

            private bool AllEqual(string[] artists1, string[] artists2)
            {
                if (artists1.Length != artists2.Length) return false;
                foreach (string artist in artists2)
                {
                    if (!artists1.Contains(artist)) return false;
                }

                return true;
            }

            public bool Check()
            {
                if (isMultiArtist1)
                {
                    if (isMutiArtist2)
                    {
                        if (isAllEqual) return AllEqual(artist1Split,artist2Split);
                        else
                        {
                            return Contain();
                        }
                    }
                    else return OneMulti(artist2, artist1Split);
                }
                else
                {
                    if (isMutiArtist2) return OneMulti(artist1, artist2Split);
                    else
                    {
                        return artist1 == artist2;
                    }
                }
            }
        }

        private static bool CheckArtist(string artist1, string artist2,bool isAllEqual)
        {
            return new CheckMultiArtist(artist1,artist2,isAllEqual).Check();
        }

        public static bool CheckArtistEqual(string artist1, string artist2,bool isAllEqual)
        {
            return CheckArtist(ChineseStringUtility.ToSimplified(artist1.ToLower()),
                ChineseStringUtility.ToSimplified(artist2.ToLower()), isAllEqual);
        }

        /// <summary>
        /// 判断字符串是否是数字
        /// </summary>
        public static bool IsNumber(string s)
        {
            if (string.IsNullOrWhiteSpace(s)) return false;
            const string pattern = "^[0-9]*$";
            Regex rx = new Regex(pattern);
            return rx.IsMatch(s);
        }

        public static string TransLyric(string rawLyric)
        {
            string resultLyric = null;
            if (rawLyric==null||!rawLyric.Contains('[')) return null;
            foreach (string row in rawLyric.Split(new char[] { '\n' }))
            {
                string[] rowSplitLyric = row.Split(new char[] { '[', ']' });
                for(int i=1;i<rowSplitLyric.Length-1;i++)
                {
                    string[] piece = rowSplitLyric[i].Split(new char[] { ':', '.',',' });
                    if (piece[0] != "")
                    {
                        resultLyric += "[";
                        if (IsNumber(piece[0]))
                        {
                            if (piece.Length == 3)
                            {
                                if (piece[2].Length == 3)
                                {
                                    piece[2] = piece[2].Remove(2);
                                }

                                resultLyric += piece[0] + ":" + piece[1] + "." + piece[2];
                            }
                            else
                            {
                                resultLyric += piece[0] + ":" + piece[1];
                            }
                        }
                        else if(piece.Length==2)resultLyric += piece[0] + ":" + piece[1];
                        else
                        {
                            resultLyric += rowSplitLyric[i];
                        }

                        resultLyric += "]";
                    }
                }

                resultLyric += rowSplitLyric[rowSplitLyric.Length - 1] + "\n";
            }

            return resultLyric;
        }
    }

    /// <summary>
    /// 中文字符工具类
    /// </summary>
    public static class ChineseStringUtility
    {
        private const int LOCALE_SYSTEM_DEFAULT = 0x0800;
        private const int LCMAP_SIMPLIFIED_CHINESE = 0x02000000;
        private const int LCMAP_TRADITIONAL_CHINESE = 0x04000000;

        [DllImport("kernel32", CharSet = CharSet.Auto, SetLastError = true)]
        private static extern int LCMapString(int Locale, int dwMapFlags, string lpSrcStr, int cchSrc,
            [Out] string lpDestStr, int cchDest);

        /// <summary>
        /// 将字符转换成简体中文
        /// </summary>
        /// <param name="source">输入要转换的字符串</param>
        /// <returns>转换完成后的字符串</returns>
        public static string ToSimplified(string source)
        {
            String target = new String(' ', source.Length);
            int ret = LCMapString(LOCALE_SYSTEM_DEFAULT, LCMAP_SIMPLIFIED_CHINESE, source, source.Length, target,
                source.Length);
            return target;
        }
    }
}
