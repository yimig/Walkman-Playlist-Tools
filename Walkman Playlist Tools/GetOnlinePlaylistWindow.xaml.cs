using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Net;
using System.Windows;
using System.Windows.Input;

namespace Walkman_Playlist_Tools
{
    /// <summary>
    /// GetOnlinePlaylistWindow.xaml 的交互逻辑
    /// </summary>
    public partial class GetOnlinePlaylistWindow : Window
    {
        private string playlistTitle;
        private ObservableCollection<MusicInfo> sourseInfos;
        private ObservableCollection<MusicInfo> targetInfos;
        private List<string> errorInfos;

        public string PlaylistTitle
        {
            get { return playlistTitle; }
        }

        public ObservableCollection<MusicInfo> TargetInfos
        {
            get { return targetInfos; }
        }

        public List<string> ErrorInfos
        {
            get { return errorInfos; }
        }

        public GetOnlinePlaylistWindow(ObservableCollection<MusicInfo> sourseInfos)
        {
            this.sourseInfos = sourseInfos;
            targetInfos=new ObservableCollection<MusicInfo>();
            errorInfos=new List<string>();
            InitializeComponent();
            urlBox.Focus();
        }

        private void GetFromNetEase(string url)
        {
            try
            {
                AnalysisResult(GetNetEaseInfo.GetPlaylist(url, out playlistTitle));
            }
            catch (WebException e)
            {
                MessageBox.Show("获取在线列表时出现问题：" + e.Message);
            }
            catch (IndexOutOfRangeException)
            {
                MessageBox.Show("获取在线列表出现问题，请检查输入的地址和本地网络连接");
            }
            catch (Exception e)
            {
                MessageBox.Show(e.Message);
            }
        }

        private void AnalysisResult(List<string> getResult)
        {
            foreach (string musicTitle in getResult)
            {
                bool checktag = false;
                foreach (MusicInfo info in sourseInfos)
                {
                    if (musicTitle.Equals(info.Title))
                    {
                        targetInfos.Add(info.Clone());
                        checktag = true;
                    }
                }
                if(!checktag)errorInfos.Add(musicTitle);
            }
        }

        private void CheckSourse()
        {
            if(FromNetEase.IsChecked==true)GetFromNetEase(urlBox.Text);
        }

        private void StartGet_OnClick(object sender, RoutedEventArgs e)
        {
            CheckSourse();
            Close();
        }

        private void UrlBox_OnKeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                CheckSourse();
                Close();
            }
        }
    }
}
