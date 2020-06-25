using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Net.Http;
using Foundation;
using UIKit;
using UserNotifications;
using Newtonsoft.Json;
using BigTed;
using WindowsAzure.Messaging;
using TabsAdmin.Mobile.Shared.Models.Users;
using TabsAdmin.Mobile.Shared.Models;
using Microsoft.AppCenter;
using Microsoft.AppCenter.Analytics;
using Microsoft.AppCenter.Crashes;

namespace TabsAdmin.Mobile.ChicagoiOS
{
    // The UIApplicationDelegate for the application. This class is responsible for launching the
    // User Interface of the application, as well as listening (and optionally responding) to application events from iOS.
    [Register("AppDelegate")]
    public class AppDelegate : UIApplicationDelegate
    {

        #region Constants, Enums, and Variables

        public const string environment = "Environment";
        public const string currentUserPref = "CurrentUser";
        public static string City = "Chicago";
        public static string ZipCode = "60616";
        public static Plugin.Geolocator.Abstractions.Address CurrentAddy;

        private static List<string> tags = new List<string>();
        private static DeviceRegistration deviceRegistration = new DeviceRegistration();

        #endregion

        #region Properties

        public override UIWindow Window { get; set; }

        public static NavController NavController { get; private set; }

		public static RootViewController RootViewController { get; set; }

        public static bool AppInBackground { get; set; }

        public static string ApiId { get; set; }

        public static string ApiKey { get; set; }

        public static Users CurrentUser { get; set; } = new Users();

        public static UIStoryboard MyStoryBoard { get; set; }

        public static Shared.Managers.Businesses.BusinessFactory BusinessFactory { get; set; }

        public static Shared.Managers.Individuals.IndividualFactory IndividualFactory { get; set; }

        public static Shared.Managers.Users.UsersFactory UsersFactory { get; set; }

        public static Shared.Managers.VerificationCodeFactory VerificationCodeFactory { get; set; }

        public static Shared.Managers.Individuals.ToastersFactory ToastersFactory { get; set; }

        public static Shared.Managers.Businesses.AddressFactory AddressFactory { get; set; }

        public static Shared.Managers.Businesses.BusinessTypesFactory BusinessTypesFactory { get; set; }

        public static Shared.Managers.Businesses.BusinessPhotoFactory BusinessPhotoFactory { get; set; }

        public static Shared.Managers.Individuals.ToasterPhotoFactory ToasterPhotoFactory { get; set; }

        public static Shared.Managers.Individuals.SMSMessageFactory SMSMessageFactory { get; set; }

        public static Shared.Managers.Reports.Spams.ReportedSpamCheckInFactory ReportedSpamCheckInFactory { get; set; }

        public static Shared.Managers.NotificationRegisterFactory NotificationRegisterFactory { get; set; }

        public static Shared.Managers.CheckIns.CheckInFactory CheckInFactory { get; set; }

        public static Shared.Managers.Reports.InappropriateReports.InappropriateReportCheckInFactory InappropriateReportCheckInFactory { get; set; }

        public static Shared.Managers.Reports.Users.ReportedUserFactory ReportedUserFactory { get; set; }

        #endregion

        /// <summary>
        /// 
        /// </summary>
        /// <param name="application"></param>
        /// <param name="launchOptions"></param>
        /// <returns></returns>
        public override bool FinishedLaunching(UIApplication application, NSDictionary launchOptions)
        {

            MyStoryBoard = UIStoryboard.FromName("Main", null);
            RootViewController = MyStoryBoard.InstantiateViewController("RootViewController") as RootViewController;

            //NavController = new NavController(RootViewController);
            Window.RootViewController = new UINavigationController(RootViewController);
            Window.MakeKeyAndVisible();

            Shared.MyEnvironment.Environment = Shared.MyEnvironment.Environment == null ? GetEnvironment() : Shared.MyEnvironment.Environment;
            CurrentUser = GetCurrentUser();
            ApiId = CurrentUser != null ? CurrentUser.Identifier : string.Empty;
            ApiKey = CurrentUser != null ? CurrentUser.UserMobileSessionId : string.Empty;

            AppCenter.Start("98ae5104-6a83-40c5-a626-44454727c1a8",
                   typeof(Analytics), typeof(Crashes));

            return true;
        }

        /// <summary>
		/// Figure out if Device is connected to a network
		/// </summary>
		public static NetworkStatus DetermineNetworkConnection()
        {
            return Reachability.InternetConnectionStatus();
        }

        /// <summary>
        /// Determines if network is currently offline
        /// </summary>
        /// <returns><c>true</c> if is offline mode; otherwise, <c>false</c>.</returns>
        public static bool IsOfflineMode()
        {
            return AppDelegate.DetermineNetworkConnection() == NetworkStatus.NotReachable;
        }
   
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public static string GetEnvironment()
        {
            return NSUserDefaults.StandardUserDefaults.StringForKey(environment);
        }

        /// <summary>
        /// Delete current user
        /// </summary>
        public static void DeleteEnvironment()
        {
            NSUserDefaults.StandardUserDefaults.SetString(string.Empty, environment);
            NSUserDefaults.StandardUserDefaults.Init();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="Environment"></param>
		public static void SaveEnvironment(string environmentvalue)
        {
            NSUserDefaults.StandardUserDefaults.SetString(environmentvalue, environment);
            NSUserDefaults.StandardUserDefaults.Init();
        }

        /// <summary>
        /// Get current user
        /// </summary>
        /// <returns></returns>
        public static Users GetCurrentUser()
        {
            string userJson = NSUserDefaults.StandardUserDefaults.StringForKey(currentUserPref);
            if (string.IsNullOrEmpty(userJson))
            {
                return null;
            }
            return JsonConvert.DeserializeObject<Users>(NSUserDefaults.StandardUserDefaults.StringForKey(currentUserPref));
        }

        /// <summary>
        /// Delete current user
        /// </summary>
        public static void DeleteCurrentUser()
        {
            NSUserDefaults.StandardUserDefaults.SetString(string.Empty, currentUserPref);
            NSUserDefaults.StandardUserDefaults.Init();
        }

        /// <summary>
        /// 
        /// </summary>
        public static void DeleteSettings()
        {
            DeleteCurrentUser();
            DeleteEnvironment();
        }


        /// <summary>
        /// Save current user
        /// </summary>
        /// <param name="roleId"></param>
        /// <param name="cuid"></param>
        public static void SaveCurrentUser(Users user)
        {
            string userJson = JsonConvert.SerializeObject(user);
            NSUserDefaults.StandardUserDefaults.SetString(userJson, currentUserPref);
            NSUserDefaults.StandardUserDefaults.Init();
        }


        public override void OnResignActivation(UIApplication application)
        {
            // Invoked when the application is about to move from active to inactive state.
            // This can occur for certain types of temporary interruptions (such as an incoming phone call or SMS message)
            // or when the user quits the application and it begins the transition to the background state.
            // Games should use this method to pause the game.
        }

        public override void DidEnterBackground(UIApplication application)
        {
            AppInBackground = true;
        }

        public override void WillEnterForeground(UIApplication application)
        {
            AppInBackground = false;
        }

        public override void OnActivated(UIApplication application)
        {
            UIApplication.SharedApplication.ApplicationIconBadgeNumber = 0;
        }

        public override void WillTerminate(UIApplication application)
        {
            // Called when the application is about to terminate. Save data, if needed. See also DidEnterBackground.
        }
    }
}

