using System.Collections.Generic;
using System.Threading.Tasks;
using TabsAdmin.Mobile.Shared.Models;
using TabsAdmin.Mobile.Shared.Managers;
using TabsAdmin.Mobile.Shared.Resources;
using TabsAdmin.Mobile.Shared.Models.Reports.InappropriateReports;
using TabsAdmin.Mobile.Shared.Models.Reports.Spams;
using TabsAdmin.Mobile.Shared.Models.Reports.Users;

namespace TabsAdmin.Mobile.Shared.Helpers
{
    public class PushNotificationHelper
    {
        public enum PushPlatform
        {
            Android,
            iOS
        }

        private NotificationRegisterFactory NotificationRegisterFactory { get; set; }

        public PushPlatform SelectedPushPlatform { get; set; }

        public PushNotificationHelper(NotificationRegisterFactory notificationRegisterFactory, PushPlatform selectedPushPlatform)
        {
            this.NotificationRegisterFactory = notificationRegisterFactory;
            this.SelectedPushPlatform = selectedPushPlatform;
        }

        public async Task BlockedInappropriatePostReporterPush(InappropriateReport checkin)
        {
            var query = new NotificationQuery();
            query.PNS = this.SelectedPushPlatform == PushPlatform.Android ? DeviceRegistration.Fcm : DeviceRegistration.Apns;
            query.Message = "We have determined that " + checkin.SenderFirstName + " " + checkin.SenderLastName + "'s check-in you reported violates our Terms of Use Guidelines. We have blocked the post on our platform. Thank you. ";
            query.Tags = new List<string>();
            query.Tags.Add(NotificationTag.Toaster + checkin.ReporterUserId);
            var a = await NotificationRegisterFactory.SendPush(query);
        }

        public async Task BlockedInappropriatePostPosterPush(InappropriateReport checkin)
        {
            var query = new NotificationQuery();
            query.PNS = this.SelectedPushPlatform == PushPlatform.Android ? DeviceRegistration.Fcm : DeviceRegistration.Apns;
            var checkInDate = checkin.CheckInDate.HasValue ? checkin.CheckInDate.Value.ToShortDateString() : "";
            query.Message = "We have determined that your check-in posted on " + checkInDate + " violates our Terms of Use Guidelines. This check-in has been removed from our platform. ";
            query.Tags = new List<string>();
            query.Tags.Add(NotificationTag.Toaster + checkin.CheckInUserId);
            var a = await NotificationRegisterFactory.SendPush(query);
        }

        public async Task BlockedSpamPostReporterPush(ReportedSpamCheckIn checkin)
        {
            var query = new NotificationQuery();
            query.PNS = this.SelectedPushPlatform == PushPlatform.Android ? DeviceRegistration.Fcm : DeviceRegistration.Apns;
            query.Message = "We have determined that " + checkin.SenderFirstName + " " + checkin.SenderLastName + "'s check-in you reported violates our Terms of Use Guidelines. We have blocked the post on our platform. Thank you. ";
            query.Tags = new List<string>();
            query.Tags.Add(NotificationTag.Toaster + checkin.ReporterUserId);
            var a = await NotificationRegisterFactory.SendPush(query);
        }

        public async Task BlockedSpamPostPosterPush(ReportedSpamCheckIn checkin)
        {
            var query = new NotificationQuery();
            query.PNS = this.SelectedPushPlatform == PushPlatform.Android ? DeviceRegistration.Fcm : DeviceRegistration.Apns;
            var checkInDate = checkin.CheckInDate.HasValue ? checkin.CheckInDate.Value.ToShortDateString() : "";
            query.Message = "We have determined that your check-in posted on " + checkInDate + " violates our Terms of Use Guidelines. This check-in has been removed from our platform. ";
            query.Tags = new List<string>();
            query.Tags.Add(NotificationTag.Toaster + checkin.CheckInUserId);
            var a = await NotificationRegisterFactory.SendPush(query);
        }

        public async Task BlockedUserReporterPush(ReportedUser reportedUser)
        {
            var query = new NotificationQuery();
            query.PNS = this.SelectedPushPlatform == PushPlatform.Android ? DeviceRegistration.Fcm : DeviceRegistration.Apns;
            query.Message = "We have determined that " + reportedUser.SenderFirstName + " " + reportedUser.SenderLastName + "'s account you reported violates our Terms of Use Guidelines. We have locked this out of our platform. Thank you. ";
            query.Tags = new List<string>();
            query.Tags.Add(NotificationTag.Toaster + reportedUser.ReporterUserId);
            var a = await NotificationRegisterFactory.SendPush(query);
        }

        public async Task BlockedUserPosterPush(ReportedUser reportedUser)
        {
            var query = new NotificationQuery();
            query.PNS = this.SelectedPushPlatform == PushPlatform.Android ? DeviceRegistration.Fcm : DeviceRegistration.Apns;
            query.Message = "We have determined that your account violates our Terms of Use Guidelines. Your account has been locked, you can contact tabs customer service for more information. Thank you. ";
            query.Tags = new List<string>();
            query.Tags.Add(NotificationTag.Toaster + reportedUser.SenderUserId);
            var a = await NotificationRegisterFactory.SendPush(query);
        }

    }
}
