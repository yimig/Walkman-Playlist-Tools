using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using Un4seen.Bass;
using ListView = System.Windows.Controls.ListView;
using MessageBox = System.Windows.MessageBox;
using TabControl = System.Windows.Controls.TabControl;
using System.Windows.Interop;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Windows.Input;
using Microsoft.VisualBasic.Devices;
using Application = System.Windows.Application;
using Binding = System.Windows.Data.Binding;
using Button = System.Windows.Controls.Button;
using ContextMenu = System.Windows.Controls.ContextMenu;
using KeyEventArgs = System.Windows.Input.KeyEventArgs;
using ListViewItem = System.Windows.Controls.ListViewItem;
using MenuItem = System.Windows.Controls.MenuItem;
using TextBox = System.Windows.Controls.TextBox;
using TreeView = System.Windows.Controls.TreeView;

namespace Walkman_Playlist_Tools
{

    /// <summary>
    /// MainWindow.xaml 的交互逻辑
    /// </summary>
    public partial class MainWindow
    {
        private bool fristInitDetector = true;

        public MainWindow()
        {
            InitializeComponent();
            InitializeWorkPlaceArea();
            InitializeBass();//初始化Bass库
        }

        #region InnerClass

        public class GetArea
        {
            private MainWindow mainWindow;
            private WorkPlaceIndex workPlace;
            private PlaylistIndex playlist;
            private bool isWalkman;

            public WorkPlaceIndex WorkPlace
            {
                get { return workPlace; }
            }

            public PlaylistIndex Playlist
            {
                get { return playlist; }
            }

            public bool IsWalkman
            {
                get { return isWalkman; }
            }

            /// <summary>
            /// 判断设备位置并返回所求控件
            /// </summary>
            /// <param name="mainWindow">主窗体类</param>
            public GetArea(MainWindow mainWindow)
            {
                this.mainWindow = mainWindow;
                StartJudge();
                BuildIndex();
            }

            /// <summary>
            /// 手动指定设备然后返回所求控件
            /// </summary>
            /// <param name="mainWindow">主窗体类</param>
            /// <param name="isWalkman">指定设备位置</param>
            public GetArea(MainWindow mainWindow, bool isWalkman)
            {
                this.mainWindow = mainWindow;
                this.isWalkman = isWalkman;
                BuildIndex();
            }

            /// <summary>
            /// 得到当前设备的音乐文件保存地址
            /// </summary>
            /// <returns></returns>
            public string GetSavePath()
            {
                string tag;
                if (isWalkman) tag = Setting.Default.WalkmanTag;
                else tag = Setting.Default.SDcardTag;
                return tag + @"\Music\";
            }

            /// <summary>
            /// 判断设备位置
            /// </summary>
            private void StartJudge()
            {
                if (mainWindow.WorkPlace.SelectedIndex == 0) isWalkman = true;
                else isWalkman = false;
            }

            /// <summary>
            /// 初始化存储控件的类
            /// </summary>
            private void BuildIndex()
            {
                workPlace = new WorkPlaceIndex(mainWindow, isWalkman);
                playlist = new PlaylistIndex(mainWindow, isWalkman);
            }


            /// <summary>
            /// 存放可获取工作区控件的类
            /// </summary>
            public class WorkPlaceIndex
            {
                private bool isWalkman;
                private MainWindow mainWindow;



                public WorkPlaceIndex(MainWindow mainWindow, bool isWalkman)
                {
                    this.mainWindow = mainWindow;
                    this.isWalkman = isWalkman;
                    ListViewItem item=new ListViewItem();
                }

                private TabItem[] GetSubTBIs(TabControl subTbc)
                {
                    return subTbc.Items.Cast<TabItem>().ToArray();
                }

                private ListView GetWorkPlace(TabItem subTbi)
                {
                    return subTbi.Content as ListView;
                }

                private ListView[] GetWorkPlaces(TabControl subTbc)
                {
                    ListView[] listViews = new ListView[subTbc.Items.Count];
                    for (int i = 0; i < listViews.Length; i++)
                    {
                        listViews[i] = GetWorkPlace(subTbc.Items[i] as TabItem);
                    }

                    return listViews;
                }

                private Dictionary<MusicInfo, string> ForeachWorkPlace(Dictionary<MusicInfo, string> dic, TabControl tabControl)
                {
                    for(int i=1;i<tabControl.Items.Count;i++)
                    {
                        foreach (MusicInfo info in GetWorkPlace(tabControl.Items[i] as TabItem).Items)
                        {
                            dic.Add(info, (tabControl.Items[i] as TabItem).Header.ToString());
                        }
                    }

                    return dic;
                }

                public TabControl GetSelectedSubTBC()
                {
                    if (isWalkman) return mainWindow.Walkman_WorkPlace;
                    else return mainWindow.SDcard_WorkPlace;
                }

                public TabItem GetSelectedSubTBI()
                {
                    if (isWalkman) return mainWindow.Walkman_WorkPlace.SelectedItem as TabItem;
                    else return mainWindow.SDcard_WorkPlace.SelectedItem as TabItem;
                }

                public ListView[] GetAllWorkPlace()
                {
                    if (isWalkman) return GetWorkPlaces(mainWindow.Walkman_WorkPlace);
                    else return GetWorkPlaces(mainWindow.SDcard_WorkPlace);
                }

                public ListView GetSelectedWorkPlace()
                {
                    if (isWalkman) return GetWorkPlace(mainWindow.Walkman_WorkPlace.SelectedItem as TabItem);
                    else return GetWorkPlace(mainWindow.SDcard_WorkPlace.SelectedItem as TabItem);
                }

                public ContextMenu GetSampleMenu()
                {
                    if (isWalkman) return MenuSample.WorkPlace_Walkman_Menu;
                    else return MenuSample.WorkPlace_SDCard_Menu;
                }

                public Dictionary<MusicInfo, string> GetAllWorkPlaceSongsExceptFirstTab()
                {
                    Dictionary<MusicInfo,string> dic=new Dictionary<MusicInfo, string>();
                    if(isWalkman)ForeachWorkPlace(dic, mainWindow.Walkman_WorkPlace);
                    else ForeachWorkPlace(dic, mainWindow.SDcard_WorkPlace);
                    return dic;
                }
            }

            /// <summary>
            /// 存放可获取播放列表区域控件的类
            /// </summary>
            public class PlaylistIndex
            {
                private bool isWalkman;
                private MainWindow mainWindow;

                public PlaylistIndex(MainWindow mainWindow, bool isWalkman)
                {
                    this.isWalkman = isWalkman;
                    this.mainWindow = mainWindow;
                }

                private ListView GetItemResult(BuildPlaylist buildPlaylist)
                {
                    if (buildPlaylist != null) return buildPlaylist.ListViewInside;
                    else return null;
                }

                private ListView[] GetPlaylistListResult(ListView playlistList)
                {
                    return playlistList.Items.Cast<BuildPlaylist>().Select(item => item.ListViewInside).ToArray();
                }

                private Dictionary<MusicInfo, BuildPlaylist> ForeachPlaylists(Dictionary<MusicInfo, BuildPlaylist> dic, ListView lv)
                {
                    foreach (BuildPlaylist playlist in lv.Items)
                    {
                        foreach (MusicInfo info in playlist.ListViewInside.Items)
                        {
                            dic.Add(info, playlist);
                        }
                    }

                    return dic;
                }

                public BuildPlaylist GetSelectedBuildPlaylist()
                {
                    if (isWalkman) return mainWindow.Walkman_PlaylistList.SelectedItem as BuildPlaylist;
                    else return mainWindow.SDCard_PlaylistList.SelectedItem as BuildPlaylist;
                }

                public ListView GetPlaylistList()
                {
                    if (isWalkman) return mainWindow.Walkman_PlaylistList;
                    else return mainWindow.SDCard_PlaylistList;
                }

                public ListView GetSelectedPlaylist()
                {
                    if (isWalkman) return GetItemResult(mainWindow.Walkman_PlaylistList.SelectedItem as BuildPlaylist);
                    else return GetItemResult(mainWindow.SDCard_PlaylistList.SelectedItem as BuildPlaylist);
                }

                public Grid GetGrid()
                {
                    if (isWalkman) return mainWindow.PlaylistGrid_Walkman;
                    else return mainWindow.PlaylistGrid_SDCard;
                }

                public ContextMenu GetSampleMenu()
                {
                    if (isWalkman) return MenuSample.Playlist_Walkman_Menu;
                    else return MenuSample.Playlist_SDCard_Menu;
                }

                public Dictionary<MusicInfo, BuildPlaylist> GetAllPlaylistSongs()
                {
                    Dictionary<MusicInfo, BuildPlaylist> dic = new Dictionary<MusicInfo, BuildPlaylist>();
                    if(isWalkman)ForeachPlaylists(dic, mainWindow.Walkman_PlaylistList);
                    else ForeachPlaylists(dic, mainWindow.SDCard_PlaylistList);
                    return dic;
                }
            }

        }



        #endregion

        #region Func

