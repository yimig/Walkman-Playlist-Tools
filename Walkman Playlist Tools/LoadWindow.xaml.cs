using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using System.Xml.Serialization.Configuration;
using Path = System.IO.Path;

namespace Walkman_Playlist_Tools
{
    /// <summary>
    /// LoadWindow.xaml 的交互逻辑
    /// </summary>
    public partial class LoadWindow : Window
    {
        private List<MusicInfo> scanResult;
        private BackgroundWorker backgroundWorker;
        private bool isCancel=false;
        private bool isCopy = false;
        private bool isCopyFolder = false;
        private string ChoiseAddr;
        private string rootPath;

        public bool IsCancel
        {
            get { return isCancel; }
        }

        public List<MusicInfo> ScanResult
        {
            get { return scanResult; }
        }

        /// <summary>
        /// 窗口：仅扫描
        /// </summary>
        /// <param name="pathList">包含被扫描的文件列表（此时这些文件应已在设备内）</param>
        public LoadWindow(List<FileSystemInfo> pathList)
        {
            InitializeComponent();
            isCopy = false;
            scanResult = new List<MusicInfo>();
            InitBGWorker();
            backgroundWorker.RunWorkerAsync(pathList);
        }

        /// <summary>
        /// 窗口：扫描且复制文件
        /// </summary>
        /// <param name="pathList">包含被扫描的文件列表（此时这些文件应不在设备内）</param>
        /// <param name="rootPath">复制到设备的盘符</param>
        public LoadWindow(List<FileSystemInfo> pathList,string rootPath)
        {
            InitializeComponent();
            this.rootPath = rootPath;
            isCopy = true;
            scanResult = new List<MusicInfo>();
            InitBGWorker();
            backgroundWorker.RunWorkerAsync(pathList);
        }

        /// <summary>
        /// 窗口：扫描且复制文件夹
        /// </summary>
        /// <param name="pathList">包含被扫描的文件列表（此时这些文件应不在设备内）</param>
        /// <param name="rootPath">复制到设备的盘符</param>
        /// <param name="ChoiseAddr">被复制的文件夹绝对路径</param>
        public LoadWindow(List<FileSystemInfo> pathList, string rootPath,string ChoiseAddr)
        {
            InitializeComponent();
            this.rootPath = rootPath;
            isCopy = true;
            isCopyFolder = true;
            this.ChoiseAddr = ChoiseAddr;
            scanResult = new List<MusicInfo>();
            InitBGWorker();
            backgroundWorker.RunWorkerAsync(pathList);
        }

        /// <summary>
        /// 初始化后台进程
        /// </summary>
        private void InitBGWorker()
        {
            backgroundWorker = new BackgroundWorker();
            backgroundWorker.WorkerReportsProgress = true;
            backgroundWorker.WorkerSupportsCancellation = true;
            backgroundWorker.DoWork += AnalyseMusic_DoWork;
            backgroundWorker.ProgressChanged += BGWorker_ProgressChanged;
            backgroundWorker.RunWorkerCompleted += BackgroundWorker_RunWorkerCompleted;
        }

        /// <summary>
        /// 复制文件（夹）（每次一条信息/文件）
        /// </summary>
        /// <param name="info">音乐信息</param>
        /// <returns>复制到的文件目录（包含文件名）</returns>
        private string CopyFileFunc(MusicInfo info)
        {
            string finalPath;
            if (isCopyFolder)
            {
                string relativeAddr = info.Path.Split(new string[] { ChoiseAddr }, StringSplitOptions.None)[1];
                string[] minAddr = ChoiseAddr.Split(new char[] { '\\' });
                finalPath = rootPath+@"\Music\"+minAddr[minAddr.Length-1] + relativeAddr;
                CreatePath(Path.GetDirectoryName(finalPath));
            }
            else
            {
                if (Setting.Default.BuildFolderMode == 0)
                {
                    finalPath = rootPath + @"\Music\" + Path.GetFileName(info.Path);
                }
                else if (Setting.Default.BuildFolderMode == 1)
                    finalPath = CheckArtist(info, rootPath) + "\\" + Path.GetFileName(info.Path);
                else finalPath = CheckAlbum(info, rootPath) + "\\" + Path.GetFileName(info.Path);
            }

            File.Copy(info.Path,finalPath);
            if(CheckLyric(info.Path))File.Copy(Path.ChangeExtension(info.Path, "lrc"), Path.ChangeExtension(finalPath, "lrc"));
            return finalPath;
        }

