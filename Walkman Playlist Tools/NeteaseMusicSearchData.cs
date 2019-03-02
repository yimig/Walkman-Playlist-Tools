using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace Walkman_Playlist_Tools
{
        class NeteaseMusicSearchData
        {
            [JsonProperty("result")] public Result result;
            [JsonProperty("code")] public int code;
        }

        class Result
        {
            [JsonProperty("songs")] public List<Song> songs;
            [JsonProperty("songCount")] public int songCount;
        }

        class Song
        {
            [JsonProperty("id")] public string id;
            [JsonProperty("name")] public string name;
            [JsonProperty("artists")] public List<Artist> artists;
            [JsonProperty("album")] public Album album;
            [JsonProperty("duration")] public string duration;
            [JsonProperty("copyrightId")] public string copyrightId;
            [JsonProperty("statues")] public int statues;
            [JsonProperty("alias")] public List<string> alias;
            [JsonProperty("rtype")] public int rtype;
            [JsonProperty("ftype")] public int ftype;
            [JsonProperty("mvid")] public string mvid;
            [JsonProperty("fee")] public int fee;
            [JsonProperty("rUrl")] private string rUrl;
        }

        class Artist
        {
            [JsonProperty("id")] public string id;
            [JsonProperty("name")] public string name;
            [JsonProperty("picUrl")] public string picUrl;
            [JsonProperty("alias")] public List<string> alias;
            [JsonProperty("albumSize")] public int albumSize;
            [JsonProperty("picId")] public string picId;
            [JsonProperty("imglvUrl")] public string imglvUrl;
            [JsonProperty("imglvl")] public int imglvl;
            [JsonProperty("trans")] public string trans;
        }

        class Album
        {
            [JsonProperty("id")] public string id;
            [JsonProperty("name")] public string name;
            [JsonProperty("artist")] public Artist artist;
            [JsonProperty("publishTime")] public string publicTime;
            [JsonProperty("size")] public int size;
            [JsonProperty("copyrightId")] public string copyrightId;
            [JsonProperty("statues")] public int statues;
            [JsonProperty("picId")] public string picId;
        }
   
}