        /// <summary>
        /// 初始化工作区
        /// </summary>
        void InitializeWorkPlaceArea()
        {
            //初始化工作区
            BuildTab buildww = new BuildTab(Walkman_WorkPlace, "音乐库", false);
            buildww.BeginBuild();
            AddWorkPlaceMenu(true,buildww.NewTabItem,buildww.NewListView);
            if (Setting.Default.IsUseSD)
            {
                SDcardTab.IsEnabled = true;
                BuildTab buildsw = new BuildTab(SDcard_WorkPlace, "音乐库", false);
                buildsw.BeginBuild();
                AddWorkPlaceMenu(false,buildsw.NewTabItem,buildsw.NewListView);
            }
        }

        /*
        /// <summary>
        /// 得到目前选中的标签页
        /// </summary>
        /// <param name="tabControl">目前选中区域</param>
        /// <returns>选中的标签页</returns>
        TabItem Get_Tab(TabControl tabControl)
        {
            TabItem item = (TabItem)tabControl.SelectedItem;
            TabControl tab = (TabControl)item.Content;
            return (TabItem)tab.SelectedItem;
        }

        /// <summary>
        /// 得到目前选中的标签页中的列表
        /// </summary>
        /// <returns>选中的列表</returns>
        ListView Get_WorkPlace()
        {
            TabItem item = (TabItem)WorkPlace.SelectedItem;
            TabControl tab = (TabControl)item.Content;
            TabItem tbi = (TabItem)tab.SelectedItem;
            return (ListView)tbi.Content;
        }

        /// <summary>
        /// 得到工作区的第一个页面（主页面）
        /// </summary>
        /// <param name="isWalkman">选择设备的主页面</param>
        /// <returns></returns>
        ListView GetFristTabList(bool isWalkman)
        {
            TabItem item;
            TabControl tbc;
            if (isWalkman)
            {
                item = WorkPlace.Items[0] as TabItem;
                tbc = item.Content as TabControl;
            }
            else
            {
                item = WorkPlace.Items[1] as TabItem;
                tbc = item.Content as TabControl;
            }
            TabItem tbi = tbc.Items[0] as TabItem;
            return tbi.Content as ListView;
        }

        /// <summary>
        /// 返回目前工作区位置是否为Walkman
        /// </summary>
        /// <returns>返回结果</returns>
        bool WorkPlaceIsWalkman()
        {
            return WorkPlace.SelectedIndex == 0;
        }

        /// <summary>
        /// 返回目前播放列表位置是否为Walkman
        /// </summary>
        /// <returns>返回结果</returns>
        bool PlaylistIsWalkman()
        {
            return Playlist.SelectedIndex == 0;
        }
        */


        /// <summary>
        /// 删除一个标签页
        /// </summary>
        /// <param name="tabc">被删除的标签所在的TabControl</param>
        /// <param name="tabi">需要删除的标签页</param>
        /// <param name="num">最少页面数量</param>
        void Del_Tab(TabControl tabc, TabItem tabi,int num)
        {
            if (tabc.Items.Count >num)
            {
                tabc.Items.Remove(tabi);
            }
        }

        /// <summary>
        /// 遍历文件夹，将其中的音乐文件加入目前选中的工作区
        /// </summary>
        /// <param name="dir">文件夹的地址</param>
        /// <param name="pathList"></param>
        /// <returns>返回由选中文件路径构成的List</returns>
        void Director(string dir, List<FileSystemInfo> pathList)
        {
            try
            {
                DirectoryInfo d = new DirectoryInfo(dir);
                FileSystemInfo[] fsinfos = d.GetFileSystemInfos();
                foreach (FileSystemInfo fsinfo in fsinfos)
                {
                    if (fsinfo is DirectoryInfo)     //判断是否为文件夹
                    {
                        Director(fsinfo.FullName,pathList);//递归调用
                    }
                    else
                    {
                        if (CheckExtention(new[]{"mp3","m4a","flac","ape","ogg","wav","aif","dff","dsf", "mp1", "mp2", "acc", "alac" },fsinfo.Extension))
                        {
                           pathList.Add(fsinfo);
                        }
                    }
                }
            }
            catch (UnauthorizedAccessException)
            {
                MessageBox.Show("访问被拒绝，请退出占用Walkman内存的程序（如音乐播放器），然后再试");
            }
            catch (DirectoryNotFoundException)
            { MessageBox.Show("地址不存在，请检查Walkman是否正确连接电脑并开启文件传输功能，或者前往设置更改默认检测盘符。"); }
            catch (IOException)
            { MessageBox.Show("设备未就绪，请重新唤醒设备"); }
        }

        /// <summary>
        /// 初始化Bass库
        /// </summary>
        void InitializeBass()
        {
            if (Bass.BASS_PluginLoad("bassdsd.dll") == 0) System.Windows.Forms.MessageBox.Show("DSD插件初始化失败");
            if (Bass.BASS_PluginLoad("bass_ape.dll") == 0) System.Windows.Forms.MessageBox.Show("APE插件初始化失败");
            if (Bass.BASS_PluginLoad("bassalac.dll") == 0) System.Windows.Forms.MessageBox.Show("AAC插件初始化失败");
            if (Bass.BASS_PluginLoad("bass_aac.dll") == 0) System.Windows.Forms.MessageBox.Show("ALAC插件初始化失败");
            if (Bass.BASS_PluginLoad("bassflac.dll") == 0) System.Windows.Forms.MessageBox.Show("FLAC插件初始化失败");
            if (!Bass.BASS_Init(-1, 44100, BASSInit.BASS_DEVICE_CPSPEAKERS, new WindowInteropHelper(this).Handle))
            {
                System.Windows.Forms.MessageBox.Show(@"Bass初始化失败：" + Bass.BASS_ErrorGetCode());
            }
        }

        /// <summary>
        /// 保存缓存
        /// </summary>
        /// <param name="lv">要保存的列表</param>
        /// <param name="path">缓存地址</param>
        /// <param name="name">缓存文件名</param>
        void SaveAche(ListView lv, string path, string name)
        {
            FileStream fs = new FileStream(path + name + ".db", FileMode.Create);
            StreamWriter wr = new StreamWriter(fs);
            foreach (MusicInfo item in lv.Items)//处理列
            {
                wr.WriteLine(item.Title + "|" + item.Format + "|" + item.Artist + "|" + item.Album + "|" + item.Length + "|" + item.Year + "|" + item.Buildtime + "|" + item.Path);
                //处理行
            }
            wr.Close();
        }

        /// <summary>
        /// 加载缓存
        /// </summary>
        /// <param name="lv">要加载到的列表</param>
        /// <param name="path">缓存地址</param>
        /// <param name="name">缓存文件名</param>
        void LoadAche(ListView lv, string path, string name)
        {
            StreamReader sr = new StreamReader(path + name + ".db", Encoding.UTF8);
            String line;
            ObservableCollection<MusicInfo> infobase = lv.ItemsSource as ObservableCollection<MusicInfo>;
            infobase.Clear();
            while ((line = sr.ReadLine()) != null)
            {
                MusicInfo info=new MusicInfo();
                string[] data = line.Split(new Char[] { '|' });
                info.Title = data[0];//标题
                info.Format=data[1];//格式
                info.Artist=data[2];//艺术家
                info.Album=data[3];//专辑
                info.Length=Convert.ToInt32(data[4]);//长度
                info.Year=data[5];//年代
                info.Buildtime=data[6];//同步时间
                info.Path = data[7];//路径
                infobase?.Add(info);
            }
            sr.Close();
        }

        /// <summary>
        /// 删除列表中选中的项目
        /// </summary>
        /// <param name="lv">列表</param>
        void DelItem(ListView lv)
        {
            try
            {
                ObservableCollection<MusicInfo> collection = lv.ItemsSource as ObservableCollection<MusicInfo>;
                List<MusicInfo> selecteditem = new List<MusicInfo>();
                foreach (MusicInfo item in lv.Items)
                {
                    if (item.Check == true) selecteditem.Add(item);
                }

                foreach (MusicInfo item in selecteditem)
                {
                    collection?.Remove(item);
                }
            }
            catch (NullReferenceException)
            {

            }
        }

        ///  <summary>
        /// 项目全选
        ///  </summary>
        /// <param name="lv">应用操作的列表</param>
        public void CheckedAll(ListView lv)
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
        public void UnCheckedAll(ListView lv)
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
        public void Inverse(ListView lv)
        {
            foreach (MusicInfo item in lv.Items)
            {
                if (item.Check == true) item.Check = false;
                else item.Check = true;
            }
        }

        /// <summary>
        /// 清空列表
        /// </summary>
        /// <param name="lv">应用操作的列表</param>
        void ClearItem(ListView lv)
        {
            ObservableCollection<MusicInfo> infobase = lv.ItemsSource as ObservableCollection<MusicInfo>;
            if(infobase!=null)infobase.Clear();
        }

        /// <summary>
        /// 勾选/取消勾选目前选中的项目
        /// </summary>
        /// <param name="lv">应用操作的列表</param>
        public void ToChecked(ListView lv)
        {
            foreach (MusicInfo item in lv.SelectedItems)
            {
                if (item.Check == true) item.Check = false;
                else item.Check = true;
            }
        }

        /// <summary>
        /// 初始化下拉菜单按钮
        /// </summary>
        /// <param name="sender">按钮</param>
        void ChooseButtonInitialize(object sender)
        {
            Button button = (Button)sender;
            button.ContextMenu = null;
        }

