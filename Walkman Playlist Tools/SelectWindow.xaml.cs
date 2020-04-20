using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Forms;
using System.Windows.Input;
using LanguageDetect;
using CheckBox = System.Windows.Controls.CheckBox;
using ListView = System.Windows.Controls.ListView;
using MessageBox = System.Windows.MessageBox;
using MouseEventArgs = System.Windows.Input.MouseEventArgs;

namespace Walkman_Playlist_Tools
{
    /// <summary>
    /// SelectWindow.xaml 的交互逻辑
    /// </summary>
    public partial class SelectWindow : Window
    {
        private ObservableCollection<MusicInfo> infobase;
        private Dictionary<string, List<MusicInfo>> infodic;
        private InfoFlag[] infoFlags;
        private Dictionary<string, ObservableCollection<MusicInfo>> selectedConections;
        private bool? isInWorkPlace=null;

        public Dictionary<string, ObservableCollection<MusicInfo>> SelectedConections
        {
            get { return selectedConections; }
        }
        public bool? IsInWorkPlace
        {
            get { return isInWorkPlace; }
        }

        /// <summary>
        /// 建立自动分类窗口（使用后需选择分类方法）
        /// </summary>
        /// <param name="lv">需要分类的列表</param>
        public SelectWindow(ListView lv)
        {
            infobase = lv.ItemsSource as ObservableCollection<MusicInfo>;
            selectedConections = new Dictionary<string, ObservableCollection<MusicInfo>>();
            infodic =new Dictionary<string, List<MusicInfo>>();
            infoFlags=new InfoFlag[infobase.Count];
            InitializeComponent();
        }

        /// <summary>
        /// 主分类算法
        /// </summary>
        void StartSort()
        {
            for (int i = 0; i < infoFlags.Length; i++)
            {
                if (infoFlags[i].Flag != null)
                {
                    List<MusicInfo> list=new List<MusicInfo>();
                    list.Add(infoFlags[i].MusicInfo);
                    infodic.Add(infoFlags[i].Flag,list);
                    for (int j = i+1; j < infoFlags.Length; j++)
                    {
                        if (infoFlags[j].Flag != null)
                        {
                            if (infoFlags[i].Flag == infoFlags[j].Flag)
                            {
                                list.Add(infoFlags[j].MusicInfo);
                                infoFlags[j].Flag = null;
                            }
                        }
                    }
                }
            }
        }

        /// <summary>
        /// 将分类结果保存到新标签页
        /// </summary>
        /// <param name="protect">最小结果数（小于该数的不进行录入）</param>
        void MoveToTab(int protect)
        {
            foreach (KeyValuePair<string, List<MusicInfo>> page in infodic)
            {
                if (page.Value.Count >= protect)
                {
                    BuildPlaylist playlist = BuildList(page.Key);
                    foreach (MusicInfo info in page.Value)
                    {
                        playlist.Infos.Add(info.Clone());
                    }
                }
            }
        }

        /// <summary>
        /// 创建存放分类结果的列表项目
        /// </summary>
        /// <param name="name">项目名（分类依据）</param>
        /// <returns></returns>
        BuildPlaylist BuildList(string name)
        {
            BuildPlaylist buildPlaylist = new BuildPlaylist(name);
            SortedPlaylistList.Items.Add(buildPlaylist);
            SortedPlaylistList.SelectionChanged += SortedChanged;
            return buildPlaylist;
        }

        /// <summary>
        /// 按文件夹名进行分类
        /// </summary>
        public void ByFolderName()
        {
            for (int i = 0; i < infobase.Count; i++)
            {
                MusicInfo info = infobase[i];
                string[] subpath = info.Path.Split(new char[] { '\\' });
                infoFlags[i]=new InfoFlag();
                infoFlags[i].Flag = subpath[subpath.Length - 2];
                infoFlags[i].MusicInfo = info;
            }
            StartSort();
            MoveToTab(Setting.Default.LeastFolder);
        }

        /// <summary>
        /// 按导入时间进行分类
        /// </summary>
        public void ByImportTime()
        {
            for (int i = 0; i < infobase.Count; i++)
            {
                MusicInfo info = infobase[i];
                string[] datetime = info.Buildtime.Split(new char[] {' '});
                string[] date = datetime[0].Split(new char[] {'/'});
                infoFlags[i] = new InfoFlag();
                infoFlags[i].Flag = date[1].Substring(2) + "年" + date[2] + "月" + date[0] + "日";
                infoFlags[i].MusicInfo = info;
            }
            StartSort();
            MoveToTab(Setting.Default.LeastBuildTime);
        }

