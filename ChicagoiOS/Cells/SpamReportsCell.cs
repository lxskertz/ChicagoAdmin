using Foundation;
using System;
using System.Linq;
using System.Collections.Generic;
using UIKit;
using BigTed;
using TabsAdmin.Mobile.Shared.Models.Reports.Spams;
using TabsAdmin.Mobile.Shared.Models.Reports.InappropriateReports;
using TabsAdmin.Mobile.Shared.Resources;
using TabsAdmin.Mobile.ChicagoiOS.DataSource.Reports.Spams;
using TabsAdmin.Mobile.ChicagoiOS.DataSource.Reports.Inappropriates;
using TabsAdmin.Mobile.ChicagoiOS.DataSource.Reports.Users;
using TabsAdmin.Mobile.Shared.Models.Reports.Users;

namespace TabsAdmin.Mobile.ChicagoiOS
{
    public partial class SpamReportsCell : UITableViewCell
    {

        /// <summary>
        /// 
        /// </summary>
        public UILabel _ReporterName
        {
            get
            {
                return ReporterName;
            }
        }

        public UILabel _PosterName
        {
            get
            {
                return PosterName;
            }
        }

        public UILabel _CheckInDate
        {
            get
            {
                return CheckInDate;
            }
        }


        public UIImageView _CheckInImage
        {
            get
            {
                return CheckInImage;
            }
        }

        public UIButton _Blockbtn
        {
            get
            {
                return Blockbtn;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public SpamReportsDatasource DataSource { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public InappropriateReportsDatasource InappropriateReportsDatasource { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public ReportedUserDatasource ReportedUserDatasource { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public NSIndexPath IndexPath { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public ReportedSpamCheckIn Item { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public InappropriateReport InappropriateReport { get; set; }

        public ReportedUser ReportedUser { get; set; }

        public SpamReportsCell (IntPtr handle) : base (handle)
        {
        }

        async partial void Blockbtn_TouchUpInside(UIButton sender)
        {
            if (this.DataSource != null)
            {
                if (!Item.BlockedByAdmin)
                {
                    await this.DataSource.Controller.BlockSpamPost(Item);
                }
                else
                {
                    await this.DataSource.Controller.UnBlockSpamPost(Item);
                }
            }

            if (this.InappropriateReportsDatasource != null)
            {
                if (!InappropriateReport.BlockedByAdmin)
                {
                    await this.InappropriateReportsDatasource.Controller.BlockInappropriatePost(InappropriateReport);
                }
                else
                {
                    await this.InappropriateReportsDatasource.Controller.UnBlockInappropriatePost(InappropriateReport);
                }
            }

            if (this.ReportedUserDatasource != null)
            {
                if (!ReportedUser.BlockedByAdmin)
                {
                    await this.ReportedUserDatasource.Controller.LockUser(ReportedUser);
                }
                else
                {
                    await this.ReportedUserDatasource.Controller.UnLockUser(ReportedUser);
                }
            }
        }
    }
}