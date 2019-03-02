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

namespace Walkman_Playlist_Tools
{
    /// <summary>
    /// AboutWindow.xaml 的交互逻辑
    /// </summary>
    public partial class AboutWindow : Window
    {
        public AboutWindow(int index)
        {
            InitializeComponent();
            AboutControl.SelectedIndex = index;
        }

        private void CheckUpdateButton_OnClick(object sender, RoutedEventArgs e)
        {
            new CheckUpdate().CheckUpdateWithWindow();
        }

        private void TeachButton_OnClick(object sender, RoutedEventArgs e)
        {
            System.Diagnostics.Process.Start("http://upane.cn/archives/1140");
        }

        private void ProjectURL_OnPreviewMouseDown(object sender, MouseButtonEventArgs e)
        {
            System.Diagnostics.Process.Start("http://upane.cn/archives/509");
        }

        private void GitURL_OnPreviewMouseDown(object sender, MouseButtonEventArgs e)
        {
            System.Diagnostics.Process.Start("https://github.com/yimig");
        }

        private void BlogURL_OnPreviewMouseDown(object sender, MouseButtonEventArgs e)
        {
            System.Diagnostics.Process.Start("http://upane.cn");
        }
    }
}
