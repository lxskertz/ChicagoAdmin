using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using TabsAdmin.Mobile.Shared.Models.Reports.Users;
using TabsAdmin.Mobile.Shared.Interfaces.Reports.Users;

namespace TabsAdmin.Mobile.Shared.Managers.Reports.Users
{
    public class ReportedUserFactory : IReportedUserFactory
    {

        #region Constants, Enums, and Variables

        private IReportedUserFactory _ReportedUserFactory;

        #endregion

        #region Constructors

        public ReportedUserFactory(IReportedUserFactory reportedUserFactory)
        {
            _ReportedUserFactory = reportedUserFactory;
        }

        #endregion

        #region Methods

        public Task ReportUser(ReportedUser reportedUser)
        {
            return _ReportedUserFactory.ReportUser(reportedUser);
        }

        public Task LockUser(int blockedByAdminUserId, int reportedUserId)
        {
            return _ReportedUserFactory.LockUser(blockedByAdminUserId, reportedUserId);
        }

        public Task UnLockUser(int reportedUserId)
        {
            return _ReportedUserFactory.UnLockUser(reportedUserId);
        }

        public Task<ICollection<ReportedUser>> GetTodaysReports()
        {
            return _ReportedUserFactory.GetTodaysReports();
        }

        public Task<ICollection<ReportedUser>> GetAll()
        {
            return _ReportedUserFactory.GetAll();
        }

        #endregion

    }
}
