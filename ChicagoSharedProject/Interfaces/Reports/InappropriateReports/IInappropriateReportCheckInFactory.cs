using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using TabsAdmin.Mobile.Shared.Models.Reports.InappropriateReports;

namespace TabsAdmin.Mobile.Shared.Interfaces.Reports.InappropriateReports
{
    public interface IInappropriateReportCheckInFactory
    {
        Task ReportInappropriate(InappropriateReport inappropriate);

        Task BlockPost(int blockedByAdminUserId, int inappropriateCheckInId);

        Task UnBlockPost(int inappropriateCheckInId);

        Task<ICollection<InappropriateReport>> GetTodaysReports();

        Task<ICollection<InappropriateReport>> GetAll();

    }
}
