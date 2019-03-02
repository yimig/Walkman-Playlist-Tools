using System;
using System.Collections.Generic;
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
using System.Text.RegularExpressions;

namespace Walkman_Playlist_Tools
{
    /// <summary>
    /// SettingWindow.xaml 的交互逻辑
    /// </summary>
    public partial class SettingWindow : Window
    {
        private MainWindow mainWindow;
        private TabItem sdtab;
        public SettingWindow(MainWindow mainWindow)
        {
            InitializeComponent();
            Initialization();
            this.mainWindow = mainWindow;
            sdtab = mainWindow.SDcardTab;
        }

        /// <summary>
        /// 初始化窗口（给各控件赋值）
        /// </summary>
        void Initialization()
        {
            IsTryQuarter.IsChecked = Setting.Default.IsTryQuarter;
            switch (Setting.Default.DelPlaylist)
            {
                case 0: NoDelFile.IsChecked = true; break;
                case 1: DelFile.IsChecked = true; break;
                case 2: AskMe.IsChecked = true; break;
            }
            for (int i = 0; i < 23; i++)
            {
                if (Setting.Default.WalkmanTag.Remove(1) == System.Text.Encoding.ASCII.GetString(new byte[] { (byte)(i + 68) }))
                {
                    WalkmanTag.SelectedIndex = i;
                    break;
                }
            }

            IsUseSD.IsChecked = Setting.Default.IsUseSD;
            if (Setting.Default.IsUseSD)
            {
                SDcardTag.IsEnabled = true;
                for (int i = 0; i < 23; i++)
                {
                    if (Setting.Default.SDcardTag.Remove(1) == System.Text.Encoding.ASCII.GetString(new byte[] { (byte)(i + 68) }))
                    {
                        SDcardTag.SelectedIndex = i;
                        break;
                    }
                }
            }
            else
            {
                SDcardTag.IsEnabled = false;
            }

            AchePath.IsReadOnly = true;
            AchePath.Text = Setting.Default.AchePath;
            switch (Setting.Default.ScanMode)
            {
                case 0: UseShellOnly.IsChecked = true;break;
                case 1: UseAll.IsChecked = true;break;
                case 2: UseBassOnly.IsChecked = true;break;
            }

            IsDriveName.IsChecked = Setting.Default.IsDriveName;
            IsDatePlaylist.IsChecked = Setting.Default.IsDatePlaylist;
            CopyFileDrive.SelectedIndex = Setting.Default.CopyFileDrive;
            switch (Setting.Default.BuildFolderMode)
            {
                case 0: NoFolder.IsChecked = true;break;
                case 1: ArtistFolder.IsChecked = true;break;
                case 2: AlbumFolder.IsChecked = true;break;
            }
            CopyFolderDrive.SelectedIndex = Setting.Default.CopyFolderDrive;

            LeastFolder.Text = Setting.Default.LeastFolder.ToString();
            LeastBuildTime.Text = Setting.Default.LeastBuildTime.ToString();
            LeastFormat.Text = Setting.Default.LeastFormat.ToString();
            LeastCountry.Text = Setting.Default.LeastCountry.ToString();
            LeastArtist.Text = Setting.Default.LeastArtist.ToString();
            LeastAge.Text = Setting.Default.LeastAge.ToString();
            ScanLyricBox.IsChecked = Setting.Default.isScanLyric;
        }

        /// <summary>
        /// 让用户选择一个文件夹作为程序缓存的存放地址
        /// </summary>
        /// <returns>选择的地址，若为null，则用户点取消</returns>
        string GetPath()
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
        /// 使文本框仅接受正整数
        /// </summary>
        /// <param name="tb">操作文本框</param>
        /// <returns>最终获取的值，若为0，则用户输入错误或未做改动</returns>
        int MatchText(TextBox tb)
        {
            int data = 0;
            Regex re = new Regex("^[1-9]\\d*$");
            if (re.IsMatch(tb.Text))
            {
                data = Convert.ToInt32(tb.Text);
            }
            return data;
        }

        private void IsTryQuarter_OnClick(object sender, RoutedEventArgs e)
        {
            Setting.Default.IsTryQuarter = (bool)IsTryQuarter.IsChecked;
            Setting.Default.Save();
        }

        private void NoDelFile_OnClick(object sender, RoutedEventArgs e)
        {
            Setting.Default.DelPlaylist = 0;
            Setting.Default.Save();
        }

        private void DelFile_OnClick(object sender, RoutedEventArgs e)
        {
            Setting.Default.DelPlaylist = 1;
            Setting.Default.Save();
        }

        private void AskMe_OnClick(object sender, RoutedEventArgs e)
        {
            Setting.Default.DelPlaylist = 2;
            Setting.Default.Save();
        }

        private void WalkmanTag_OnSelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            Setting.Default.WalkmanTag = (System.Text.Encoding.ASCII.GetString(new byte[] { (byte)(WalkmanTag.SelectedIndex + 68) })) + ":";
            Setting.Default.Save();
        }

