using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using LanguageDetect;

namespace Walkman_List_Tools
{
    public enum BuildListType
    {
        Artist,Path,FileType,Year,Country,Generation
    }

    /// <summary>
    /// 控制全类型列表
    /// </summary>
    class FullListControl
    {
        /// <summary>
        /// 建立列表
        /// </summary>
        /// <param name="tab">包含列表的标签页</param>
        /// <param name="name">列表的名字</param>
        virtual public void BuildList(TabPage tab,string name)
        {
            ListView lv = new ListView();
            lv.Columns.Add("标题", 150, HorizontalAlignment.Center);
            lv.Columns.Add("长度（秒）", 65, HorizontalAlignment.Center);
            lv.Columns.Add("格式", 65, HorizontalAlignment.Center);
            lv.Columns.Add("艺术家", 65, HorizontalAlignment.Center);
            lv.Columns.Add("专辑", 150, HorizontalAlignment.Center);
            lv.Columns.Add("年代", 65, HorizontalAlignment.Center);
            lv.Name = name;
            lv.Columns.Add("路径", 260, HorizontalAlignment.Center);
            lv.View = System.Windows.Forms.View.Details;
            lv.FullRowSelect = true;
            lv.CheckBoxes = true;
            lv.MultiSelect = true;
            lv.GridLines = true;
            lv.AllowColumnReorder = true;
            lv.Dock = DockStyle.Fill;
            tab.Controls.Add(lv);
        }

        /// <summary>
        /// 清空目标播放列表
        /// </summary>
        /// <param name="lv">目标播放列表</param>
        public void ClearList(ListView lv)
        {
            lv.Items.Clear();
        }

        /// <summary>
        /// 将全类型列表选中的项目转换到准播放列表列表
        /// </summary>
        /// <param name="lv1">全类型列表</param>
        /// <param name="lv2">准播放列表列表</param>
        public void TransListItem(ListView lv1,ListView lv2)
        {
            lv2.BeginUpdate();   //数据更新，UI暂时挂起，直到EndUpdate绘制控件，可以有效避免闪烁并大大提高加载速度
            for (int i = 0; i < lv1.CheckedItems.Count; i++)
            {
                ListViewItem lvi = new ListViewItem();
                lvi.Text = lv1.Items[lv1.CheckedItems[i].Index].SubItems[0].Text;//标题
                lvi.SubItems.Add(lv1.Items[lv1.CheckedItems[i].Index].SubItems[1].Text);//长度
                lvi.SubItems.Add(lv1.Items[lv1.CheckedItems[i].Index].SubItems[6].Text.Remove(0, 9));//路径
                lv2.Items.Add(lvi);
            }
            lv2.EndUpdate();
        }

        /// <summary>
        /// 将全类型列表转换为准播放列表列表
        /// </summary>
        /// <param name="lv1">全类型列表</param>
        /// <param name="lv2">准播放列表列表</param>
        public void TransList(ListView lv1, ListView lv2)
        {
            lv2.BeginUpdate();   //数据更新，UI暂时挂起，直到EndUpdate绘制控件，可以有效避免闪烁并大大提高加载速度
            for (int i = 0; i < lv1.Items.Count; i++)
            {
                ListViewItem lvi = new ListViewItem();
                lvi.Text = lv1.Items[lv1.CheckedItems[i].Index].SubItems[0].Text;//标题
                lvi.SubItems.Add(lv1.Items[lv1.CheckedItems[i].Index].SubItems[1].Text);//长度
                lvi.SubItems.Add(lv1.Items[lv1.CheckedItems[i].Index].SubItems[6].Text.Remove(0, 9));//路径
                lv2.Items.Add(lvi);
            }
            lv2.EndUpdate();
        }
    }

