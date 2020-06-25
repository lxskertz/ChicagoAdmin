// WARNING
//
// This file has been generated automatically by Visual Studio from the outlets and
// actions declared in your storyboard file.
// Manual changes to this file will not be maintained.
//
using Foundation;
using System;
using System.CodeDom.Compiler;
using UIKit;

namespace TabsAdmin.Mobile.ChicagoiOS
{
    [Register ("AdminHomeController")]
    partial class AdminHomeController
    {
        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UITableView AdminHomeTable { get; set; }

        void ReleaseDesignerOutlets ()
        {
            if (AdminHomeTable != null) {
                AdminHomeTable.Dispose ();
                AdminHomeTable = null;
            }
        }
    }
}