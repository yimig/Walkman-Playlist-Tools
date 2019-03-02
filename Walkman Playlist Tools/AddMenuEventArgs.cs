using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace Walkman_Playlist_Tools
{
    public delegate void AddWorkPlaceMenuEventHandler(BuildTab tab, AddMenuEventArgs e);

    public delegate void AddPlaylistMenuEventHandler(BuildPlaylist buildPlaylist, AddMenuEventArgs e);

    public class AddMenuEventArgs:EventArgs
    {
        public bool isWorkPlace { get; set; }
    }
}
