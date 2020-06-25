using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Linq;
using System.Net;
using System.Net.Http;
using Android.OS;
using Android.App;
using Android.Preferences;
using Android.Views;
using Android.Widget;
using Android.Support.V7.App;
using Android.Graphics.Drawables;
using Android.Graphics;
using Android.Support.V4.Content;
using Android.Support.Design.Widget;
using Android.Net;
using Android.Content;
using Android.Telephony;
using Android.Content.PM;
using Android.Runtime;
using Android.Support.V7.Widget;
using V7Toolbar = Android.Support.V7.Widget.Toolbar;
using Newtonsoft.Json;
using TabsAdmin.Mobile.Shared.Models.Users;
using TabsAdmin.Mobile.Shared.Helpers;
using TabsAdmin.Mobile.Shared.Models;
using TabsAdmin.Mobile.Shared.Models.Events;
using Android.Util;
using Android.Gms.Common;

namespace TabsAdmin.Mobile.ChicagoAndroid.Activities
{
    [Activity(Label = "BaseActivity", Theme = "@style/AppTheme", ConfigurationChanges = ConfigChanges.Orientation | ConfigChanges.ScreenSize)]
    public class BaseActivity : AppCompatActivity
    {
        #region Constants, Enums and Variables


        public const string environment = "ENVIRONMENT";
        public const string NotificationRegId = "NotificationRegId";
        public const string PnsHandle = "PnsHandle";
        public const string PnsHandleRefreshed = "PnsHandleRefreshed";
        public const string currentUserPref = "CurrentUser";

        public const string TAG = "MainActivity";
        //internal static readonly string CHANNEL_ID = "my_notification_channel";

        // Android.Manifest.Permission.SendSms,
        public readonly string[] RequiredPermissions = new[] {
           Android.Manifest.Permission.ReadExternalStorage,
           Android.Manifest.Permission.WriteExternalStorage,
           Android.Manifest.Permission.AccessCoarseLocation,
           Android.Manifest.Permission.AccessFineLocation,
           Android.Manifest.Permission.ReadContacts
        };
        public bool waitingForPermission = false;

        public List<string> permissionsToRequest = new List<string>();

        private static BaseActivity instance = new BaseActivity();
        private List<string> tags = new List<string>();
        private DeviceRegistration deviceRegistration = new DeviceRegistration();

        #endregion

        #region Properties

