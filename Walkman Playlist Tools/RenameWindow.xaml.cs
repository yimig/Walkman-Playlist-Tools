using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace Walkman_Playlist_Tools
{
    /// <summary>
    /// RenameWindow.xaml 的交互逻辑
    /// </summary>
    public partial class RenameWindow : Window
    {
        private bool _confirm=false;
        private string _newtitle;

        public bool Confirm
        {
            get { return _confirm; }
        }

        public string NewTitle
        {
            get { return _newtitle; }
        }

        public RenameWindow(string content)
        {
            InitializeComponent();
            TextBox.Text = content;
            TextBox.Focus();
            TextBox.SelectAll();
            TextBox.SelectionStart = 0;
            ShowDialog();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            Regex filenamereg=new Regex(@"^[^\\/:*?""<>|,]+$");
            if (filenamereg.IsMatch(TextBox.Text))
            {
                _newtitle = TextBox.Text;
                _confirm = true;
                Close();
            }
            else
            {
                MessageBox.Show(@"输入文本不符合文件名命名标准，请不要输入包含“\ / : * ? # ” < > |”的标题");
            }
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            Close();
        }

        private void TextBox_KeyDown(object sender, System.Windows.Input.KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                RoutedEventArgs ea=new RoutedEventArgs();
                Button_Click(sender,ea);
            }
        }
    }
}
