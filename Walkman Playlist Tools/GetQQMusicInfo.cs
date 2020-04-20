using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Net;
using System.Net.Security;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;
using HtmlAgilityPack;
using Newtonsoft.Json;
using System.Web;
using Newtonsoft.Json.Converters;
using System.Runtime.Serialization;
using System.Text.RegularExpressions;

namespace Walkman_Playlist_Tools
{
    public class GetQQMusicInfo
    {
        private string artist, album, title, lyric, url, id,mid;
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

        public string Mid
        {
            get { return mid; }
            set { mid = value; }
        }

        /// <summary>
        /// 根据音乐ID和MID得到音乐信息
        /// </summary>
        /// <param name="id"></param>
        /// <param name="mid"></param>
        public GetQQMusicInfo(string id, string mid)
        {
            this.id = id;
            this.mid = mid;
        }

        /// <summary>
        /// 上传请求
        /// </summary>
        /// <param name="source">URL上传地址</param>
        /// <returns>请求数据</returns>
        private static HttpWebResponse SentLyricRequest(string mid)
        {
            HttpWebRequest webRequest = (HttpWebRequest)WebRequest.Create(new Uri("https://c.y.qq.com/lyric/fcgi-bin/fcg_query_lyric_new.fcg?-=MusicJsonCallback_lrc&pcachetime=1568550703813&songmid="+mid+"&g_tk=5381&loginUin=0&hostUin=0&format=json&inCharset=utf8&outCharset=utf-8&notice=0&platform=yqq.json&needNewCode=0"));
            webRequest.UserAgent =
                "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/70.0.3538.110 Safari/537.36";
            webRequest.Accept = "application/json, text/javascript, */*; q=0.01";
            webRequest.Referer = "https://y.qq.com/portal/player.html";
            webRequest.Headers.Add("DNT", "1");
            webRequest.Headers.Add("Sec-Fetch-Mode", "cors");
            webRequest.Method = "GET";
            return (HttpWebResponse)webRequest.GetResponse();
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
        /// 得到音乐搜索结果
        /// </summary>
        /// <param name="title"></param>
        /// <returns></returns>
        private static QQMusicSearchData GetSearchResult(string title)
        {
            WebClient webClient = new WebClient();
            webClient.Encoding = Encoding.UTF8;
            string rawjson = webClient.DownloadString(
                @"https://c.y.qq.com/soso/fcgi-bin/search_cp?remoteplace=txt.yqq.center&searchid=37159670560306796&t=0&aggr=1&cr=1&catZhIDA=1&lossless=0&flag_qc=0&p=1&n=100&w=" +
                title +
                "&g_tk=5381&jsonpCallback=searchCallbacksong1291&loginUin=0&hostUin=0&format=jsonp&inCharset=utf8&outCharset=utf-8&notice=0&platform=yqq&needNewCode=0");
            string[] datas=rawjson.Split(new char[] {'(', ')'});
            string json = null;
            for (int i = 1; i < datas.Length - 1; i++)
            {
                json += datas[i];
            }
            JsonSerializerSettings setting=new JsonSerializerSettings();
            setting.NullValueHandling = NullValueHandling.Ignore;
            QQMusicSearchData resultData = JsonConvert.DeserializeObject<QQMusicSearchData>(json,setting);//反序列化
            return resultData;
        }

        /// <summary>
        /// 提取搜索结果中有用的信息
        /// </summary>
        /// <param name="title">搜索标题</param>
        /// <returns></returns>
        public static FiltratedData[] GetFiltratedSearchResult(string title)
        {
            QQMusicSearchData rawdata = GetSearchResult(title);
            FiltratedData[] datas=new FiltratedData[rawdata.data.song.list.Count];
            for (int i = 0; i < datas.Length; i++)
            {
                datas[i]=new FiltratedData();
                datas[i].Album = rawdata.data.song.list[i].albumname;
                foreach (var singer in rawdata.data.song.list[i].singer)
                {
                    if (datas[i].Aritist != null) datas[i].Aritist += "/" + singer.name;
                    else datas[i].Aritist = singer.name;
                }
                datas[i].SongId = rawdata.data.song.list[i].songid;
                datas[i].SongMid = rawdata.data.song.list[i].songmid;
                datas[i].SongTitle = rawdata.data.song.list[i].songname;
            }
            return datas;
        }

        /// <summary>
        /// 解码Base64字符串
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        private static string UnBase64String(string value)
        {
            if (value == null || value == "")
            {
                return "";
            }
            byte[] bytes = Convert.FromBase64String(value);
            return Encoding.UTF8.GetString(bytes);
        }

        /// <summary>
        /// 根据歌曲mid得到歌词
        /// </summary>
        /// <param name="mid"></param>
        /// <returns></returns>
        public static string GetLyric(string mid)
        {
            Stream rawStream = UnzipStream(SentLyricRequest(mid));
            StreamReader rawReader=new StreamReader(rawStream,Encoding.GetEncoding("GBK"));
            string rawInfo= rawReader.ReadToEnd();
            JsonSerializerSettings setting = new JsonSerializerSettings();
            setting.NullValueHandling = NullValueHandling.Ignore;
            RawLyricData rawLyricData = JsonConvert.DeserializeObject<RawLyricData>(rawInfo, setting);
            rawStream.Close();
            return HttpUtility.HtmlDecode(UnBase64String(rawLyricData.lyric));
        }

    }
}
