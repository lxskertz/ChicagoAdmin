using System;
using System.Collections.Generic;

namespace TabsAdmin.Mobile.Shared.Helpers
{
    public static class MoreScreenHelper
    {

        #region Constants, Enums, and Variables

        public const string AllCheckinReports = "All Inappropriate Check-In Reposts";
        public const string DailyCheckinReports = "Daily Inappropriate Check-In Reposts";
        public const string UsersReports = "All Reported Users";
        public const string DailyUserReports = "Daily Reported Users";
        public const string LockUnLockUsers = "Lock/Unlock Users";
        public const string AllUsers = "Search All Users";
        public const string Logout = "Logout";
        public const string AllSpamReports = "All Spam Check-Ins Reports";
        public const string DailySpamReports = "Daily Spam Check-Ins Reports";

        #endregion

        #region Methods

        public static List<string> TableRows()
        {
            List<string> rows = new List<string>();
            rows.Add(AllCheckinReports);
            rows.Add(DailyCheckinReports);
            rows.Add(AllSpamReports);
            rows.Add(DailySpamReports);
            rows.Add(UsersReports);
            rows.Add(DailyUserReports);
            rows.Add(LockUnLockUsers);
            rows.Add(AllUsers);
            rows.Add(Logout);

            return rows;
        }

        #endregion

    }
}
