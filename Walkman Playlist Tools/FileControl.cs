using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;
using Un4seen.Bass;
using Shell32;

namespace Walkman_List_Tools
{
    /// <summary>
    /// 文件类型
    /// </summary>
    public enum FileType
    {
        Music,
        NormalPlaylist,
        WalkmanPlaylist
    }

    public enum GetInfoType
    {
        Title,Artist,Album,Year,FileType,Length
    }

    /// <summary>
    /// 文件管理、包括文件控制类
    /// </summary>
    public class FileControl
    {

        List<string>list =new List<string>();

        /// <summary>
        /// 打开文件
        /// </summary>
        /// <param name="ft">文件类型</param>
        /// <returns>符合类型的文件名路径数组</returns>
        public string[] OpenFile(FileType ft)
        {
            string[] files = null;
            OpenFileDialog ofd = new OpenFileDialog();
            ofd.Multiselect = true;
            if (ft == FileType.Music) ofd.Filter = "全部支持的音频文件|*.mp1;*.mp2;*.mp3;*.m4a;*.flac;*.wav;*.aif;*.ogg;*.ape;*.alac;*.aac;*.dff;*.dsf|MPEG Audio|*.mp1;*.mp2;*.mp3;*.m4a|Free Lossless Audio|*.flac|Windows Wave|*.wav|Audio Interchange File Format|*.aif|OGGVobis|*.ogg|Monkey's audio|*.ape|Apple lossless audio|*.alac|Advanced Audio Coding|*.aac|Direct Stream Digital|*.dff;*.dsf";
            else if (ft == FileType.NormalPlaylist) ofd.Filter = "通用型播放列表文件|*.m3u,*.m3u8";
            else if (ft == FileType.WalkmanPlaylist) ofd.Filter = "Walkman播放列表文件|*.m3u";
            if (ofd.ShowDialog() == DialogResult.OK)
            {
                files = ofd.FileNames;
            }
            return files;
        }

        /// <summary>
        /// 打开文件夹
        /// </summary>
        /// <returns>返回选中的文件夹地址</returns>
        public string OpenFolder()
        {
            string path = null;
            System.Windows.Forms.FolderBrowserDialog dialog = new System.Windows.Forms.FolderBrowserDialog();
            if (dialog.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                path = dialog.SelectedPath;
            }
            return path;
        }

        /// <summary>
        /// 遍历文件夹
        /// </summary>
        /// <param name="dir">文件夹的地址</param>
        /// <param name="ft">遍历选中的文件类型</param>
        /// <returns>返回由选中文件路径构成的List</returns>
        public List<string> Director(string dir,FileType ft)
        {
            try
            {
                DirectoryInfo d = new DirectoryInfo(dir);
                FileSystemInfo[] fsinfos = d.GetFileSystemInfos();
                foreach (FileSystemInfo fsinfo in fsinfos)
                {
                    if (fsinfo is DirectoryInfo)     //判断是否为文件夹
                    {
                        Director(fsinfo.FullName,ft);//递归调用
                    }
                    else
                    {
                        if (ft == FileType.Music)
                        {
                            if (Path.GetExtension(fsinfo.FullName) == ".mp1" || Path.GetExtension(fsinfo.FullName) == ".mp2" || Path.GetExtension(fsinfo.FullName) == ".mp3" || Path.GetExtension(fsinfo.FullName) == ".m4a" || Path.GetExtension(fsinfo.FullName) == ".flac" || Path.GetExtension(fsinfo.FullName) == ".ape" || Path.GetExtension(fsinfo.FullName) == ".ogg" || Path.GetExtension(fsinfo.FullName) == ".acc" || Path.GetExtension(fsinfo.FullName) == ".alac" || Path.GetExtension(fsinfo.FullName) == ".wav" || Path.GetExtension(fsinfo.FullName) == ".dff" || Path.GetExtension(fsinfo.FullName) == ".dsf" || Path.GetExtension(fsinfo.FullName) == ".aif")
                            {
                                list.Add(fsinfo.FullName);
                            }
                        }
                        else if (ft == FileType.NormalPlaylist)
                        {
                            if (Path.GetExtension(fsinfo.FullName) == ".m3u" || Path.GetExtension(fsinfo.FullName) == ".m3u8") list.Add(fsinfo.FullName);
                        }
                        else if (ft == FileType.WalkmanPlaylist)
                        {
                            if (Path.GetExtension(fsinfo.FullName) == ".m3u") list.Add(fsinfo.FullName);
                        }
                    }
                }
            }
            catch (System.UnauthorizedAccessException)
            {
                MessageBox.Show("访问被拒绝，请退出占用Walkman内存的程序（如音乐播放器），然后再试");
            }
            catch (System.IO.DirectoryNotFoundException)
            { MessageBox.Show("地址不存在，请检查Walkman是否正确连接电脑并开启文件传输功能，或者前往设置更改默认检测盘符。"); }
            catch (System.IO.IOException)
            { MessageBox.Show("设备未就绪，请重新唤醒设备"); }
            return list;
        }

    }