        private void IsUseSD_OnClick(object sender, RoutedEventArgs e)
        {
            Setting.Default.IsUseSD = (bool)IsUseSD.IsChecked;
            Setting.Default.Save();
            if (IsUseSD.IsChecked == false)
            {
                SDcardTag.IsEnabled = false;
                sdtab.IsEnabled = false;
            }
            else
            {
                TabControl tab = (TabControl)sdtab.Content;
                SDcardTag.IsEnabled = true;
                IsUseSD.IsChecked = Setting.Default.IsUseSD;
                sdtab.IsEnabled = true;
                BuildTab buildsw;
                if (tab.Items.Count == 0)
                {
                    buildsw = new BuildTab((TabControl)sdtab.Content, "音乐库", false);
                    buildsw.AddMenu += AddMenu_WorkPlace_OnBuilt;
                    buildsw.BeginBuild();
                }
                
            }
        }

        public void AddMenu_WorkPlace_OnBuilt(BuildTab sender, AddMenuEventArgs e)
        {
            mainWindow.AddWorkPlaceMenu(false, sender.NewTabItem, sender.NewListView);
        }

        private void SDcardTag_OnSelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            Setting.Default.SDcardTag = (System.Text.Encoding.ASCII.GetString(new byte[] { (byte)(SDcardTag.SelectedIndex + 68) })) + ":";
            Setting.Default.Save();
        }

        private void ChangeAchePath_OnClick(object sender, RoutedEventArgs e)
        {
            string path = GetPath();
            if (path != null)
            {
                Setting.Default.AchePath = path;
                AchePath.Text = path;
                Setting.Default.Save();
            }
        }

        private void UseShellOnly_OnClick(object sender, RoutedEventArgs e)
        {
            Setting.Default.ScanMode = 0;
            Setting.Default.Save();
        }

        private void UseAll_OnClick(object sender, RoutedEventArgs e)
        {
            Setting.Default.ScanMode = 1;
            Setting.Default.Save();
        }


        private void UseBassOnly_OnClick(object sender, RoutedEventArgs e)
        {
            Setting.Default.ScanMode = 2;
            Setting.Default.Save();
        }

        private void IsDriveName_OnClick(object sender, RoutedEventArgs e)
        {
            Setting.Default.IsDriveName = (bool) IsDriveName.IsChecked;
            Setting.Default.Save();
        }

        private void IsDatePlaylist_OnClick(object sender, RoutedEventArgs e)
        {
            Setting.Default.IsDatePlaylist = (bool)IsDatePlaylist.IsChecked;
            Setting.Default.Save();
        }

        private void CopyFileDrive_OnSelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            Setting.Default.CopyFileDrive = CopyFileDrive.SelectedIndex;
            Setting.Default.Save();
        }


        private void NoFolder_OnClick(object sender, RoutedEventArgs e)
        {
            Setting.Default.BuildFolderMode = 0;
            Setting.Default.Save();
        }

        private void ArtistFolder_OnClick(object sender, RoutedEventArgs e)
        {
            Setting.Default.BuildFolderMode = 1;
            Setting.Default.Save();
        }

        private void AlbumFolder_OnClick(object sender, RoutedEventArgs e)
        {
            Setting.Default.BuildFolderMode = 2;
            Setting.Default.Save();
        }

        private void CopyFolderDrive_OnSelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            Setting.Default.CopyFolderDrive = CopyFolderDrive.SelectedIndex;
            Setting.Default.Save();
        }


        private void LeastFolder_OnTextChanged(object sender, TextChangedEventArgs e)
        {
            int data = MatchText((TextBox)sender);
            if (data != 0)
            {
                Setting.Default.LeastFolder = data;
                Setting.Default.Save();
            }
        }

        private void LeastBuildTime_OnTextChanged(object sender, TextChangedEventArgs e)
        {
            int data = MatchText((TextBox)sender);
            if (data != 0)
            {
                Setting.Default.LeastBuildTime = data;
                Setting.Default.Save();
            }
        }

        private void LeastFormat_OnTextChanged(object sender, TextChangedEventArgs e)
        {
            int data = MatchText((TextBox)sender);
            if (data != 0)
            {
                Setting.Default.LeastFormat = data;
                Setting.Default.Save();
            }
        }

        private void LeastCountry_OnTextChanged(object sender, TextChangedEventArgs e)
        {
            int data = MatchText((TextBox)sender);
            if (data != 0)
            {
                Setting.Default.LeastCountry = data;
                Setting.Default.Save();
            }
        }

        private void LeastArtist_OnTextChanged(object sender, TextChangedEventArgs e)
        {
            int data = MatchText((TextBox)sender);
            if (data != 0)
            {
                Setting.Default.LeastArtist = data;
                Setting.Default.Save();
            }
        }

        private void LeastAge_OnTextChanged(object sender, TextChangedEventArgs e)
        {
            int data = MatchText((TextBox)sender);
            if (data != 0)
            {
                Setting.Default.LeastAge = data;
                Setting.Default.Save();
            }
        }

        private void ScanLyricBox_OnClick(object sender, RoutedEventArgs e)
        {
            if (ScanLyricBox.IsChecked != null) Setting.Default.isScanLyric = (bool) ScanLyricBox.IsChecked;
            Setting.Default.Save();
        }
    }
}
