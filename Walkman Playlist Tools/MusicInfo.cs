using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using Shell32;
using Un4seen.Bass;
using Un4seen.Bass.AddOn.Flac;
using System.Windows.Media;
using Brush = System.Windows.Media.Brush;

namespace Walkman_Playlist_Tools
{
    // { "标题", "格式", "艺术家", "专辑", "时长", "年代", "创建时间", "文件路径" }
    public class MusicInfo : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;
        private string title, format, artist, album, path, buildtime,year;
        private int length;
        private bool? check=false;
        private Brush brush;

        public string Title
        {
            get { return title; }
            set
            {
                title = value;
                if (PropertyChanged != null)
                {
                    PropertyChanged(this, new PropertyChangedEventArgs("Title"));
                }
            }
        }

        public string Format
        {
            get { return format; }
            set
            {
                format = value;
                if (PropertyChanged != null)
                {
                    PropertyChanged(this, new PropertyChangedEventArgs("Format"));
                }
            }
        }

        public string Artist
        {
            get { return artist; }
            set
            {
                artist = value;
                if (PropertyChanged != null)
                {
                    PropertyChanged(this, new PropertyChangedEventArgs("Artist"));
                }
            }
        }

        public string Album
        {
            get { return album; }
            set
            {
                album = value;
                if (PropertyChanged != null)
                {
                    PropertyChanged(this, new PropertyChangedEventArgs("Album"));
                }
            }
        }

        public string Path
        {
            get { return path; }
            set
            {
                path = value;
                if (PropertyChanged != null)
                {
                    PropertyChanged(this, new PropertyChangedEventArgs("Path"));
                }
            }
        }

        public string Buildtime
        {
            get { return buildtime; }
            set
            {
                buildtime = value;
                if (PropertyChanged != null)
                {
                    PropertyChanged(this, new PropertyChangedEventArgs("Buildtime"));
                }
            }
        }

        public int Length
        {
            get { return length; }
            set
            {
                length = value;
                if (PropertyChanged != null)
                {
                    PropertyChanged(this, new PropertyChangedEventArgs("Length"));
                }
            }
        }

        public string Year
        {
            get { return year; }
            set
            {
                year = value;
                if (PropertyChanged != null)
                {
                    PropertyChanged(this, new PropertyChangedEventArgs("Year"));
                }
            }
        }

        public bool? Check
        {
            get { return check; }
            set
            {
                check = value;
                if (PropertyChanged != null)
                {
                    PropertyChanged(this, new PropertyChangedEventArgs("Check"));
                }
            }
        }

        public Brush Brush
        {
            get { return brush; }
            set
            {
                brush = value;
                if (PropertyChanged != null)
                {
                    PropertyChanged(this, new PropertyChangedEventArgs("Brush"));
                }
            }
        }

        /// <summary>
        /// 建立一个音乐信息档案（后期补充信息）
        /// </summary>
        public MusicInfo() { }

        /// <summary>
        /// 建立一个音乐的歌曲信息档案
        /// </summary>
        /// <param name="path">音乐所在地址</param>
        public MusicInfo(string path)
        {
            this.path = path;
            switch (Setting.Default.ScanMode)
            {
                case 0: UseShellOnly(path); break;
                case 1: UseAll(path); break;
                case 2: UseBassOnly(path); break;
            }
        }

        /// <summary>
        /// 只使用Shell32读取音乐信息
        /// </summary>
        /// <param name="path">音乐地址</param>
        private void UseShellOnly(string path)
        {
            Shell32.Shell sh = new Shell();
            Folder dir = sh.NameSpace(System.IO.Path.GetDirectoryName(path));
            FolderItem item = dir.ParseName(System.IO.Path.GetFileName(path));
            format = Get_Format(path);
            buildtime = Get_BuildTime(dir, item);
            title = Shell_Title(dir, item, path);
            artist = Shell_Artist(dir, item);
            album = Shell_Album(dir, item);
            year = Shell_Year(dir, item);
            length = Shell_Length(dir, item);
        }

        /// <summary>
        /// 使用Shell32读取mp3、wav、flac音乐信息，其他使用Bass读取音乐信息
        /// </summary>
        /// <param name="path">音乐地址</param>
        private void UseAll(string path)
        {
            format = Get_Format(path);
            if (MainWindow.CheckExtention(new string[] {"mp3","flac","wav" }, format)) UseShellOnly(path);
            else UseBassOnly(path);
        }

