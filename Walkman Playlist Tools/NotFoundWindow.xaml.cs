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
    /// NotFoundWindow.xaml 的交互逻辑
    /// </summary>
    public partial class NotFoundWindow : Window
    {
        private List<string> list;
        public NotFoundWindow(List<string> errorList)
        {
            list = errorList;
            InitializeComponent();
            LoadList();
        }

        private void LoadList()
        {
            foreach (string item in list)
            {
                ResultBox.Text += item + '\n';
            }
        }


    }
}
