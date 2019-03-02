using System;
using System.Collections.Generic;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Interop;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using System.Windows.Threading;
using Un4seen.Bass;

namespace Walkman_Playlist_Tools
{
    /// <summary>
    /// PlayWindow.xaml 的交互逻辑
    /// </summary>
    public partial class PlayWindow : Window
    {
        private TimerBind timerBind;
        private DispatcherTimer timer = null;
        private MusicInfo info;
        private int stream;
        private bool isPlaying = true;
        public PlayWindow(MusicInfo info)
        {
            this.info = info;
            InitializeComponent();
            Initializtion();
        }


        /// <summary>
        /// 显示歌曲信息
        /// </summary>
        void GetInfo()
        {
            InfoList.Text += "标题：" + info.Title;
            InfoList.Text += "\n艺术家：" + info.Artist;
            InfoList.Text += "\n所在专辑：" + info.Album;
            InfoList.Text += "\n年代：" + info.Year;
            InfoList.Text += "\n文件格式：" + info.Format;
            InfoList.Text += "\n导入时间：" + info.Buildtime;
            InfoList.Text += "\n所在路径：" + info.Path;
        }

        /// <summary>
        /// 初始化窗口
        /// </summary>
        void Initializtion()
        {
            int min, sec;
            timerBind=new TimerBind();
            SetClock();
            GetInfo();
            TimeTrack.Minimum = 0;
            TimeTrack.Maximum = info.Length;
            ConvertMin(out min,out sec,info.Length);
            stream = Bass.BASS_StreamCreateFile(info.Path, 0L, 0L, BASSFlag.BASS_SAMPLE_FLOAT);
            LengthBox.Text = min + ":" + sec;
            Play();
            if(Setting.Default.IsTryQuarter)PositionButton_OnClick(PositionButton,new RoutedEventArgs());
            Binding bind = new Binding();
            bind.Source = timerBind;
            bind.Path = new PropertyPath("Nowtime");
            TimeTrack.SetBinding(Slider.ValueProperty, bind);
        }

        /// <summary>
        /// 播放音乐
        /// </summary>
        void Play()
        {
            Bass.BASS_ChannelPlay(stream, false);
            stopButton.Source = new BitmapImage(new Uri(@"icon\pause.ico", UriKind.Relative));
            isPlaying = true;
        }

        /// <summary>
        /// 暂停音乐
        /// </summary>
        void Pause()
        {
            Bass.BASS_ChannelPause(stream);
            stopButton.Source = new BitmapImage(new Uri(@"icon\play.ico", UriKind.Relative));
            isPlaying = false;
        }

        /// <summary>
        /// 设置到四分之一的位置播放
        /// </summary>
        void SetClock()
        {
            timer=new DispatcherTimer();
            timer.Interval=new TimeSpan(0,0,1);
            timer.Tick+=new EventHandler(Timer_Tick);
            timer.Start();
        }

        /// <summary>
        /// 将时间总量换算为分秒计时
        /// </summary>
        /// <param name="min">分</param>
        /// <param name="sec">秒</param>
        /// <param name="time">时间总量</param>
        void ConvertMin(out int min,out int sec,int time)
        {
            min = time / 60;
            sec = time % 60;
        }

        private void PlayWindow_OnClosed(object sender, EventArgs e)
        {
            Bass.BASS_ChannelStop(stream);
            Bass.BASS_StreamFree(stream);
        }

        private void Timer_Tick(object sender, EventArgs e)
        {
            int min, sec;
            timerBind.Nowtime = (int) Bass.BASS_ChannelBytes2Seconds(stream, Bass.BASS_ChannelGetPosition(stream));
            ConvertMin(out min, out sec, timerBind.Nowtime);
            NowBox.Text = min + ":" + sec;
        }

        private void StopButton_OnClick(object sender, RoutedEventArgs e)
        {
            if (isPlaying) Pause();
            else Play();
        }

        private void PositionButton_OnClick(object sender, RoutedEventArgs e)
        {
            Bass.BASS_ChannelSetPosition(stream, info.Length / 4.0);
        }

        private void BackButton_OnClick(object sender, RoutedEventArgs e)
        {
            int time = timerBind.Nowtime;
            if(time>5) Bass.BASS_ChannelSetPosition(stream, (time-5.0));
            else Bass.BASS_ChannelSetPosition(stream, 0.0);
        }

        private void SkipButton_OnClick(object sender, RoutedEventArgs e)
        {
            int time = timerBind.Nowtime;
            if (info.Length > time + 5) Bass.BASS_ChannelSetPosition(stream, (time + 5.0));
        }
    }
}