        /// <summary>
        /// 给下拉菜单按钮添加左单击事件
        /// </summary>
        /// <param name="sender">按钮</param>
        /// <param name="menu">菜单</param>
        void ChooseButtonClick(object sender, ContextMenu menu)
        {
            Button button = (Button)sender;
            button.ContextMenu = menu;
            menu.IsOpen = true;
            button.ContextMenu = null;
        }

        /// <summary>
        /// 寻找项目
        /// </summary>
        /// <param name="lv">应用操作的列表</param>
        /// <param name="content">搜索内容</param>
        /// <param name="isWorkPlace">指定设备位置</param>
        /// <param name="ListLabel">存储当前搜索列表的序号</param>
        void SearchLv(ListView lv, string content,bool isWorkPlace,int ListLabel)
        {
            try
            {
                var infos = lv.ItemsSource as ObservableCollection<MusicInfo>;
                List<MusicInfo> searchResult = infos.Where(p => p.Title.Contains(content)).ToList();
                if (searchResult.Count != 0)
                {
                    if (SearchResult.isWorkPlace == isWorkPlace && SearchResult.isWalkman == new GetArea(this).IsWalkman &&
                        SearchResult.ListLabel == ListLabel && SearchResult.content == content)
                    {
                        lv.ScrollIntoView(SearchResult.searchResult[SearchResult.searchLabel]);
                        lv.SelectedItem = SearchResult.searchResult[SearchResult.searchLabel];
                        if (SearchResult.searchLabel == SearchResult.searchResult.Count - 1) SearchResult.searchLabel = 0;
                        else SearchResult.searchLabel++;
                    }
                    else
                    {
                        SearchResult.isWorkPlace = isWorkPlace;
                        SearchResult.isWalkman = new GetArea(this).IsWalkman;
                        SearchResult.ListLabel = ListLabel;
                        SearchResult.searchResult = searchResult;
                        SearchResult.content = content;
                        SearchResult.searchLabel = 0;
                        lv.ScrollIntoView(SearchResult.searchResult[SearchResult.searchLabel]);
                        lv.SelectedItem = SearchResult.searchResult[0];
                    }
                }
            }
            catch (NullReferenceException)
            {
            }
        }

        /// <summary>
        /// 在用户按下回车后，再将焦点转回TextBox，以便接收下个回车
        /// </summary>
        /// <param name="sender">TextBox控件</param>
        void ReFocusTextBox(Object sender)
        {
            var tb = sender as TextBox;
            if (tb != null) tb.Focus();
        }

        /// <summary>
        /// 删除转移菜单的项目
        /// </summary>
        /// <param name="menu">被操作的转移菜单（存储在MenuSample类中）</param>
        /// <param name="name">被删除的项目名称</param>
        void DelMenuItem(ContextMenu menu,string name)
        {
            MenuItem delitem = null;
            foreach (MenuItem item in menu.Items)
            {
                if (item.Header.ToString() == name)
                {
                    delitem = item;
                    break;
                }
            }
            if(delitem!=null)menu.Items.Remove(delitem);
        }

        /// <summary>
        /// 读缓存
        /// </summary>
        void ReadTemp()
        {
            try
            {
                if (File.Exists(Setting.Default.AchePath + "WalkmanData.db")) LoadAche(new GetArea(this).WorkPlace.GetSelectedWorkPlace(), Setting.Default.AchePath, "WalkmanData");
                if (Setting.Default.IsUseSD)
                    if (File.Exists(Setting.Default.AchePath + "SDcardData.db"))
                    {
                        TabItem tab = (TabItem)SDcard_WorkPlace.Items[0];
                        LoadAche((ListView)tab.Content, Setting.Default.AchePath, "SDcardData");
                    }
            }
            catch (FormatException)
            {
                MessageBox.Show("读取缓存失败！");
            }
        }

        /// <summary>
        /// 写缓存
        /// </summary>
        void WriteTemp()
        {
            TabItem item1 = (TabItem)Walkman_WorkPlace.Items[0];
            TabItem item2;
            SaveAche((ListView)item1.Content, Setting.Default.AchePath, "WalkmanData");
            if (Setting.Default.IsUseSD)
            {
                item2 = (TabItem)SDcard_WorkPlace.Items[0];
                SaveAche((ListView)item2.Content, Setting.Default.AchePath, "SDcardData");
            }
        }

        /// <summary>
        /// 为工作区转移菜单添加项目
        /// </summary>
        /// <param name="isWalkman">音乐是否在本机</param>
        /// <param name="item">要关联的选项卡</param>
        /// <param name="lv">要关联的列表</param>
        public void AddWorkPlaceMenu(bool isWalkman, TabItem item, ListView lv)
        {
            MenuItemWithListView menuItem = new MenuItemWithListView(lv);
            Binding bind = new Binding();
            bind.Source = item;
            bind.Path = new PropertyPath("Header");
            menuItem.SetBinding(HeaderedItemsControl.HeaderProperty, bind);
            menuItem.Click += WorkPlaceMenuItem_OnClick;
            if (isWalkman) MenuSample.WorkPlace_Walkman_Menu.Items.Add(menuItem);
            else MenuSample.WorkPlace_SDCard_Menu.Items.Add(menuItem);
        }

        /// <summary>
        /// 为播放列表转移菜单添加项目
        /// </summary>
        /// <param name="isWalkman">音乐是否在本机</param>
        /// <param name="buildPlaylist">要关联的音乐列表（读取名称）</param>
        /// <param name="lv">要关联的列表</param>
        void AddPlaylistMenu(bool isWalkman,BuildPlaylist buildPlaylist, ListView lv)
        {
            MenuItemWithListView menuItem = new MenuItemWithListView(lv);
            Binding bind = new Binding();
            bind.Source = buildPlaylist;
            bind.Path = new PropertyPath("Name");
            menuItem.SetBinding(HeaderedItemsControl.HeaderProperty, bind);
            menuItem.Click += PlaylistMenuItem_OnClick;
            new GetArea(this,isWalkman).Playlist.GetSampleMenu().Items.Add(menuItem);
        }

        /// <summary>
        /// 检查文件扩展名是否为制定扩展名
        /// </summary>
        /// <param name="targetExt">指定的扩展名数组(不需要加点)</param>
        /// <param name="checkExt">需要检查的扩展名(需要加点)</param>
        /// <returns></returns>
        public static bool CheckExtention(string[] targetExt,string checkExt)
        {
            bool result = false;
            checkExt=checkExt.Remove(0, 1);
            foreach (var extention in targetExt)
            {
                if (checkExt.Equals(extention.ToLower())||checkExt.Equals(extention.ToUpper()))
                {
                    result = true;
                    break;
                }
            }
            return result;
        }

        /// <summary>
        /// 在多个ListView间移动数据
        /// </summary>
        /// <param name="source">源列表</param>
        /// <param name="target">目标列表</param>
        void MoveItem(ListView source, ListView target)
        {
            foreach (MusicInfo item in source.Items)
            {
                ObservableCollection<MusicInfo> infos = target.ItemsSource as ObservableCollection<MusicInfo>;
                if (item.Check == true) infos?.Add(item.Clone());
            }
        }

        /// <summary>
        /// 建立一个播放列表
        /// </summary>
        /// <param name="PlaylistList">承载播放列表的列表</param>
        /// <param name="name">播放列表名</param>
        /// <param name="isAutoName">是否为名称添加序号</param>
        /// <returns></returns>
        public BuildPlaylist BuildPlaylist(ListView PlaylistList,string name,bool isAutoName)
        {
            BuildPlaylist buildPlaylist;
            buildPlaylist = new BuildPlaylist(PlaylistList, name, isAutoName);
            ObservableCollection<BuildPlaylist> infos;
            if (PlaylistList.ItemsSource == null)infos = new ObservableCollection<BuildPlaylist>();
            else infos = PlaylistList.ItemsSource as ObservableCollection<BuildPlaylist>;
            infos.Add(buildPlaylist);
            PlaylistList.ItemsSource = infos;
            PlaylistList.SelectionChanged += SelectionChanged;
            buildPlaylist.AddMenu += Playlist_AddMenu;
            buildPlaylist.BeginBuild();
            return buildPlaylist;
        }

        /// <summary>
        /// 更改状态栏提示信息，暂停3秒后还原
        /// </summary>
        /// <param name="text">提示信息</param>
        void ChangeStatueInfo(string text)
        {
            Task.Run(() =>
            {
                Dispatcher.Invoke(() =>
                {
                    StatueBar.Text = text;
                });

                //线程休眠5秒
                Thread.Sleep(3000);

                Dispatcher.Invoke(() =>
                {
                    StatueBar.Text = "状态：就绪";
                });
            });
        }