        /// <summary>
        /// 只使用Bass读取音乐信息
        /// </summary>
        /// <param name="path">音乐地址</param>
        private void UseBassOnly(string path)
        {
            Shell sh = new Shell();
            Folder dir = sh.NameSpace(System.IO.Path.GetDirectoryName(path));
            FolderItem item = dir.ParseName(System.IO.Path.GetFileName(path));
            int stream = Bass.BASS_StreamCreateFile(path, 0L, 0L, BASSFlag.BASS_SAMPLE_FLOAT);
            format = Get_Format(path);
            buildtime = Get_BuildTime(dir, item);
            title = Bass_Title(path, stream, format);
            artist = Bass_Artist(path, stream, format);
            album = Bass_Album(stream, format);
            year = Bass_Year(stream, format);
            length = Bass_Length(path, stream);
            Bass.BASS_StreamFree(stream);
        }

        /// <summary>
        /// 获得音乐格式（扩展名）
        /// </summary>
        /// <param name="path">音乐地址</param>
        /// <returns>格式</returns>
        private string Get_Format(string Path)
        {
            return System.IO.Path.GetExtension(path);
        }

        /// <summary>
        /// 获得音乐文件建立时间
        /// </summary>
        /// <param name="dir"></param>
        /// <param name="item"></param>
        /// <returns>建立时间</returns>
        private string Get_BuildTime(Folder dir, FolderItem item)
        {
            return ConvertTime(dir.GetDetailsOf(item, 4));
        }

        /// <summary>
        /// 使用Shell获得音乐标题
        /// </summary>
        /// <param name="dir"></param>
        /// <param name="item"></param>
        /// <param name="path">音乐地址</param>
        /// <returns>标题</returns>
        private string Shell_Title(Folder dir, FolderItem item, string path)
        {
            var content = dir.GetDetailsOf(item, 21);
            if (content == "") content = System.IO.Path.GetFileNameWithoutExtension(path);
            return content;
        }

        /// <summary>
        /// 使用Shell获得歌曲艺术家
        /// </summary>
        /// <param name="dir"></param>
        /// <param name="item"></param>
        /// <returns>艺术家</returns>
        private string Shell_Artist(Folder dir, FolderItem item)
        {
            return dir.GetDetailsOf(item, 20);
        }

        /// <summary>
        /// 使用Shell获得音乐专辑名
        /// </summary>
        /// <param name="dir"></param>
        /// <param name="item"></param>
        /// <returns>专辑名</returns>
        private string Shell_Album(Folder dir, FolderItem item)
        {
            return dir.GetDetailsOf(item, 14);
        }

        /// <summary>
        /// 使用Shell获得音乐年代
        /// </summary>
        /// <param name="dir"></param>
        /// <param name="item"></param>
        /// <returns>年代</returns>
        private string Shell_Year(Folder dir, FolderItem item)
        {
            return dir.GetDetailsOf(item, 15);
        }

        /// <summary>
        /// 使用Shell获得歌曲长度
        /// </summary>
        /// <param name="dir"></param>
        /// <param name="item"></param>
        /// <returns>曲长</returns>
        private int Shell_Length(Folder dir, FolderItem item)
        {
            if (dir.GetDetailsOf(item, 27) != "")
            {
                string hor = dir.GetDetailsOf(item, 27).Substring(0, 2);
                string min = dir.GetDetailsOf(item, 27).Substring(3, 2);
                string sec = dir.GetDetailsOf(item, 27).Substring(6, 2);
                return Convert.ToInt32(hor) * 360 + Convert.ToInt32(min) * 60 + Convert.ToInt32(sec);
            }
            return 0;
        }

