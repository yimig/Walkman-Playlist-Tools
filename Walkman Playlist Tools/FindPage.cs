using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace Walkman_Playlist_Tools
{
    static class FindPage
    {
        /// <summary>
        /// 得到目前选中的标签页
        /// </summary>
        /// <param name="tabControl">目前选中区域</param>
        /// <returns>选中的标签页</returns>
        static TabItem Get_Tab(TabControl tabControl)
        {
            TabItem item = (TabItem)tabControl.SelectedItem;
            TabControl tab = (TabControl)item.Content;
            return (TabItem)tab.SelectedItem;
        }

        /// <summary>
        /// 得到目前选中的标签页中的列表
        /// </summary>
        /// <param name="tabControl">目前选中的区域</param>
        /// <returns>选中的列表</returns>
        static ListView Get_ListView(TabControl tabControl)
        {
            TabItem item = (TabItem)tabControl.SelectedItem;
            TabControl tab = (TabControl)item.Content;
            TabItem tbi = (TabItem)tab.SelectedItem;
            return (ListView)tbi.Content;
        }

        /// <summary>
        /// 得到目前选中的播放列表
        /// </summary>
        /// <param name="tabControl">播放列表“BuildPlaylist”</param>
        /// <returns></returns>
        static ListView Get_Playlist(TabControl tabControl)
        {
            ListView lv=null;
            TabItem item = (TabItem)tabControl.SelectedItem;
            Grid grid = (Grid) item.Content;
            foreach (var control in grid.Children)
            {
                if (control is ListView)
                {
                    lv = (ListView) control;
                }
            }

            return lv;
        }
    }
}
