using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using TabsAdmin.Mobile.Shared.Models.Reports.InappropriateReports;
using TabsAdmin.Mobile.Shared.Interfaces.Reports.InappropriateReports;

namespace TabsAdmin.Mobile.Shared.Managers.Reports.InappropriateReports
{
    public class InappropriateReportCheckInFactory : IInappropriateReportCheckInFactory
    {
        #region Constants, Enums, and Variables

        private IInappropriateReportCheckInFactory _InappropriateReportCheckInFactory;

        #endregion

        #region Constructors

        public InappropriateReportCheckInFactory(IInappropriateReportCheckInFactory inappropriateReportCheckInFactory)
        {
            _InappropriateReportCheckInFactory = inappropriateReportCheckInFactory;
        }

        #endregion

        #region Methods

        public Task ReportInappropriate(InappropriateReport inappropriate)
        {
            return _InappropriateReportCheckInFactory.ReportInappropriate(inappropriate);
        }

        public Task BlockPost(int blockedByAdminUserId, int inappropriateCheckInId)
        {
            return _InappropriateReportCheckInFactory.BlockPost(blockedByAdminUserId, inappropriateCheckInId);
        }

        public Task UnBlockPost(int inappropriateCheckInId)
        {
            return _InappropriateReportCheckInFactory.UnBlockPost(inappropriateCheckInId);
        }

        public Task<ICollection<InappropriateReport>> GetTodaysReports()
        {
            return _InappropriateReportCheckInFactory.GetTodaysReports();
        }

        public Task<ICollection<InappropriateReport>> GetAll()
        {
            return _InappropriateReportCheckInFactory.GetAll();
        }

        #endregion

    }
}
