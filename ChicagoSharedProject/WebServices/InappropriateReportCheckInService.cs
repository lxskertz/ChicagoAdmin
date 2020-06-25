using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using TabsAdmin.Mobile.Shared.Models.Reports.InappropriateReports;
using TabsAdmin.Mobile.Shared.Interfaces.Reports.InappropriateReports;

namespace TabsAdmin.Mobile.Shared.WebServices
{
    public class InappropriateReportCheckInService : BaseService, IInappropriateReportCheckInFactory
    {

        #region Methods

        public async Task ReportInappropriate(InappropriateReport inappropriate)
        {
            string methodPath = "inappropriate";
            HttpRequestMessage response = null;
            var parameters = new
            {
                CheckInId = inappropriate.CheckInId,
                CheckInUserId = inappropriate.CheckInUserId,
                CheckInReportReason = inappropriate.CheckInReportReason,
                ReporterUserId = inappropriate.ReporterUserId,
                EventId = inappropriate.EventId,
                BusinessId = inappropriate.BusinessId,
                CheckInDate = inappropriate.CheckInDate,
                CheckInType = inappropriate.CheckInType,
                CheckInDateString = inappropriate.CheckInDate.ToString(),
                BusinessName = inappropriate.BusinessName,
                BlockedByAdmin = inappropriate.BlockedByAdmin,
                BlockedByAdminUserId = inappropriate.BlockedByAdminUserId,
                ReporterFirstName = inappropriate.ReporterFirstName,
                ReporterLastName = inappropriate.ReporterLastName,
                SenderFirstName = inappropriate.SenderFirstName,
                SenderLastName = inappropriate.SenderLastName
            };
            var request = Task.Run(() => response = this.ServiceClient.MakeRequest<HttpRequestMessage>(methodPath, parameters, true, "PUT"));
            response = await request;
        }

        public async Task BlockPost(int blockedByAdminUserId, int inappropriateCheckInId)
        {
            string methodPath = "inappropriate/checkin/block";
            HttpRequestMessage response = null;
            var parameters = new
            {
                UserId = blockedByAdminUserId,
                CheckInId = 0,
                SpamCheckInId = inappropriateCheckInId,
                EventId = 0,
                CheckInType = 0
            };
            var request = Task.Run(() => response = this.ServiceClient.MakeRequest<HttpRequestMessage>(methodPath, parameters, true, "POST"));
            response = await request;
        }

        public async Task UnBlockPost(int inappropriateCheckInId)
        {
            string methodPath = "inappropriate/checkin/unblock/" + inappropriateCheckInId;
            HttpRequestMessage response = null;
            var request = Task.Run(() => response = this.ServiceClient.MakeRequest<HttpRequestMessage>(methodPath, null, true, "GET"));
            response = await request;
        }

        public async Task<ICollection<InappropriateReport>> GetTodaysReports()
        {
            string methodPath = "inappropriate/today";
            ICollection<InappropriateReport> response = null;
            var request = Task.Run(() => response = this.ServiceClient.MakeRequest<ICollection<InappropriateReport>>(methodPath, null, true, "GET"));
            response = await request;

            return response;
        }

        public async Task<ICollection<InappropriateReport>> GetAll()
        {
            string methodPath = "inappropriate";
            ICollection<InappropriateReport> response = null;
            var request = Task.Run(() => response = this.ServiceClient.MakeRequest<ICollection<InappropriateReport>>(methodPath, null, true, "GET"));
            response = await request;

            return response;
        }

        #endregion

    }
}
