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

namespace ListViewDragTest
{
    /// <summary>
    /// MainWindow.xaml 的交互逻辑
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private bool inMouseSelectionMode = false;

        private List<ListBoxItem> selectedItems = new List<ListBoxItem>();

        private void lbItem_PreviewMouseLeftButtonDown(object sender, MouseButtonEventArgs e)

        {

            // MouseDown时清空已选Item

            // 同时开始"inMouseSelectionMode"

            foreach (var item in selectedItems)

            {

                item.ClearValue(ListBoxItem.BackgroundProperty);

                item.ClearValue(TextElement.ForegroundProperty);

            }

            selectedItems.Clear();

            inMouseSelectionMode = true;

        }

        private void lbItem_PreviewMouseUp(object sender, MouseButtonEventArgs e)

        {

            // MouseUp时停止"inMouseSelectionMode"

            ListBoxItem mouseUpItem = sender as ListBoxItem;

            inMouseSelectionMode = false;

        }

        private void lbItem_PreviewMouseMove(object sender, MouseEventArgs e)

        {

            ListBoxItem mouseOverItem = sender as ListBoxItem;
            if (mouseOverItem != null && inMouseSelectionMode && e.LeftButton == MouseButtonState.Pressed)

            {

                // Mouse所在的Item设置高亮

                mouseOverItem.Background = SystemColors.HighlightBrush;

                mouseOverItem.SetValue(TextElement.ForegroundProperty, SystemColors.HighlightTextBrush);

                if (!selectedItems.Contains(mouseOverItem)) { selectedItems.Add(mouseOverItem); }

            }

        }
    }
}
