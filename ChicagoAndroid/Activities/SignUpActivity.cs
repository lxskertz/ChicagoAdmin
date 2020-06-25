using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Android.Content.PM;
using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Views.InputMethods;
using Android.Support.V7.Widget;
using Android.Support.Design.Widget;
using Android.Widget;
using TabsAdmin.Mobile.Shared.Resources;
using Newtonsoft.Json;
using TabsAdmin.Mobile.Shared.Models.Users;
using TabsAdmin.Mobile.Shared.Models.Individuals;
using TabsAdmin.Mobile.Shared.Models.Businesses;

namespace TabsAdmin.Mobile.ChicagoAndroid.Activities
{
    [Activity(Label = "Create Account", Theme = "@style/AppTheme", ScreenOrientation = ScreenOrientation.Portrait, ConfigurationChanges = ConfigChanges.Orientation | ConfigChanges.ScreenSize)]
    public class SignUpActivity : BaseActivity, TextView.IOnEditorActionListener
    {

        #region Constants, Enums, and Variables

        private AppCompatEditText fname;
        private AppCompatEditText lname;
        private AppCompatEditText email;
        private AppCompatEditText password;
        private AppCompatEditText reEnterpassword;
        private TextInputLayout fnameLayout;
        private TextInputLayout lastNameLayout;
        private TextInputLayout emailLayout;
        private TextInputLayout passwordLayout;
        private TextInputLayout reEnterpasswordLayout;
        private FrameLayout signupLayout;

        #endregion

        #region Methods

        /// <summary>
        /// handle when user tap done on the keyboard
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private async void HandleEditorAction(object sender, TextView.EditorActionEventArgs e)
        {
            e.Handled = false;
            if (e.ActionId == ImeAction.Done)
            {
                await Next();
            }
        }

        /// <summary>
        /// Observes the TextView's ImeAction so an action can be taken on keypress
        /// Called when an action is being performed.
        /// </summary>
        /// <param name="v"></param>
        /// <param name="actionId"></param>
        /// <param name="e"></param>
        /// <returns></returns>
        public bool OnEditorAction(TextView v, ImeAction actionId, KeyEvent e)
        {
            return true;
        }

