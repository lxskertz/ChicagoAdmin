using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Foundation;
using UIKit;
using TabsAdmin.Mobile.Shared.Helpers;

namespace TabsAdmin.Mobile.ChicagoiOS.DataSource
{
    public class AdminHomeDataSource : UITableViewSource
    {

        #region Constants, Enums, and Variables

        /// <summary>
        /// Gets or sets the cell
        /// </summary>
        private NSString AdminHomeCell = new NSString("AdminHomeCell"); 

        #endregion

        #region Properties

        /// <summary>
        /// GEts or set the controller
        /// </summary>
        private AdminHomeController Controller { get; set; }

        /// <summary>
        /// Gets or sets rows
        /// </summary>
        private List<string> Rows { get; set; }

        #endregion

        #region Constructors

        /// <summary>
        /// 
        /// </summary>
        /// <param name="controller"></param>
        public AdminHomeDataSource(AdminHomeController controller, List<string> rows)
        {
            this.Controller = controller;
            this.Rows = rows;
        }

        #endregion

        #region Methods

        public override UITableViewCell GetCell(UITableView tableView, NSIndexPath indexPath)
        {
            var cell = (AdminHomeCell)tableView.DequeueReusableCell(this.AdminHomeCell);
            cell.TextLabel.Text = this.Rows[indexPath.Row];

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
        public async override void RowSelected(UITableView tableView, NSIndexPath indexPath)
        {
            tableView.DeselectRow(indexPath, true);
            var item = this.Rows[indexPath.Row];
            switch (item)
            {
                case MoreScreenHelper.Logout:
                    await Logout();
                    break;
                case MoreScreenHelper.AllCheckinReports:
                    var allInp = this.Controller.Storyboard.InstantiateViewController("DailySpamReportsController") as DailySpamReportsController;
                    allInp.ControllerCaller = DailySpamReportsController.Caller.AllInappropriateReports;
                    this.Controller.NavigationController.PushViewController(allInp, true);
                    break;
                case MoreScreenHelper.DailyCheckinReports:
                    var dailyInp = this.Controller.Storyboard.InstantiateViewController("DailySpamReportsController") as DailySpamReportsController;
                    dailyInp.ControllerCaller = DailySpamReportsController.Caller.DailyInappropriateReports;
                    this.Controller.NavigationController.PushViewController(dailyInp, true);
                    break;
                case MoreScreenHelper.LockUnLockUsers:
                    OpenLockedUsers();
                    break;
                case MoreScreenHelper.AllUsers:
                    break;
                case MoreScreenHelper.UsersReports:
                    var uRep = this.Controller.Storyboard.InstantiateViewController("DailySpamReportsController") as DailySpamReportsController;
                    uRep.ControllerCaller = DailySpamReportsController.Caller.AllReportedUser;
                    this.Controller.NavigationController.PushViewController(uRep, true);
                    break;
                case MoreScreenHelper.DailyUserReports:
                    var dRep = this.Controller.Storyboard.InstantiateViewController("DailySpamReportsController") as DailySpamReportsController;
                    dRep.ControllerCaller = DailySpamReportsController.Caller.DailyReportedUsers;
                    this.Controller.NavigationController.PushViewController(dRep, true);
                    break;
                case MoreScreenHelper.DailySpamReports:
                    var dailySpam = this.Controller.Storyboard.InstantiateViewController("DailySpamReportsController") as DailySpamReportsController;
                    dailySpam.ControllerCaller = DailySpamReportsController.Caller.DailySpam;
                    this.Controller.NavigationController.PushViewController(dailySpam, true);
                    break;
                case MoreScreenHelper.AllSpamReports:
                    var allSpam = this.Controller.Storyboard.InstantiateViewController("DailySpamReportsController") as DailySpamReportsController;
                    allSpam.ControllerCaller = DailySpamReportsController.Caller.AllSpam;
                    this.Controller.NavigationController.PushViewController(allSpam, true);
                    break;
            }
        }

        /// <summary>
        /// Called by the TableView to determine how many cells to create for that particular section.
        /// </summary>
        /// <param name="tableview"></param>
        /// <param name="section"></param>
        /// <returns></returns>
        public override nint RowsInSection(UITableView tableview, nint section)
        {
            return this.Rows.Count;
        }

        private void OpenLockedUsers()
        {
            var controller = this.Controller.Storyboard.InstantiateViewController("SearchLockedUsersController") as SearchLockedUsersController;
            this.Controller.NavigationController.PushViewController(controller, true);
        }

        /// <summary>
        /// Logout
        /// </summary>
        /// <returns></returns>
        private async Task Logout()
        {
            try
            {
                AppDelegate.DeleteSettings();
                UIViewController login = this.Controller.Storyboard.InstantiateViewController("HomeController") as HomeController;
                this.Controller.NavigationController.SetViewControllers(new UIViewController[] { login }, true);

                if (!AppDelegate.IsOfflineMode())
                {
                    await AppDelegate.UsersFactory.Logout(AppDelegate.CurrentUser.Email);
                    //if (!string.IsNullOrEmpty(AppDelegate.DeviceRegistrationId()))
                    //{
                    //    await AppDelegate.NotificationRegisterManager.Delete(AppDelegate.DeviceRegistrationId());
                    //}
                }
            }
            catch (Exception) { }
        }

        #endregion

    }
}