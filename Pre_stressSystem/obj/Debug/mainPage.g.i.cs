﻿#pragma checksum "..\..\mainPage.xaml" "{406ea660-64cf-4c82-b6f0-42d48172a799}" "A73EBD165E28D668135090A7AD14D3E8"
//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:4.0.30319.42000
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

using Pre_stressSystem;
using System;
using System.Diagnostics;
using System.Windows;
using System.Windows.Automation;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Forms.Integration;
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


namespace Pre_stressSystem {
    
    
    /// <summary>
    /// mainPage
    /// </summary>
    public partial class mainPage : System.Windows.Controls.Page, System.Windows.Markup.IComponentConnector {
        
        
        #line 17 "..\..\mainPage.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.DockPanel toolBar2;
        
        #line default
        #line hidden
        
        
        #line 18 "..\..\mainPage.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Button dectct;
        
        #line default
        #line hidden
        
        
        #line 19 "..\..\mainPage.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Button sensor;
        
        #line default
        #line hidden
        
        
        #line 20 "..\..\mainPage.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Button sensor_history;
        
        #line default
        #line hidden
        
        
        #line 21 "..\..\mainPage.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Button line_history;
        
        #line default
        #line hidden
        
        
        #line 22 "..\..\mainPage.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Button user_managment;
        
        #line default
        #line hidden
        
        
        #line 23 "..\..\mainPage.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Image logout;
        
        #line default
        #line hidden
        
        
        #line 24 "..\..\mainPage.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.TextBlock user_name;
        
        #line default
        #line hidden
        
        
        #line 45 "..\..\mainPage.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Frame function_frame;
        
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
            System.Uri resourceLocater = new System.Uri("/Pre_stressSystem;component/mainpage.xaml", System.UriKind.Relative);
            
            #line 1 "..\..\mainPage.xaml"
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
            this.toolBar2 = ((System.Windows.Controls.DockPanel)(target));
            return;
            case 2:
            this.dectct = ((System.Windows.Controls.Button)(target));
            
            #line 18 "..\..\mainPage.xaml"
            this.dectct.Click += new System.Windows.RoutedEventHandler(this.dectct_Click);
            
            #line default
            #line hidden
            return;
            case 3:
            this.sensor = ((System.Windows.Controls.Button)(target));
            
            #line 19 "..\..\mainPage.xaml"
            this.sensor.Click += new System.Windows.RoutedEventHandler(this.sensor_Click);
            
            #line default
            #line hidden
            return;
            case 4:
            this.sensor_history = ((System.Windows.Controls.Button)(target));
            
            #line 20 "..\..\mainPage.xaml"
            this.sensor_history.Click += new System.Windows.RoutedEventHandler(this.sensor_history_Click);
            
            #line default
            #line hidden
            return;
            case 5:
            this.line_history = ((System.Windows.Controls.Button)(target));
            
            #line 21 "..\..\mainPage.xaml"
            this.line_history.Click += new System.Windows.RoutedEventHandler(this.line_history_Click);
            
            #line default
            #line hidden
            return;
            case 6:
            this.user_managment = ((System.Windows.Controls.Button)(target));
            
            #line 22 "..\..\mainPage.xaml"
            this.user_managment.Click += new System.Windows.RoutedEventHandler(this.user_managment_Click);
            
            #line default
            #line hidden
            return;
            case 7:
            this.logout = ((System.Windows.Controls.Image)(target));
            
            #line 23 "..\..\mainPage.xaml"
            this.logout.MouseUp += new System.Windows.Input.MouseButtonEventHandler(this.logout_MouseUp);
            
            #line default
            #line hidden
            
            #line 23 "..\..\mainPage.xaml"
            this.logout.MouseEnter += new System.Windows.Input.MouseEventHandler(this.logout_MouseEnter);
            
            #line default
            #line hidden
            
            #line 23 "..\..\mainPage.xaml"
            this.logout.MouseLeave += new System.Windows.Input.MouseEventHandler(this.logout_MouseLeave);
            
            #line default
            #line hidden
            return;
            case 8:
            this.user_name = ((System.Windows.Controls.TextBlock)(target));
            return;
            case 9:
            this.function_frame = ((System.Windows.Controls.Frame)(target));
            return;
            }
            this._contentLoaded = true;
        }
    }
}

