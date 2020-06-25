using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using TabsAdmin.Mobile.Shared.Models.Reports.Users;

namespace TabsAdmin.Mobile.Shared.Interfaces.Reports.Users
{
    public interface IReportedUserFactory
    {

        Task ReportUser(ReportedUser reportedUser);

        Task LockUser(int blockedByAdminUserId, int reportedUserId);

        Task UnLockUser(int reportedUserId);

        Task<ICollection<ReportedUser>> GetTodaysReports();

        Task<ICollection<ReportedUser>> GetAll();
    }
}