        /// <summary>
        /// 检查该音乐同位置下是否包含歌词
        /// </summary>
        /// <param name="path">音乐路径</param>
        /// <returns>判断结果</returns>
        bool CheckLyric(string path)
        {
            bool isLyric;
            if (File.Exists(Path.ChangeExtension(path, "lrc"))) isLyric = true;
            else isLyric = false;
            return isLyric;
        }

        /// <summary>
        /// 检查设备音乐目录中是否有歌手名文件夹，如果有就返回地址，没有就创建
        /// </summary>
        /// <param name="info">音乐信息（获得歌手名）</param>
        /// <param name="path">设备盘符</param>
        /// <returns>返回歌手文件夹目录</returns>
        string CheckArtist(MusicInfo info, string path)
        {
            string artistPath;
            if (info.Artist != null) artistPath = path + @"\Music\" + info.Artist;
            else artistPath = path + @"\Music\未知艺术家";
            CreatePath(artistPath);
            return artistPath;
        }

        /// <summary>
        /// 检查设备音乐目录中是否有专辑名文件夹，如果有就返回地址，没有就创建
        /// </summary>
        /// <param name="info">音乐信息（获得专辑名）</param>
        /// <param name="path">设备盘符</param>
        /// <returns>返回专辑文件夹目录</returns>
        string CheckAlbum(MusicInfo info, string path)
        {
            string albumPath;
            if (info.Album != null) albumPath = path + @"\Music\" + info.Album;
            else albumPath = path + @"\Music\未知专辑";
            CreatePath(albumPath);
            return albumPath;
        }

        /// <summary>
        /// 尝试创建一个路径
        /// </summary>
        /// <param name="path">路径地址</param>
        void CreatePath(string path)
        {
            try
            {
                Directory.CreateDirectory(path);
            }
            catch (NotSupportedException e)
            {
                MessageBox.Show("\"" + path + "\"" + "创建路径失败，错误信息：" + e.Message);
                throw;
            }
        }

        private void BackgroundWorker_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            Close();
        }

        private void BGWorker_ProgressChanged(object sender, ProgressChangedEventArgs e)
        {
            int nowPercent = (e.ProgressPercentage >= 100) ? 99 : e.ProgressPercentage;
            ProgressBar.Value = nowPercent;
            if(isCopy)TextBlock.Text = "复制并分析：" + e.UserState;
            else TextBlock.Text = "分析：" + e.UserState;
            PercentText.Text = nowPercent + "%";
        }

        private void AnalyseMusic_DoWork(object sender, DoWorkEventArgs e)
        {
            var backgroundWorker = sender as BackgroundWorker;
            var pathList = e.Argument as List<FileSystemInfo>;
            double weight = (pathList.Count / 100.0);
            for (int i = 0; i < pathList.Count; i++)
            {
                backgroundWorker.ReportProgress((int)(i / weight), pathList[i].FullName);
                //在这里判断文件重复

                MusicInfo musicInfo = new MusicInfo(pathList[i].FullName);
                if(!isCopy)scanResult.Add(musicInfo);
                else
                {
                    try
                    {
                        musicInfo.Path = CopyFileFunc(musicInfo);
                        scanResult.Add(musicInfo);
                    }
                    catch (IOException ex)
                    {
                        MessageBox.Show("\"" + musicInfo.Path + "\"" + "复制失败，错误信息：" + ex.Message);
                    }
                    catch (NotSupportedException)
                    {
                        continue;
                    }
                }
            }
        }

        private void Window_Closing(object sender, CancelEventArgs e)
        {
            if (backgroundWorker.IsBusy) isCancel = true;
            backgroundWorker.CancelAsync();
        }


    }
}
