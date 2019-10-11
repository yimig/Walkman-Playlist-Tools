using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Net;
using System.Reflection;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Timers;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using Newtonsoft.Json;
using Path = System.IO.Path;
using Timer = System.Timers.Timer;

namespace Walkman_Playlist_Tools
{
    /// <summary>
    /// GetLyricWindow.xaml 的交互逻辑
    /// </summary>
    public partial class GetLyricWindow : Window,INotifyPropertyChanged
    {
        private ObservableCollection<MusicInfo> infos;
        private BackgroundWorker backgroundWorker;
        public event PropertyChangedEventHandler PropertyChanged;
        private bool cancelProcess;
        private bool isResultMouseDown=false;
        private bool isLocalMouseDown=false;
        private bool isFindArtist;
        private int processNum = 100;
        private int positionNum;

        public int PositionNum
        {
            get { return positionNum; }
            set
            {
                positionNum = value;
                if (PropertyChanged != null)
                {
                    PropertyChanged(this, new PropertyChangedEventArgs("PositionNum"));
                }
            }
        }

        

        public GetLyricWindow(ListView lv)
        {
            InitializeComponent();
            SwitchPlatformCBX.SelectedIndex = Setting.Default.GetLryicPlatform;
            infos = lv.ItemsSource as ObservableCollection<MusicInfo>;
            LocalInfoList.ItemsSource = infos;
            SetBinding();
            InitBGWorker();
            backgroundWorker.RunWorkerAsync();
        }

        private bool CheckMultiArtist()
        {
            return MultiArtistBox.SelectedIndex != 0;
        }

        private void SetBinding()
        {
            Binding PositonBind = new Binding();
            PositonBind.Source = this;
            PositonBind.Path = new PropertyPath("PositionNum");
            LocalInfoList.SetBinding(ListView.SelectedIndexProperty, PositonBind);
            Binding PositionTextBind=new Binding();
            PositionTextBind.Source = LocalInfoList;
            PositionTextBind.Path= new PropertyPath("SelectedIndex");
            PositionTextBind.Converter=new OrderConverter();
            PositionText.SetBinding(TextBlock.TextProperty, PositionTextBind);
        }

        private string GetButtonString(Button button)
        {
            return button.Content as string;
        }

        private void SetButtonString(Button button, string text)
        {
            button.Content = text;
        }

        private void InitBGWorker()
        {
            backgroundWorker = new BackgroundWorker();
            backgroundWorker.WorkerReportsProgress = true;
            backgroundWorker.WorkerSupportsCancellation = true;
            backgroundWorker.DoWork += BackgroundWorker_DoWork;
            backgroundWorker.ProgressChanged += BackgroundWorker_ProgressChanged;
            backgroundWorker.RunWorkerCompleted += BackgroundWorker_RunWorkerCompleted;
        }

        private void ChangeInfo()
        {
            try
            {
                BuildOnlineList(GetSearchResult(infos[positionNum].Title));
            }
            catch (System.NullReferenceException)
            {
                BuildOnlineList(new List<OnlineMusicInfo>());
            }
            catch (WebException we)
            {
                MessageBox.Show("获取在线列表时出现问题：" + we.Message);
                BuildOnlineList(new List<OnlineMusicInfo>());
            }
        }

        private void BuildOnlineList(List<OnlineMusicInfo> infos)
        {
            ObservableCollection<OnlineMusicInfo> collection=new ObservableCollection<OnlineMusicInfo>();
            foreach (OnlineMusicInfo info in infos) collection.Add(info);
            Dispatcher.Invoke(() =>
            {
                SearchResultList.ItemsSource = infos;
                if (SearchResultList.Items.Count != 0)
                {
                    if(SearchResultList.SelectedIndex==-1)SearchResultList.SelectedIndex = 0;
                    GetLyric(infos[0].Id);
                    UpdateLyricBox();
                }
            });
        }

        private List<OnlineMusicInfo> GetSearchResult(string searchText)
        {
            List<OnlineMusicInfo> onlineResult;
            if (Setting.Default.GetLryicPlatform == 1) onlineResult=GetNetEaseSearchData(searchText);
            else if (Setting.Default.GetLryicPlatform == 2) onlineResult=GetQQMusicSearchData(searchText);
            else
            {
                onlineResult=new List<OnlineMusicInfo>();
            }

            return onlineResult;
        }

