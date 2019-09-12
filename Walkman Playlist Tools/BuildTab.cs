using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Input;
using System.Xml.Linq;
using System.Xml;
using System.Windows.Markup;
using System.Windows.Media;

namespace Walkman_Playlist_Tools
{
    public class BuildTab
    { 
        private TabControl insideTabControl;
        private TabItem newTabItem;
        private ListView newListView;
        private string name;
        private ObservableCollection<MusicInfo> infos;

        public event AddWorkPlaceMenuEventHandler AddMenu;

        /// <summary>
        /// 建立一个带列表的标签页，且为注册事件保留方法
        /// </summary>
        /// <param name="tbc">包含该标签页的控件</param>
        /// <param name="name">标签页标题</param>
        /// <param name="isArea">包含标签页的控件是否为主页面两大区域</param>
        /// <param name="isAutoName">是否根据标签页数量自动命名</param>
        public BuildTab(TabControl tbc,string name,bool isAutoName)
        {
            infos = new ObservableCollection<MusicInfo>();
            insideTabControl = tbc;
            this.name = name;
            Add_Tab();
            if (isAutoName) newTabItem.Header = name + "_" + (insideTabControl.Items.Count-1);
            else newTabItem.Header = name;
        }

        /// <summary>
        /// 建立一个带列表的标签页，不为注册事件保留方法
        /// </summary>
        /// <param name="tbc">包含该标签页的控件</param>
        /// <param name="name">标签页标题</param>
        public BuildTab(TabControl tbc, string name)
        {
            infos=new ObservableCollection<MusicInfo>();
            insideTabControl = tbc;
            this.name = name;
            Add_Tab();
            newTabItem.Header = name;
            InitializeTab();
        }

        public void BeginBuild()
        {
            InitializeTab();
            if (AddMenu != null)
            {
                var e = new AddMenuEventArgs();
                e.isWorkPlace = true;
                AddMenu.Invoke(this, e);
            }
        }

        public TabControl InsideTabControl => insideTabControl;

        public TabItem NewTabItem => newTabItem;

        public ListView NewListView
        {
            get => newListView;
            set => newListView = value;
        }

        public string Name => name;

        public ObservableCollection<MusicInfo> Infos => infos;

        /// <summary>
        /// 建立标准标签页（内含一个列表）
        /// </summary>
        private void Add_Tab()
        {
            var lv = new ListView();
            var newtab = new TabItem();
            InsideTabControl.Items.Add(newtab);
            newtab.Content = lv;
            newListView = lv;
            newTabItem = newtab;
        }

        /// <summary>
        /// 初始化列表（添加列名、绑定）
        /// </summary>
        private void InitializeTab()
        {
            var gv = new GridView();
            var checkColumn = new GridViewColumn();
            checkColumn.Width = 30;
            /*XAttribute[] attribute = new XAttribute[3];
            attribute[0] = new XAttribute("IsChecked", @"{Binding Check}");
            attribute[1] = new XAttribute("ContentType",DragCheckBox.Content.MusicInfo);
            attribute[2]=new XAttribute("xmlns", "http://schemas.microsoft.com/winfx/2006/xaml/presentation");
            XNamespace ns = "http://schemas.microsoft.com/winfx/2006/xaml/presentation";
            XNamespace local = "clr-namespace:Walkman_Playlist_Tools";
            var xe = new XElement(ns + "DataTemplate",
                new XAttribute("xmlns", "http://schemas.microsoft.com/winfx/2006/xaml/presentation"),
                new XElement(ns + "CheckBox", new XAttribute("IsChecked", @"{Binding Check}"))
            );
            var xr = xe.CreateReader();
            var dt = XamlReader.Load(xr) as DataTemplate;*/
            FrameworkElementFactory dataFactory = new FrameworkElementFactory(typeof(DragCheckBox));
            Binding bind=new Binding("Check");
            dataFactory.SetBinding(DragCheckBox.IsCheckedProperty,bind);
            var dataTemplate=new DataTemplate();
            dataTemplate.VisualTree = dataFactory;
            checkColumn.CellTemplate = dataTemplate;

            gv.Columns.Add(checkColumn);
            var titleColumn = new GridViewColumn();
            titleColumn.Header = GetLabel("标题", titleColumn);
            titleColumn.Width = 200;
            titleColumn.DisplayMemberBinding = new Binding("Title");
            gv.Columns.Add(titleColumn);
            var formatColumn = new GridViewColumn();
            formatColumn.Header = GetLabel("格式", formatColumn);
            formatColumn.Width = 70;
            formatColumn.DisplayMemberBinding = new Binding("Format");
            gv.Columns.Add(formatColumn);
            var artistColumn = new GridViewColumn();
            artistColumn.Header = GetLabel("艺术家", artistColumn);
            artistColumn.Width = 70;
            artistColumn.DisplayMemberBinding = new Binding("Artist");
            gv.Columns.Add(artistColumn);
            var albumColumn = new GridViewColumn();
            albumColumn.Header = GetLabel("专辑名称",albumColumn);
            albumColumn.Width = 100;
            albumColumn.DisplayMemberBinding = new Binding("Album");
            gv.Columns.Add(albumColumn);
            var lengthColumn = new GridViewColumn();
            lengthColumn.Header = GetLabel("曲长",lengthColumn);
            lengthColumn.Width = 70;
            lengthColumn.DisplayMemberBinding = new Binding("Length");
            gv.Columns.Add(lengthColumn);
            var ageColumn = new GridViewColumn();
            ageColumn.Header = GetLabel("年代",ageColumn);
            ageColumn.Width = 70;
            ageColumn.DisplayMemberBinding = new Binding("Year");
            gv.Columns.Add(ageColumn);
            var buildtimeColumn = new GridViewColumn();
            buildtimeColumn.Header = GetLabel("同步时间",buildtimeColumn);
            buildtimeColumn.Width = 70;
            buildtimeColumn.DisplayMemberBinding = new Binding("Buildtime");
            gv.Columns.Add(buildtimeColumn);
            var pathColumn = new GridViewColumn();
            pathColumn.Header = GetLabel("文件路径",pathColumn);
            pathColumn.Width = 400;
            pathColumn.DisplayMemberBinding = new Binding("Path");
            gv.Columns.Add(pathColumn);
            NewListView.View = gv;
            NewListView.ItemsSource = infos;
        }

        private Label GetLabel(string name, GridViewColumn column)
        {
            Label label = new Label();
            label.Content = name;
            Binding binding = new Binding("Width");
            binding.Source = column;
            label.SetBinding(Label.WidthProperty, binding);
            label.HorizontalContentAlignment = HorizontalAlignment.Center;
            label.Height = 17;
            label.Padding = new Thickness(0);
            label.MouseLeftButtonDown += SortMusicInfo.Label_MouseLeftButtonDown;
            return label;
        }
    }
}
