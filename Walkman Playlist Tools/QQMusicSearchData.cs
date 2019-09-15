using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Util;
using System.Windows.Forms.VisualStyles;
using Newtonsoft.Json;

namespace Walkman_Playlist_Tools
{



    class QQMusicSearchData
    {
        [JsonProperty("code")]
        public int code { get; set; }
        [JsonProperty("data")]
        public SearchData data { get; set; }
        [JsonProperty("message")]
        public string message { get; set; }
        [JsonProperty("notice")]
        public string notice { get; set; }
        [JsonProperty("subcode")]
        public int subcode { get; set; }
        [JsonProperty("time")]
        public int time { get; set; }
        [JsonProperty("tips")]
        public string tips { get; set; }
    }

    class SearchData
    {
        [JsonProperty("keyword")]
        public string keyword { get; set; }
        [JsonProperty("priority")]
        public int priority { get; set; }
        [JsonProperty("qc")]
        public List<QC> qc { get; set; }
        [JsonProperty("semantic")]
        public Semantic semantic { get; set; }
        [JsonProperty("song")]
        public Songs song { get; set; }
        [JsonProperty("totaltime")]
        public int totaltime { get; set; }
        [JsonProperty("zhida")]
        public ZhiDa zhida { get; set; }
    }

    class Semantic
    {
        [JsonProperty("curnum")]
        public int curnum { get; set; }
        [JsonProperty("curpage")]
        public int curpage { get; set; }
        [JsonProperty("list")]
        public List<MList> list { get; set; }
        [JsonProperty("totalnum")]
        public int totalnum { get; set; }
    }

    class Songs
    {
        [JsonProperty("curnum")]
        public int curnum { get; set; }
        [JsonProperty("curpage")]
        public int curpage { get; set; }
        [JsonProperty("list")]
        public List<MList> list { get; set; }
        [JsonProperty("totalnum")]
        public int totalnum { get; set; }
    }

    class ZhiDa
    {
        [JsonProperty("chinesesinger")]
        public int chinesesinger { get; set; }
        [JsonProperty("type")]
        public int type { get; set; }
    }

    class MList
    {
        [JsonProperty("albumid")]
        public int albumid { get; set; }
        [JsonProperty("albummid")]
        public string albummid { get; set; }
        [JsonProperty("albumname")]
        public string albumname { get; set; }
        [JsonProperty("albumname_hilight")]
        public string albumname_hilight { get; set; }
        [JsonProperty("albumtransname")]
        public string albumtransname { get; set; }
        [JsonProperty("albumtransname_hilight")]
        public string albumtransname_hilight { get; set; }
        [JsonProperty("alertid")]
        public int alertid { get; set; }
        [JsonProperty("chinesesinger")]
        public int chinesesinger { get; set; }
        [JsonProperty("docid")]
        public string docid { get; set; }
        [JsonProperty("format")]
        public string format { get; set; }
        /*[JsonProperty("grp")]
        public List<string> grp { get; set; }*/
        [JsonProperty("interval")]
        public int interval { get; set; }
        [JsonProperty("isonly")]
        public bool isonly { get; set; }
        [JsonProperty("lyric")]
        public string lyric { get; set; }
        [JsonProperty("lyric_hilight")]
        public string lyric_hilight { get; set; }
        [JsonProperty("media_mid")]
        public string media_mid { get; set; }
        [JsonProperty("msgid")]
        public int msgid { get; set; }
        [JsonProperty("nt")]
        public string nt { get; set; }
        [JsonProperty("pay")]
        public Pay pay { get; set; }
        [JsonProperty("preview")]
        public Preview preview { get; set; }
        [JsonProperty("pubtime")]
        public int pubtime { get; set; }
        [JsonProperty("pure")]
        public int pure { get; set; }
        [JsonProperty("singer")]
        public List<Singer> singer { get; set; }
        [JsonProperty("size128")]
        public int size128 { get; set; }
        [JsonProperty("size320")]
        public int size320 { get; set; }
        [JsonProperty("sizeape")]
        public int sizeape { get; set; }
        [JsonProperty("sizeflac")]
        public int sizeflac { get; set; }
        [JsonProperty("sizeogg")]
        public int sizeogg { get; set; }
        [JsonProperty("songid")]
        public int songid { get; set; }
        [JsonProperty("songmid")]
        public string songmid { get; set; }
        [JsonProperty("songname")]
        public string songname { get; set; }
        [JsonProperty("songname_hilight")]
        public string songname_hilight { get; set; }
        [JsonProperty("songurl")]
        public string songurl { get; set; }
        [JsonProperty("strMediaMid")]
        public string strMediaMid { get; set; }
        [JsonProperty("stream")]
        public int stream { get; set; }
        [JsonProperty("switch")]
        public int @switch { get; set; }
        [JsonProperty("t")]
        public int t { get; set; }
        [JsonProperty("tag")]
        public int tag { get; set; }
        [JsonProperty("type")]
        public int type { get; set; }
        [JsonProperty("ver")]
        public int ver { get; set; }
        [JsonProperty("vid")]
        public string vid { get; set; }
    }

    class Pay
    {
        [JsonProperty("payalbum")]
        public int payalbum { get; set; }
        [JsonProperty("payalbumprice")]
        public int payalbumprice { get; set; }
        [JsonProperty("paydownload")]
        public int paydownload { get; set; }
        [JsonProperty("payinfo")]
        public int payinfo { get; set; }
        [JsonProperty("payplay")]
        public int payplay { get; set; }
        [JsonProperty("paytrackmouth")]
        public int paytrackmouth { get; set; }
        [JsonProperty("paytrackprice")]
        public int paytrackprice { get; set; }
    }

    class Preview
    {
        [JsonProperty("trybegin")]
        public int trybegin { get; set; }
        [JsonProperty("tryend")]
        public int tryend { get; set; }
        [JsonProperty("trysize")]
        public int trysize { get; set; }
    }

    class Singer
    {
        [JsonProperty("id")]
        public int id { get; set; }
        [JsonProperty("mid")]
        public string mid { get; set; }
        [JsonProperty("name")]
        public string name { get; set; }
        [JsonProperty("name_hilight")]
        public string name_hilight { get; set; }
    }

    class QC
    {
        [JsonProperty("text")]
        public string text { get; set; }
        [JsonProperty("type")]
        public int type { get; set; }
    }

    public class FiltratedData
    {
        private int songID;
        private string songMID;
        private string songTitle;
        private string aritist;
        private string album;

        public int SongId
        {
            get { return songID; }
            set { songID = value; }
        }

        public string SongMid
        {
            get { return songMID; }
            set { songMID = value; }
        }

        public string SongTitle
        {
            get { return songTitle; }
            set { songTitle = value; }
        }

        public string Aritist
        {
            get { return aritist; }
            set { aritist = value; }
        }

        public string Album
        {
            get { return album; }
            set { album = value; }
        }
    }

    public class RawLyricData
    {
        [JsonProperty("code")]
        public int code { get; set; }
        [JsonProperty("lyric")]
        public string lyric { get; set; }
        [JsonProperty("retcode")]
        public int retcode { get; set; }
        [JsonProperty("subcode")]
        public int subcode { get; set; }
        [JsonProperty("trans")]
        public string trans { get; set; }
    }
}
