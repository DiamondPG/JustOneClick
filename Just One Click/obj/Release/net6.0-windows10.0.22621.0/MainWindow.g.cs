<<<<<<< Updated upstream
﻿#pragma checksum "..\..\..\MainWindow.xaml" "{ff1816ec-aa5e-4d10-87f7-6f4963833460}" "F6CBA95B37BB496A73ED52BF773011CF773CACFE"
=======
﻿#pragma checksum "..\..\..\MainWindow.xaml" "{ff1816ec-aa5e-4d10-87f7-6f4963833460}" "A89AD40A9137E320114C398048F518DC8AEF6F38"
>>>>>>> Stashed changes
//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:4.0.30319.42000
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

using Just_One_Click;
using System;
using System.Diagnostics;
using System.Windows;
using System.Windows.Automation;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Controls.Ribbon;
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


namespace Just_One_Click {
    
    
    /// <summary>
    /// MainWindow
    /// </summary>
    public partial class MainWindow : System.Windows.Window, System.Windows.Markup.IComponentConnector {
        
        
        #line 37 "..\..\..\MainWindow.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Button LaunchBTN;
        
        #line default
        #line hidden
        
        
        #line 59 "..\..\..\MainWindow.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Button SettingsBTN;
        
        #line default
        #line hidden
        
        
        #line 66 "..\..\..\MainWindow.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Button RefreshButton;
        
        #line default
        #line hidden
        
        
        #line 75 "..\..\..\MainWindow.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.ListBox Profiles;
        
        #line default
        #line hidden
        
        
<<<<<<< Updated upstream
        #line 152 "..\..\..\MainWindow.xaml"
=======
        #line 166 "..\..\..\MainWindow.xaml"
>>>>>>> Stashed changes
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.TextBlock Console;
        
        #line default
        #line hidden
        
        
<<<<<<< Updated upstream
        #line 153 "..\..\..\MainWindow.xaml"
=======
        #line 167 "..\..\..\MainWindow.xaml"
>>>>>>> Stashed changes
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.ListBox AppsLB;
        
        #line default
        #line hidden
        
        private bool _contentLoaded;
        
