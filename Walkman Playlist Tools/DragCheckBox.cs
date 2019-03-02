using System;
using System.Collections.Generic;
using System.ComponentModel;
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

namespace Walkman_Playlist_Tools
{
    /// <summary>
    /// 按照步骤 1a 或 1b 操作，然后执行步骤 2 以在 XAML 文件中使用此自定义控件。
    ///
    /// 步骤 1a) 在当前项目中存在的 XAML 文件中使用该自定义控件。
    /// 将此 XmlNamespace 特性添加到要使用该特性的标记文件的根 
    /// 元素中: 
    ///
    ///     xmlns:MyNamespace="clr-namespace:Walkman_Playlist_Tools"
    ///
    ///
    /// 步骤 1b) 在其他项目中存在的 XAML 文件中使用该自定义控件。
    /// 将此 XmlNamespace 特性添加到要使用该特性的标记文件的根 
    /// 元素中: 
    ///
    ///     xmlns:MyNamespace="clr-namespace:Walkman_Playlist_Tools;assembly=Walkman_Playlist_Tools"
    ///
    /// 您还需要添加一个从 XAML 文件所在的项目到此项目的项目引用，
    /// 并重新生成以避免编译错误: 
    ///
    ///     在解决方案资源管理器中右击目标项目，然后依次单击
    ///     “添加引用”->“项目”->[浏览查找并选择此项目]
    ///
    ///
    /// 步骤 2)
    /// 继续操作并在 XAML 文件中使用控件。
    ///
    ///     <MyNamespace:DragCheckBox/>
    ///
    /// </summary>
    public class DragCheckBox : CheckBox
    {
        static DragCheckBox()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(DragCheckBox), new FrameworkPropertyMetadata(typeof(DragCheckBox)));
        }

        public enum Content{ BuildPlaylist,MusicInfo}

        public static readonly DependencyProperty ContentTypeProperty=DependencyProperty.Register("ContentType",typeof(Content),typeof(DragCheckBox),new FrameworkPropertyMetadata(Content.MusicInfo,FrameworkPropertyMetadataOptions.AffectsRender));
        [Description("选择内容数据的类型")]
        public Content ContentType
        {
            get { return (Content) GetValue(ContentTypeProperty); }
            set { SetValue(ContentTypeProperty,value);}
        }

        
        /*public DragCheckBox()
        {
            this.PreviewMouseMove += UIElement_OnPreviewMouseMove;
            this.MouseEnter += UIElement_OnMouseEnter;
            this.PreviewMouseDown += UIElement_OnPreviewMouseDown;
        }*/

        protected override void OnPreviewMouseMove(MouseEventArgs e)
        {
            //base.OnPreviewMouseMove(e);
            ReleaseMouseCapture();
        }

        protected override void OnMouseEnter(MouseEventArgs e)
        {
            //base.OnMouseEnter(e);
            if (e.LeftButton == MouseButtonState.Pressed)
            {
                ReleaseMouseCapture();
                var content = this.TemplatedParent as ContentPresenter;
                if (ContentType == Content.BuildPlaylist)CheckedPlaylist(content,false);
                else CheckedMusicInfo(content,false);
            }
        }

        private void CheckedPlaylist(ContentPresenter content,bool Inverse)
        {
            var playlist = content.Content as BuildPlaylist;
            if (Inverse) playlist.Checked = !playlist.Checked;
            else playlist.Checked = true;
        }

        private void CheckedMusicInfo(ContentPresenter content,bool Inverse)
        {
            var info = content.Content as MusicInfo;
            if (Inverse) info.Check = !info.Check;
            else info.Check = true;
        }

        protected override void OnPreviewMouseDown(MouseButtonEventArgs e)
        {
            //base.OnPreviewMouseDown(e);
            var content = this.TemplatedParent as ContentPresenter;
            if (ContentType == Content.BuildPlaylist) CheckedPlaylist(content, true);
            else CheckedMusicInfo(content, true);
        }

        /*private void UIElement_OnPreviewMouseMove(object sender, MouseEventArgs e)
        {
            var a = sender as CheckBox;
            a.ReleaseMouseCapture();
        }

        private void UIElement_OnMouseEnter(object sender, MouseEventArgs e)
        {
            if (e.LeftButton == MouseButtonState.Pressed)
            {
                var a = sender as CheckBox;
                a.ReleaseMouseCapture();
                var b = a.TemplatedParent as ContentPresenter;
                var c = b.Content as BuildPlaylist;
                c.Checked = true;
            }
        }

        private void UIElement_OnPreviewMouseDown(object sender, MouseButtonEventArgs e)
        {
            var a = sender as CheckBox;
            var b = a.TemplatedParent as ContentPresenter;
            var c = b.Content as BuildPlaylist;
            c.Checked = !c.Checked;
        }*/
    }
}