    /// <summary>
    /// 存储、读取等缓存操作
    /// </summary>
    class Ache
    {
        /// <summary>
        /// 新建缓存
        /// </summary>
        /// <param name="lv">要缓存的列表</param>
        /// <param name="name">缓存文件名</param>
        public void SaveData(ListView lv, string name)
        {
            SettingsList.Default.InfoExist = true;
            FileStream fs = new FileStream(SettingsList.Default.datapath + name + ".database", FileMode.Create);
            StreamWriter wr = null;
            wr = new StreamWriter(fs);
            foreach (ListViewItem item in lv.Items)//处理列
            {
                wr.WriteLine(item.SubItems[0].Text + "|" + item.SubItems[1].Text + "|" + item.SubItems[2].Text + "|" + item.SubItems[3].Text + "|" + item.SubItems[4].Text + "|" + item.SubItems[5].Text + "|" + item.SubItems[6].Text);
                //处理行
            }
            wr.Close();
        }

        /// <summary>
        /// 读取缓存
        /// </summary>
        /// <param name="lv">载入缓存到目的列表</param>
        /// <param name="name">载入缓存的名字</param>
        public void LoadData(ListView lv, string name)
        {
            StreamReader sr = new StreamReader(SettingsList.Default.datapath + name + ".database", Encoding.UTF8);
            String line;
            lv.BeginUpdate();
            while ((line = sr.ReadLine()) != null)
            {
                string[] data = line.ToString().Split(new Char[] { '|' });
                ListViewItem lvi = new ListViewItem();
                lvi.Text = data[0];//标题
                lvi.SubItems.Add(data[1]);//长度
                lvi.SubItems.Add(data[2]);//格式
                lvi.SubItems.Add(data[3]);//艺术家
                lvi.SubItems.Add(data[4]);//专辑
                lvi.SubItems.Add(data[5]);//年代
                lvi.SubItems.Add(data[6]);//路径
                lv.Items.Add(lvi);
            }
            lv.EndUpdate();
            sr.Close();
        }
    }

    /// <summary>
    /// 获得音乐信息
    /// </summary>
    class GetMusicInfo
    {