        /// <summary>
        /// InitializeComponent
        /// </summary>
        [System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [System.CodeDom.Compiler.GeneratedCodeAttribute("PresentationBuildTasks", "7.0.10.0")]
        public void InitializeComponent() {
            if (_contentLoaded) {
                return;
            }
            _contentLoaded = true;
            System.Uri resourceLocater = new System.Uri("/Just One Click;component/mainwindow.xaml", System.UriKind.Relative);
            
            #line 1 "..\..\..\MainWindow.xaml"
            System.Windows.Application.LoadComponent(this, resourceLocater);
            
            #line default
            #line hidden
        }
        
        [System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [System.CodeDom.Compiler.GeneratedCodeAttribute("PresentationBuildTasks", "7.0.10.0")]
        [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Never)]
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Design", "CA1033:InterfaceMethodsShouldBeCallableByChildTypes")]
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Maintainability", "CA1502:AvoidExcessiveComplexity")]
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1800:DoNotCastUnnecessarily")]
        void System.Windows.Markup.IComponentConnector.Connect(int connectionId, object target) {
            switch (connectionId)
            {
            case 1:
            this.LaunchBTN = ((System.Windows.Controls.Button)(target));
            
            #line 37 "..\..\..\MainWindow.xaml"
            this.LaunchBTN.Click += new System.Windows.RoutedEventHandler(this.LaunchBTN_Click);
            
            #line default
            #line hidden
            return;
            case 2:
            this.SettingsBTN = ((System.Windows.Controls.Button)(target));
            
            #line 59 "..\..\..\MainWindow.xaml"
            this.SettingsBTN.Click += new System.Windows.RoutedEventHandler(this.Settings_Click);
            
            #line default
            #line hidden
            return;
            case 3:
            this.RefreshButton = ((System.Windows.Controls.Button)(target));
            
            #line 66 "..\..\..\MainWindow.xaml"
            this.RefreshButton.Click += new System.Windows.RoutedEventHandler(this.RefreshButton_Click);
            
            #line default
            #line hidden
            return;
            case 4:
            this.Profiles = ((System.Windows.Controls.ListBox)(target));
            
            #line 77 "..\..\..\MainWindow.xaml"
            this.Profiles.SelectionChanged += new System.Windows.Controls.SelectionChangedEventHandler(this.Profiles_SelectionChanged);
            
            #line default
            #line hidden
            return;
            case 5:
            
<<<<<<< Updated upstream
            #line 120 "..\..\..\MainWindow.xaml"
=======
            #line 139 "..\..\..\MainWindow.xaml"
>>>>>>> Stashed changes
            ((System.Windows.Controls.MenuItem)(target)).Click += new System.Windows.RoutedEventHandler(this.AddProfile_Click);
            
            #line default
            #line hidden
            return;
            case 6:
            
<<<<<<< Updated upstream
            #line 126 "..\..\..\MainWindow.xaml"
=======
            #line 145 "..\..\..\MainWindow.xaml"
>>>>>>> Stashed changes
            ((System.Windows.Controls.MenuItem)(target)).Click += new System.Windows.RoutedEventHandler(this.RenameProfile_Click);
            
            #line default
            #line hidden
            return;
            case 7:
            
<<<<<<< Updated upstream
            #line 131 "..\..\..\MainWindow.xaml"
            ((System.Windows.Controls.MenuItem)(target)).Click += new System.Windows.RoutedEventHandler(this.ChangePathMenuItem_Click);
=======
            #line 156 "..\..\..\MainWindow.xaml"
            ((System.Windows.Controls.MenuItem)(target)).Click += new System.Windows.RoutedEventHandler(this.DeleteProfile_Click);
>>>>>>> Stashed changes
            
            #line default
            #line hidden
            return;
            case 8:
            
<<<<<<< Updated upstream
            #line 142 "..\..\..\MainWindow.xaml"
            ((System.Windows.Controls.MenuItem)(target)).Click += new System.Windows.RoutedEventHandler(this.DeleteProfile_Click);
=======
            #line 231 "..\..\..\MainWindow.xaml"
            ((System.Windows.Controls.MenuItem)(target)).Click += new System.Windows.RoutedEventHandler(this.AddMenuItem_Click);
>>>>>>> Stashed changes
            
            #line default
            #line hidden
            return;
            case 9:
            this.Console = ((System.Windows.Controls.TextBlock)(target));
            return;
            case 10:
            this.AppsLB = ((System.Windows.Controls.ListBox)(target));
            return;
            case 11:
            
<<<<<<< Updated upstream
            #line 217 "..\..\..\MainWindow.xaml"
            ((System.Windows.Controls.MenuItem)(target)).Click += new System.Windows.RoutedEventHandler(this.AddMenuItem_Click);
=======
            #line 236 "..\..\..\MainWindow.xaml"
            ((System.Windows.Controls.MenuItem)(target)).Click += new System.Windows.RoutedEventHandler(this.AddBrowserSource);
>>>>>>> Stashed changes
            
            #line default
            #line hidden
            return;
            case 12:
            
<<<<<<< Updated upstream
            #line 222 "..\..\..\MainWindow.xaml"
            ((System.Windows.Controls.MenuItem)(target)).Click += new System.Windows.RoutedEventHandler(this.AddBrowserSource);
=======
            #line 241 "..\..\..\MainWindow.xaml"
            ((System.Windows.Controls.MenuItem)(target)).Click += new System.Windows.RoutedEventHandler(this.AddTextSource_Click);
>>>>>>> Stashed changes
            
            #line default
            #line hidden
            return;
            case 13:
            
<<<<<<< Updated upstream
            #line 233 "..\..\..\MainWindow.xaml"
=======
            #line 247 "..\..\..\MainWindow.xaml"
>>>>>>> Stashed changes
            ((System.Windows.Controls.MenuItem)(target)).Click += new System.Windows.RoutedEventHandler(this.RenameMenuItem_Click);
            
            #line default
            #line hidden
            return;
            case 14:
            
<<<<<<< Updated upstream
            #line 238 "..\..\..\MainWindow.xaml"
=======
            #line 252 "..\..\..\MainWindow.xaml"
>>>>>>> Stashed changes
            ((System.Windows.Controls.MenuItem)(target)).Click += new System.Windows.RoutedEventHandler(this.ChangePathMenuItem_Click);
            
            #line default
            #line hidden
            return;
            case 15:
            
<<<<<<< Updated upstream
            #line 243 "..\..\..\MainWindow.xaml"
=======
            #line 257 "..\..\..\MainWindow.xaml"
>>>>>>> Stashed changes
            ((System.Windows.Controls.MenuItem)(target)).Click += new System.Windows.RoutedEventHandler(this.ChangeFavicon_Click);
            
            #line default
            #line hidden
            return;
            case 16:
            
<<<<<<< Updated upstream
            #line 254 "..\..\..\MainWindow.xaml"
=======
            #line 268 "..\..\..\MainWindow.xaml"
>>>>>>> Stashed changes
            ((System.Windows.Controls.MenuItem)(target)).Click += new System.Windows.RoutedEventHandler(this.Duplicate_Click);
            
            #line default
            #line hidden
            return;
            case 17:
            
<<<<<<< Updated upstream
            #line 259 "..\..\..\MainWindow.xaml"
=======
            #line 273 "..\..\..\MainWindow.xaml"
>>>>>>> Stashed changes
            ((System.Windows.Controls.MenuItem)(target)).Click += new System.Windows.RoutedEventHandler(this.ContextMenu_DeleteClick);
            
            #line default
            #line hidden
            return;
            }
            this._contentLoaded = true;
        }
    }
}

