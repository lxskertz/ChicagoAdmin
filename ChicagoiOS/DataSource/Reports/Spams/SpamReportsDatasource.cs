using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Foundation;
using UIKit;
using TabsAdmin.Mobile.Shared.Models.Reports.Spams;
using TabsAdmin.Mobile.Shared.Resources;
using TabsAdmin.Mobile.Shared.Models;
using TabsAdmin.Mobile.Shared.Helpers;

namespace TabsAdmin.Mobile.ChicagoiOS.DataSource.Reports.Spams
{
    public class SpamReportsDatasource : UITableViewSource
    {

        #region Constants, Enums, and Variables

        /// <summary>
        /// Gets or sets the cell
        /// </summary>
        private NSString SpamReportsCell = new NSString("SpamReportsCell"); 

        #endregion

        #region Properties

        /// <summary>
        /// 
        /// </summary>
        public List<ReportedSpamCheckIn> CheckIns { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public List<ImageViewImage> ImageViewImage { get; set; }

        /// <summary>
        /// Gets or sets the controller
        /// </summary>
        public DailySpamReportsController Controller { get; set; }

        #endregion

        #region Constructors

        public SpamReportsDatasource(DailySpamReportsController controller, List<ReportedSpamCheckIn> checkIns,
             List<ImageViewImage> ImageViewImage)
        {
            this.Controller = controller;
            this.CheckIns = checkIns;
            this.ImageViewImage = ImageViewImage;
        }

        #endregion


        #region Methods

        /// <summary>
        /// Returns a table cell for the row indicated by row property of the NSIndexPath
        /// This method is called multiple times to populate each row of the table.
        /// The method automatically uses cells that have scrolled off the screen or creates new ones as necessary
        /// </summary>
        /// <param name="tableView"></param>
        /// <param name="indexPath"></param>
        /// <returns></returns>
        public override UITableViewCell GetCell(UITableView tableView, NSIndexPath indexPath)
        {
            var cell = (SpamReportsCell)tableView.DequeueReusableCell(this.SpamReportsCell);
            var item = this.CheckIns.ElementAt(indexPath.Row);

            if (item != null)
            {
                var itemLogo = this.ImageViewImage.Where(x => x.Id == item.CheckInId).FirstOrDefault();

                var reporterFname = string.IsNullOrEmpty(item.ReporterFirstName) ? "" : item.ReporterFirstName;
                var reporterLname = string.IsNullOrEmpty(item.ReporterLastName) ? "" : item.ReporterLastName;
                cell._ReporterName.Text = "Reporter: " + reporterFname + " " + reporterLname;

                var posterFname = string.IsNullOrEmpty(item.SenderFirstName) ? "" : item.SenderFirstName;
                var posterLname = string.IsNullOrEmpty(item.SenderLastName) ? "" : item.SenderLastName;
                cell._PosterName.Text = "Poster: " + posterFname + " " + posterLname;

                cell._CheckInDate.Text = item.CheckInDate.HasValue ? item.CheckInDate.Value.ToString() : "";

                cell.Tag = indexPath.Row;
                cell._Blockbtn.Layer.CornerRadius = 4;
                cell._Blockbtn.Layer.BorderWidth = 1;
                cell._Blockbtn.Layer.BorderColor = UIColor.FromRGB(145, 200, 244).CGColor;

                var btnTxt = item.BlockedByAdmin ? "Unblock Post" : "Block Post";
                cell._Blockbtn.SetTitle(btnTxt, UIControlState.Normal);
                // = item.UserId == AppDelegate.CurrentUser.UserId;

               
                // If the Image for this App has not been downloaded,
                // use the Placeholder image while we try to download
                // the real image from the web.
                cell._CheckInImage.ClipsToBounds = true;
                if (itemLogo != null)
                {
                    if (itemLogo != null && itemLogo.Image == null)
                    {
                        //app.Image = PlaceholderImage;
                        BeginDownloadingImage(itemLogo, indexPath, tableView);
                    }
                    cell._CheckInImage.Image = itemLogo.Image;
                }

                cell.Item = item;
                cell.DataSource = this;
                cell.IndexPath = indexPath;
            }

            return cell;

        }

        /// <summary>
        /// Gets number of section.... which is 1 in this case
        /// </summary>
        /// <param name="tableView"></param>
        /// <returns></returns>
        public override nint NumberOfSections(UITableView tableView)
        {
            return 1;
        }

        /// <summary>
        /// Called when a row is touched
        /// </summary>
        public override void RowSelected(UITableView tableView, NSIndexPath indexPath)
        {
            tableView.DeselectRow(indexPath, true);
            //var item = this.CheckIns.ElementAt(indexPath.Row);
            //var itemLogo = this.ImageViewImage.Where(x => x.Id == item.CheckInId).FirstOrDefault();

            //if (itemLogo != null && itemLogo.Image != null)
            //{
            //    var controller = this.Controller.Storyboard.InstantiateViewController("MyImageViewController") as MyImageViewController;
            //    controller.SelectedImage = itemLogo.Image;
            //    this.Controller.NavigationController.PushViewController(controller, true);
            //}
        }

        /// <summary>
        /// return num of rows that will be in the section
        /// </summary>
        /// <param name="tableview"></param>
        /// <param name="section"></param>
        /// <returns></returns>
        public override nint RowsInSection(UITableView tableview, nint section)
        {
            return this.CheckIns.Count;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="logo"></param>
        /// <param name="path"></param>
        /// <param name="tableView"></param>
        private async void BeginDownloadingImage(ImageViewImage logo, NSIndexPath path, UITableView tableView)
        {
            try
            {
                // Queue the image to be downloaded. This task will execute
                // as soon as the existing ones have finished.
                byte[] data = null;

                data = await BlobStorageHelper.GetImageData(logo.ImageUrl);
                logo.Image = UIImage.LoadFromData(NSData.FromArray(data));

                InvokeOnMainThread(() =>
                {
                    var cell = tableView.VisibleCells.Where(c => c.Tag == path.Row).FirstOrDefault();
                    if (cell != null && cell is SpamReportsCell)
                    {
                        var bcell = (SpamReportsCell)cell;
                        bcell._CheckInImage.Image = logo.Image;
                    }

                    //var cell = (SpamReportsCell)tableView.VisibleCells.Where(c => c.Tag == this.ImageViewImage.IndexOf(logo)).FirstOrDefault();
                    //if (cell != null)
                    //    cell._UserImage.Image = logo.Image;
                });
            }
            catch (Exception)
            {

            }
        }

        #endregion

    }
}