using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using TabsAdmin.Mobile.Shared.Models.Reports.Users;
using TabsAdmin.Mobile.Shared.Interfaces.Reports.Users;

namespace TabsAdmin.Mobile.Shared.WebServices
{
    public class ReportedUserService : BaseService, IReportedUserFactory
    {

        #region Methods

        public async Task ReportUser(ReportedUser reportedUser)
        {
            string methodPath = "report_user";
            HttpRequestMessage response = null;
            var parameters = new
            {
                ReporterUserId = reportedUser.ReporterUserId,
                ReportDate = reportedUser.ReportDate,
                ReportDateString = DateTime.UtcNow.ToString(),
                BlockedByAdmin = reportedUser.BlockedByAdmin,
                BlockedByAdminUserId = reportedUser.BlockedByAdminUserId,
                ReporterFirstName = reportedUser.ReporterFirstName,
                ReporterLastName = reportedUser.ReporterLastName,
                SenderFirstName = reportedUser.SenderFirstName,
                SenderLastName = reportedUser.SenderLastName,
                SenderUserId = reportedUser.SenderUserId
            };
            var request = Task.Run(() => response = this.ServiceClient.MakeRequest<HttpRequestMessage>(methodPath, parameters, true, "PUT"));
            response = await request;
        }

        public async Task LockUser(int blockedByAdminUserId, int reportedUserId)
        {
            string methodPath = "report_user/lock";
            HttpRequestMessage response = null;
            var parameters = new
            {
                UserId = blockedByAdminUserId,
                ReportedUserId = reportedUserId,
            };
            var request = Task.Run(() => response = this.ServiceClient.MakeRequest<HttpRequestMessage>(methodPath, parameters, true, "POST"));
            response = await request;
        }

        public async Task UnLockUser(int reportedUserId)
        {
            string methodPath = "report_user/unlock/" + reportedUserId;
            HttpRequestMessage response = null;
            var request = Task.Run(() => response = this.ServiceClient.MakeRequest<HttpRequestMessage>(methodPath, null, true, "GET"));
            response = await request;
        }

        public async Task<ICollection<ReportedUser>> GetTodaysReports()
        {
            string methodPath = "report_user/today";
            ICollection<ReportedUser> response = null;
            var request = Task.Run(() => response = this.ServiceClient.MakeRequest<ICollection<ReportedUser>>(methodPath, null, true, "GET"));
            response = await request;

            return response;
        }

        public async Task<ICollection<ReportedUser>> GetAll()
        {
            string methodPath = "report_user";
            ICollection<ReportedUser> response = null;
            var request = Task.Run(() => response = this.ServiceClient.MakeRequest<ICollection<ReportedUser>>(methodPath, null, true, "GET"));
            response = await request;

            return response;
        }

        #endregion

    }
}