    /// <summary>
    /// 建立准播放列表列表
    /// </summary>
    class LiteListControl:FullListControl
    {
        /// <summary>
        /// 建立列表
        /// </summary>
        /// <param name="tab">包含列表的标签页</param>
        /// <param name="name">列表的名字</param>
        override public void BuildList(TabPage tab, string name)
        {
            Data.newlist++;
            ListView lv = new ListView();
            lv.Name = name;
            lv.Columns.Add("标题", 150, HorizontalAlignment.Center);
            lv.Columns.Add("长度（秒）", 65, HorizontalAlignment.Center);
            lv.Columns.Add("路径", 260, HorizontalAlignment.Center);
            lv.View = System.Windows.Forms.View.Details;
            lv.FullRowSelect = true;
            lv.CheckBoxes = true;
            lv.MultiSelect = true;
            lv.GridLines = true;
            lv.AllowColumnReorder = true;
            lv.Dock = DockStyle.Fill;
            tab.Controls.Add(lv);
        }
    }

    class NewWindowList
    {
        /// <summary>
        /// 新建自动分类窗口
        /// </summary>
        /// <param name="type">分类标准</param>
        /// <param name="select">窗口对象</param>
        /// <param name="lv">被分类列表</param>
        /// <param name="protect">最小分类数量</param>
        public void NewList(BuildListType type,select select, ListView lv,int protect)
        {
            int num = 0;
            switch(type)
            {
                case BuildListType.Artist:num = 3;break;
                case BuildListType.FileType:num = 2;break;
                case BuildListType.Path:num = 6;break;
                case BuildListType.Country:
                    {
                        select.Show();
                        Dictionary<int, ListViewItem> selectdic2 = new Dictionary<int, ListViewItem>();
                        Dictionary<int, string> langdic = new Dictionary<int, string>();
                        int j = 0;
                        foreach (ListViewItem lvi in lv.Items)
                        {
                            Detector detector = DetectorFactory.create();
                            try
                            {
                                detector.append(lvi.SubItems[0].Text);
                                langdic.Add(j, detector.detect());
                            }
                            catch (LanguageDetect.LangDetectException) { }
                            selectdic2.Add(j, lvi);
                            j++;
                        }
                        for (int x = 0; x < lv.Items.Count; x++)
                        {
                            int tempnum = 0;
                            string tempname = null;
                            Dictionary<int, ListViewItem> itemdic = new Dictionary<int, ListViewItem>();
                            if (langdic.ContainsKey(x))
                            {
                                int selectindex = 0;
                                for (int y = x; y < lv.Items.Count; y++)
                                {
                                    if (langdic.ContainsKey(y))
                                    {
                                        if (langdic[x] == langdic[y])
                                        {
                                            tempnum++;
                                            tempname = langdic[x];
                                            itemdic.Add(selectindex, selectdic2[y]);
                                            if (x != y) langdic.Remove(y);
                                            selectindex++;
                                        }

                                    }
                                }
                            }
                            if (tempnum > 0)
                            {
                                switch (tempname)
                                {
                                    case "th": tempname = "泰语"; break;
                                    case "fi": tempname = "芬兰语"; break;
                                    case "fr": tempname = "法语"; break;
                                    case "it": tempname = "意大利语"; break;
                                    case "ru": tempname = "俄语"; break;
                                    case "es": tempname = "西班牙语"; break;
                                    case "ja": tempname = "日语"; break;
                                    case "en": tempname = "英语"; break;
                                    case "ko": tempname = "韩语"; break;
                                    case "zh-cn": tempname = "中文（简）"; break;
                                    case "zh-tw": tempname = "中文（繁）"; break;
                                }
                                TabPage tpage = new TabPage();
                                tpage.Text = tempname;
                                tpage.Name = "New List " + Data.selectlist;
                                ListView lv2 = new ListView();
                                lv2.Name = "List" + Data.selectlist;
                                lv2.Columns.Add("标题", 150, HorizontalAlignment.Center);
                                lv2.Columns.Add("长度（秒）", 65, HorizontalAlignment.Center);
                                lv2.Columns.Add("路径", 260, HorizontalAlignment.Center);
                                lv2.View = System.Windows.Forms.View.Details;
                                lv2.FullRowSelect = true;
                                lv2.MultiSelect = true;
                                lv2.GridLines = true;
                                lv2.AllowColumnReorder = true;
                                lv2.Dock = DockStyle.Fill;
                                lv2.Location = new System.Drawing.Point(0, 0);
                                tpage.Controls.Add(lv2);
                                select.SelectControl.TabPages.Add(tpage);
                                lv2.BeginUpdate();   //数据更新，UI暂时挂起，直到EndUpdate绘制控件，可以有效避免闪烁并大大提高加载速度
                                for (int k = 0; k < tempnum; k++)
                                {
                                    ListViewItem lvi = new ListViewItem();
                                    lvi.Text = itemdic[k].SubItems[0].Text;//标题
                                    lvi.SubItems.Add(itemdic[k].SubItems[1].Text);//长度
                                    lvi.SubItems.Add(itemdic[k].SubItems[6].Text.Remove(0, 9));//路径
                                    lv2.Items.Add(lvi);
                                }
                                lv2.EndUpdate();
                            }
                        }
                        return;
                        break;
                    }    
            }
            select.Show();
            Dictionary<int, ListViewItem> selectdic = new Dictionary<int, ListViewItem>();
            int i = 0;
            foreach (ListViewItem lvi in lv.Items)
            {
                selectdic.Add(i, lvi);
                i++;
            }
            for (int x = 0; x < lv.Items.Count; x++)
            {
                int tempnum = 0;
                string tempname = null;
                Dictionary<int, ListViewItem> itemdic = new Dictionary<int, ListViewItem>();
                if (selectdic.ContainsKey(x))
                {
                    int selectindex = 0;
                    for (int y = x; y < lv.Items.Count; y++)
                    {
                        if (selectdic.ContainsKey(y))
                        {
                            if (num == 6)
                            {
                                string[] index = selectdic[x].SubItems[num].Text.Split(new char[] { '\\' });
                                string[] index2 = selectdic[y].SubItems[num].Text.Split(new char[] { '\\' });
                                if (index[index.Length - 2] == index2[index2.Length - 2])
                                {
                                    tempnum++;
                                    tempname = index2[index2.Length - 2];
                                    itemdic.Add(selectindex, selectdic[y]);
                                    if (x != y) selectdic.Remove(y);
                                    selectindex++;
                                }
                            }
                            else
                            {
                                string[] index = selectdic[x].SubItems[num].Text.Split(new char[] { '\\' });
                                string[] index2 = selectdic[y].SubItems[num].Text.Split(new char[] { '\\' });
                                if (selectdic[x].SubItems[num].Text == selectdic[y].SubItems[num].Text)
                                {
                                    tempnum++;
                                    tempname = selectdic[y].SubItems[num].Text;
                                    itemdic.Add(selectindex, selectdic[y]);
                                    if (x != y) selectdic.Remove(y);
                                    selectindex++;
                                }
                            }
                        }
                    }
                }
                if (tempnum > protect)
                {
                    if (tempname == "") tempname = "Unknown";
                    TabPage tpage = new TabPage();
                    tpage.Text = tempname;
                    tpage.Name = "New List " + Data.selectlist;
                    ListView lv1 = new ListView();
                    lv1.Name = "List" + Data.selectlist;
                    lv1.Columns.Add("标题", 150, HorizontalAlignment.Center);
                    lv1.Columns.Add("长度（秒）", 65, HorizontalAlignment.Center);
                    lv1.Columns.Add("路径", 260, HorizontalAlignment.Center);
                    lv1.View = System.Windows.Forms.View.Details;
                    lv1.FullRowSelect = true;
                    lv1.MultiSelect = true;
                    lv1.GridLines = true;
                    lv1.AllowColumnReorder = true;
                    lv1.Dock = DockStyle.Fill;
                    lv1.Location = new System.Drawing.Point(0, 0);
                    tpage.Controls.Add(lv1);
                    select.SelectControl.TabPages.Add(tpage);
                    lv1.BeginUpdate();
                    for (int j = 0; j < tempnum; j++)
                    {
                        ListViewItem lvi = new ListViewItem();
                        lvi.Text = itemdic[j].SubItems[0].Text;//标题
                        lvi.SubItems.Add(itemdic[j].SubItems[1].Text);//长度
                        lvi.SubItems.Add(itemdic[j].SubItems[6].Text.Remove(0, 9));//路径
                        lv1.Items.Add(lvi);
                    }
                    lv1.EndUpdate();
                }
            }
        }

        public void NewLanauage()
        {

        }
    }
}
