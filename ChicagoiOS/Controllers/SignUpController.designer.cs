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
    [Register ("SignUpController")]
    partial class SignUpController
    {
        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UIBarButtonItem CreateAcctBtn { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UITableView CreateAcctTable { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UIToolbar CreateAcctToolbar { get; set; }

        [Action ("CreateAcctBtn_Activated:")]
        [GeneratedCode ("iOS Designer", "1.0")]
        partial void CreateAcctBtn_Activated (UIKit.UIBarButtonItem sender);

        void ReleaseDesignerOutlets ()
        {
            if (CreateAcctBtn != null) {
                CreateAcctBtn.Dispose ();
                CreateAcctBtn = null;
            }

            if (CreateAcctTable != null) {
                CreateAcctTable.Dispose ();
                CreateAcctTable = null;
            }

            if (CreateAcctToolbar != null) {
                CreateAcctToolbar.Dispose ();
                CreateAcctToolbar = null;
            }
        }
    }
}