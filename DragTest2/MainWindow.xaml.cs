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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace DragTest2
{
    /// <summary>
    /// MainWindow.xaml 的交互逻辑
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            for (int i = 1; i < 11; i++)
            {
                Data data = new Data();
                data.Name = data.GetHashCode().ToString();
                data.Num = i;
                listView1.Items.Add(data);
            }
        }

        private void listView1_OnPreviewMouseMove(object sender, MouseEventArgs e)
        {
            if (e.LeftButton == MouseButtonState.Pressed)
            {
                var pos = e.GetPosition(listView1);
                HitTestResult result = VisualTreeHelper.HitTest(listView1, pos);
                if (result == null)
                {
                    return;
                }
                var listBoxItem = Utils.FindVisualParent<ListViewItem>(result.VisualHit);
                if (listBoxItem == null || listBoxItem.Content != listView1.SelectedItem)
                {
                    return;
                }
                DataObject dataObj = new DataObject(listBoxItem.Content as Data);
                DragDrop.DoDragDrop(listView1, dataObj, DragDropEffects.Move);
            }
        }

        private void listView1_OnDrop(object sender, DragEventArgs e)
        {
            var pos = e.GetPosition(listView1);
            var result = VisualTreeHelper.HitTest(listView1, pos);
            if (result == null)
            {
                return;
            }
            //查找元数据
            var sourcePerson = e.Data.GetData(typeof(ListViewItem)) as ListViewItem;
            if (sourcePerson == null)
            {
                return;
            }
            //查找目标数据
            var listBoxItem = Utils.FindVisualParent<ListViewItem>(result.VisualHit);
            if (listBoxItem == null)
            {
                return;
            }
            var targetPerson = listBoxItem.Content as Data;
            if (ReferenceEquals(targetPerson, sourcePerson))
            {
                return;
            }
            listView1.Items.Remove(sourcePerson);
            listView1.Items.Insert(listView1.Items.IndexOf(targetPerson), sourcePerson);
        }
    }
    internal static class Utils
    {
        //根据子元素查找父元素
        public static T FindVisualParent<T>(DependencyObject obj) where T : class
        {
            while (obj != null)
            {
                if (obj is T)
                    return obj as T;

                obj = VisualTreeHelper.GetParent(obj);
            }
            return null;
        }
    }


    class Data
    {
        private string name;
        private int num;

        public string Name
        {
            get { return name; }
            set { name = value; }
        }

        public int Num
        {
            get { return num; }
            set { num = value; }
        }
    }
}
