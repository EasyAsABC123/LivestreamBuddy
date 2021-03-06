﻿#pragma checksum "..\..\..\Controls\Channels.xaml" "{406ea660-64cf-4c82-b6f0-42d48172a799}" "5F854317E8F4F26B2BDE1265F59D3815"
//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:4.0.30319.18408
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
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


namespace LiveStreamBuddy.Controls {
    
    
    /// <summary>
    /// Channels
    /// </summary>
    public partial class Channels : System.Windows.Controls.UserControl, System.Windows.Markup.IComponentConnector {
        
        
        #line 11 "..\..\..\Controls\Channels.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Button btnShowFeaturedStreams;
        
        #line default
        #line hidden
        
        
        #line 12 "..\..\..\Controls\Channels.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Button btnShowFollowedStreams;
        
        #line default
        #line hidden
        
        
        #line 15 "..\..\..\Controls\Channels.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.DataGrid grdChannels;
        
        #line default
        #line hidden
        
        
        #line 74 "..\..\..\Controls\Channels.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Button btnAddToFavoriteChannel;
        
        #line default
        #line hidden
        
        
        #line 75 "..\..\..\Controls\Channels.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Button btnRemoveChannel;
        
        #line default
        #line hidden
        
        
        #line 78 "..\..\..\Controls\Channels.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Button btnGoToChannel;
        
        #line default
        #line hidden
        
        
        #line 79 "..\..\..\Controls\Channels.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.TextBox txtGoToChannel;
        
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
            System.Uri resourceLocater = new System.Uri("/LiveStreamBuddy;component/controls/channels.xaml", System.UriKind.Relative);
            
            #line 1 "..\..\..\Controls\Channels.xaml"
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
            this.btnShowFeaturedStreams = ((System.Windows.Controls.Button)(target));
            
            #line 11 "..\..\..\Controls\Channels.xaml"
            this.btnShowFeaturedStreams.Click += new System.Windows.RoutedEventHandler(this.ShowFeaturedStreamsClick);
            
            #line default
            #line hidden
            return;
            case 2:
            this.btnShowFollowedStreams = ((System.Windows.Controls.Button)(target));
            
            #line 12 "..\..\..\Controls\Channels.xaml"
            this.btnShowFollowedStreams.Click += new System.Windows.RoutedEventHandler(this.ShowFollowedStreamsClick);
            
            #line default
            #line hidden
            return;
            case 3:
            this.grdChannels = ((System.Windows.Controls.DataGrid)(target));
            
            #line 15 "..\..\..\Controls\Channels.xaml"
            this.grdChannels.MouseDoubleClick += new System.Windows.Input.MouseButtonEventHandler(this.grdChannels_MouseDoubleClick);
            
            #line default
            #line hidden
            
            #line 15 "..\..\..\Controls\Channels.xaml"
            this.grdChannels.SelectionChanged += new System.Windows.Controls.SelectionChangedEventHandler(this.grdChannels_SelectionChanged);
            
            #line default
            #line hidden
            return;
            case 4:
            this.btnAddToFavoriteChannel = ((System.Windows.Controls.Button)(target));
            
            #line 74 "..\..\..\Controls\Channels.xaml"
            this.btnAddToFavoriteChannel.Click += new System.Windows.RoutedEventHandler(this.btnAddToFavoriteChannel_Click);
            
            #line default
            #line hidden
            return;
            case 5:
            this.btnRemoveChannel = ((System.Windows.Controls.Button)(target));
            
            #line 75 "..\..\..\Controls\Channels.xaml"
            this.btnRemoveChannel.Click += new System.Windows.RoutedEventHandler(this.RemoveChannelClick);
            
            #line default
            #line hidden
            return;
            case 6:
            this.btnGoToChannel = ((System.Windows.Controls.Button)(target));
            
            #line 78 "..\..\..\Controls\Channels.xaml"
            this.btnGoToChannel.Click += new System.Windows.RoutedEventHandler(this.btnGoToChannel_Click);
            
            #line default
            #line hidden
            return;
            case 7:
            this.txtGoToChannel = ((System.Windows.Controls.TextBox)(target));
            return;
            }
            this._contentLoaded = true;
        }
    }
}

