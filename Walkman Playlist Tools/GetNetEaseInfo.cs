using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Net;
using System.Security.Policy;
using System.Text;
using HtmlAgilityPack;
using Un4seen.Bass.AddOn.Tags;

namespace Walkman_Playlist_Tools
{
    class GetNetEaseInfo
    {
        private string artist, album, title, lyric,url, id;
        private Image image;

        public string Artist
        {
            get { return artist; }
        }

        public string Album
        {
            get { return album; }
        }

        public string Title
        {
            get { return title; }
        }

        public string Url
        {
            get { return url; }
            set { url = value; }
        }

        public string Id
        {
            get { return id; }
            set { id = value; }
        }

        public string Lyric
        {
            get { return lyric; }
        }

        public Image Image
        {
            get { return image; }
        }

        /// <summary>
        /// 根据ID得到音乐信息
        /// </summary>
        /// <param name="id">音乐id</param>
        public GetNetEaseInfo(string id)
        {
            this.id = id;
            url = "https://music.163.com/song?id=" + id;
            image=GetNetEaseTAA(out this.title, out this.artist, out this.album);
            lyric = GetLyric(id);
        }

        /// <summary>
        /// 根据URL得到音乐信息
        /// </summary>
        /// <param name="url">URL</param>
        public GetNetEaseInfo(Url url)
        {
            this.url = url.Value;
            string[] preurl = this.url.Split(new string[] {"id="}, StringSplitOptions.RemoveEmptyEntries);
            id = preurl[1];
            image = GetNetEaseTAA(out title, out artist, out album);
            lyric = GetLyric(id);
        }

        /// <summary>
        /// 处理网易云歌单原地址，并返回歌曲ID
        /// </summary>
        /// <param name="preURL">原地址</param>
        /// <returns>歌曲ID</returns>
        private static string GetPlaylistID(string preURL)
        {
            string[] SplitedURL = preURL.Split(new char[] {'/'});
            string checktag = SplitedURL[3];
            if (checktag == "#" || checktag == "m") return preURL.Split(new char[] {'=', '&'})[1];
            else if (SplitedURL.Any(p=>p.Contains("playlist")))
                return SplitedURL[3].Split(new char[] {'=', '&'})[1];
            else return preURL.Split(new char[] {'/'})[4];
        }

        /// <summary>
        /// 得到歌词
        /// </summary>
        /// <param name="id">音乐id</param>
        /// <returns>歌词</returns>
        public static string GetLyric(string id)
        {
            Stream RawStream = UnzipStream(SentInfo(@"http://music.163.com/api/song/lyric?id=" + id + "&lv=1&kv=1&tv=-1"));
            StreamReader RawReader = new StreamReader(RawStream, Encoding.UTF8);
            string[] rawdata = RawReader.ReadToEnd().Split(new string[] { "\"lrc\":" }, StringSplitOptions.RemoveEmptyEntries);
            if (rawdata[0].Contains("\"nolyric\":true")||rawdata[0].Contains("\"uncollected\":true")) return null;
            string[] predata = rawdata[1].Split(new char[] { '{', '}' });
            RawStream.Close();
            string[] data = predata[1].Split(new char[] { '\"' });
            string lyric = null;
            for (int i = 5; i < data.Length; i++) lyric += data[i];
            return lyric.Replace("\\n", "\n");
        }

        /// <summary>
        /// 得到歌曲封面
        /// </summary>
        /// <param name="id">音乐</param>
        /// <param name="title">得到的音乐标题（out）</param>
        /// <param name="artist">得到的艺术家（out）</param>
        /// <param name="album">得到的专辑名称（out）</param>
        /// <returns>专辑封面</returns>
        Image GetNetEaseTAA(out string title, out string artist, out string album)
        {
            HttpWebResponse web = SentInfo(url);
            Stream responseStream = UnzipStream(web);
            HtmlDocument html = new HtmlDocument();
            html.Load(responseStream, Encoding.UTF8);
            responseStream.Close();
            HtmlNodeCollection titlenodes = html.DocumentNode.SelectNodes("//meta[@name='keywords']");
            HtmlNodeCollection imgnodes = html.DocumentNode.SelectNodes("//meta[@property='og:image']");
            HtmlNodeCollection descripnodes = html.DocumentNode.SelectNodes("//meta[@name='description']");
            string[] titledata = titlenodes[0].Attributes["content"].Value.Split(new char[] { '，' });
            string[] descdata = descripnodes[0].Attributes["content"].Value.Split(new char[] { '：', '。' });
            title = HtmlDiscode(titledata[0]);
            artist = HtmlDiscode(descdata[1]);
            album = HtmlDiscode(descdata[3]);
            return GetImg(imgnodes[0].Attributes["content"].Value);
        }

