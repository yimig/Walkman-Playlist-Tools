using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace Walkman_Playlist_Tools
{
    /// <summary>
    /// 带列表的菜单项目
    /// </summary>
    public class MenuItemWithListView:MenuItem
    {
        public MenuItemWithListView(ListView lv)
        {
            ListViewInseide = lv;
        }
        public ListView ListViewInseide { get; set; }

       
    }
}