        /// <summary>
        /// 使用bass库获得MP3、AIF、AAC音乐信息
        /// </summary>
        /// <param name="path">音乐地址</param>
        /// <param name="type">获得的信息类型</param>
        /// <returns></returns>
        public string GetMP3InfoPlus(string path, GetInfoType type)
        {
            string content = null;
            int stream = 0;
            if (type != GetInfoType.FileType)
            {
                try
                {
                    stream = Bass.BASS_StreamCreateFile(path, 0L, 0L, BASSFlag.BASS_SAMPLE_FLOAT);
                    string[] tags = Bass.BASS_ChannelGetTagsID3V2(stream);
                    if (tags != null)
                    {
                        switch (type)
                        {
                            case GetInfoType.Title:
                                {
                                    foreach (string x in tags)
                                    {
                                        if (x.Contains("TIT"))
                                        {
                                            content = x.Remove(0, 5);
                                            break;
                                        }
                                        else content = null;
                                    }//标题
                                    break;
                                }
                            case GetInfoType.Artist:
                                {
                                    foreach (string x in tags)
                                    {
                                        if (x.Contains("TPE"))
                                        {
                                            content = x.Remove(0, 5);
                                            break;
                                        }
                                        else content = null;
                                    }//艺术家
                                    break;
                                }
                            case GetInfoType.Album:
                                {
                                    foreach (string x in tags)
                                    {
                                        if (x.Contains("TALB"))
                                        {
                                            content = x.Remove(0, 5);
                                            break;
                                        }
                                        else content = null;
                                    }//专辑名
                                    break;
                                }
                            case GetInfoType.Year:
                                {
                                    foreach (string x in tags)
                                    {
                                        if (x.Contains("TYER"))
                                        {
                                            content = x.Remove(0, 5);
                                            break;
                                        }
                                        else content = null;
                                    }//年代
                                    break;
                                }
                            case GetInfoType.Length:
                                {
                                    if (Convert.ToInt32(Bass.BASS_ChannelBytes2Seconds(stream, Bass.BASS_ChannelGetLength(stream))) != -1) content = Convert.ToString(Convert.ToInt32(Bass.BASS_ChannelBytes2Seconds(stream, Bass.BASS_ChannelGetLength(stream))));//曲长
                                    else
                                    {
                                        content = "文件损坏";
                                        MessageBox.Show("异常：不支持的格式或文件已损坏。\n错误路径：" + path);
                                    }
                                    break;
                                }
                            default: break;
                        }
                    }
                }
                catch (System.AccessViolationException)
                {
                    MessageBox.Show("警告！内存溢出！程序将退出");
                    Application.Exit();
                }
            }
            if (type == GetInfoType.FileType) content = Path.GetExtension(path);//格式
            if (type == GetInfoType.Title && content == null) content = Path.GetFileNameWithoutExtension(path);
            return content;
        }


        /// <summary>
        /// 使用Windows属性获得MP3音乐信息
        /// </summary>
        /// <param name="path">音乐地址</param>
        /// <param name="type">获得的信息类型</param>
        /// <returns></returns>
        public string GetMP3Info(string path, GetInfoType type)
        {
            string content = null;
            if (!SettingsList.Default.IsAllBass)
            {
                Shell32.Shell sh = new Shell();
                Folder dir = sh.NameSpace(Path.GetDirectoryName(path));
                FolderItem item = dir.ParseName(Path.GetFileName(path));
                switch (type)
                {
                    case GetInfoType.Title:
                        {
                            content = dir.GetDetailsOf(item, 21);
                            if (content == "") content = Path.GetFileNameWithoutExtension(path);
                            break;
                        }//标题
                    case GetInfoType.Artist: content = dir.GetDetailsOf(item, 20); break;//艺术家
                    case GetInfoType.Album: content = dir.GetDetailsOf(item, 14); break;//专辑名
                    case GetInfoType.Year: content = dir.GetDetailsOf(item, 15); break;//日期
                    case GetInfoType.Length:
                        {
                            try
                            {
                                string hor, min, sec;
                                hor = dir.GetDetailsOf(item, 27).Substring(0, 2);
                                min = dir.GetDetailsOf(item, 27).Substring(3, 2);
                                sec = dir.GetDetailsOf(item, 27).Substring(6, 2);
                                content = Convert.ToString(Convert.ToInt32(hor) * 360 + Convert.ToInt32(min) * 60 + Convert.ToInt32(sec));
                            }//长度
                            catch (System.ArgumentOutOfRangeException)
                            {
                                MessageBox.Show("获取文件信息失败，可尝试前往设置打开“全部使用Bass库检测音乐信息”");
                            }
                        }
                        break;
                    case GetInfoType.FileType: content = Path.GetExtension(path);break;//格式
                }
            }
            return content;
        }


