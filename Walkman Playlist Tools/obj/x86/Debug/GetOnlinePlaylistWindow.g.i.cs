﻿#pragma checksum "..\..\..\GetOnlinePlaylistWindow.xaml" "{ff1816ec-aa5e-4d10-87f7-6f4963833460}" "30D7EC2D1D7EF03ED6A9D676388278BF1F8541D2"
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
    /// GetOnlinePlaylistWindow
    /// </summary>
    public partial class GetOnlinePlaylistWindow : System.Windows.Window, System.Windows.Markup.IComponentConnector {
        
        
        #line 13 "..\..\..\GetOnlinePlaylistWindow.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.TextBox urlBox;
        
        #line default
        #line hidden
        
        
        #line 14 "..\..\..\GetOnlinePlaylistWindow.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Button StartGet;
        
        #line default
        #line hidden
        
        
        #line 23 "..\..\..\GetOnlinePlaylistWindow.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.RadioButton FromNetEase;
        
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
            System.Uri resourceLocater = new System.Uri("/Walkman Playlist Tools;component/getonlineplaylistwindow.xaml", System.UriKind.Relative);
            
            #line 1 "..\..\..\GetOnlinePlaylistWindow.xaml"
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
            this.urlBox = ((System.Windows.Controls.TextBox)(target));
            
            #line 13 "..\..\..\GetOnlinePlaylistWindow.xaml"
            this.urlBox.KeyDown += new System.Windows.Input.KeyEventHandler(this.UrlBox_OnKeyDown);
            
            #line default
            #line hidden
            return;
            case 2:
            this.StartGet = ((System.Windows.Controls.Button)(target));
            
            #line 14 "..\..\..\GetOnlinePlaylistWindow.xaml"
            this.StartGet.Click += new System.Windows.RoutedEventHandler(this.StartGet_OnClick);
            
            #line default
            #line hidden
            return;
            case 3:
            this.FromNetEase = ((System.Windows.Controls.RadioButton)(target));
            return;
            }
            this._contentLoaded = true;
        }
    }
}