        /// <summary>
        /// 使用Bass获得音乐标题
        /// </summary>
        /// <param name="stream">Bass音乐流</param>
        /// <param name="extension">音乐格式（扩展名）</param>
        /// <returns>标题</returns>
        private string Bass_Title(string Path, int stream, string extension)
        {
            string content = null;
            switch (extension)
            {
                case ".mp3":
                case ".m4a":
                case ".flac":
                case ".aif":
                case ".aac":
                    {
                        string[] tags = Bass.BASS_ChannelGetTagsID3V2(stream);
                        if (tags != null)
                        {
                            foreach (string x in tags)
                            {
                                if (x.Contains("TIT")) content = x.Remove(0, 5);
                                break;
                            }
                        }
                        break;
                    }
                case ".ogg":
                    {
                        string[] tags = Bass.BASS_ChannelGetTagsOGG(stream);
                        if (tags != null)
                        {
                            foreach (string x in tags)
                            {
                                if (x.Contains("TITLE"))
                                {
                                    content = x.Remove(0, 6);
                                    break;
                                }
                                else content = null;
                            }
                        }
                        break;
                    }
                case ".ape":
                    {
                        string[] tags = Bass.BASS_ChannelGetTagsAPE(stream);
                        if (tags != null)
                        {
                            foreach (string x in tags)
                            {
                                if (x.Contains("Title"))
                                {
                                    content = x.Remove(0, 6);
                                    break;
                                }
                            }
                        }
                        break;
                    }
                case ".dff":
                case ".dsf":
                    {
                        stream = Bass.BASS_StreamCreateFile(Path, 0L, 0L, BASSFlag.BASS_SAMPLE_FLOAT);
                        content = Bass.BASS_ChannelGetTagsDSDTitle(stream);
                        break;
                    }
            }

            if (content == null)
            {
                content = System.IO.Path.GetFileNameWithoutExtension(Path);
            }
            return content;
        }

        /// <summary>
        /// 使用Bass获得歌曲艺术家
        /// </summary>
        /// <param name="stream">Bass音乐流</param>
        /// <param name="extension">音乐格式（扩展名）</param>
        /// <returns>艺术家</returns>
        private string Bass_Artist(string Path, int stream, string extension)
        {
            string content = null;
            switch (extension)
            {
                case ".mp3":
                case ".m4a":
                case ".flac":
                case ".aif":
                case ".aac":
                    {
                        string[] tags = Bass.BASS_ChannelGetTagsID3V2(stream);
                        string[] tags7 = Bass.BASS_ChannelGetTagsID3V1(stream);
                        string[] tags8 = Bass.BASS_ChannelGetTagsArrayNullTermAnsi(stream, BASSTag.BASS_TAG_FLAC_CUE);
                        string[] tags9 = Bass.BASS_ChannelGetTagsArrayNullTermAnsi(stream, BASSTag.BASS_TAG_FLAC_PICTURE);
                        BASS_TAG_FLAC_PICTURE[] tags10 = Bass.BASS_ChannelGetTagsFLACPictures(stream);
                        string[] tags2 = Bass.BASS_ChannelGetTagsMETA(stream);
                        string[] tags3 = Bass.BASS_ChannelGetTagsBWF(stream);
                        string[] tags4 = Bass.BASS_ChannelGetTagsICY(stream);
                        string[] tags5 = Bass.BASS_ChannelGetTagsMF(stream);
                        string[] tags6 = Bass.BASS_ChannelGetTagsRIFF(stream);
                        if (tags != null)
                        {
                            foreach (string x in tags)
                            {
                                if (x.Contains("TPE"))
                                {
                                    content = x.Remove(0, 5);
                                    break;
                                }
                            }
                        }
                        break;
                    }
                case ".ogg":
                    {
                        string[] tags = Bass.BASS_ChannelGetTagsOGG(stream);
                        if (tags != null)
                        {
                            foreach (string x in tags)
                            {
                                if (x.Contains("ARTIST"))
                                {
                                    content = x.Remove(0, 7);
                                    break;
                                }
                                else content = null;
                            }
                        }
                        break;
                    }
                case ".ape":
                    {
                        string[] tags = Bass.BASS_ChannelGetTagsAPE(stream);
                        if (tags != null)
                        {
                            foreach (string x in tags)
                            {
                                if (x.Contains("Artist"))
                                {
                                    content = x.Split(new char[] {'='})[1];
                                    break;
                                }
                            }
                        }
                        break;
                    }
                case ".dff":
                case ".dsf":
                    {
                        stream = Bass.BASS_StreamCreateFile(Path, 0L, 0L, BASSFlag.BASS_SAMPLE_FLOAT);
                        content = Bass.BASS_ChannelGetTagsDSDArtist(stream);
                        break;
                    }
            }
            return content;
        }

