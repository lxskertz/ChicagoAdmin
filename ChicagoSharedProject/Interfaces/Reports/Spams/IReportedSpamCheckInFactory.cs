using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using TabsAdmin.Mobile.Shared.Models.Reports.Spams;

namespace TabsAdmin.Mobile.Shared.Interfaces.Reports.Spams
{
    public interface IReportedSpamCheckInFactory
    {
        Task ReportSpam(ReportedSpamCheckIn spamCheckIn);

        Task<ICollection<ReportedSpamCheckIn>> GetTodaysReports();

        Task<ICollection<ReportedSpamCheckIn>> GetAll();

        Task BlockPost(int blockedByAdminUserId, int spamCheckInId);

        Task UnBlockPost(int spamCheckInId);

    }
}