        /// <summary>
        /// 从文件载入播放列表
        /// </summary>
        /// <param name="firstWorkPlace">承载播放列表的列表</param>
        /// <param name="path">播放列表文件位置</param>
        /// <param name="isWalkmanPlaylist">播放列表类型是否为walkman专属播放列表</param>
        void LoadPlaylist(string path,bool isPath)
        {
            try
            {
                if (isPath)
                {
                    DirectoryInfo folder = new DirectoryInfo(path);
                    foreach (FileInfo file in folder.GetFiles())
                    {
                        if (CheckExtention(new[] {"m3u", "m3u8"}, file.Extension))
                        {
                            ScanPlaylist(file,true);
                        }
                    }
                }
                else ScanPlaylist(new FileInfo(path),false);
            }
            catch (UnauthorizedAccessException)
            {
                MessageBox.Show("访问被拒绝，请退出占用SD卡内存的程序（如音乐播放器），然后再试");
            }
            catch (DirectoryNotFoundException)
            {
                MessageBox.Show("地址不存在，请检查Walkman是否正确连接电脑并开启文件传输功能，或者前往设置更改默认检测盘符。");
            }
            catch (IOException)
            {
                MessageBox.Show("设备未就绪，请重新唤醒设备");
            }
        }

        /// <summary>
        /// 扫描本地播放列表文件
        /// </summary>
        /// <param name="file">播放列表文件</param>
        /// <param name="buildPath">是否同时保存播放列表地址（软件控制删除源文件）</param>
        private void ScanPlaylist(FileInfo file,bool buildPath)
        {
            var firstWorkPlaceInfos = new GetArea(this).WorkPlace.GetAllWorkPlace()[0].ItemsSource as ObservableCollection<MusicInfo>;
            var delList = new List<MusicInfo>();
            BuildPlaylist buildPlaylist = BuildPlaylist(new GetArea(this).Playlist.GetPlaylistList(), Path.GetFileNameWithoutExtension(file.FullName), false);
            if(buildPath)buildPlaylist.Path = file.FullName;
            var lvdata = buildPlaylist.ListViewInside.ItemsSource as ObservableCollection<MusicInfo>;
            StreamReader sr3 = new StreamReader(file.FullName, Encoding.UTF8);
            sr3.ReadLine();
            for (; ; )
            {
                string one = sr3.ReadLine();
                if (one == null) break;
                string two = sr3.ReadLine();
                bool firstget = false;
                foreach (MusicInfo info in firstWorkPlaceInfos)
                {
                    if (Path.GetFileName(info.Path) == Path.GetFileName(two))
                    {
                        if(!firstget)lvdata?.Add(info.Clone());
                        else
                        {
                                if (MessageBox.Show(
                                        "已经添加音乐文件：" + lvdata[lvdata.Count - 1].Path + "。现发现音乐库中含有重名音乐文件：" + info.Path +
                                        "为保证程序正常运行，请选择立刻删除该文件或暂时跳过稍后手动修改文件名。是否立刻删除该文件？", "文件重名",
                                        MessageBoxButton.YesNo) == MessageBoxResult.Yes)
                                {
                                    try
                                    {
                                        File.Delete(info.Path);
                                        delList.Add(info);
                                    }
                                    catch (IOException e)
                                    {
                                        MessageBox.Show("删除文件失败：" + e.Message);
                                    }
                                }
                        }
                        firstget = true;
                    }
                }
                foreach (var delInfo in delList)
                {
                    firstWorkPlaceInfos.Remove(delInfo);
                }
            }
            sr3.Close();
        }

        /// <summary>
        /// 将工作区保存到播放列表
        /// </summary>
        /// <param name="playlistList">承载播放列表的列表</param>
        /// <param name="tab">工作区Tab</param>
        private void SaveWorkPlaceToPlaylist(ListView playlistList,TabItem tab)
        {
            BuildPlaylist buildPlaylist;
            buildPlaylist=BuildPlaylist(playlistList, tab.Header.ToString(), false);
            ListView source = tab.Content as ListView;
            if (source != null)
                foreach (MusicInfo item in source.Items)
                {
                    buildPlaylist.Infos.Add(item.Clone());
                }
        }

        /// <summary>
        /// 将播放列表保存到工作区
        /// </summary>
        /// <param name="workPlaceList">承载工作区的TabControl</param>
        /// <param name="buildPlaylist">要保存的播放列表</param>
        private void SavePlaylistToWorkPlace(TabControl workPlaceList,BuildPlaylist buildPlaylist)
        {
            try
            {
                BuildTab tab = new BuildTab(workPlaceList, buildPlaylist.Name, false);
                tab.AddMenu += AddMenu_WorkPlace_OnBuilt;
                tab.BeginBuild();
                ObservableCollection<MusicInfo> infobase = tab.NewListView.ItemsSource as ObservableCollection<MusicInfo>;
                foreach (MusicInfo info in new GetArea(this).Playlist.GetSelectedPlaylist().Items)
                {
                    infobase?.Add(info.Clone());
                }
            }
            catch (NullReferenceException)
            {
                ChangeStatueInfo("没有选择播放列表哦！");
            }
        }

        /// <summary>
        /// 保存一个播放列表
        /// </summary>
        /// <param name="isWalkman">指定存储设备是否为Walkman本机</param>
        /// <param name="path">保存地址</param>
        /// <param name="buildPlaylist">需要保存的播放列表</param>
        void SavePlaylist(bool isWalkmanPlaylist,string path,BuildPlaylist buildPlaylist,bool isOutput)
        {
            string devicetag;
            FileStream fs;
            devicetag = "[SD]";
                if(isOutput)fs=new FileStream(path,FileMode.Create);
                else
                {
                    if (Setting.Default.IsDriveName && !buildPlaylist.Name.Contains(devicetag)&&!isWalkmanPlaylist)
                    {
                        fs = new FileStream(path + devicetag + buildPlaylist.Name + ".m3u", FileMode.Create);
                        File.Delete(path +  buildPlaylist.Name + ".m3u");
                    }
                    else fs = new FileStream(path + buildPlaylist.Name + ".m3u", FileMode.Create);
                }

                StreamWriter wr;
                wr = new StreamWriter(fs,new UTF8Encoding(true));
                wr.WriteLine("#EXTM3U");
                wr.Flush();
                foreach (MusicInfo info in buildPlaylist.ListViewInside.Items)
                {
                    wr.WriteLine("#EXTINF:" + info.Title);
                    wr.Flush();
                    if (isWalkmanPlaylist) wr.WriteLine(info.Path.Remove(0, 9));
                    else wr.WriteLine(info.Path);
                    wr.Flush();
                    //处理行
                }
                wr.Close();
                if(!isOutput)buildPlaylist.Path = path + buildPlaylist.Name + ".m3u";
        }

        /// <summary>
        /// 删除播放列表时，仅从程序中删除
        /// </summary>
        /// <param name="lv">承载播放列表的列表</param>
         public void DelPlaylistOnly(ObservableCollection<BuildPlaylist> infos,BuildPlaylist selectedPlaylist)
        {
            if (selectedPlaylist != null)
            {
                if (new GetArea(this).IsWalkman) DelMenuItem(MenuSample.Playlist_Walkman_Menu, selectedPlaylist.Name);
                else DelMenuItem(MenuSample.Playlist_SDCard_Menu, selectedPlaylist.Name);
                infos.Remove(selectedPlaylist);
            }
            else ChangeStatueInfo("未选择播放列表哦！");
        }

        /// <summary>
        /// 删除播放列表时，既删除文件也从程序中删除
        /// </summary>
        /// <param name="lv">承载播放列表的列表</param>
        public void DelFileAndPlaylist(ObservableCollection<BuildPlaylist> infos, BuildPlaylist selectedPlaylist)
        {
            if (selectedPlaylist == null)
            {
                ChangeStatueInfo("未选择要删除的播放列表哦！");
                return;
            }
            if (selectedPlaylist.Path != null)
            {
                try
                {
                    File.Delete(selectedPlaylist.Path);
                }
                catch (IOException e)
                {
                    MessageBox.Show("删除文件失败，失败信息：\n" + e.Message);
                }
            }
            DelPlaylistOnly(infos,selectedPlaylist);
        }

        /// <summary>
        /// 修改工作区标签页的标题
        /// </summary>
        /// <param name="tbi">被修改的标签页</param>
        /// <param name="name">新标题</param>
        void RenameWorkPlace(TabItem tbi, string name)
        {
            tbi.Header = name;
        }

        /// <summary>
        /// 修改播放列表的标题
        /// </summary>
        /// <param name="isWalkman">设置设备位置</param>
        /// <param name="buildPlaylist">被修改的播放列表</param>
        /// <param name="name">新标题</param>
        public void RenamePlaylist(bool isWalkman,BuildPlaylist buildPlaylist, string name)
        {
            string devicetag;
            Computer myComputer = new Computer();
            if (isWalkman) devicetag =null;
            else devicetag = "[SD]";
            try
            {
                if (buildPlaylist.Path != null)
                {
                    if (Setting.Default.IsDriveName)
                    {
                        myComputer.FileSystem.RenameFile(buildPlaylist.Path, devicetag + name + ".m3u");
                        buildPlaylist.Path =
                            (Path.GetDirectoryName(buildPlaylist.Path) + "\\" + devicetag + name + ".m3u");
                    }
                    else
                    {
                        myComputer.FileSystem.RenameFile(buildPlaylist.Path, name + ".m3u");
                        buildPlaylist.Path = (Path.GetDirectoryName(buildPlaylist.Path) + "\\" + name + ".m3u");
                    }
                }

                buildPlaylist.Name = name;
            }
            catch (IOException e)
            {
                MessageBox.Show(e.Message);
            }
        }

