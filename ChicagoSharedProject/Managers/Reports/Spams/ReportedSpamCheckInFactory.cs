using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using TabsAdmin.Mobile.Shared.Interfaces.Reports.Spams;
using TabsAdmin.Mobile.Shared.Models.Reports.Spams;

namespace TabsAdmin.Mobile.Shared.Managers.Reports.Spams
{
    public class ReportedSpamCheckInFactory : IReportedSpamCheckInFactory
    {

        #region Constants, Enums, and Variables

        private IReportedSpamCheckInFactory _ReportedSpamCheckInFactory;

        #endregion

        #region Constructors

        public ReportedSpamCheckInFactory(IReportedSpamCheckInFactory reportedSpamCheckInFactory)
        {
            _ReportedSpamCheckInFactory = reportedSpamCheckInFactory;
        }

        #endregion

        #region Methods

        public Task ReportSpam(ReportedSpamCheckIn spamCheckIn)
        {
            return _ReportedSpamCheckInFactory.ReportSpam(spamCheckIn);
        }

        public Task<ICollection<ReportedSpamCheckIn>> GetTodaysReports()
        {
            return _ReportedSpamCheckInFactory.GetTodaysReports();
        }

        public Task<ICollection<ReportedSpamCheckIn>> GetAll()
        {
            return _ReportedSpamCheckInFactory.GetAll();
        }

        public Task BlockPost(int blockedByAdminUserId, int spamCheckInId)
        {
            return _ReportedSpamCheckInFactory.BlockPost(blockedByAdminUserId, spamCheckInId);
        }

        public Task UnBlockPost(int spamCheckInId)
        {
            return _ReportedSpamCheckInFactory.UnBlockPost(spamCheckInId);
        }

        #endregion

    }
}
