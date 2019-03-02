using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

namespace Walkman_Playlist_Tools
{
    class ListViewDragCrotrol
    {
        private ListView mainLV;

        public ListView MainListView => mainLV;

        public ListViewDragCrotrol(ListView mainLV)
        {
            this.mainLV = mainLV;
        }

        private void ButtonBase_OnClick(object sender, RoutedEventArgs e)
        {
            throw new NotImplementedException();
        }
    }
}