        /// <summary>
        /// 获得OGG音乐信息
        /// </summary>
        /// <param name="path">音乐地址</param>
        /// <param name="type">获取音乐信息类型</param>
        /// <returns></returns>
        public string GetOGGInfo(string path,GetInfoType type)
        {
            int stream = 0;
            string content = null;
            if (type != GetInfoType.FileType)
            {
                try
                {
                    stream = Bass.BASS_StreamCreateFile(path, 0L, 0L, BASSFlag.BASS_SAMPLE_FLOAT);
                    string[] tags = Bass.BASS_ChannelGetTagsOGG(stream);
                    if (tags != null)
                    {
                        switch (type)
                        {
                            case GetInfoType.Title:
                                {
                                    foreach (string x in tags)
                                    {
                                        if (x.Contains("TITLE"))
                                        {
                                            content = x.Remove(0, 6);
                                            break;
                                        }
                                        else content = null;
                                    }//标题
                                    break;
                                }
                            case GetInfoType.Artist:
                                {
                                    foreach (string x in tags)
                                    {
                                        if (x.Contains("ARTIST"))
                                        {
                                            content = x.Remove(0, 7);
                                            break;
                                        }
                                        else content = null;
                                    }//艺术家
                                    break;
                                }
                            case GetInfoType.Album:
                                {
                                    foreach (string x in tags)
                                    {
                                        if (x.Contains("ALBUM"))
                                        {
                                            content = x.Remove(0, 6);
                                            break;
                                        }
                                        else content = null;
                                    }//专辑名
                                    break;
                                }
                            case GetInfoType.Year:
                                {
                                    foreach (string x in tags)
                                    {
                                        if (x.Contains("DATE"))
                                        {
                                            content = x.Remove(0, 5);
                                            break;
                                        }
                                        else content = null;
                                    }//年代
                                    break;
                                }
                            case GetInfoType.Length:
                                {
                                    if (Convert.ToInt32(Bass.BASS_ChannelBytes2Seconds(stream, Bass.BASS_ChannelGetLength(stream))) != -1) content = Convert.ToString(Convert.ToInt32(Bass.BASS_ChannelBytes2Seconds(stream, Bass.BASS_ChannelGetLength(stream))));//曲长
                                    else
                                    {
                                        content = "文件损坏";
                                        MessageBox.Show("异常：不支持的格式或文件已损坏。\n错误路径：" + path);
                                    }
                                    break;
                                }
                            default: break;
                        }
                    }
                }
                catch (System.AccessViolationException)
                {
                    MessageBox.Show("警告！内存溢出！程序将退出");
                    Application.Exit();
                }
            }
            if (type == GetInfoType.FileType) content = Path.GetExtension(path);//格式
            if (type == GetInfoType.Title && content == null) content = Path.GetFileNameWithoutExtension(path);
            return content;
        }