        /// <summary>
        /// This hook is called whenever an item in your options menu is selected.
        /// </summary>
        /// <Param name="item"></Param>
        /// <returns></returns>
        public override bool OnOptionsItemSelected(IMenuItem item)
        {
            switch (item.ItemId)
            {
                case Android.Resource.Id.Home:
                    this.Finish();
                    return true;
            }

            return base.OnOptionsItemSelected(item);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="savedInstanceState"></param>
        protected override void OnCreate(Bundle savedInstanceState)
        {
            try
            {
                base.OnCreate(savedInstanceState);

                SetContentView(Resource.Layout.SignUp);

                //add the back arrow
                this.SupportActionBar.SetDisplayHomeAsUpEnabled(true);
                this.SupportActionBar.SetDisplayShowHomeEnabled(true);

                var signUp = FindViewById<Button>(Resource.Id.signup);
                emailLayout = FindViewById<TextInputLayout>(Resource.Id.email_layout);
                passwordLayout = FindViewById<TextInputLayout>(Resource.Id.password_layout);
                fnameLayout = FindViewById<TextInputLayout>(Resource.Id.firstname_layout);
                lastNameLayout = FindViewById<TextInputLayout>(Resource.Id.lastname_layout);
                reEnterpasswordLayout = FindViewById<TextInputLayout>(Resource.Id.reEnterPassword_layout);
                signupLayout = FindViewById<FrameLayout>(Resource.Id.signuoLayout);

                fname = FindViewById<AppCompatEditText>(Resource.Id.firstname);
                lname = FindViewById<AppCompatEditText>(Resource.Id.lastname);
                email = FindViewById<AppCompatEditText>(Resource.Id.email);
                password = FindViewById<AppCompatEditText>(Resource.Id.password);
                reEnterpassword = FindViewById<AppCompatEditText>(Resource.Id.reEnterPassword);

                signUp.Click += async delegate
                {
                    await Next();
                };

                //App.Track("Sign Up", "View");
            }
            catch (Exception) { }

        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        private async Task Next()
        {
            try
            {
                emailLayout.ErrorEnabled = false;
                emailLayout.Error = "";

                if (!ValidateInput(fnameLayout, fname, ToastMessage.EmptyFirstname))
                {
                    return;
                }
                else if (!ValidateInput(lastNameLayout, lname, ToastMessage.EmptyLastname))
                {
                    return;
                }
                else if (!ValidateInput(emailLayout, email, ToastMessage.EmptyEmail))
                {
                    return;
                }
                else if (!IsEmailValid(email.Text) && !email.Text.Contains("@@"))
                {
                    emailLayout.ErrorEnabled = true;
                    emailLayout.Error = ToastMessage.NotAValidEmail;
                    return;
                }
                else if (!ValidateInput(passwordLayout, password, ToastMessage.EmptyPassword))
                {
                    return;
                }
                else if (password.Text.Length <= 8)
                {
                    ShowSnack(signupLayout, ToastMessage.PasswordLength, "OK");
                    return;
                }
                else if (!string.Equals(password.Text, reEnterpassword.Text))
                {
                    ShowSnack(signupLayout, ToastMessage.UnequalPassword, "OK");
                    return;
                }
                else
                {
                    await SignUp();
                }
            }
            catch (Exception e)
            {
                var a = e;
                ShowSnack(signupLayout, ToastMessage.ServerError, "OK");
            }
        }

        /// <summary>
        /// Sign up
        /// </summary>
        /// <returns></returns>
        private async Task SignUp()
        {
            // stop if no internet connection
            if (CheckNetworkConnectivity() == null)
            {
                ShowSnack(signupLayout, ToastMessage.NoInternet, "OK");
                return;
            }
            Users user = null;
            this.ShowProgressbar(true, "", ToastMessage.CreatingAccount);
            try
            {
                //this hides the keyboard
                var imm = (InputMethodManager)GetSystemService(Context.InputMethodService);
                imm.HideSoftInputFromWindow(password.WindowToken, HideSoftInputFlags.NotAlways);

                var emailExist = await App.UsersFactory.EmailExist(email.Text.Trim());

                if (emailExist)
                {
                    ShowSnack(signupLayout, ToastMessage.AccountExist, "OK");
                }

                else
                {
                    var newUser = new Users
                    {
                        FirstName = fname.Text.Trim(),
                        LastName = lname.Text.Trim(),
                        Email = email.Text.Trim(),
                        PasswordHash = password.Text.Trim(),
                        Username = "",
                        IsBusiness = false,
                        IsIndividual = false,
                        TabsAdmin = true
                    };

                    await App.UsersFactory.AddNewUser(newUser);
                    user = await App.UsersFactory.GetUser(email.Text.Trim());

                    if (user != null)
                    {
                        DeleteCredentials();
                        SaveCrendentials(user);
                        this.MyPreferences.SaveEnvironment(Shared.MyEnvironment.Environment);
                        this.CurrentUser = GetCurrentUser();
                        StartActivity(typeof(AdminHomeActivity));
                    }
                    else
                    {
                        ShowSnack(signupLayout, ToastMessage.ServerError, "OK");
                    }


                    //VerificationCode verificationCode = new VerificationCode();
                    //verificationCode.Email = email.Text.Trim();
                    //verificationCode.Code = KazopiMobile.Helpers.PasswordHash.GenerateCodeNumber();
                    //verificationCode.UserId = user.UserId;
                    //await App.VerificationCodeManager.AddVerificationCode(verificationCode);

                    //var verifyAccountActivity = new Intent(this, typeof(VerifyAccountActivity));
                    //verifyAccountActivity.PutExtra("UserEmail", verificationCode.Email);
                    //StartActivity(verifyAccountActivity);
                }

                //App.Track("Sign Up", "View");
                this.ShowProgressbar(false, "", ToastMessage.CreatingAccount);
            }
            catch (Exception ex)
            {
                var a = ex;
                if (user != null)
                {
                    StartActivity(typeof(AdminHomeActivity));
                }
                else
                {
                    ShowSnack(signupLayout, ToastMessage.ServerError, "OK");
                }
                this.ShowProgressbar(false, "", ToastMessage.CreatingAccount);
            }
        }

        #endregion

    }
}