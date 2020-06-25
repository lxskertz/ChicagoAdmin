using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using TabsAdmin.Mobile.Shared.Interfaces.Reports.Spams;
using TabsAdmin.Mobile.Shared.Models.Reports.Spams;

namespace TabsAdmin.Mobile.Shared.WebServices
{
    public class ReportedSpamCheckInService : BaseService, IReportedSpamCheckInFactory
    {

        #region Methods

        public async Task ReportSpam(ReportedSpamCheckIn spamCheckIn)
        {
            string methodPath = "spam";
            HttpRequestMessage response = null;
            var parameters = new
            {
                CheckInId = spamCheckIn.CheckInId,
                CheckInUserId = spamCheckIn.CheckInUserId,
                ReporterUserId = spamCheckIn.ReporterUserId,
                EventId = spamCheckIn.EventId,
                BusinessId = spamCheckIn.BusinessId,
                CheckInDate = spamCheckIn.CheckInDate,
                CheckInType = spamCheckIn.CheckInType,
                CheckInDateString = spamCheckIn.CheckInDate.ToString(),
                BusinessName = spamCheckIn.BusinessName,
                BlockedByAdmin = spamCheckIn.BlockedByAdmin,
                BlockedByAdminUserId = spamCheckIn.BlockedByAdminUserId,
                ReporterFirstName = spamCheckIn.ReporterFirstName,
                ReporterLastName = spamCheckIn.ReporterLastName,
                SenderFirstName = spamCheckIn.SenderFirstName,
                SenderLastName = spamCheckIn.SenderLastName
            };
            var request = Task.Run(() => response = this.ServiceClient.MakeRequest<HttpRequestMessage>(methodPath, parameters, true, "PUT"));
            response = await request;
        }

        public async Task<ICollection<ReportedSpamCheckIn>> GetTodaysReports()
        {
            string methodPath = "spam/today";
            ICollection<ReportedSpamCheckIn> response = null;
            var request = Task.Run(() => response = this.ServiceClient.MakeRequest<ICollection<ReportedSpamCheckIn>>(methodPath, null, true, "GET"));
            response = await request;

            return response;
        }

        public async Task<ICollection<ReportedSpamCheckIn>> GetAll()
        {
            string methodPath = "spam";
            ICollection<ReportedSpamCheckIn> response = null;
            var request = Task.Run(() => response = this.ServiceClient.MakeRequest<ICollection<ReportedSpamCheckIn>>(methodPath, null, true, "GET"));
            response = await request;

            return response;
        }

        public async Task BlockPost(int blockedByAdminUserId, int spamCheckInId)
        {
            string methodPath = "spam/checkin/block";
            HttpRequestMessage response = null;
            var parameters = new
            {
                UserId = blockedByAdminUserId,
                CheckInId = 0,
                SpamCheckInId = spamCheckInId,
                EventId = 0,
                CheckInType = 0
            };
            var request = Task.Run(() => response = this.ServiceClient.MakeRequest<HttpRequestMessage>(methodPath, parameters, true, "POST"));
            response = await request;
        }

        public async Task UnBlockPost(int spamCheckInId)
        {
            string methodPath = "spam/checkin/unblock/" + spamCheckInId;
            HttpRequestMessage response = null;
            var request = Task.Run(() => response = this.ServiceClient.MakeRequest<HttpRequestMessage>(methodPath, null, true, "GET"));
            response = await request;
        }

        #endregion

    }
}