        /// <summary>
        /// 获得APE音乐信息
        /// </summary>
        /// <param name="path">音乐路径</param>
        /// <param name="type">获得的信息类型</param>
        /// <returns></returns>
        public string GetAPEInfo(string path,GetInfoType type)
        {
            int stream=0;
            string content = null;
            if (type != GetInfoType.FileType)
            {
                try
                {
                    stream = Bass.BASS_StreamCreateFile(path, 0L, 0L, BASSFlag.BASS_SAMPLE_FLOAT);
                    string[] tags = Bass.BASS_ChannelGetTagsAPE(stream);
                    if (tags != null)
                    {
                        switch (type)
                        {
                            case GetInfoType.Title:
                                {
                                    foreach (string x in tags)
                                    {
                                        if (x.Contains("Title"))
                                        {
                                            content = x.Remove(0, 6);
                                            break;
                                        }
                                        else content = null;
                                    }//标题
                                    break;
                                }
                            case GetInfoType.Artist:
                                {
                                    foreach (string x in tags)
                                    {
                                        if (x.Contains("Artist"))
                                        {
                                            content = x.Remove(0, 7);
                                            break;
                                        }
                                        else content = null;
                                    }//艺术家
                                    break;
                                }
                            case GetInfoType.Album:
                                {
                                    foreach (string x in tags)
                                    {
                                        if (x.Contains("Album"))
                                        {
                                            content = x.Remove(0, 6);
                                            break;
                                        }
                                        else content = null;
                                    }//专辑名
                                    break;
                                }
                            case GetInfoType.Year:
                                {
                                    foreach (string x in tags)
                                    {
                                        if (x.Contains("Year"))
                                        {
                                            content = x.Remove(0, 5);
                                            break;
                                        }
                                        else content = null;
                                    }//年代
                                    break;
                                }
                            case GetInfoType.Length:
                                {
                                    if (Convert.ToInt32(Bass.BASS_ChannelBytes2Seconds(stream, Bass.BASS_ChannelGetLength(stream))) != -1) content = Convert.ToString(Convert.ToInt32(Bass.BASS_ChannelBytes2Seconds(stream, Bass.BASS_ChannelGetLength(stream))));//曲长
                                    else
                                    {
                                        content = "文件损坏";
                                        MessageBox.Show("异常：不支持的格式或文件已损坏。\n错误路径：" + path);
                                    }
                                    break;
                                }
                            default: break;
                        }
                    }
                }
                catch (System.AccessViolationException)
                {
                    MessageBox.Show("警告！内存溢出！程序将退出");
                    Application.Exit();
                }
            }
            if (type == GetInfoType.FileType) content = Path.GetExtension(path);//格式
            if (type == GetInfoType.Title && content == null) content = Path.GetFileNameWithoutExtension(path);
            return content;
        }


        /// <summary>
        /// 获得DSD音乐信息
        /// </summary>
        /// <param name="path">音乐路径</param>
        /// <param name="type">获得的信息类型</param>
        /// <returns></returns>
        public string GetDSDInfo(string path,GetInfoType type)
        {
            int stream=0;
            string content = null;
            stream = Bass.BASS_StreamCreateFile(path, 0L, 0L, BASSFlag.BASS_SAMPLE_FLOAT);
            switch(type)
            {
                case GetInfoType.Title: content = Bass.BASS_ChannelGetTagsDSDTitle(stream);break;//标题
                case GetInfoType.Artist: content = Bass.BASS_ChannelGetTagsDSDArtist(stream);break;//艺术家
                case GetInfoType.Length:
                    {
                        if (Convert.ToInt32(Bass.BASS_ChannelBytes2Seconds(stream, Bass.BASS_ChannelGetLength(stream))) != -1) content = Convert.ToString(Convert.ToInt32(Bass.BASS_ChannelBytes2Seconds(stream, Bass.BASS_ChannelGetLength(stream))));
                        else
                        {
                            content = "文件损坏";
                            MessageBox.Show("异常：不支持的格式或文件已损坏。\n错误路径：" + path);
                        }
                        break;//曲长
                    }
            }
            return content;
        }


        /// <summary>
        ///仅使用Windows属性获得歌曲名 
        /// </summary>
        /// <param name="path">音乐路径</param>
        /// <param name="type">获得的信息类型</param>
        /// <returns></returns>
        public string GetMusicInfoLite(string path,GetInfoType type)
        {
            string content = null;
            if (!SettingsList.Default.IsAllBass)
            {
                Shell32.Shell sh = new Shell();
                Folder dir = sh.NameSpace(Path.GetDirectoryName(path));
                FolderItem item = dir.ParseName(Path.GetFileName(path));
                switch (type)
                {
                    case GetInfoType.Title:
                        {
                            content = dir.GetDetailsOf(item, 21);
                            if (content == "") content = Path.GetFileNameWithoutExtension(path);
                            break;
                        }//标题
                    case GetInfoType.FileType: content = Path.GetExtension(path); break;//格式
                }
            }
            return content;
        }
    }

}
