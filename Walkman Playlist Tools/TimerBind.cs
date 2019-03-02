using System.ComponentModel;

namespace Walkman_Playlist_Tools
{
    class TimerBind : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;
        private int nowtime;
        public int Nowtime

        {
            get { return nowtime; }
            set
            {
                nowtime = value;
                if (PropertyChanged != null)
                {
                    PropertyChanged(this, new PropertyChangedEventArgs("NowTime"));
                }
            }
        }
    }
}