        private List<OnlineMusicInfo> GetNetEaseSearchData(string searchText)
        {
            ServicePointManager.DefaultConnectionLimit = 100;
            WebClient webClient = new WebClient();
            webClient.Encoding = Encoding.UTF8;
            string json = webClient.DownloadString(
                @"http://music.163.com/api/search/get/web?csrf_token=hlpretag=&hlposttag=&s=" + searchText +
                "&type=1&offset=0&total=true&limit=30");
            webClient.Dispose();
            JsonSerializerSettings settings = new JsonSerializerSettings();
            settings.NullValueHandling = NullValueHandling.Ignore;
           NeteaseMusicSearchData resultData= JsonConvert.DeserializeObject<NeteaseMusicSearchData>(json, settings);
            List<OnlineMusicInfo> neteaseResult = new List<OnlineMusicInfo>();
            if (resultData.result.songs != null) foreach (Song song in resultData.result.songs)
            {
                neteaseResult.Add(new OnlineMusicInfo(song));
            }
            return neteaseResult;
        }

        private List<OnlineMusicInfo> GetQQMusicSearchData(string searchText)
        {
            var filtratedDatas = GetQQMusicInfo.GetFiltratedSearchResult(searchText);
            var resultData = new List<OnlineMusicInfo>();
            foreach (var filtratedData in filtratedDatas)
            {
                resultData.Add(new OnlineMusicInfo(filtratedData));
            }

            return resultData;
        }

        private void PauseProcess()
        {

            if (backgroundWorker != null)
            {
                cancelProcess = true;
                SavePanel.Visibility = Visibility.Visible;
                backgroundWorker.CancelAsync();
                if (GetButtonString(PauseButton) == "暂停") SetButtonString(PauseButton, "继续");
            }
        }

        private void SaveLyric()
        {
            var info = SearchResultList.SelectedItem as OnlineMusicInfo;
            if (info.Lyric!=null&&info.Lyric!="")
            {
                StreamWriter writer =
                    new StreamWriter(System.IO.Path.ChangeExtension((LocalInfoList.SelectedItem as MusicInfo).Path,"lrc"), false, Encoding.UTF8);
                writer.Write(info.Lyric);
                writer.Close();
            }
        }

        private void UpdateLyricBox()
        {
            var info = SearchResultList.SelectedItem as OnlineMusicInfo;
            LyricBox.Text = Format.LyricDelTime(info.Lyric);
            //LyricBox.UpdateLayout();
        }

        private bool CheckArtist(MusicInfo localInfo, List<OnlineMusicInfo> onlineInfos)
        {
            isFindArtist=false;
            if (onlineInfos == null|| localInfo.Artist == null) return false;
            foreach (OnlineMusicInfo onlineInfo in onlineInfos)
            {
                if (Format.CheckArtistEqual(onlineInfo.Artist, localInfo.Artist,CheckMultiArtist()))
                {
                    SearchResultList.SelectedItem = onlineInfo;
                    SearchResultList.ScrollIntoView(onlineInfo);
                    isFindArtist = true;
                    break;
                }
            }
            return isFindArtist;
        }

        private void GetLyric(string id)
        {
            try
            {
                if(Setting.Default.GetLryicPlatform==1) Format.TransLyric(GetNetEaseInfo.GetLyric(id));
                else if(Setting.Default.GetLryicPlatform==2)(SearchResultList.SelectedItem as OnlineMusicInfo).Lyric =GetQQMusicInfo.GetLyric(id);
                //
            }
            catch (IndexOutOfRangeException)
            {
                MessageBox.Show("解析该歌词失败，请手动选择其他歌词");
                PauseProcess();
            }
        }

        //private string AutoGetLyric(string id)
        //{
        //    if (!isFindArtist)
        //    {

        //    }
        //}

        private void ChangeColor(bool isPass)
        {
            var info = LocalInfoList.SelectedItem as MusicInfo;
            if (isPass) info.Brush = new SolidColorBrush(Color.FromRgb(144, 238, 144));
            else info.Brush = new SolidColorBrush(Color.FromRgb(255, 204, 204));
        }

        private void BackgroundWorker_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            try
            {
                if (isFindArtist) SaveLyric();
                if (!cancelProcess)
                {
                    PositionNum++;
                    LocalInfoList.ScrollIntoView(LocalInfoList.SelectedItem);
                    backgroundWorker.RunWorkerAsync();
                    processNum = 100;
                }
            }
            catch (IOException ex)
            {
                MessageBox.Show("保存歌词时出现问题：" + ex.Message);
            }

