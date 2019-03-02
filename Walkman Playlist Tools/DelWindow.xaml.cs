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
    /// DelWindow.xaml 的交互逻辑
    /// </summary>
    public partial class DelWindow : Window
    {
        public DelWindow()
        {
            InitializeComponent();
        }

        public bool isConform = false;

        private void DelWindow_OnLoaded(object sender, RoutedEventArgs e)
        {
            if (Setting.Default.IsDelInProgram) DelInProgram.IsChecked = true;
            else DelInFile.IsChecked = true;
        }


        private void DelInProgram_OnClick(object sender, RoutedEventArgs e)
        {
            DelInProgram.IsChecked = true;
            Setting.Default.IsDelInProgram = true;
        }


        private void DelInFile_OnClick(object sender, RoutedEventArgs e)
        {
            DelInFile.IsChecked = true;
            Setting.Default.IsDelInProgram = false;
        }

        private void Conform_OnClick(object sender, RoutedEventArgs e)
        {
            Setting.Default.Save();
            isConform = true;
            Close();
        }


        private void Cancel_OnClick(object sender, RoutedEventArgs e)
        {
            Close();
        }
    }
}
