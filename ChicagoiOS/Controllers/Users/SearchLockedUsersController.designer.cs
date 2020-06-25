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
    [Register ("SearchLockedUsersController")]
    partial class SearchLockedUsersController
    {
        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UITableView LockedTable { get; set; }

        void ReleaseDesignerOutlets ()
        {
            if (LockedTable != null) {
                LockedTable.Dispose ();
                LockedTable = null;
            }
        }
    }
}