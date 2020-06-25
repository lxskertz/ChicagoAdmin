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
    [Register ("SpamReportsCell")]
    partial class SpamReportsCell
    {
        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UIButton Blockbtn { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UILabel CheckInDate { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UIImageView CheckInImage { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UILabel PosterName { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UILabel ReporterName { get; set; }

        [Action ("Blockbtn_TouchUpInside:")]
        [GeneratedCode ("iOS Designer", "1.0")]
        partial void Blockbtn_TouchUpInside (UIKit.UIButton sender);

        void ReleaseDesignerOutlets ()
        {
            if (Blockbtn != null) {
                Blockbtn.Dispose ();
                Blockbtn = null;
            }

            if (CheckInDate != null) {
                CheckInDate.Dispose ();
                CheckInDate = null;
            }

            if (CheckInImage != null) {
                CheckInImage.Dispose ();
                CheckInImage = null;
            }

            if (PosterName != null) {
                PosterName.Dispose ();
                PosterName = null;
            }

            if (ReporterName != null) {
                ReporterName.Dispose ();
                ReporterName = null;
            }
        }
    }
}