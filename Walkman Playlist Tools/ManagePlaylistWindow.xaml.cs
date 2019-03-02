using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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

namespace Walkman_Playlist_Tools
{
    /// <summary>
    /// ManagePlaylistWindow.xaml 的交互逻辑
    /// </summary>
    public partial class ManagePlaylistWindow : Window
    {
        private MainWindow mainWindow;

        public ManagePlaylistWindow(MainWindow mainWindow)
        {
            this.mainWindow = mainWindow;
            InitializeComponent();
            ManageListView.ItemsSource = new MainWindow.GetArea(mainWindow).Playlist.GetPlaylistList().ItemsSource;
        }

        class MusicInfoComparer : IEqualityComparer<MusicInfo>
        {
            public bool Equals(MusicInfo info1, MusicInfo info2)
            {
                if (info1.Path == info2.Path) return true;
                else return false;
            }

            public int GetHashCode(MusicInfo info)
            {
                return info.Path.GetHashCode();
            }
        }

        private List<BuildPlaylist> GetCheckedItem()
        {
            List<BuildPlaylist> list = new List<BuildPlaylist>();
            foreach (BuildPlaylist playlist in ManageListView.Items)
            {
                if(playlist.Checked)list.Add(playlist);
            }

            return list;
        }

        ///  <summary>
        /// 项目全选
        ///  </summary>
        /// <param name="lv">应用操作的列表</param>
        public void CheckedAll(ListView lv)
        {
            foreach (BuildPlaylist item in lv.Items)
            {
                item.Checked = true;
            }
        }

        /// <summary>
        /// 取消全选
        /// </summary>
        /// <param name="lv">应用操作的列表</param>
        public void UnCheckedAll(ListView lv)
        {
            foreach (BuildPlaylist item in lv.Items)
            {
                item.Checked = false;
            }
        }

        /// <summary>
        /// 选择反选
        /// </summary>
        /// <param name="lv">应用操作的列表</param>
        public void Inverse(ListView lv)
        {
            foreach (BuildPlaylist item in lv.Items)
            {
                if (item.Checked == true) item.Checked = false;
                else item.Checked = true;
            }
        }

        /// <summary>
        /// 勾选/取消勾选目前选中的项目
        /// </summary>
        /// <param name="lv">应用操作的列表</param>
        public void ToChecked(ListView lv)
        {
            foreach (BuildPlaylist item in lv.SelectedItems)
            {
                if (item.Checked == true) item.Checked = false;
                else item.Checked = true;
            }
        }

        private BuildPlaylist GetUnionCollection(List<BuildPlaylist> originList)
        {
            if (originList.Count > 1)
            {
                originList.Add(mainWindow.BuildPlaylist(ManageListView,"并集",true));
                originList[originList.Count - 1].ListViewInside.ItemsSource = originList[0].ListViewInside.ItemsSource;
                for (int i = 1; i < originList.Count - 1; i++)
                {
                    originList[originList.Count-1].ListViewInside.ItemsSource=originList[originList.Count - 1].ListViewInside.ItemsSource.Cast<MusicInfo>()
                        .Union(originList[i].ListViewInside.ItemsSource.Cast<MusicInfo>(), new MusicInfoComparer());
                }
            }
            return originList.Count-1>0?originList[originList.Count - 1]:null;
        }

        private BuildPlaylist GetIntersectionCollection(List<BuildPlaylist> originList)
        {
            if (originList.Count > 1)
            {
                originList.Add(mainWindow.BuildPlaylist(ManageListView, "交集", true));
                originList[originList.Count - 1].ListViewInside.ItemsSource = originList[0].ListViewInside.ItemsSource;
                for (int i = 1; i < originList.Count - 1; i++)
                {
                    originList[originList.Count - 1].ListViewInside.ItemsSource = originList[originList.Count - 1].ListViewInside.ItemsSource.Cast<MusicInfo>()
                        .Intersect(originList[i].ListViewInside.ItemsSource.Cast<MusicInfo>(), new MusicInfoComparer());
                }
            }
            return originList.Count - 1 > 0 ? originList[originList.Count - 1] : null;
        }

        private void AddPlaylistButton_OnClick(object sender, RoutedEventArgs e)
        {
            mainWindow.BuildPlaylist(ManageListView, "新播放列表", true);
        }

        private void RenameButtonR_OnClick(object sender, RoutedEventArgs e)
        {
            MainWindow.GetArea area = new MainWindow.GetArea(mainWindow);
            BuildPlaylist buildPlaylist;
            buildPlaylist = ManageListView.SelectedItem as BuildPlaylist;
            if (buildPlaylist != null)
            {
                RenameWindow rename = new RenameWindow(buildPlaylist.Name);
                if (rename.Confirm) mainWindow.RenamePlaylist(area.IsWalkman, buildPlaylist, rename.NewTitle);
            }
        }

        private void CheckedButtonR_OnClick(object sender, RoutedEventArgs e)
        {
            CheckedAll(ManageListView);
        }

        private void ClearCheckedButtonR_OnClick(object sender, RoutedEventArgs e)
        {
            UnCheckedAll(ManageListView);
        }

        private void InverseButtonR_OnClick(object sender, RoutedEventArgs e)
        {
            Inverse(ManageListView);
        }

        private void ToCheckButtonR_OnClick(object sender, RoutedEventArgs e)
        {
            ToChecked(ManageListView);
        }

        private void DelFileButton_OnClick(object sender, RoutedEventArgs e)
        {
            foreach (BuildPlaylist playlist in GetCheckedItem())
            {
                mainWindow.DelFileAndPlaylist(ManageListView.ItemsSource as ObservableCollection<BuildPlaylist>,playlist);
            }
        }

        private void DelPlaylistButton_OnClick(object sender, RoutedEventArgs e)
        {
            foreach (BuildPlaylist playlist in GetCheckedItem())
            {
                mainWindow.DelPlaylistOnly(ManageListView.ItemsSource as ObservableCollection<BuildPlaylist>, playlist);
            }
        }

        private void GetUnionButton_OnClick(object sender, RoutedEventArgs e)
        {
            GetUnionCollection(GetCheckedItem());
        }

        private void GetIntersectionButton_OnClick(object sender, RoutedEventArgs e)
        {
            GetIntersectionCollection(GetCheckedItem());
        }

        private void ManagePlaylistWindow_OnClosed(object sender, EventArgs e)
        {
            foreach (BuildPlaylist playlist in ManageListView.Items)
            {
                playlist.Checked = false;
            }
        }
    }
}
