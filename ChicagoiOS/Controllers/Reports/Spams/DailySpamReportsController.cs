using Foundation;
using System;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.Linq;
using UIKit;
using TabsAdmin.Mobile.Shared.Models.Reports.Spams;
using TabsAdmin.Mobile.Shared.Models.Reports.InappropriateReports;
using TabsAdmin.Mobile.Shared.Models.Reports.Users;
using TabsAdmin.Mobile.Shared.Resources;
using TabsAdmin.Mobile.ChicagoiOS.DataSource.Reports.Spams;
using TabsAdmin.Mobile.ChicagoiOS.DataSource.Reports.Inappropriates;
using TabsAdmin.Mobile.ChicagoiOS.DataSource.Reports.Users;
using TabsAdmin.Mobile.Shared.Models;
using TabsAdmin.Mobile.Shared.Helpers;
using BigTed;

namespace TabsAdmin.Mobile.ChicagoiOS
{
    public partial class DailySpamReportsController : UIViewController
    {
        #region Constants, Enums, and Variables

        private UIRefreshControl RefreshControl;
        public SearchParameters param = new SearchParameters();
        public bool loadMore = true;

        public enum Caller
        {
            DailySpam,
            AllSpam,
            DailyInappropriateReports,
            AllInappropriateReports,
            DailyReportedUsers,
            AllReportedUser
        }

        #endregion

        #region Properties

        public Caller ControllerCaller { get; set; }

        /// <summary>
        /// Gets or sets data source
        /// </summary>
        private SpamReportsDatasource SpamReportsDatasource { get; set; }

        /// <summary>
        /// Gets or sets data source
        /// </summary>
        private InappropriateReportsDatasource InappropriateReportsDatasource { get; set; }