        /// <summary>
        /// 将自动生成的列表添加入工作区/播放列表
        /// </summary>
        /// <param name="inputDic">用户选择添加的数据</param>
        /// <param name="isWp">指示添加到的位置（工作区/播放列表）</param>
        void AddAutoBuildList(Dictionary<string, ObservableCollection<MusicInfo>> inputDic, bool? isWp)
        {
            if (isWp != null)
            {
                if (isWp == true)
                {
                    foreach (var page in inputDic)
                    {
                        TabItem tbi = (TabItem)WorkPlace.SelectedItem;
                        BuildTab buildTab = new BuildTab((TabControl)tbi.Content, page.Key, false);
                        buildTab.AddMenu += AddMenu_WorkPlace_OnBuilt;
                        buildTab.BeginBuild();
                        buildTab.NewListView.ItemsSource = page.Value;
                    }
                }
                else
                {
                    foreach (var page in inputDic)
                    {
                        BuildPlaylist playlist=BuildPlaylist(new GetArea(this).Playlist.GetPlaylistList(), page.Key, false);
                        playlist.ListViewInside.ItemsSource = page.Value;
                    }
                }
            }
        }

        /// <summary>
        /// 扫描音乐信息,并弹出窗口报告扫描进度
        /// </summary>
        /// <param name="pathList">音乐地址集合</param>
        /// <param name="isRefurbish">是否刷新页面（清除旧信息）</param>
        /// <param name="isCopy">设置是否复制文件</param>
        /// <param name="isCopyFolder">设置是否复制文件夹</param>
        /// <param name="choiceAddr">如果复制文件夹，设置被复制文件夹的绝对路径</param>
        void ScanMusic(List<FileSystemInfo> pathList,bool isRefurbish,bool isCopy,bool isCopyFolder,string choiceAddr)
        {
            LoadWindow loadWindow;
            ObservableCollection<MusicInfo> infobase;
            ListView selectedListView=null;
            GetArea area=null;
            if(!isCopy) selectedListView = new GetArea(this).WorkPlace.GetSelectedWorkPlace();
            else if (Setting.Default.CopyFileDrive == 2 && !isCopyFolder)
            {
                area = new GetArea(this);
                selectedListView =area.WorkPlace.GetSelectedWorkPlace();
            }
            else if (Setting.Default.CopyFolderDrive == 2 && isCopyFolder)
            {
                area = new GetArea(this);
                selectedListView = area.WorkPlace.GetSelectedWorkPlace();
            }
            else if (Setting.Default.CopyFileDrive == 0 && !isCopyFolder)
            {
                area = new GetArea(this, true);
                selectedListView = area.WorkPlace.GetAllWorkPlace()[0];
            }
            else if (Setting.Default.CopyFolderDrive == 0 && isCopyFolder)
            {
                area = new GetArea(this, true);
                selectedListView = area.WorkPlace.GetAllWorkPlace()[0];
            }
            else
            {
                area = new GetArea(this, false);
                selectedListView = area.WorkPlace.GetAllWorkPlace()[0];
            }
            infobase = selectedListView.ItemsSource as ObservableCollection<MusicInfo>;
            if (isCopy)
            {
                if (isCopyFolder) loadWindow = new LoadWindow(pathList, GetCopyRoot(Setting.Default.CopyFolderDrive), choiceAddr);
                else loadWindow = new LoadWindow(pathList, GetCopyRoot(Setting.Default.CopyFileDrive));
            }
            else loadWindow = new LoadWindow(pathList,infobase);
            loadWindow.ShowDialog();
            if (!loadWindow.IsCancel)
            {
                if(isRefurbish)infobase?.Clear();
                foreach (var result in loadWindow.ScanResult)
                {
                    infobase?.Add(result);
                }
                if (isCopy && Setting.Default.IsDatePlaylist) BuildTodayPlaylist(loadWindow.ScanResult,area.Playlist.GetPlaylistList());
            }
        }

        /// <summary>
        /// 得到复制文件的根路径
        /// </summary>
        /// <returns>根路径</returns>
        string GetCopyRoot(int setting)
        {
            string rootPath;
            if (setting == 0) rootPath = Setting.Default.WalkmanTag;
            else if (setting == 1) rootPath = Setting.Default.SDcardTag;
            else
            {
                if (WorkPlace.SelectedIndex == 0) rootPath = Setting.Default.WalkmanTag;
                else rootPath = Setting.Default.SDcardTag;
            }

            return rootPath;
        }

        /// <summary>
        /// 打开文件（弹出选择窗口）
        /// </summary>
        /// <param name="isCopy">是否复制文件</param>
        List<FileSystemInfo> OpenMusicFunc()
        {
            List<FileSystemInfo> pathList = new List<FileSystemInfo>();
            OpenFileDialog fileDialog = new OpenFileDialog();
            fileDialog.Multiselect = true;
            fileDialog.Filter = @"全部支持的音频文件|*.mp1;*.mp2;*.mp3;*.m4a;*.flac;*.wav;*.aif;*.ogg;*.ape;*.dff;*.dsf|MPEG Audio|*.mp1;*.mp2;*.mp3;|Free Lossless Audio|*.flac|Windows Wave|*.wav|Audio Interchange File Format|*.aif|OGGVobis|*.ogg|Monkey's audio|*.ape|Apple lossless audio&Advanced Audio Coding|*.m4a|Direct Stream Digital|*.dff;*.dsf";
            if (fileDialog.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                foreach (var path in fileDialog.FileNames)
                {
                    pathList.Add(new FileInfo(path));
                }
            }
            return pathList;
        }

        /// <summary>
        /// 导入播放列表
        /// </summary>
        void OpenPlaylistFunc()
        {
            OpenFileDialog fileDialog = new OpenFileDialog();
            fileDialog.Multiselect = true;
            fileDialog.Filter = @"全部支持的音乐列表文件|*.m3u;*.m3u8|音乐列表文件|*.m3u|音乐列表文件|*.m3u8";
            if (fileDialog.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                foreach (var path in fileDialog.FileNames)
                {
                    LoadPlaylist(path,false);
                }
            }
        }

        /// <summary>
        /// 打开文件夹（弹出选择窗口）
        /// </summary>
        /// <param name="isCopy">是否复制文件夹</param>
        List<FileSystemInfo> OpenFolderFunc(out string path)
        {
            List<FileSystemInfo> pathList = new List<FileSystemInfo>();
            FolderBrowserDialog folderDialog = new FolderBrowserDialog();
            if (folderDialog.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                Director(folderDialog.SelectedPath, pathList);
            }

            path = folderDialog.SelectedPath;
            return pathList;
        }

        /// <summary>
        /// 选择位置保存播放列表
        /// </summary>
        /// <param name="isWalkmanPlaylist"></param>
        void ChooseToSavePlaylist(bool isWalkmanPlaylist)
        {
            GetArea area=new GetArea(this);
            if (area.Playlist.GetSelectedPlaylist() != null)
            {
                SaveFileDialog saveFileDialog = new SaveFileDialog();
                saveFileDialog.Filter = @"音乐列表文件|*.m3u|音乐列表文件|*.m3u8|全部支持的音乐列表文件|*.m3u;*.m3u8";
                saveFileDialog.AddExtension = true;
                saveFileDialog.OverwritePrompt = true;
                saveFileDialog.FileName = area.Playlist.GetSelectedBuildPlaylist().Name;
                try
                {
                    if (saveFileDialog.ShowDialog() == System.Windows.Forms.DialogResult.OK)
                    {
                        SavePlaylist(isWalkmanPlaylist, saveFileDialog.FileName,
                            area.Playlist.GetSelectedBuildPlaylist(),
                            true);
                    }
                }
                catch (UnauthorizedAccessException)
                { MessageBox.Show("访问被拒绝，请退出占用设备内存的程序（如音乐播放器），然后再试"); }
                catch (DirectoryNotFoundException)
                { StatueBar.Text = "地址不存在，请检查Walkman是否正确连接电脑并开启文件传输功能，或者前往设置更改默认检测盘符。"; }
                catch (IOException e1)
                { MessageBox.Show("设备未就绪，请重新唤醒设备，错误信息：" + e1.Message); }
                catch (ArgumentException e2)
                {
                    MessageBox.Show("播放列表名称中含有非法字符，无法建立文件。请尝试重命名播放列表。" + e2.ParamName);
                }
            }
        }

        /// <summary>
        /// 向播放列表区域添加播放列表
        /// </summary>
        /// <param name="isRefurbish">是否刷新播放列表</param>
        /// <param name="isWalkmanPlaylist"></param>
        void AddPlaylist(bool isRefurbish)
        {
            GetArea area = new GetArea(this);
            if (isRefurbish)
            {
                var list = area.Playlist.GetPlaylistList().ItemsSource as ObservableCollection<BuildPlaylist>;
                list?.Clear();
            }
            LoadPlaylist(area.GetSavePath(),true);
        }

        /// <summary>
        /// 建立以今天日期为名的播放列表
        /// </summary>
        /// <param name="infos">播放列表内存储的音乐</param>
        /// <returns>建立的播放列表</returns>
        private BuildPlaylist BuildTodayPlaylist(List<MusicInfo> infos,ListView playlistList)
        {
            string date = DateTime.Today.Year.ToString().Remove(0, 2) + "年" + DateTime.Today.Month + "月" +
                          DateTime.Today.Day + "日";
            BuildPlaylist playlist=BuildPlaylist(playlistList, date, false);
            ObservableCollection<MusicInfo> collection =
                playlist.ListViewInside.ItemsSource as ObservableCollection<MusicInfo>;
            foreach (var info in infos)
            {
                if (collection != null) collection.Add(info);
            }

            return playlist;
        }

