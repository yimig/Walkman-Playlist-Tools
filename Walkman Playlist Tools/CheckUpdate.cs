using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.NetworkInformation;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Forms;
using MessageBox = System.Windows.MessageBox;

namespace Walkman_Playlist_Tools
{
    public class CheckUpdate
    {
        private bool isUpdate;
        public string newVersion;
        public string description;

        public bool IsUpdate => isUpdate;

        public string NewVersion => newVersion;

        public string Description => description;

        private int[] Formation(string version)
        {
            var finver=new int[4];
            var subver= version.Split(new char[] { '.' });
            for (var i = 0; i < finver.Length; i++)
            {
                finver[i] = Convert.ToInt32(subver[i]);
            }

            return finver;
        }

        public bool CheckForUpDate()
        {
            var webClient=new WebClient();
            var info=webClient.DownloadString("http://upane.cn/version.txt");
            newVersion = info.Split(new char[]{'\n'})[0];
            description = info.Split(new char[] {'{', '}'})[1];
            var oldver = Formation(System.Reflection.Assembly.GetExecutingAssembly().GetName().Version.ToString());
            var newver = Formation(newVersion);
            if (newver[3] > oldver[3]) isUpdate = true;
            else
            {
                if (newver[2] > oldver[2]) isUpdate = true;
                else
                {
                    if (newver[1] > oldver[1]) isUpdate = true;
                    else
                    {
                        if (newver[0] > oldver[0]) isUpdate = true;
                        else isUpdate = false;
                    }
                }
            }

            return isUpdate;
        }

        public void CheckUpdateWithWindow()
        {
            if (CheckForUpDate())
            {
                if (MessageBox.Show(newVersion + "\n" + description, "发现更新！要跳转到网页下载最新版本吗？", MessageBoxButton.YesNo) ==
                    MessageBoxResult.Yes)
                {
                    System.Diagnostics.Process.Start("http://upane.cn/archives/509");
                }
            }
            else MessageBox.Show("没更新哦~", "_(:з」∠)_");
        }
    }
}