        private ReportedUserDatasource ReportedUserDatasource { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public List<ReportedSpamCheckIn> CheckIns { get; set; } = new List<ReportedSpamCheckIn>();

        /// <summary>
        /// 
        /// </summary>
        public List<InappropriateReport> InappropriateReportCheckIns { get; set; }

        public List<ReportedUser> ReportedUsers { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public List<ImageViewImage> CheckInsImageViewImage { get; set; } = new List<ImageViewImage>();

        public static bool RequiresRefresh { get; set; }


        #endregion

        #region Constructors

        public DailySpamReportsController (IntPtr handle) : base (handle)
        {
        }

        #endregion

        #region Methods

        private void SetScreenTitle()
        {
            switch (this.ControllerCaller)
            {
                case Caller.AllInappropriateReports:
                    this.Title = "All Inappropriate Reports";
                    break;
                case Caller.AllSpam:
                    this.Title = "All Spams";
                    break;
                case Caller.DailyInappropriateReports:
                    this.Title = "Daily Inappropriate Reports";
                    break;
                case Caller.DailySpam:
                    this.Title = "Daily Spams";
                    break;
                case Caller.AllReportedUser:
                    this.Title = "All User Reports";
                    break;
                case Caller.DailyReportedUsers:
                    this.Title = "Daily User Reports";
                    break;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public override void ViewDidLoad()
        {
            try
            {
                base.ViewDidLoad();
                SetScreenTitle();
                RefreshControl = new UIRefreshControl();
                RefreshControl.ValueChanged += HandleValueChanged;
                DailySpamTable.AddSubview(RefreshControl);
                LoadCheckIns();
            }
            catch (Exception)
            {
            }
        }

        /// <summary>
        /// 
        /// </summary>
        private async void LoadCheckIns()
        {
            try
            {
                if (this.ControllerCaller == Caller.AllSpam || this.ControllerCaller == Caller.DailySpam)
                {
                    await this.GetCheckIns();
                    if (this.CheckIns != null && this.CheckIns.Count > 0)
                    {
                        DailySpamTable.EstimatedRowHeight = 341f;
                        DailySpamTable.RowHeight = UITableView.AutomaticDimension;
                        SpamReportsDatasource = new SpamReportsDatasource(this, this.CheckIns, this.CheckInsImageViewImage);
                        DailySpamTable.Source = SpamReportsDatasource;
                        DailySpamTable.TableFooterView = new UIView();
                    }
                }

                if(this.ControllerCaller == Caller.AllInappropriateReports || this.ControllerCaller == Caller.DailyInappropriateReports)
                {
                    await this.GetInappropriateCheckIns();
                    if (this.InappropriateReportCheckIns != null && this.InappropriateReportCheckIns.Count > 0)
                    {
                        DailySpamTable.EstimatedRowHeight = 341f;
                        DailySpamTable.RowHeight = UITableView.AutomaticDimension;
                        InappropriateReportsDatasource = new InappropriateReportsDatasource(this, this.InappropriateReportCheckIns, this.CheckInsImageViewImage);
                        DailySpamTable.Source = InappropriateReportsDatasource;
                        DailySpamTable.TableFooterView = new UIView();
                    }
                }
                if (this.ControllerCaller == Caller.AllReportedUser || this.ControllerCaller == Caller.DailyReportedUsers)
                {
                    await this.GetReportedUsers();
                    if (this.ReportedUsers != null && this.ReportedUsers.Count > 0)
                    {
                        DailySpamTable.EstimatedRowHeight = 341f;
                        DailySpamTable.RowHeight = UITableView.AutomaticDimension;
                        ReportedUserDatasource = new ReportedUserDatasource(this, this.ReportedUsers, this.CheckInsImageViewImage);
                        DailySpamTable.Source = ReportedUserDatasource;
                        DailySpamTable.TableFooterView = new UIView();
                    }
                }
            }
            catch (Exception)
            {
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="searchTerm"></param>
        private void InitSearchParameters(string searchTerm)
        {
            param = new SearchParameters();
            param.PageSize = 10;
            param.SearchTerm = searchTerm;
            param.PageNumber = 0;
        }

        /// <summary>
        /// Called when recycler view is scrolled to the bottom 
        /// </summary>
        public async Task ScrolledToBottom()
        {
            //if (!AppDelegate.IsOfflineMode() && loadMore)
            //{
            //    try
            //    {
            //        param.PageNumber += this.LiveEventsDataSource.BusinessEvents.Count;
            //        var events = await AppDelegate.BusinessEventsFactory.GetLiveEvents(param.ZipCode, param.City, param.PageNumber, param.PageSize);
            //        if (events != null && events.Count > 0)
            //        {
            //            this.LiveEventsDataSource.AddRowItems(events.ToList());
            //            ToastersLiveTable.ReloadData();
            //        }
            //        else
            //        {
            //            loadMore = false;
            //        }
            //    }
            //    catch (Exception) { }
            //}
        }

        /// <summary> 
        /// 
        /// </summary>
        /// <returns></returns>
        public async Task BlockSpamPost(ReportedSpamCheckIn checkIn)
        {
            try
            {
                if (AppDelegate.IsOfflineMode())
                {
                    BTProgressHUD.ShowErrorWithStatus(ToastMessage.NoInternet, Helpers.ToastTime.ErrorTime);
                    return;
                }
                else
                {
                    BTProgressHUD.Show(ToastMessage.Loading, -1f, ProgressHUD.MaskType.Black);

                    await AppDelegate.ReportedSpamCheckInFactory.BlockPost(AppDelegate.CurrentUser.UserId, checkIn.SpamCheckInId);
                    await AppDelegate.CheckInFactory.BlockPost(AppDelegate.CurrentUser.UserId, checkIn.CheckInId);

                    RefreshCheckIns();

                    var PushNotificationHelper = new PushNotificationHelper(AppDelegate.NotificationRegisterFactory, Shared.Helpers.PushNotificationHelper.PushPlatform.iOS);
                    await PushNotificationHelper.BlockedSpamPostReporterPush(checkIn);
                    await PushNotificationHelper.BlockedSpamPostPosterPush(checkIn);

                    BTProgressHUD.Dismiss();
                }
            }
            catch (Exception ex)
            {
                var a = ex;
                BTProgressHUD.Dismiss();
            }
        }

        /// <summary> 
        /// 
        /// </summary>
        /// <returns></returns>
        public async Task UnBlockSpamPost(ReportedSpamCheckIn checkIn)
        {
            try
            {
                if (AppDelegate.IsOfflineMode())
                {
                    BTProgressHUD.ShowErrorWithStatus(ToastMessage.NoInternet, Helpers.ToastTime.ErrorTime);
                    return;
                }
                else
                {
                    BTProgressHUD.Show(ToastMessage.Loading, -1f, ProgressHUD.MaskType.Black);

                    await AppDelegate.ReportedSpamCheckInFactory.UnBlockPost(checkIn.SpamCheckInId);
                    await AppDelegate.CheckInFactory.UnBlockPost(checkIn.CheckInId);

                    RefreshCheckIns();

                    BTProgressHUD.Dismiss();
                }
            }
            catch (Exception ex)
            {
                var a = ex;
                BTProgressHUD.Dismiss();
            }
        }

        /// <summary> 
        /// 
        /// </summary>
        /// <returns></returns>
        public async Task BlockInappropriatePost(InappropriateReport checkIn)
        {
            try
            {
                if (AppDelegate.IsOfflineMode())
                {
                    BTProgressHUD.ShowErrorWithStatus(ToastMessage.NoInternet, Helpers.ToastTime.ErrorTime);
                    return;
                }
                else
                {
                    BTProgressHUD.Show(ToastMessage.Loading, -1f, ProgressHUD.MaskType.Black);

                    await AppDelegate.InappropriateReportCheckInFactory.BlockPost(AppDelegate.CurrentUser.UserId, checkIn.InappropriateCheckInId);
                    await AppDelegate.CheckInFactory.BlockPost(AppDelegate.CurrentUser.UserId, checkIn.CheckInId);

                    RefreshCheckIns();

                    var PushNotificationHelper = new PushNotificationHelper(AppDelegate.NotificationRegisterFactory, Shared.Helpers.PushNotificationHelper.PushPlatform.iOS);
                    await PushNotificationHelper.BlockedInappropriatePostReporterPush(checkIn);
                    await PushNotificationHelper.BlockedInappropriatePostPosterPush(checkIn);

                    BTProgressHUD.Dismiss();

                }
            }
            catch (Exception ex)
            {
                var a = ex;
                BTProgressHUD.Dismiss();
            }
        }

        /// <summary> 
        /// 
        /// </summary>
        /// <returns></returns>
        public async Task UnBlockInappropriatePost(InappropriateReport checkIn)
        {
            try
            {
                if (AppDelegate.IsOfflineMode())
                {
                    BTProgressHUD.ShowErrorWithStatus(ToastMessage.NoInternet, Helpers.ToastTime.ErrorTime);
                    return;
                }
                else
                {
                    BTProgressHUD.Show(ToastMessage.Loading, -1f, ProgressHUD.MaskType.Black);

                    await AppDelegate.InappropriateReportCheckInFactory.UnBlockPost(checkIn.InappropriateCheckInId);
                    await AppDelegate.CheckInFactory.UnBlockPost(checkIn.CheckInId);

                    RefreshCheckIns();

                    BTProgressHUD.Dismiss();
                }
            }
            catch (Exception ex)
            {
                var a = ex;
                BTProgressHUD.Dismiss();
            }
        }

        /// <summary> 
        /// 
        /// </summary>
        /// <returns></returns>
        public async Task LockUser(ReportedUser reportedUser)
        {
            try
            {
                if (AppDelegate.IsOfflineMode())
                {
                    BTProgressHUD.ShowErrorWithStatus(ToastMessage.NoInternet, Helpers.ToastTime.ErrorTime);
                    return;
                }
                else
                {
                    BTProgressHUD.Show(ToastMessage.Loading, -1f, ProgressHUD.MaskType.Black);

                    await AppDelegate.ReportedUserFactory.LockUser(AppDelegate.CurrentUser.UserId, reportedUser.ReportedUserId);
                    await AppDelegate.UsersFactory.LockUser(reportedUser.SenderUserId);

                    RefreshCheckIns();

                    var PushNotificationHelper = new PushNotificationHelper(AppDelegate.NotificationRegisterFactory, Shared.Helpers.PushNotificationHelper.PushPlatform.iOS);
                    await PushNotificationHelper.BlockedUserReporterPush(reportedUser);
                    await PushNotificationHelper.BlockedUserPosterPush(reportedUser);

                    BTProgressHUD.Dismiss();
                }
            }
            catch (Exception)
            {
                BTProgressHUD.Dismiss();
            }
        }

        /// <summary> 
        /// 
        /// </summary>
        /// <returns></returns>
        public async Task UnLockUser(ReportedUser reportedUser)
        {
            try
            {
                if (AppDelegate.IsOfflineMode())
                {
                    BTProgressHUD.ShowErrorWithStatus(ToastMessage.NoInternet, Helpers.ToastTime.ErrorTime);
                    return;
                }
                else
                {
                    BTProgressHUD.Show(ToastMessage.Loading, -1f, ProgressHUD.MaskType.Black);

                    await AppDelegate.ReportedUserFactory.UnLockUser(reportedUser.ReportedUserId);
                    await AppDelegate.UsersFactory.UnlockUser(reportedUser.SenderUserId);

                    RefreshCheckIns();

                    BTProgressHUD.Dismiss();
                }
            }
            catch (Exception ex)
            {
                var a = ex;
                BTProgressHUD.Dismiss();
            }
        }

        /// <summary> 
        /// 
        /// </summary>
        /// <returns></returns>
        private async Task GetCheckIns()
        {
            try
            {
                if (AppDelegate.IsOfflineMode())
                {
                    BTProgressHUD.ShowErrorWithStatus(ToastMessage.NoInternet, Helpers.ToastTime.ErrorTime);
                    return;
                }
                else
                {
                    BTProgressHUD.Show(ToastMessage.Loading, -1f, ProgressHUD.MaskType.Black);

                    var storageKey = await AppDelegate.UsersFactory.GetStorageConnectionKey();
                    BlobStorageHelper.ConnectionString = storageKey;

                    ICollection<ReportedSpamCheckIn> checkIns;

                    if (this.ControllerCaller == Caller.AllSpam)
                    {
                        checkIns = await AppDelegate.ReportedSpamCheckInFactory.GetAll();
                    } else
                    {
                        checkIns = await AppDelegate.ReportedSpamCheckInFactory.GetTodaysReports();
                    }
                    if (checkIns != null)
                    {
                        this.CheckIns = checkIns.ToList();
                        await GetCheckInLogoUris();
                    }

                    BTProgressHUD.Dismiss();
                }
            }
            catch (Exception ex)
            {
                var a = ex;
                BTProgressHUD.Dismiss();
            }
        }

        /// <summary> 
        /// 
        /// </summary>
        /// <returns></returns>
        private async Task GetReportedUsers()
        {
            try
            {
                if (AppDelegate.IsOfflineMode())
                {
                    BTProgressHUD.ShowErrorWithStatus(ToastMessage.NoInternet, Helpers.ToastTime.ErrorTime);
                    return;
                }
                else
                {
                    BTProgressHUD.Show(ToastMessage.Loading, -1f, ProgressHUD.MaskType.Black);

                    if (BlobStorageHelper.ConnectionString == null)
                    {
                        var storageKey = await AppDelegate.UsersFactory.GetStorageConnectionKey();
                        BlobStorageHelper.ConnectionString = storageKey;
                    }

                    ICollection<ReportedUser> reportedUsers;

                    if (this.ControllerCaller == Caller.AllReportedUser)
                    {
                        reportedUsers = await AppDelegate.ReportedUserFactory.GetAll();
                    }
                    else
                    {
                        reportedUsers = await AppDelegate.ReportedUserFactory.GetTodaysReports();
                    }
                    if (reportedUsers != null)
                    {
                        this.ReportedUsers = reportedUsers.ToList();
                        await GetCheckInLogoUris();
                    }

                    BTProgressHUD.Dismiss();
                }
            }
            catch (Exception ex)
            {
                var a = ex;
                BTProgressHUD.Dismiss();
            }
        }

        /// <summary> 
        /// 
        /// </summary>
        /// <returns></returns>
        private async Task GetInappropriateCheckIns()
        {
            try
            {
                if (AppDelegate.IsOfflineMode())
                {
                    BTProgressHUD.ShowErrorWithStatus(ToastMessage.NoInternet, Helpers.ToastTime.ErrorTime);
                    return;
                }
                else
                {
                    BTProgressHUD.Show(ToastMessage.Loading, -1f, ProgressHUD.MaskType.Black);

                    if (BlobStorageHelper.ConnectionString == null)
                    {
                        var storageKey = await AppDelegate.UsersFactory.GetStorageConnectionKey();
                        BlobStorageHelper.ConnectionString = storageKey;
                    }

                    ICollection<InappropriateReport> checkIns;

                    if (this.ControllerCaller == Caller.AllInappropriateReports)
                    {
                        checkIns = await AppDelegate.InappropriateReportCheckInFactory.GetAll();
                    }
                    else
                    {
                        checkIns = await AppDelegate.InappropriateReportCheckInFactory.GetTodaysReports();
                    }
                    if (checkIns != null)
                    {
                        this.InappropriateReportCheckIns = checkIns.ToList();
                        await GetCheckInLogoUris();
                    }

                    BTProgressHUD.Dismiss();
                }
            }
            catch (Exception ex)
            {
                var a = ex;
                BTProgressHUD.Dismiss();
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public override void DidReceiveMemoryWarning()
        {
            if (this.CheckInsImageViewImage != null)
            {
                foreach (var v in this.CheckInsImageViewImage)
                    v.Image = null;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public async Task GetCheckInLogoUris()
        {
            if (this.ControllerCaller == Caller.DailySpam ||
                this.ControllerCaller == Caller.AllSpam)
            {
                foreach (var b in this.CheckIns)
                {

                    ImageViewImage logo = new ImageViewImage();
                    logo.Id = b.CheckInId;

                    var uriString = await BlobStorageHelper.GetCheckIntLogoUri(b.CheckInId);
                    if (!string.IsNullOrEmpty(uriString))
                    {
                        Uri imageUri = new Uri(uriString);
                        logo.ImageUrl = imageUri;
                        this.CheckInsImageViewImage.Add(logo);
                    }
                    else
                    {

                        var userUriString = await BlobStorageHelper.GetToasterBlobUri(b.CheckInUserId);
                        if (!string.IsNullOrEmpty(userUriString))
                        {
                            Uri imageUri = new Uri(userUriString);
                            logo.ImageUrl = imageUri;
                            this.CheckInsImageViewImage.Add(logo);
                        }
                    }
                }
            }
            await GetInappropriateReportsLogo();
            await GetReportedUsersLogos();
        }

        private async Task GetInappropriateReportsLogo()
        {

            if (this.ControllerCaller == Caller.AllInappropriateReports ||
               this.ControllerCaller == Caller.DailyInappropriateReports)
            {
                foreach (var b in this.InappropriateReportCheckIns)
                {

                    ImageViewImage logo = new ImageViewImage();
                    logo.Id = b.CheckInId;

                    var uriString = await BlobStorageHelper.GetCheckIntLogoUri(b.CheckInId);
                    if (!string.IsNullOrEmpty(uriString))
                    {
                        Uri imageUri = new Uri(uriString);
                        logo.ImageUrl = imageUri;
                        this.CheckInsImageViewImage.Add(logo);
                    }
                    else
                    {

                        var userUriString = await BlobStorageHelper.GetToasterBlobUri(b.CheckInUserId);
                        if (!string.IsNullOrEmpty(userUriString))
                        {
                            Uri imageUri = new Uri(userUriString);
                            logo.ImageUrl = imageUri;
                            this.CheckInsImageViewImage.Add(logo);
                        }
                    }
                }
            }

        }

        private async Task GetReportedUsersLogos()
        {

            if (this.ControllerCaller == Caller.AllReportedUser ||
               this.ControllerCaller == Caller.DailyReportedUsers)
            {
                if (ReportedUsers != null)
                {
                    foreach (var b in this.ReportedUsers)
                    {

                        ImageViewImage itemLogo = new ImageViewImage();
                        var userId = b.SenderUserId;
                        itemLogo.Id = userId;
                        var userUriString = await BlobStorageHelper.GetToasterBlobUri(userId);
                        if (!string.IsNullOrEmpty(userUriString))
                        {
                            Uri imageUri = new Uri(userUriString);
                            itemLogo.ImageUrl = imageUri;
                            this.CheckInsImageViewImage.Add(itemLogo);
                        }
                    }
                }
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        private void RefreshCheckIns()
        {
            try
            {
                if (AppDelegate.IsOfflineMode())
                {
                    BTProgressHUD.ShowErrorWithStatus(ToastMessage.NoInternet, Helpers.ToastTime.ErrorTime);
                }
                else
                {
                    //try
                    //{
                    RefreshControl.BeginRefreshing();

                    BTProgressHUD.Show(ToastMessage.Loading, -1f, ProgressHUD.MaskType.Black);

                    LoadCheckIns();

                    RefreshControl.EndRefreshing();

                    BTProgressHUD.Dismiss();
                }
            }
            catch (Exception)
            {
                BTProgressHUD.Dismiss();
            }

        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public void HandleValueChanged(object sender, EventArgs e)
        {
            try
            {
                RefreshCheckIns();
            }
            catch (Exception)
            {
            }
        }


        #endregion

    }
}