        /// <summary>
        /// 将任意格式的文本转换为UTF8格式
        /// </summary>
        /// <param name="path">文件地址</param>
        /// <param name="encoding">文本格式</param>
        private void TransTextFormatToUTF8(string path,Encoding encoding)
        {
            StreamReader reader=new StreamReader(new FileStream(path,FileMode.Open,FileAccess.Read),encoding);
            string content = reader.ReadToEnd();
            reader.Close();
            StreamWriter writer =
                new StreamWriter(new FileStream(path, FileMode.Create, FileAccess.Write),Encoding.UTF8);
            writer.Write(content);
            writer.Close();
        }

        /// <summary>
        /// 转换某个列表中歌曲的歌词文本格式
        /// </summary>
        /// <param name="lv">承载音乐的列表</param>
        /// <returns>转换歌词成功的音乐</returns>
        private List<MusicInfo> TransformLyricFormat(ListView lv)
        {
            List<MusicInfo> changedLyric=new List<MusicInfo>();
            foreach (MusicInfo info in lv.Items)
            {
                string lrcPath = Path.ChangeExtension(info.Path, "lrc");
                if (File.Exists(lrcPath))
                {
                    try
                    {
                        Encoding fileEncoding = EncodingType.GetType(lrcPath);
                        if (fileEncoding!= Encoding.UTF8)
                        {
                            TransTextFormatToUTF8(lrcPath, fileEncoding);
                            changedLyric.Add(info);
                        }
                    }
                    catch (IOException e)
                    {
                        MessageBox.Show("读取歌曲文件失败：" + lrcPath + "。" + e.Message);
                    }
                }
            }

            return changedLyric;
        }

        /// <summary>
        ///  音乐替换
        /// </summary>
        /// <param name="replacedmusic">最新的音乐</param>
        private void ReplaceMusic(List<FileSystemInfo> replacedmusic)
        {
            GetArea area=new GetArea(this);
            foreach (MusicInfo info in area.WorkPlace.GetAllWorkPlace()[0].Items)
            {
                foreach (FileSystemInfo musicFile in replacedmusic)
                {
                    if (Path.GetFileNameWithoutExtension(info.Path) ==
                        Path.GetFileNameWithoutExtension(musicFile.FullName))
                    {
                        try
                        {
                            File.Delete(info.Path);
                            File.Copy(musicFile.FullName, Path.GetDirectoryName(info.Path) +"\\"+ musicFile.Name);
                            if (area.Playlist.GetAllPlaylistSongs().Count > 0)
                                foreach (MusicInfo playlistinfo in area.Playlist.GetAllPlaylistSongs().Keys)
                                {
                                    if (info.Path == playlistinfo.Path)
                                        playlistinfo.Path = Path.GetDirectoryName(info.Path) + "\\" + musicFile.Name;
                                }
                            if (area.WorkPlace.GetAllWorkPlaceSongsExceptFirstTab().Count > 0)
                                foreach (MusicInfo workPlaceInfo in area.WorkPlace.GetAllWorkPlaceSongsExceptFirstTab()
                                    .Keys)
                                {
                                    if (info.Path == workPlaceInfo.Path)
                                        workPlaceInfo.Path = Path.GetDirectoryName(info.Path) + "\\" + musicFile.Name;
                                }
                            info.Path = Path.GetDirectoryName(info.Path) + "\\" + musicFile.Name;
                            ChangeStatueInfo("替换完成！");
                        }
                        catch (IOException e)
                        {
                            ChangeStatueInfo("对文件进行操作时出现错误："+e.Message);
                        }
                    }
                }
            }
        }

        private List<MusicInfo> RepairPlaylistFunc()
        {
            List<MusicInfo> list = new List<MusicInfo>();
            GetArea area = new GetArea(this);
            foreach (MusicInfo info in area.Playlist.GetAllPlaylistSongs().Keys)
            {
                if (!File.Exists(info.Path))
                {
                    List<MusicInfo> infos = area.WorkPlace.GetAllWorkPlace()[0].ItemsSource.Cast<MusicInfo>().Where(
                        subinfo => subinfo.Title == info.Title && subinfo.Album == info.Album).ToList();
                    if (infos.Count > 0) list.Add(info);
                    else MessageBox.Show("未能匹配到音乐项目：" + info.Title);
                }
            }

            return list;
        }

        private void ReflushPlaylistStatue(ListView lv)
        {
            if (lv.Items.Count != 0)
            {
                foreach (BuildPlaylist playlist in lv.Items)
                {
                    playlist.ReflushLv();
                }
                int index = lv.SelectedIndex;
                lv.SelectedItem = null;
                lv.SelectedIndex = index;
                //硬核刷新
            }
        }

        private void TestFunc()
        {
            var tree = Walkman_PlaylistList.View;
            
        }




        #endregion

        #region ControlFunc


        private void SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            BuildPlaylist buildPlaylist;
            GetArea area = new GetArea(this);
            try
            {
                area.Playlist.GetGrid().Children.Clear();
                buildPlaylist = area.Playlist.GetPlaylistList().SelectedItem as BuildPlaylist;
                if (buildPlaylist != null) area.Playlist.GetGrid().Children.Add(buildPlaylist.ListViewInside);
            }
            catch (Exception)
            {
                // ignored
            }
        }

        private void PlaylistMenuItem_OnClick(object sender, RoutedEventArgs e)
        {
            MenuItemWithListView menuItem = sender as MenuItemWithListView;
            if (menuItem != null)
            {
                ListView lv = new GetArea(this).Playlist.GetSelectedPlaylist();
                if (lv!=null)foreach (MusicInfo info in lv.Items)
                {
                    ObservableCollection<MusicInfo> infos = menuItem.ListViewInseide.ItemsSource as ObservableCollection<MusicInfo>;
                    if (info.Check == true)
                    {
                        if (infos == null) menuItem.ListViewInseide.Items.Add(info.Clone());
                        else infos.Add(info.Clone());
                    }
                }
            }
        }

        private void SettingButton_Click(object sender, RoutedEventArgs e)
        {
            SettingWindow setting = new SettingWindow(this);
            setting.ShowDialog();
        }

        private void AddWorkplaceButton_OnClick(object sender, RoutedEventArgs e)
        {
            TabItem tbi = (TabItem) WorkPlace.SelectedItem;
            BuildTab buildTab=new BuildTab((TabControl)tbi.Content, "工作区_",true);
            buildTab.AddMenu += AddMenu_WorkPlace_OnBuilt;
            buildTab.BeginBuild();
        }

        private void AddPlaylistButton_OnClick(object sender, RoutedEventArgs e)
        {
            BuildPlaylist(new GetArea(this).Playlist.GetPlaylistList(), "新播放列表", true);
        }

        private void Playlist_AddMenu(BuildPlaylist buildPlaylist, AddMenuEventArgs e)
        {
            AddPlaylistMenu(new GetArea(this).IsWalkman, buildPlaylist, buildPlaylist.ListViewInside);
        }

        private void DeleteWorkplaceButton_OnClick(object sender, RoutedEventArgs e)
        {
            TabItem item = (TabItem)WorkPlace.SelectedItem;
            TabControl tab = (TabControl)item.Content;
            if (tab.SelectedIndex != 0)
            {
                TabItem tbi = (TabItem) tab.SelectedItem;
                Del_Tab(tab, tbi,1);
                DelMenuItem(new GetArea(this).WorkPlace.GetSampleMenu(),tbi.Header.ToString());
            }
        }

        private void DeletePlaylistButton_OnClick(object sender, RoutedEventArgs e)
        {
            ListView lv=new GetArea(this).Playlist.GetPlaylistList();
            if (Setting.Default.DelPlaylist == 2)
            {
                DelWindow delWindow = new DelWindow();
                delWindow.ShowDialog();
                if (delWindow.isConform)
                {
                    if(Setting.Default.IsDelInProgram)DelPlaylistOnly(lv.ItemsSource as ObservableCollection<BuildPlaylist>,lv.SelectedItem as BuildPlaylist);
                    else DelFileAndPlaylist(lv.ItemsSource as ObservableCollection<BuildPlaylist>, lv.SelectedItem as BuildPlaylist);
                }
            }
            else
            {
                if(Setting.Default.DelPlaylist==1)DelFileAndPlaylist(lv.ItemsSource as ObservableCollection<BuildPlaylist>, lv.SelectedItem as BuildPlaylist);
                else DelPlaylistOnly(lv.ItemsSource as ObservableCollection<BuildPlaylist>, lv.SelectedItem as BuildPlaylist);
            }
        }

        private void RenameButtonL_Click(object sender, RoutedEventArgs e)
        {
            TabItem tbi = new GetArea(this).WorkPlace.GetSelectedSubTBI();
            RenameWindow rename = new RenameWindow(tbi.Header.ToString());
            if(rename.Confirm)RenameWorkPlace(tbi,rename.NewTitle);
        }

