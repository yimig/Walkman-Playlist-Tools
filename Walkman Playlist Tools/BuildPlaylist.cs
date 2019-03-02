using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Markup;
using System.Xml;
using System.Xml.Linq;
using ListViewDragDemo.Utils;

namespace Walkman_Playlist_Tools
{
    /// <summary>
    /// 播放列表区域
    /// </summary>
    public class BuildPlaylist : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;
        public event AddPlaylistMenuEventHandler AddMenu;
        private ListView listViewInside;
        private string name,path;
        private bool @checked;
        private ObservableCollection<MusicInfo> infos;
        private ListViewDragDropManager<MusicInfo> dragManager;

        public bool Checked
        {
            get => @checked;
            set
            {
                @checked = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("Checked"));
            }
        }

        public string Name
        {
            get => name;
            set
            {
                name = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("Name"));
            }
        }

        public ListView ListViewInside
        {
            get => listViewInside;
            set
            {
                listViewInside = value;
                if (PropertyChanged != null)
                {
                    PropertyChanged(this,new PropertyChangedEventArgs("ListViewInside"));
                }
            }
        }

        public string Path
        {
            get => path;
            set => path = value;
        }

        public ObservableCollection<MusicInfo> Infos
        {
            get => infos;
            set => infos = value;
        }

        /// <summary>
        /// 添加并创建播放列表
        /// </summary>
        /// <param name="targetlv">承载播放列表的列表</param>
        /// <param name="name">名字</param>
        /// <param name="isAutoName">是否按照序号自动重命名</param>
        public BuildPlaylist(ListView targetlv,string name,bool isAutoName)
        {
            if (isAutoName) this.name = name + "_" + (targetlv.Items.Count+1);
            else this.name = name;
            infos = new ObservableCollection<MusicInfo>();
        }

        /// <summary>
        /// 复制对象时使用的构造函数
        /// </summary>
        /// <param name="listViewInside"></param>
        /// <param name="name"></param>
        /// <param name="path"></param>
        /// <param name="infos"></param>
        public BuildPlaylist(ListView listViewInside, string name, string path, ObservableCollection<MusicInfo> infos)
        {

            this.listViewInside = listViewInside;
            this.name = this.name;
            this.path = path;
            this.infos = infos;
        }

        /// <summary>
        /// 仅添加播放列表
        /// </summary>
        /// <param name="name">名字</param>
        public BuildPlaylist(string name)
        {
            listViewInside=new ListView();
            this.name = name;
            InitializeFullListview();
            infos = new ObservableCollection<MusicInfo>();
            ListViewInside.ItemsSource = infos;
        }

        public void BeginBuild()
        {
            listViewInside=new ListView();
            InitializeListView();
            if (AddMenu != null)
            {
                var e = new AddMenuEventArgs {isWorkPlace = false};
                AddMenu.Invoke(this,e);
            }

            ListViewInside.ItemsSource = infos;
            if (Setting.Default.isDragToChangeOrder)
            {
                AddDragToChangeOrderFunc();
            }
        }

        public void AddDragToChangeOrderFunc()
        {
            dragManager = new ListViewDragDropManager<MusicInfo>(listViewInside);
            dragManager.ShowDragAdorner = true;
            dragManager.DragAdornerOpacity = 0.7;
        }

        public void ReflushLv()
        {
            if (Setting.Default.isDragToChangeOrder)
            {
                AddDragToChangeOrderFunc();
            }
            else
            {
                var newListView = new ListView();
                newListView.ItemsSource = listViewInside.ItemsSource;
                listViewInside = newListView;
                InitializeListView();
            }
        }

        public void InitializeFullListview()
        {
            var gv = new GridView();
            var checkColumn = new GridViewColumn();
            checkColumn.Width = 30;
            /*XNamespace ns = "http://schemas.microsoft.com/winfx/2006/xaml/presentation";
            var xe = new XElement(ns + "DataTemplate",
                new XAttribute("xmlns", "http://schemas.microsoft.com/winfx/2006/xaml/presentation"),
                new XElement(ns + "CheckBox", new XAttribute("IsChecked", @"{Binding Check}"))
            );
            var xr = xe.CreateReader();
            var dt = XamlReader.Load(xr) as DataTemplate;*/
            FrameworkElementFactory dataFactory = new FrameworkElementFactory(typeof(DragCheckBox));
            Binding bind = new Binding("Check");
            dataFactory.SetBinding(DragCheckBox.IsCheckedProperty, bind);
            var dataTemplate = new DataTemplate();
            dataTemplate.VisualTree = dataFactory;
            checkColumn.CellTemplate = dataTemplate;
            gv.Columns.Add(checkColumn);
            var titleColumn = new GridViewColumn();
            titleColumn.Header = "标题";
            titleColumn.Width = 200;
            titleColumn.DisplayMemberBinding = new Binding("Title");
            gv.Columns.Add(titleColumn);
            var formatColumn = new GridViewColumn();
            formatColumn.Header = "格式";
            formatColumn.Width = 70;
            formatColumn.DisplayMemberBinding = new Binding("Format");
            gv.Columns.Add(formatColumn);
            var artistColumn = new GridViewColumn();
            artistColumn.Header = "艺术家";
            artistColumn.Width = 70;
            artistColumn.DisplayMemberBinding = new Binding("Artist");
            gv.Columns.Add(artistColumn);
            var albumColumn = new GridViewColumn();
            albumColumn.Header = "专辑名称";
            albumColumn.Width = 100;
            albumColumn.DisplayMemberBinding = new Binding("Album");
            gv.Columns.Add(albumColumn);
            var lengthColumn = new GridViewColumn();
            lengthColumn.Header = "曲长";
            lengthColumn.Width = 70;
            lengthColumn.DisplayMemberBinding = new Binding("Length");
            gv.Columns.Add(lengthColumn);
            var ageColumn = new GridViewColumn();
            ageColumn.Header = "年代";
            ageColumn.Width = 70;
            ageColumn.DisplayMemberBinding = new Binding("Year");
            gv.Columns.Add(ageColumn);
            var buildtimeColumn = new GridViewColumn();
            buildtimeColumn.Header = "同步时间";
            buildtimeColumn.Width = 70;
            buildtimeColumn.DisplayMemberBinding = new Binding("Buildtime");
            gv.Columns.Add(buildtimeColumn);
            var pathColumn = new GridViewColumn();
            pathColumn.Header = "文件路径";
            pathColumn.Width = 400;
            pathColumn.DisplayMemberBinding = new Binding("Path");
            gv.Columns.Add(pathColumn);
            ListViewInside.View = gv;
        }

        /// <summary>
        /// 创建播放列表
        /// </summary>
        public  void InitializeListView()
        {
            var gv = new GridView();
            var checkColumn = new GridViewColumn();
            checkColumn.Width = 30;
            /*XAttribute[] attribute=new XAttribute[3];
            attribute[0] = new XAttribute("IsChecked", @"{Binding Check}");
            attribute[1]=new XAttribute("ContentType" ,DragCheckBox.Content.MusicInfo);
            attribute[2] = new XAttribute("xmlns", "clr-namespace:Walkman_Playlist_Tools");
            XNamespace ns = "http://schemas.microsoft.com/winfx/2006/xaml/presentation";
            XNamespace local = "clr-namespace:Walkman_Playlist_Tools";
            var xe = new XElement(ns + "DataTemplate",
                new XAttribute("xmlns", "http://schemas.microsoft.com/winfx/2006/xaml/presentation"),
                new XElement(ns + "CheckBox", new XAttribute("IsChecked", @"{Binding Check}"))
            );
            var xr = xe.CreateReader();
            var dt = XamlReader.Load(xr) as DataTemplate;*/
            FrameworkElementFactory dataFactory = new FrameworkElementFactory(typeof(DragCheckBox));
            Binding bind = new Binding("Check");
            dataFactory.SetBinding(DragCheckBox.IsCheckedProperty, bind);
            var dataTemplate = new DataTemplate();
            dataTemplate.VisualTree = dataFactory;
            checkColumn.CellTemplate = dataTemplate;
            gv.Columns.Add(checkColumn);
            var titleColumn = new GridViewColumn();
            titleColumn.Header = "标题";
            titleColumn.Width = 200;
            titleColumn.DisplayMemberBinding = new Binding("Title");
            gv.Columns.Add(titleColumn);
            var pathColumn = new GridViewColumn();
            pathColumn.Header = "文件路径";
            pathColumn.Width = 400;
            pathColumn.DisplayMemberBinding = new Binding("Path");
            gv.Columns.Add(pathColumn);
            ListViewInside.View = gv;
            //titleColumn.CellTemplate=new DataTemplate(new DragCheckBox().GetType());
        }

        public BuildPlaylist Clone()
        {
            return new BuildPlaylist(listViewInside,name,path,infos);
        }
    }
}
