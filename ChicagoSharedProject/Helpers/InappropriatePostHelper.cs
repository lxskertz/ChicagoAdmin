using System;
using System.Collections.Generic;
using System.Text;
using TabsAdmin.Mobile.Shared.Models.Reports.InappropriateReports;

namespace TabsAdmin.Mobile.Shared.Helpers
{
    public class InappropriatePostHelper
    {

        public static string GetReason(InappropriateReport.ReportReason reason)
        {
            string reasonText = " - Report Reason - (";
            switch (reason)
            {
                case InappropriateReport.ReportReason.Abusive:
                    return reasonText + InappropriateReport.Abusive + ")";
                case InappropriateReport.ReportReason.Bullying:
                    return reasonText + InappropriateReport.Bullying + ")";
                case InappropriateReport.ReportReason.Copyright:
                    return reasonText + InappropriateReport.Copyright + ")";
                case InappropriateReport.ReportReason.Drugs:
                    return reasonText + InappropriateReport.Drugs + ")";
                case InappropriateReport.ReportReason.Firearms:
                    return reasonText + InappropriateReport.Firearms + ")";
                case InappropriateReport.ReportReason.Harmful:
                    return reasonText + InappropriateReport.Harmful + ")";
                case InappropriateReport.ReportReason.HateSpeech:
                    return reasonText + InappropriateReport.HateSpeech + ")";
                case InappropriateReport.ReportReason.JustHating:
                    return reasonText + InappropriateReport.JustHating + ")";
                case InappropriateReport.ReportReason.Nudity:
                    return reasonText + InappropriateReport.Nudity + ")";
                case InappropriateReport.ReportReason.Pornography:
                    return reasonText + InappropriateReport.Pornography + ")";
                default:
                    return "";
            }
        }

    }
}
