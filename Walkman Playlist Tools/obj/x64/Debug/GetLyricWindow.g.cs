﻿#pragma checksum "..\..\..\GetLyricWindow.xaml" "{ff1816ec-aa5e-4d10-87f7-6f4963833460}" "27FC914D60A9BA759AEC4DE117A302D2935D0D7D"
//------------------------------------------------------------------------------
// <auto-generated>
//     此代码由工具生成。
//     运行时版本:4.0.30319.42000
//
//     对此文件的更改可能会导致不正确的行为，并且如果
//     重新生成代码，这些更改将会丢失。
// </auto-generated>
//------------------------------------------------------------------------------

using System;
using System.Diagnostics;
using System.Windows;
using System.Windows.Automation;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Ink;
using System.Windows.Input;
using System.Windows.Markup;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Media.Effects;
using System.Windows.Media.Imaging;
using System.Windows.Media.Media3D;
using System.Windows.Media.TextFormatting;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Windows.Shell;
using Walkman_Playlist_Tools;


namespace Walkman_Playlist_Tools {
    
    
    /// <summary>
    /// GetLyricWindow
    /// </summary>
    public partial class GetLyricWindow : System.Windows.Window, System.Windows.Markup.IComponentConnector {
        
        
        #line 26 "..\..\..\GetLyricWindow.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Border ColorBorder;
        
        #line default
        #line hidden
        
        
        #line 30 "..\..\..\GetLyricWindow.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.TextBlock PositionText;
        
        #line default
        #line hidden
        
        
        #line 34 "..\..\..\GetLyricWindow.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.ListView LocalInfoList;
        
        #line default
        #line hidden
        
        
        #line 58 "..\..\..\GetLyricWindow.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.ListView SearchResultList;
        
        #line default
        #line hidden
        
        
        #line 72 "..\..\..\GetLyricWindow.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.StackPanel SavePanel;
        
        #line default
        #line hidden
        
        
        #line 73 "..\..\..\GetLyricWindow.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Button SaveButton;
        
        #line default
        #line hidden
        
        
        #line 75 "..\..\..\GetLyricWindow.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.TextBox LyricBox;
        
        #line default
        #line hidden
        
        
        #line 108 "..\..\..\GetLyricWindow.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.ComboBox MultiArtistBox;
        
        #line default
        #line hidden
        
        
        #line 115 "..\..\..\GetLyricWindow.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Button PauseButton;
        
        #line default
        #line hidden
        
        
        #line 116 "..\..\..\GetLyricWindow.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.ProgressBar TimeBar;
        
        #line default
        #line hidden
        
        private bool _contentLoaded;
        
        /// <summary>
        /// InitializeComponent
        /// </summary>
        [System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [System.CodeDom.Compiler.GeneratedCodeAttribute("PresentationBuildTasks", "4.0.0.0")]
        public void InitializeComponent() {
            if (_contentLoaded) {
                return;
            }
            _contentLoaded = true;
            System.Uri resourceLocater = new System.Uri("/Walkman Playlist Tools;component/getlyricwindow.xaml", System.UriKind.Relative);
            
            #line 1 "..\..\..\GetLyricWindow.xaml"
            System.Windows.Application.LoadComponent(this, resourceLocater);
            
            #line default
            #line hidden
        }
        
        [System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [System.CodeDom.Compiler.GeneratedCodeAttribute("PresentationBuildTasks", "4.0.0.0")]
        [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Never)]
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Design", "CA1033:InterfaceMethodsShouldBeCallableByChildTypes")]
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Maintainability", "CA1502:AvoidExcessiveComplexity")]
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1800:DoNotCastUnnecessarily")]
        void System.Windows.Markup.IComponentConnector.Connect(int connectionId, object target) {
            switch (connectionId)
            {
            case 1:
            
            #line 8 "..\..\..\GetLyricWindow.xaml"
            ((Walkman_Playlist_Tools.GetLyricWindow)(target)).Closed += new System.EventHandler(this.GetLyricWindow_OnClosed);
            
            #line default
            #line hidden
            return;
            case 2:
            this.ColorBorder = ((System.Windows.Controls.Border)(target));
            return;
            case 3:
            this.PositionText = ((System.Windows.Controls.TextBlock)(target));
            return;
            case 4:
            this.LocalInfoList = ((System.Windows.Controls.ListView)(target));
            
            #line 34 "..\..\..\GetLyricWindow.xaml"
            this.LocalInfoList.SelectionChanged += new System.Windows.Controls.SelectionChangedEventHandler(this.LocalInfoList_OnSelectionChanged);
            
            #line default
            #line hidden
            
            #line 34 "..\..\..\GetLyricWindow.xaml"
            this.LocalInfoList.PreviewMouseDown += new System.Windows.Input.MouseButtonEventHandler(this.LocalInfoList_OnPreviewMouseDown);
            
            #line default
            #line hidden
            return;
            case 5:
            this.SearchResultList = ((System.Windows.Controls.ListView)(target));
            
            #line 58 "..\..\..\GetLyricWindow.xaml"
            this.SearchResultList.SelectionChanged += new System.Windows.Controls.SelectionChangedEventHandler(this.SearchResultList_OnSelectionChanged);
            
            #line default
            #line hidden
            
            #line 58 "..\..\..\GetLyricWindow.xaml"
            this.SearchResultList.PreviewMouseDown += new System.Windows.Input.MouseButtonEventHandler(this.SearchResultList_OnPreviewMouseDown);
            
            #line default
            #line hidden
            return;
            case 6:
            this.SavePanel = ((System.Windows.Controls.StackPanel)(target));
            return;
            case 7:
            this.SaveButton = ((System.Windows.Controls.Button)(target));
            
            #line 73 "..\..\..\GetLyricWindow.xaml"
            this.SaveButton.Click += new System.Windows.RoutedEventHandler(this.SaveButton_OnClick);
            
            #line default
            #line hidden
            return;
            case 8:
            this.LyricBox = ((System.Windows.Controls.TextBox)(target));
            return;
            case 9:
            this.MultiArtistBox = ((System.Windows.Controls.ComboBox)(target));
            return;
            case 10:
            this.PauseButton = ((System.Windows.Controls.Button)(target));
            
            #line 115 "..\..\..\GetLyricWindow.xaml"
            this.PauseButton.Click += new System.Windows.RoutedEventHandler(this.PauseButton_OnClick);
            
            #line default
            #line hidden
            return;
            case 11:
            this.TimeBar = ((System.Windows.Controls.ProgressBar)(target));
            return;
            }
            this._contentLoaded = true;
        }
    }
}

