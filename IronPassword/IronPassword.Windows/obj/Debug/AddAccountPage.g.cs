﻿

#pragma checksum "C:\Users\Jake\Documents\Visual Studio 2013\Projects\IronPassword\IronPassword\IronPassword.Windows\AddAccountPage.xaml" "{406ea660-64cf-4c82-b6f0-42d48172a799}" "6D76B4B364B99218F1C36D091EF178CE"
//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace IronPassword
{
    partial class AddAccountPage : global::Windows.UI.Xaml.Controls.Page, global::Windows.UI.Xaml.Markup.IComponentConnector
    {
        [global::System.CodeDom.Compiler.GeneratedCodeAttribute("Microsoft.Windows.UI.Xaml.Build.Tasks"," 4.0.0.0")]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
 
        public void Connect(int connectionId, object target)
        {
            switch(connectionId)
            {
            case 1:
                #line 75 "..\..\AddAccountPage.xaml"
                ((global::Windows.UI.Xaml.Controls.Primitives.ButtonBase)(target)).Click += this.generatePasswordButton_Click;
                 #line default
                 #line hidden
                break;
            case 2:
                #line 77 "..\..\AddAccountPage.xaml"
                ((global::Windows.UI.Xaml.Controls.Primitives.ButtonBase)(target)).Click += this.addAccountButton_Click;
                 #line default
                 #line hidden
                break;
            }
            this._contentLoaded = true;
        }
    }
}