        /// <summary>
        /// 使用Bass获得音乐专辑名
        /// </summary>
        /// <param name="stream">Bass音乐流</param>
        /// <param name="extension">音乐格式（扩展名）</param>
        /// <returns>专辑名</returns>
        private string Bass_Album(int stream, string extension)
        {
            string content = null;
            switch (extension)
            {
                case ".mp3":
                case ".m4a":
                case ".flac":
                case ".aif":
                case ".aac":
                    {
                        string[] tags = Bass.BASS_ChannelGetTagsID3V2(stream);
                        if (tags != null)
                        {
                            foreach (string x in tags)
                            {
                                if (x.Contains("TALB"))
                                {
                                    content = x.Remove(0, 5);
                                    break;
                                }
                            }
                        }
                        break;
                    }
                case ".ogg":
                    {
                        string[] tags = Bass.BASS_ChannelGetTagsOGG(stream);
                        if (tags != null)
                        {
                            foreach (string x in tags)
                            {
                                if (x.Contains("ALBUM"))
                                {
                                    content = x.Remove(0, 6);
                                    break;
                                }
                                else content = null;
                            }
                        }
                        break;
                    }
                case ".ape":
                    {
                        string[] tags = Bass.BASS_ChannelGetTagsAPE(stream);
                        if (tags != null)
                        {
                            foreach (string x in tags)
                            {
                                if (x.Contains("Album"))
                                {
                                    content = x.Split(new char[] {'='})[1];
                                    break;
                                }
                            }
                        }
                        break;
                    }
            }
            return content;
        }

        /// <summary>
        /// 使用Bass获得音乐年代
        /// </summary>
        /// <param name="stream">Bass音乐流</param>
        /// <param name="extension">音乐格式（扩展名）</param>
        /// <returns>年代</returns>
        private string Bass_Year(int stream, string extension)
        {
            string content = null;
            switch (extension)
            {
                case ".mp3":
                case ".m4a":
                case ".flac":
                case ".aif":
                case ".aac":
                    {
                        string[] tags = Bass.BASS_ChannelGetTagsID3V2(stream);
                        if (tags != null)
                        {
                            foreach (string x in tags)
                            {
                                if (x.Contains("TYER"))
                                {
                                    content = x.Remove(0, 5);
                                    break;
                                }
                            }
                        }
                        break;
                    }
                case ".ogg":
                    {
                        string[] tags = Bass.BASS_ChannelGetTagsOGG(stream);
                        if (tags != null)
                        {
                            foreach (string x in tags)
                            {
                                if (x.Contains("DATE"))
                                {
                                    content = x.Remove(0, 5);
                                    break;
                                }
                                else content = null;
                            }
                        }
                        break;
                    }
                case ".ape":
                    {
                        string[] tags = Bass.BASS_ChannelGetTagsAPE(stream);
                        if (tags != null)
                        {
                            foreach (string x in tags)
                            {
                                if (x.Contains("Year"))
                                {
                                    content = x.Remove(0, 5);
                                    break;
                                }
                            }
                        }
                        break;
                    }
            }
            return content;
        }

        /// <summary>
        /// 使用Bass获得歌曲长度
        /// </summary>
        /// <param name="stream">Bass音乐流</param>
        /// <param name="extension">音乐格式（扩展名）</param>
        /// <returns>曲长</returns>
        private int Bass_Length(string Path, int stream)
        {
            int content = 0;
            if (Convert.ToInt32(Bass.BASS_ChannelBytes2Seconds(stream, Bass.BASS_ChannelGetLength(stream))) != -1) content = Convert.ToInt32(Bass.BASS_ChannelBytes2Seconds(stream, Bass.BASS_ChannelGetLength(stream)));//曲长
            else
            {
                MessageBox.Show("异常：不支持的格式或文件已损坏。\n错误路径：" + Path);
            }

            return content;
        }

        public MusicInfo Clone()
        {
            MusicInfo info =new MusicInfo();
            info.Album = this.Album;
            info.Artist = this.Artist;
            info.Buildtime = this.Buildtime;
            info.Check = false;
            info.Format = this.Format;
            info.Length = this.Length;
            info.Path = this.Path;
            info.Title = this.Title;
            info.Year = this.Year;
            return info;
        }

        public static string ConvertTime(string time)
        {
            string[] dateSplit = time.Split(new char[] {'/', ' '});
            string newtime;
            if (dateSplit[dateSplit.Length - 1] == "PM")
            {
                string[] timeSplit = dateSplit[3].Split(new char[] {':'});
                newtime = Convert.ToInt32(timeSplit[0]) + 12 + ":" + timeSplit[1];
            }
            else
            {
                newtime = dateSplit[3];
            }

            return dateSplit[2] + "/" + dateSplit[1] + "/" + dateSplit[0] + " " + newtime;
        }
    }
}