            //else cancelProcess = false;
        }

        private void BackgroundWorker_ProgressChanged(object sender, ProgressChangedEventArgs e)
        {
            TimeBar.Value = e.ProgressPercentage;
        }

        private void BackgroundWorker_DoWork(object sender, DoWorkEventArgs e)
        {
            ChangeInfo();
            Dispatcher.Invoke(()=> ChangeColor(CheckArtist(LocalInfoList.SelectedItem as MusicInfo, SearchResultList.ItemsSource.Cast<OnlineMusicInfo>().ToList())));
            Random random=new Random();
            do
            {
                backgroundWorker.ReportProgress(processNum--);
                Thread.Sleep(random.Next(30, 100));
                if (backgroundWorker.CancellationPending)
                {
                    cancelProcess = true;
                    break;
                }
            } while (processNum>0);
            if (positionNum == LocalInfoList.Items.Count-1) cancelProcess = true;
        }

        private void PauseButton_OnClick(object sender, RoutedEventArgs e)
        {
            if (GetButtonString(PauseButton) == "暂停")
            {
                backgroundWorker.CancelAsync();
                SetButtonString(PauseButton, "继续");
                SavePanel.Visibility = Visibility.Visible;
            }
            else
            {
                cancelProcess = false;
                backgroundWorker.RunWorkerAsync();
                SetButtonString(PauseButton, "暂停");
                SavePanel.Visibility = Visibility.Collapsed;
            }
        }

        private void SearchResultList_OnSelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (SearchResultList.SelectedItem != null)
            {
                if ( isResultMouseDown)
                {
                    PauseProcess();
                    isResultMouseDown = false;
                    GetLyric((SearchResultList.SelectedItem as OnlineMusicInfo).Id);
                    UpdateLyricBox();
                }
            }

            //isResultFirstMove = false;
        }

        private void LocalInfoList_OnSelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (LocalInfoList.SelectedItem != null&&isLocalMouseDown)
            {
                isLocalMouseDown = false;
                //cancelProcess = false;
                PauseProcess();
                processNum = 100;
                TimeBar.Value = 100;
                ChangeInfo();
                //SearchResultList.UpdateLayout();
            }
        }

        private void SearchResultList_OnPreviewMouseDown(object sender, MouseButtonEventArgs e)
        {
            isResultMouseDown = true;
        }

        private void LocalInfoList_OnPreviewMouseDown(object sender, MouseButtonEventArgs e)
        {
            isLocalMouseDown = true;
        }

        private void SaveButton_OnClick(object sender, RoutedEventArgs e)
        {
            try
            {
                SaveLyric();
                MessageBox.Show("已保存");
            }
            catch (IOException ex)
            {
                MessageBox.Show("保存歌词时出现问题：" + ex.Message);
            }
        }

        private void GetLyricWindow_OnClosed(object sender, EventArgs e)
        {
            PauseProcess();
        }

        private void Selector_OnSelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            Setting.Default.GetLryicPlatform = SwitchPlatformCBX.SelectedIndex;
            Setting.Default.Save();
        }
    }

    class OnlineMusicInfo:INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;
        private string title, artist, album, id,lyric;

        public OnlineMusicInfo(Song NeteaseSong)
        {
            title = NeteaseSong.name;
            album = NeteaseSong.album.name;
            id = NeteaseSong.id;
            foreach (Artist artist in NeteaseSong.artists)
            {
                if (this.artist == null) this.artist = artist.name;
                else this.artist += ";" + artist.name;
            }
        }

        public OnlineMusicInfo(FiltratedData QQMusicSong)
        {
            title = QQMusicSong.SongTitle;
            album = QQMusicSong.Album;
            id = QQMusicSong.SongMid;
            artist = QQMusicSong.Aritist;
        }

        public string Title
        {
            get { return title; }
            set { title = value; }
        }

        public string Id
        {
            get { return id; }
            set { id = value; }
        }

        public string Artist
        {
            get { return artist; }
            set { artist = value; }
        }

        public string Album
        {
            get { return album; }
            set { album = value; }
        }

        public string Lyric
        {
            get { return lyric; }
            set
            {
                lyric = value;
                if (PropertyChanged != null)
                {
                    PropertyChanged(this, new PropertyChangedEventArgs("Lyric"));
                }
            }
        }
    }

    [ValueConversion(typeof(int), typeof(string))]
    public class OrderConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return System.Convert.ToInt32(value) + 1;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return null;
        }
    }
}