        /// <summary>
        /// 
        /// </summary>
        private ProgressBar ProgressBar { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public static string ApiId { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public static string ApiKey { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public Users CurrentUser { get; set; } = new Users();

        /// <summary>
        /// 
        /// </summary>
        private RelativeLayout ProgressBarRelativeLayout { get; set; }

        public static BaseActivity CurrentActivity
        {
            get
            {
                return instance;
            }
        }

        public MyPreferences MyPreferences { get; set; }

        #endregion

        #region Methods

        /// <summary>
        /// 
        /// </summary>
        /// <param name="savedInstanceState"></param>
        protected override void OnCreate(Bundle savedInstanceState)
        {
            try
            {
                base.OnCreate(savedInstanceState);
                instance = this;
                //this.CurrentUser = new Users();

                this.MyPreferences = new MyPreferences(this);

                //change actionbar bar color
                var actionBarColorInt = ContextCompat.GetColor(this, Resource.Color.colorPrimary);
                var actionBarColor = new Color(actionBarColorInt);
                this.SupportActionBar.SetBackgroundDrawable(new ColorDrawable(actionBarColor));
                var statusBarColorInt = ContextCompat.GetColor(this, Resource.Color.colorPrimary); //TODO:Change
                var statusBarColor = new Color(statusBarColorInt);
                this.CurrentUser = GetCurrentUser();
                ApiId = CurrentUser != null ? CurrentUser.Identifier : string.Empty;
                ApiKey = CurrentUser != null ? CurrentUser.UserMobileSessionId : string.Empty;
                Shared.MyEnvironment.Environment = this.MyPreferences.GetEnvironment();
            }
            catch (Exception)
            {
            }

        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="elementLayout"></param>
        /// <param name="element"></param>
        /// <param name="error"></param>
        /// <returns></returns>
        public bool ValidateInput(TextInputLayout elementLayout, TextInputEditText element, string error)
        {
            if (string.IsNullOrEmpty(element.Text))
            {
                elementLayout.ErrorEnabled = true;
                elementLayout.Error = error;
                return false;
            }
            else
            {
                elementLayout.ErrorEnabled = false;
                elementLayout.Error = "";
            }

            return true;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="elementLayout"></param>
        /// <param name="element"></param>
        /// <param name="error"></param>
        /// <returns></returns>
        public bool ValidateInput(TextInputLayout elementLayout, AppCompatEditText element, string error)
        {
            if (string.IsNullOrEmpty(element.Text))
            {
                elementLayout.ErrorEnabled = true;
                elementLayout.Error = error;
                return false;
            }
            else
            {
                elementLayout.ErrorEnabled = false;
                elementLayout.Error = "";
            }

            return true;
        }
            
        /// <summary>
        /// 
        /// </summary>
        /// <param name="show"></param>
        /// <param name="title"></param>
        /// <param name="message"></param>
        public void ShowProgressbar(bool show, string title, string message)
        {
            this.RunOnUiThread(() => {
                //if ((int)Build.VERSION.SdkInt < (int)BuildVersionCodes.O)
                //{

                //    if (show)
                //    {

                //        if (this.ProgressDialog != null && this.ProgressDialog.IsShowing)
                //        {

                //            this.ProgressDialog.Hide();

                //            this.ProgressDialog.Dismiss();

                //        }

                //        this.ProgressDialog = ProgressDialog.Show(this, title, message, true);

                //    }
                //    else
                //    {

                //        if (this.ProgressDialog != null)
                //        {

                //            this.ProgressDialog.Hide();

                //            this.ProgressDialog.Dismiss();

                //        }

                //    }

                //}
                //else
                //{
                    if (show)
                    {
                        if (this.ProgressBar != null && this.ProgressBar.Visibility == ViewStates.Visible)
                        {
                            this.ProgressBar.Visibility = ViewStates.Gone;
                        }
                        Window.SetFlags(WindowManagerFlags.NotTouchable, WindowManagerFlags.NotTouchable);
                        this.ProgressBar = new ProgressBar(this, null, Android.Resource.Attribute.ProgressBarStyleLarge);
                        ViewGroup layout = (ViewGroup)FindViewById(Android.Resource.Id.Content).RootView;
                        this.ProgressBar.Indeterminate = true;
                        RelativeLayout.LayoutParams p = new RelativeLayout.LayoutParams(RelativeLayout.LayoutParams.MatchParent, RelativeLayout.LayoutParams.MatchParent);
                        ProgressBarRelativeLayout = new RelativeLayout(this);
                        ProgressBarRelativeLayout.SetGravity(GravityFlags.Center);
                        LinearLayout.LayoutParams lp = new LinearLayout.LayoutParams(LinearLayout.LayoutParams.MatchParent, LinearLayout.LayoutParams.WrapContent);
                        LinearLayout ll = new LinearLayout(this);
                        ll.SetBackgroundColor(Shared.Resources.Color.BackgroundColor);
                        ll.AddView(this.ProgressBar);
                        ProgressBarRelativeLayout.AddView(ll);
                        layout.AddView(ProgressBarRelativeLayout, p);
                        this.ProgressBar.Visibility = ViewStates.Visible;
                    }
                    else
                    {
                        if (this.ProgressBar != null)
                        {
                            if (ProgressBarRelativeLayout != null)
                            {
                                ProgressBarRelativeLayout.Visibility = ViewStates.Gone;
                            }
                            this.ProgressBar.Visibility = ViewStates.Gone;
                            Window.ClearFlags(WindowManagerFlags.NotTouchable);
                        }
                    }
                //}
            });
        }      

        /// <summary>
        /// Show snack
        /// </summary>
        /// <param name="view"></param>
        /// <param name="text"></param>
        /// <param name="actionText"></param>
        public void ShowSnack(View view, string text, string actionText, int duration = Snackbar.LengthShort)
        {
            Snackbar.Make(view, text, duration).SetAction(actionText, (v) => { }).Show();
        }
      
        /// <summary>
        /// Request runtime permission callback
        /// </summary>
        /// <param name="requestCode"></param>
        /// <param name="permissions"></param>
        /// <param name="grantResults"></param>
        public override void OnRequestPermissionsResult(int requestCode, string[] permissions, [GeneratedEnum] Permission[] grantResults)
        {
            Xamarin.Essentials.Platform.OnRequestPermissionsResult(requestCode, permissions, grantResults);

            base.OnRequestPermissionsResult(requestCode, permissions, grantResults);

            if (waitingForPermission)
            {
                waitingForPermission = false;
                for (int i = 0; i < permissions.Length; i++)
                {
                }
            }
        }

        /// <summary>
        /// Check if device is connected to a network
        /// </summary>
        /// <returns></returns>
        public NetworkInfo CheckNetworkConnectivity()
        {
            var connectivityManager = (ConnectivityManager)GetSystemService(ConnectivityService);
            return connectivityManager.ActiveNetworkInfo;
        }

        /// <summary>
        /// Check if app runtime permissions
        /// </summary>
        public void CheckPermission()
        {
            permissionsToRequest = new List<string>();
            foreach (var permission in this.RequiredPermissions)
            {
                if (Helpers.PlatformChecks.IsPermissionInManifest(this, permission))
                {
                    if (!Helpers.PlatformChecks.IsPermissionGranted(this, permission))
                        permissionsToRequest.Add(permission);
                }
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="exception"></param>
        /// <param name="title"></param>
        public void CreateAndShowDialog(Exception exception, String title)
        {
            CreateAndShowDialog(exception.Message, title);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="message"></param>
        /// <param name="title"></param>
        public void CreateAndShowDialog(string message, string title)
        {
            Android.App.AlertDialog.Builder builder = new Android.App.AlertDialog.Builder(this);

            builder.SetMessage(message);
            builder.SetTitle(title);
            builder.Create().Show();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="email"></param>
        /// <returns></returns>
        public bool IsEmailValid(string email)
        {
            return Android.Util.Patterns.EmailAddress.Matcher(email).Matches();
        }

        /// <summary>
        /// Save credentials
        /// </summary>
        /// <param name="user"></param>
        public void SaveCrendentials(Users user)
        {
            ISharedPreferences prefs = PreferenceManager.GetDefaultSharedPreferences(this);
            ISharedPreferencesEditor editor = prefs.Edit();
            string userJson = JsonConvert.SerializeObject(user);
            editor.Remove(currentUserPref);
            editor.Commit();
            editor.PutString(currentUserPref, userJson);
            editor.Apply();
        }

        /// <summary>
        /// 
        /// </summary>
        public void DeleteSavedPreferences()
        {
            ISharedPreferences prefs = PreferenceManager.GetDefaultSharedPreferences(this);
            ISharedPreferencesEditor editor = prefs.Edit();
            editor.Remove(environment);
            editor.Remove(currentUserPref);
            editor.Remove(NotificationRegId);
            editor.Remove(PnsHandle);
            editor.Remove(PnsHandleRefreshed);
            editor.Apply();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public Users GetCurrentUser()
        {
            ISharedPreferences prefs = PreferenceManager.GetDefaultSharedPreferences(this);

            return JsonConvert.DeserializeObject<Users>(prefs.GetString(currentUserPref, ""));
        }

        /// <summary>
        /// delete credentials
        /// </summary>
        /// <param name="user"></param>
        public void DeleteCredentials()
        {
            ISharedPreferences prefs = PreferenceManager.GetDefaultSharedPreferences(this);
            ISharedPreferencesEditor editor = prefs.Edit();
            editor.Remove(currentUserPref);
            editor.Apply();
        }

        /// <summary>
        /// Get user avatar
        /// </summary>
        /// <param name="barberId"></param>
        /// <returns></returns>
        public async Task<System.Uri> GetUserAvatarUri(int userId)
        {
            try
            {
                return new System.Uri(await BlobStorageHelper.GetToasterBlobUri(userId));

            }
            catch (Exception ex)
            {
                var a = ex;
            }

            return null;
        }

        /// <summary>
        /// Get user avatar
        /// </summary>
        /// <param name="barberId"></param>
        /// <returns></returns>
        public async Task<System.Uri> GetBusinessLogoUri(int businessId)
        {
            try
            {
                return new System.Uri(await BlobStorageHelper.GetBusinessLogoUri(businessId));

            }
            catch (Exception ex)
            {
                var a = ex;
            }

            return null;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="logo"></param>
        /// <param name="imageView"></param>
        public async void BeginDownloadingImage(ImageViewImage logo, ImageView imageView)
        {
            try
            {
                // Queue the image to be downloaded. This task will execute
                // as soon as the existing ones have finished.
                byte[] data = null;

                if (logo != null && logo.ImageUrl != null)
                {
                    data = await BlobStorageHelper.GetImageData(logo.ImageUrl);

                    //logo.ImageBitmap = BitmapFactory.DecodeByteArray(data, 0, data.Length);
                    logo.ImageBitmap = await BitmapFactory.DecodeByteArrayAsync(data, 0, data.Length);

                    this.RunOnUiThread(() =>
                    {
                        imageView.SetImageBitmap(logo.ImageBitmap);
                    });
                }
            }
            catch (Exception)
            {
            }
        }

        #endregion

    }
}