        private void RenameButtonR_OnClick(object sender, RoutedEventArgs e)
        {
            GetArea area = new GetArea(this);
            BuildPlaylist buildPlaylist;
            buildPlaylist = area.Playlist.GetPlaylistList().SelectedItem as BuildPlaylist;
            if (buildPlaylist != null)
            {
                RenameWindow rename = new RenameWindow(buildPlaylist.Name);
                if (rename.Confirm) RenamePlaylist(area.IsWalkman,buildPlaylist, rename.NewTitle);
            }
        }

        private void RefurbishPCButton_OnClick(object sender, RoutedEventArgs e)
        {
            List<FileSystemInfo> pathList=new List<FileSystemInfo>();
            if (WorkPlace.SelectedIndex==0 )Director(Setting.Default.WalkmanTag + "\\Music\\",pathList);
            else Director(Setting.Default.SDcardTag+"\\Music\\",pathList);
            try
            {
                ScanMusic(pathList, true, false, false, null);
            }
            catch (NullReferenceException)
            {
                MessageBox.Show("未能完成操作，因为音乐信息尝试添加到了一个不存在的工作区。");
            }
        }

        private void MainWindow_OnLoaded(object sender, RoutedEventArgs e)
        {
            ReadTemp();
        }

        private void MainWindow_OnClosing(object sender, CancelEventArgs e)
        {
            WriteTemp();
        }

        private void DelItemButtonL_OnClick(object sender, RoutedEventArgs e)
        {
            DelItem(new GetArea(this).WorkPlace.GetSelectedWorkPlace());
        }

        private void CheckedButtonL_OnClick(object sender, RoutedEventArgs e)
        {
            CheckedAll(new GetArea(this).WorkPlace.GetSelectedWorkPlace());
        }

        private void ClearCheckedButtonL_OnClick(object sender, RoutedEventArgs e)
        {
            UnCheckedAll(new GetArea(this).WorkPlace.GetSelectedWorkPlace());
        }

        private void InverseButtonL_OnClick(object sender, RoutedEventArgs e)
        {
            Inverse(new GetArea(this).WorkPlace.GetSelectedWorkPlace());
        }

        private void ClearButtonL_OnClick(object sender, RoutedEventArgs e)
        {
            ClearItem(new GetArea(this).WorkPlace.GetSelectedWorkPlace());
        }

        private void ToCheckButtonL_OnClick(object sender, RoutedEventArgs e)
        {
            ToChecked(new GetArea(this).WorkPlace.GetSelectedWorkPlace());
        }

        private void SaveButtonL_OnInitialized(object sender, EventArgs e)
        {
            ChooseButtonInitialize(sender);
        }

        private void SaveButtonL_OnClick(object sender, RoutedEventArgs e)
        {
            ChooseButtonClick(sender, new GetArea(this).WorkPlace.GetSampleMenu());
        }

        private void PlayButtonL_OnClick(object sender, RoutedEventArgs e)
        {

            PlayWindow playWindow;
            MusicInfo info = (MusicInfo)new GetArea(this).WorkPlace.GetSelectedWorkPlace().SelectedItem;
            if ( info!= null)
            {
                playWindow=new PlayWindow(info);
                playWindow.Show();
            }
        }

        private void SearchButtonL_OnClick(object sender, RoutedEventArgs e)
        {
            var tbi = WorkPlace.SelectedItem as TabItem;
            var tbc = tbi?.Content as TabControl;
            if (tbc != null)
                SearchLv(new GetArea(this).WorkPlace.GetSelectedWorkPlace(), SearchBoxL.Text, true, tbc.SelectedIndex);
        }

        private void AddMenu_WorkPlace_OnBuilt(BuildTab sender, AddMenuEventArgs e)
        {
            AddWorkPlaceMenu(new GetArea(this).IsWalkman,sender.NewTabItem,sender.NewListView);
        }

        private void WorkPlaceMenuItem_OnClick(object sender, RoutedEventArgs e)
        {
            MenuItemWithListView menuItem =sender as MenuItemWithListView;
            if (menuItem != null)
            {
                ListView targetlv = menuItem.ListViewInseide;
                foreach (MusicInfo info in new GetArea(this).WorkPlace.GetSelectedWorkPlace().Items)
                {
                    if (info.Check == true)
                    {
                        ObservableCollection<MusicInfo> infobase = targetlv.ItemsSource as ObservableCollection<MusicInfo>;
                        infobase?.Add(info.Clone());
                    }
                }
            }
        }