        /// <summary>
        /// 解压网页回传的数据流
        /// </summary>
        /// <param name="web">请求数据</param>
        /// <returns>解压后的数据流</returns>
        static Stream UnzipStream(HttpWebResponse web)
        {
            Stream responseStream = web.GetResponseStream();
            if (web.ContentEncoding.ToLower().Contains("gzip"))
                responseStream = new GZipStream(responseStream, CompressionMode.Decompress);
            else if (web.ContentEncoding.ToLower().Contains("deflate"))
                responseStream = new DeflateStream(responseStream, CompressionMode.Decompress);
            return responseStream;
        }

        /// <summary>
        /// 得到网络图片
        /// </summary>
        /// <param name="URL"></param>
        /// <returns></returns>
        Image GetImg(string URL)
        {
            var stream = System.Net.WebRequest.Create(URL).GetResponse().GetResponseStream();
            Image image = Image.FromStream(stream);
            stream.Close();
            return image;
        }

        /// <summary>
        /// 上传请求
        /// </summary>
        /// <param name="source">URL上传地址</param>
        /// <returns>请求数据</returns>
        static public HttpWebResponse SentInfo(string source)
        {
            HttpWebRequest webRequest = (HttpWebRequest)WebRequest.Create(new Uri(source));
            webRequest.KeepAlive = false;
            webRequest.Host = "music.163.com";
            webRequest.UserAgent =
                "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/70.0.3538.110 Safari/537.36";
            webRequest.Accept = "text/html,application/xhtml+xml,application/xml;q=0.9,image/webp,image/apng,*/*;q=0.8";
            webRequest.Referer = "https://music.163.com/";
            webRequest.Headers.Add("Upgrade-Insecure-Requests", "1");
            webRequest.Headers.Add("Accept-Encoding", "gzip, deflate, br");
            webRequest.Headers.Add("Accept-Language", "zh-CN,zh;q=0.9,en-US;q=0.8,en;q=0.7");
            webRequest.Method = "GET";
            return (HttpWebResponse)webRequest.GetResponse();
        }

        ///<summary>
        ///恢复html中的特殊字符
        ///</summary>
        ///<param name="theString">需要恢复的文本。</param>
        ///<returns>恢复好的文本。</returns>
        public static string HtmlDiscode(string theString)
        {
            theString = theString.Replace("&gt;", ">");
            theString = theString.Replace("&lt;", "<");
            theString = theString.Replace("&nbsp;", " ");
            theString = theString.Replace("&quot;", "\"");
            theString = theString.Replace("&#39;", "\'");
            theString = theString.Replace("<br/>", "\n");
            theString = theString.Replace("&amp;", "&");
            return theString;
        }

        /// <summary>
        /// 得到网易云歌单
        /// </summary>
        /// <param name="preurl">未处理的原地址</param>
        /// <param name="title">歌单名</param>
        /// <returns>歌单歌曲</returns>
        public static List<string> GetPlaylist(string preurl,out string title)
        {
            List<string> list=new List<string>();
            //string[] suburl = preurl.Split(new string[] {"/"}, StringSplitOptions.RemoveEmptyEntries);
            Stream rawStream = UnzipStream(SentInfo(@"https://music.163.com/playlist?id=" + GetPlaylistID(preurl)));
            HtmlDocument html=new HtmlDocument();
            html.Load(rawStream, Encoding.UTF8);
            rawStream.Close();
            HtmlNodeCollection listNodes = html.DocumentNode.SelectNodes("//ul[@class='f-hide']");
            HtmlNodeCollection titlenodes = html.DocumentNode.SelectNodes("//meta[@property='og:title']");
            string[] titledata = titlenodes[0].Attributes["content"].Value.Split(new string[]{ " -" },StringSplitOptions.RemoveEmptyEntries);
            title = titledata[0];
            foreach (HtmlNode node in listNodes[0].ChildNodes)
            {
                list.Add(HtmlDiscode(node.FirstChild.InnerText.Trim()));
            }

            return list;

        }
    }
}