        /// <summary>
        /// 按文件格式进行分类
        /// </summary>
        public void ByFormat()
        {
            for (int i = 0; i < infobase.Count; i++)
            {
                MusicInfo info = infobase[i];
                infoFlags[i] = new InfoFlag();
                infoFlags[i].Flag = info.Format;
                infoFlags[i].MusicInfo = info;
            }
            StartSort();
            MoveToTab(Setting.Default.LeastFormat);
        }

        /// <summary>
        /// 按标题语言进行分类
        /// </summary>
        public void ByTitle(bool fristInit)
        {
            if (fristInit)
            {
                try
                {
                    DetectorFactory.loadProfile(@"profiles\");
                    Detector test = DetectorFactory.create();
                    test.append("test");
                }
                catch (BadImageFormatException)
                {
                    MessageBox.Show(
                        "检测到您未安装相关依赖包，如果需要请前往以下地址下载\n32位：http://www.microsoft.com/en-us/download/details.aspx?id=18084 \n64位：http://www.microsoft.com/en-us/download/details.aspx?id=15468");
                    return;
                }
            }

            for (int i = 0; i < infobase.Count; i++)
            {
                MusicInfo info = infobase[i];
                infoFlags[i] = new InfoFlag();
                Detector detector = DetectorFactory.create();
                string tag = info.Title;
                if (Setting.Default.isScanLyric)
                {
                    if (File.Exists(Path.ChangeExtension(info.Path, "lrc")))
                    {
                        try
                        {
                            StreamReader reader1 =
                                new StreamReader(
                                    new FileStream(Path.ChangeExtension(info.Path, "lrc"), FileMode.Open,
                                        FileAccess.Read), EncodingType.GetType(Path.ChangeExtension(info.Path, "lrc")));
                            tag += Format.LyricDelTime(reader1.ReadToEnd());
                        }
                        catch (IOException e)
                        {
                            MessageBox.Show("无法读取歌词文件：" + Path.ChangeExtension(info.Path, "lrc") + "。" + e.Message);
                        }
                    }
                }

                try
            {
                detector.append(tag);
                switch (detector.detect())
                {
                    case "th": infoFlags[i].Flag = "泰语"; break;
                    case "fi": infoFlags[i].Flag = "芬兰语"; break;
                    case "fr": infoFlags[i].Flag = "法语"; break;
                    case "it": infoFlags[i].Flag = "意大利语"; break;
                    case "ru": infoFlags[i].Flag = "俄语"; break;
                    case "es": infoFlags[i].Flag = "西班牙语"; break;
                    case "ja": infoFlags[i].Flag = "日语"; break;
                    case "en": infoFlags[i].Flag = "英语"; break;
                    case "ko": infoFlags[i].Flag = "韩语"; break;
                    case "zh-cn": infoFlags[i].Flag = "中文（简）"; break;
                    case "zh-tw": infoFlags[i].Flag = "中文（繁）"; break;
                }
                }
            catch (BadImageFormatException)
            {
                MessageBox.Show("检测到您未安装相关依赖包，如果需要请前往以下地址下载\n32位：http://www.microsoft.com/en-us/download/details.aspx?id=18084 \n64位：http://www.microsoft.com/en-us/download/details.aspx?id=15468");
                return;
            }
                catch (LangDetectException)
            {
                infoFlags[i].Flag = "检测失败的项目";
            }
                infoFlags[i].MusicInfo = info;
            }
            StartSort();
            MoveToTab(Setting.Default.LeastCountry);
        }

        /// <summary>
        /// 按艺术家名称进行分类
        /// </summary>
        public void ByArtist()
        {
            for (int i = 0; i < infobase.Count; i++)
            {
                MusicInfo info = infobase[i];
                infoFlags[i] = new InfoFlag();
                if (info.Artist == "" || info.Artist == null)continue;
                infoFlags[i].Flag = info.Artist;
                infoFlags[i].MusicInfo = info;
            }
            StartSort();
            MoveToTab(Setting.Default.LeastArtist);
        }

        /// <summary>
        /// 按作品年代进行分类
        /// </summary>
        public void ByAge()
        {
            for (int i = 0; i < infobase.Count; i++)
            {
                MusicInfo info = infobase[i];
                infoFlags[i] = new InfoFlag();
                if (info.Year==""||info.Year=="0"||info.Year==null)continue;
                string century = info.Year.Substring(0, 2);
                string age = info.Year.Substring(2, 1);
                try
                {
                    if (Convert.ToInt32(century) < 20)
                    {
                        infoFlags[i].Flag = age + "0年代";
                        if (Convert.ToInt32(century) < 19) infoFlags[i].Flag = century + "世纪";
                    }
                    else
                    {
                        if (Convert.ToInt32(age) == 0) infoFlags[i].Flag = "2000年代";
                        else infoFlags[i].Flag = "近年";
                    }
                }
                catch (FormatException e) { }
                infoFlags[i].MusicInfo = info;
            }
            StartSort();
            MoveToTab(Setting.Default.LeastAge);
        }
        
        /// <summary>
        /// 包含分类标准（flag）的音乐信息
        /// </summary>
        class InfoFlag
        {
            private MusicInfo _musicinfo;
            private string _flag;
            public MusicInfo MusicInfo
            {
                get { return _musicinfo; }
                set { _musicinfo = value; }
            }
            public string Flag
            {
                get { return _flag; }
                set { _flag = value; }
            }
        }

        /// <summary>
        /// 将分类结果全部存放到工作区/播放列表
        /// </summary>
        /// <param name="isWP">位置选择（是否为工作区）</param>
        private void SaveAllTo(bool isWP)
        {
            isInWorkPlace = isWP;
            foreach (BuildPlaylist item in SortedPlaylistList.Items)
            {
                selectedConections.Add(item.Name, item.ListViewInside.ItemsSource as ObservableCollection<MusicInfo>);
            }
            Close();
        }

        /// <summary>
        /// 将目前选中的分类结果全部存放到工作区/播放列表
        /// </summary>
        /// <param name="isWP">位置选择（是否为工作区）</param>
        private void SaveTo(bool isWP)
        {
            isInWorkPlace = isWP;
            try
            {
                foreach (BuildPlaylist playlist in SortedPlaylistList.Items)
                {
                    if(playlist.Checked) selectedConections.Add(playlist.Name, playlist.ListViewInside.ItemsSource as ObservableCollection<MusicInfo>);
                }
                if(selectedConections.Count>0)Close();
            }
            catch (NullReferenceException) { }
        }

        /// <summary>
        /// 得到目前选中分类依据的结果
        /// </summary>
        /// <returns>分类结果</returns>
        ListView GetList()
        {
            BuildPlaylist buildPlaylist = SortedPlaylistList.SelectedItem as BuildPlaylist;
            return buildPlaylist.ListViewInside;
        }

        /// <summary>
        /// 删除一个分类项目（分类依据）
        /// </summary>
        /// <param name="lv">存放分类依据的表</param>
        void DelList(ListView lv)
        {
            foreach (BuildPlaylist sortedPlaylist in SortedPlaylistList.Items.Cast<BuildPlaylist>().Where(playlist=>playlist.Checked).ToList())
            {
                lv.Items.Remove(sortedPlaylist);
            }
        }

        /// <summary>
        /// 删除列表中选中的项目
        /// </summary>
        /// <param name="lv">列表</param>
        void DelItem(ListView lv)
        {
            ObservableCollection<MusicInfo> collection = lv.ItemsSource as ObservableCollection<MusicInfo>;
            List<MusicInfo> selecteditem = new List<MusicInfo>();
            foreach (MusicInfo item in lv.Items)
            {
                if (item.Check == true) selecteditem.Add(item);
            }

            foreach (MusicInfo item in selecteditem)
            {
                collection.Remove(item);
            }
        }

        /// <summary>
        /// 选择所有列表
        /// </summary>
        void CheckedAll()
        {
            foreach (BuildPlaylist item in SortedPlaylistList.Items)
            {
                item.Checked = true;
            }
        }

        /// <summary>
        /// 取消列表全选
        /// </summary>
        /// <param name="lv">应用操作的列表</param>
        void UnCheckedAll()
        {
            foreach (BuildPlaylist item in SortedPlaylistList.Items)
            {
                item.Checked = false;
            }
        }

        /// <summary>
        /// 列表选择反选
        /// </summary>
        /// <param name="lv">应用操作的列表</param>
        void Inverse()
        {
            foreach (BuildPlaylist item in SortedPlaylistList.Items)
            {
                if (item.Checked) item.Checked = false;
                else item.Checked = true;
            }
        }

        /// <summary>
        /// 勾选/取消勾选目前选中的列表
        /// </summary>
        /// <param name="lv">应用操作的列表</param>
        void ToChecked()
        {
            foreach (BuildPlaylist item in SortedPlaylistList.SelectedItems)
            {
                if (item.Checked) item.Checked = false;
                else item.Checked = true;
            }
        }

        ///  <summary>
        /// 项目全选
        ///  </summary>
        /// <param name="lv">应用操作的列表</param>
        void CheckedAll(ListView lv)
        {
            foreach (MusicInfo item in lv.Items)
            {
                item.Check = true;
            }
        }

        /// <summary>
        /// 取消全选
        /// </summary>
        /// <param name="lv">应用操作的列表</param>
        void UnCheckedAll(ListView lv)
        {
            foreach (MusicInfo item in lv.Items)
            {
                item.Check = false;
            }
        }

        /// <summary>
        /// 选择反选
        /// </summary>
        /// <param name="lv">应用操作的列表</param>
        void Inverse(ListView lv)
        {
            foreach (MusicInfo item in lv.Items)
            {
                if (item.Check == true) item.Check = false;
                else item.Check = true;
            }
        }

        /// <summary>
        /// 勾选/取消勾选目前选中的项目
        /// </summary>
        /// <param name="lv">应用操作的列表</param>
        void ToChecked(ListView lv)
        {
            foreach (MusicInfo item in lv.SelectedItems)
            {
                if (item.Check == true) item.Check = false;
                else item.Check = true;
            }
        }

        private void SortedChanged(object sender, EventArgs eventArgs)
        {
            SortedPlaylistGrid.Children.Clear();
            BuildPlaylist playlist = SortedPlaylistList.SelectedItem as BuildPlaylist;
            if (playlist != null) SortedPlaylistGrid.Children.Add(playlist.ListViewInside);
        }

        private void SaveAllToWP_OnClick(object sender, RoutedEventArgs e)
        {
            SaveAllTo(true);
        }

        private void SaveAllToPlaylist_OnClick(object sender, RoutedEventArgs e)
        {
            SaveAllTo(false);
        }

        private void SaveToWP_OnClick(object sender, RoutedEventArgs e)
        {
            SaveTo(true);
        }

        private void SaveToPlaylist_OnClick(object sender, RoutedEventArgs e)
        {
            SaveTo(false);
        }

        private void CheckedItemButton_OnClick(object sender, RoutedEventArgs e)
        {
            if (GetList() != null) CheckedAll(GetList());
        }

        private void ClearCheckedItemButton_OnClick(object sender, RoutedEventArgs e)
        {
            if (GetList() != null) UnCheckedAll(GetList());
        }

        private void InverseItemButton_OnClick(object sender, RoutedEventArgs e)
        {
            if (GetList() != null) Inverse(GetList());
        }

        private void ToCheckItemButton_OnClick(object sender, RoutedEventArgs e)
        {
            if (GetList() != null) ToChecked(GetList());
        }

        private void DelItemButton_OnClick(object sender, RoutedEventArgs e)
        {
            if (GetList() != null) DelItem(GetList());
        }

        private void CheckedPlaylistButton_OnClick(object sender, RoutedEventArgs e)
        {
            if (SortedPlaylistList != null) CheckedAll();
        }

        private void ClearCheckedPlaylistButton_OnClick(object sender, RoutedEventArgs e)
        {
            if (SortedPlaylistList != null) UnCheckedAll();
        }

        private void InversePlaylistButton_OnClick(object sender, RoutedEventArgs e)
        {
            if (SortedPlaylistList != null) Inverse();
        }

        private void ToCheckPlaylistButton_OnClick(object sender, RoutedEventArgs e)
        {
            if (SortedPlaylistList != null) ToChecked();
        }

        private void DeleteWorkplaceButton_OnClick(object sender, RoutedEventArgs e)
        {
            if (SortedPlaylistList != null) DelList(SortedPlaylistList);
        }
    }
}