        private void SearchBoxL_OnKeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key is Key.Enter)
            {
                var tbi = WorkPlace.SelectedItem as TabItem;
                var tbc = tbi?.Content as TabControl;
                if (tbc != null)
                    SearchLv(new GetArea(this).WorkPlace.GetSelectedWorkPlace(), SearchBoxL.Text, true,
                        tbc.SelectedIndex);
                ReFocusTextBox(sender);
            }
        }

        private void MoveButtonR_OnClick(object sender, RoutedEventArgs e)
        {
            try
            {
                ListView playlist = new GetArea(this).Playlist.GetSelectedPlaylist();
                ListView lv =new GetArea(this).WorkPlace.GetSelectedWorkPlace();
                MoveItem(lv, playlist);
                foreach (MusicInfo info in lv.Items) info.Check = false;
            }
            catch (NullReferenceException)
            {
                ChangeStatueInfo("还没有选择播放列表哦！");
            }

        }

        private void DelItemButtonR_OnClick(object sender, RoutedEventArgs e)
        {
            DelItem(new GetArea(this).Playlist.GetSelectedPlaylist());
        }

        private void CheckedButtonR_OnClick(object sender, RoutedEventArgs e)
        {
            CheckedAll(new GetArea(this).Playlist.GetSelectedPlaylist());
        }

        private void ClearCheckedButtonR_OnClick(object sender, RoutedEventArgs e)
        {
            UnCheckedAll(new GetArea(this).Playlist.GetSelectedPlaylist());
        }

        private void InverseButtonR_OnClick(object sender, RoutedEventArgs e)
        {
            Inverse(new GetArea(this).Playlist.GetSelectedPlaylist());
        }

        private void ToCheckButtonR_OnClick(object sender, RoutedEventArgs e)
        {
            ToChecked(new GetArea(this).Playlist.GetSelectedPlaylist());
        }

        private void ClearButtonR_OnClick(object sender, RoutedEventArgs e)
        {
            ClearItem(new GetArea(this).Playlist.GetSelectedPlaylist());
        }

        private void SearchButtonR_OnClick(object sender, RoutedEventArgs e)
        {
            int listLabel;
            if (Playlist.SelectedIndex == 0) listLabel = Walkman_PlaylistList.SelectedIndex;
            else listLabel = SDCard_PlaylistList.SelectedIndex;
            SearchLv(new GetArea(this).Playlist.GetSelectedPlaylist(), SearchBoxR.Text,false,listLabel);
        }

        private void SearchBoxR_OnKeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                int listLabel;
                if (Playlist.SelectedIndex == 0) listLabel = Walkman_PlaylistList.SelectedIndex;
                else listLabel = SDCard_PlaylistList.SelectedIndex;
                SearchLv(new GetArea(this).Playlist.GetSelectedPlaylist(), SearchBoxR.Text, false, listLabel);
                ReFocusTextBox(sender);
            }
        }

        private void SaveButtonR_OnInitialized(object sender, EventArgs e)
        {
            ChooseButtonInitialize(sender);
        }

        private void SaveButtonR_OnClick(object sender, RoutedEventArgs e)
        {
            ChooseButtonClick(sender, new GetArea(this).Playlist.GetSampleMenu());
        }

        private void MoveAllButtonR_OnClick(object sender, RoutedEventArgs e)
        {
            GetArea area = new GetArea(this);
            SaveWorkPlaceToPlaylist(area.Playlist.GetPlaylistList(),area.WorkPlace.GetSelectedSubTBI());
        }

        private void SaveButton_OnClick(object sender, RoutedEventArgs e)
        {
            GetArea area = new GetArea(this);
            try
            {
                SavePlaylist(true, area.GetSavePath(), area.Playlist.GetPlaylistList().SelectedItem as BuildPlaylist, false);
                ChangeStatueInfo("已保存");
            }
            catch (UnauthorizedAccessException)
            { MessageBox.Show("访问被拒绝，请退出占用设备内存的程序（如音乐播放器），然后再试"); }
            catch (DirectoryNotFoundException)
            { StatueBar.Text = "地址不存在，请检查Walkman是否正确连接电脑并开启文件传输功能，或者前往设置更改默认检测盘符。"; }
            catch (IOException e1)
            { MessageBox.Show("设备未就绪，请重新唤醒设备，错误信息：" + e1.Message); }
            catch (ArgumentException e2)
            {
                MessageBox.Show("播放列表名称中含有非法字符，无法建立文件。请尝试重命名播放列表。" + e2.ParamName);
            }
        }

        private void SaveAllButton_OnClick(object sender, RoutedEventArgs e)
        {
            GetArea area = new GetArea(this);
            try
            {
                foreach (BuildPlaylist playlist in area.Playlist.GetPlaylistList().Items)
                {
                    SavePlaylist(true, area.GetSavePath(), playlist, false);
                }
                ChangeStatueInfo("已保存");
            }
            catch (UnauthorizedAccessException)
            { MessageBox.Show("访问被拒绝，请退出占用设备内存的程序（如音乐播放器），然后再试"); }
            catch (DirectoryNotFoundException)
            { StatueBar.Text = "地址不存在，请检查Walkman是否正确连接电脑并开启文件传输功能，或者前往设置更改默认检测盘符。"; }
            catch (IOException e1)
            { MessageBox.Show("设备未就绪，请重新唤醒设备，错误信息：" + e1.Message); }
            catch (ArgumentException e2)
            {
                MessageBox.Show("播放列表名称中含有非法字符，无法建立文件。请尝试重命名播放列表。" + e2.ParamName);
            }
        }

        private void RefurbishDeviceButton_OnClick(object sender, RoutedEventArgs e)
        {
            AddPlaylist(true);
        }

        private void MoveButtonL_Click(object sender, RoutedEventArgs e)
        {
            GetArea area = new GetArea(this);
            ListView playlist = area.Playlist.GetSelectedPlaylist();
            ListView lv;
            if (playlist == null) ChangeStatueInfo("还没有选择播放列表哦！");
            else
            {
                lv = area.WorkPlace.GetSelectedWorkPlace();
                MoveItem(playlist,lv);
                foreach (MusicInfo info in playlist.Items) info.Check = false;
            }
        }

        private void MoveAllButtonL_OnClick(object sender, RoutedEventArgs e)
        {
            GetArea area = new GetArea(this);
            SavePlaylistToWorkPlace(area.WorkPlace.GetSelectedSubTBC(), area.Playlist.GetPlaylistList().SelectedItem as BuildPlaylist);
        }

        private void ByFileName_OnClick(object sender, RoutedEventArgs e)
        {
            SelectWindow selectWindow=new SelectWindow(new GetArea(this).WorkPlace.GetSelectedWorkPlace());
            selectWindow.ByFolderName();
            selectWindow.ShowDialog();
            AddAutoBuildList(selectWindow.SelectedConections,selectWindow.IsInWorkPlace);
        }

        private void ByImportTime_OnClick(object sender, RoutedEventArgs e)
        {
            SelectWindow selectWindow=new SelectWindow(new GetArea(this).WorkPlace.GetSelectedWorkPlace());
            selectWindow.ByImportTime();
            selectWindow.ShowDialog();
            AddAutoBuildList(selectWindow.SelectedConections, selectWindow.IsInWorkPlace);
        }

        private void ByFormat_OnClick(object sender, RoutedEventArgs e)
        {
            SelectWindow selectWindow = new SelectWindow(new GetArea(this).WorkPlace.GetSelectedWorkPlace());
            selectWindow.ByFormat();
            selectWindow.ShowDialog();
            AddAutoBuildList(selectWindow.SelectedConections, selectWindow.IsInWorkPlace);
        }

        private void ByArtist_OnClick(object sender, RoutedEventArgs e)
        {
            SelectWindow selectWindow = new SelectWindow(new GetArea(this).WorkPlace.GetSelectedWorkPlace());
            selectWindow.ByArtist();
            selectWindow.ShowDialog();
            AddAutoBuildList(selectWindow.SelectedConections, selectWindow.IsInWorkPlace);
        }

        private void ByAge_OnClick(object sender, RoutedEventArgs e)
        {
            SelectWindow selectWindow = new SelectWindow(new GetArea(this).WorkPlace.GetSelectedWorkPlace());
            selectWindow.ByAge();
            selectWindow.ShowDialog();
            AddAutoBuildList(selectWindow.SelectedConections, selectWindow.IsInWorkPlace);
        }

        private void ByTitle_OnClick(object sender, RoutedEventArgs e)
        {
            try
            {
                SelectWindow selectWindow = new SelectWindow(new GetArea(this).WorkPlace.GetSelectedWorkPlace());
                selectWindow.ByTitle(fristInitDetector);
                fristInitDetector = false;
                selectWindow.ShowDialog();
                AddAutoBuildList(selectWindow.SelectedConections, selectWindow.IsInWorkPlace);
            }
            catch (System.IO.FileLoadException ex)
            {
                MessageBox.Show("message:" + ex.Message + "\nfilename:" + ex.FileName + "\nfusionlog:" + ex.FusionLog);
            }
        }

        private void OpenFile_OnClick(object sender, RoutedEventArgs e)
        {
            List<FileSystemInfo> list = OpenMusicFunc();
            if(list.Count>0)ScanMusic(list, false, false, false, null);
        }

        private void OpenFolder_OnClick(object sender, RoutedEventArgs e)
        {
            string path;
            List<FileSystemInfo> list=OpenFolderFunc(out path);
            if (list.Count>0) ScanMusic(list, false, false, false, null);
        }

        private void CopyFIle_OnClick(object sender, RoutedEventArgs e)
        {
            try
            {
                List<FileSystemInfo> list = OpenMusicFunc();
                if (list.Count > 0) ScanMusic(list, false, true, false, null);
            }
            catch (NullReferenceException)
            {
                MessageBox.Show("未能完成操作，因为音乐信息尝试添加到了一个不存在的工作区。");
            }
        }

        private void CopyFolder_OnClick(object sender, RoutedEventArgs e)
        {
            string path;
            List<FileSystemInfo>list =OpenFolderFunc(out path);
            if(list.Count>0)ScanMusic(list, false, true, true,path);
        }

        private void ImportLocalPlaylist_OnClick(object sender, RoutedEventArgs e)
        {
            OpenPlaylistFunc();
        }

        private void OutputWalkmanPlaylist_OnClick(object sender, RoutedEventArgs e)
        {
            ChooseToSavePlaylist(true);
        }

        private void OutputNormalPlaylist_OnClick(object sender, RoutedEventArgs e)
        {
            ChooseToSavePlaylist(false);
        }

        private void ReloadDatabase_OnClick(object sender, RoutedEventArgs e)
        {
            ReadTemp();
        }

        private void ImportOnlinePlaylist_OnClick(object sender, RoutedEventArgs e)
        {
            GetArea area = new GetArea(this);
            GetOnlinePlaylistWindow onlineWindow = new GetOnlinePlaylistWindow(
                area.WorkPlace.GetAllWorkPlace()[0].ItemsSource as ObservableCollection<MusicInfo>);
            onlineWindow.ShowDialog();
            if(onlineWindow.PlaylistTitle!=null)
            {
                BuildPlaylist(area.Playlist.GetPlaylistList(), onlineWindow.PlaylistTitle, false).ListViewInside.ItemsSource =
                onlineWindow.TargetInfos;
                if (onlineWindow.ErrorInfos.Count != 0) new NotFoundWindow(onlineWindow.ErrorInfos).ShowDialog();
            }
        }

        private void ManagePlaylist_OnClick(object sender, RoutedEventArgs e)
        {
            ManagePlaylistWindow managePlaylistWindow = new ManagePlaylistWindow(this);
            managePlaylistWindow.ShowDialog();
        }

        private void UpgradeMusic_OnClick(object sender, RoutedEventArgs e)
        {
            var list = OpenMusicFunc();
            if(list.Count>0)ReplaceMusic(list);
        }

        private void RepairLyric_OnClick(object sender, RoutedEventArgs e)
        {
            List<MusicInfo> list=TransformLyricFormat(new GetArea(this).WorkPlace.GetAllWorkPlace()[0]);
            if (list.Count > 0) MessageBox.Show("歌词格式转换完毕，在目前选中的设备音乐库中共转换了" + list.Count + "个歌词文件。");
            else MessageBox.Show("当前设备中没有发现需要转换格式的歌词文件。");
        }

        private void CustomPlaylist_OnClick(object sender, RoutedEventArgs e)
        { 
            GetArea area = new GetArea(this);
            CustomPlaylistWindow customPlaylistWindow=new CustomPlaylistWindow(area.WorkPlace.GetSelectedWorkPlace());
            customPlaylistWindow.ShowDialog();
            if(customPlaylistWindow.ResultCollection!=null)BuildPlaylist(area.Playlist.GetPlaylistList(), "规则生成列表", true).ListViewInside.ItemsSource =
                customPlaylistWindow.ResultCollection;
        }

        private void AutoDownloadLyric_OnClick(object sender, RoutedEventArgs e)
        {
            var area = new GetArea(this);
            if(area.WorkPlace.GetSelectedWorkPlace().Items.Count>0)(new GetLyricWindow(area.WorkPlace.GetSelectedWorkPlace())).ShowDialog();
        }

        private void About_OnClick(object sender, RoutedEventArgs e)
        {
            (new AboutWindow(0)).Show();
        }

        private void QandA_OnClick(object sender, RoutedEventArgs e)
        {
            (new AboutWindow(1)).Show();
        }

        private void Support_OnClick(object sender, RoutedEventArgs e)
        {
            (new AboutWindow(2)).Show();
        }

        private void DragButton_OnClick(object sender, RoutedEventArgs e)
        {
            Setting.Default.isDragToChangeOrder = (bool)DragButton.IsChecked;
            Setting.Default.Save();
            ReflushPlaylistStatue(new GetArea(this).Playlist.GetPlaylistList());
        }



        #endregion

        private void TestButton_OnClick(object sender, RoutedEventArgs e)
        {
            TestFunc();
        }
    }
}
