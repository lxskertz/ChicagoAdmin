using Foundation;
using System;
using System.Threading.Tasks;
using UIKit;
using BigTed;
using TabsAdmin.Mobile.Shared.Resources;

namespace TabsAdmin.Mobile.ChicagoiOS
{
    public partial class LoginViewController : BaseViewController
    {

        #region Constructors

        public LoginViewController(IntPtr handle) : base(handle) { }

        #endregion

        #region Methods

        /// <summary>
        /// 
        /// </summary>
        public override void ViewDidLoad()
        {
            try
            {
                base.ViewDidLoad();

                Title = "Login";

                Email.ShouldReturn += (textField) =>
                {
                    textField.ResignFirstResponder();
                    Password.BecomeFirstResponder();

                    return true;
                };

                Password.ShouldReturn += (textField) =>
                {
                    textField.ResignFirstResponder();
                    //Login();

                    return true;
                };

                //AppDelegate.Track("Login", "View");

            }
            catch (Exception)
            {

            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="animated"></param>
        //public override void ViewWillAppear(bool animated)
        //{
        //    base.ViewWillAppear(animated);
        //    this.NavigationItem.HidesBackButton = true;
        //    this.NavigationController.NavigationBarHidden = true;
        //}

        /// <summary>
        /// Dismiss the keyboard when one or more fingers touches the screen.
        /// </summary>
        /// <param name="touches"></param>
        /// <param name="evt"></param>
        public override void TouchesBegan(NSSet touches, UIEvent evt)
        {
            base.TouchesBegan(touches, evt);
            Password.ResignFirstResponder();
            Email.ResignFirstResponder();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        async partial void LoginBtn_TouchUpInside(UIButton sender)
        {
            await Login();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        partial void ForgotPasswordBtn_TouchUpInside(UIButton sender)
        {
            UIViewController controller = this.Storyboard.InstantiateViewController("ResetPasswordController") as ResetPasswordController;
            this.NavigationController.PushViewController(controller, true);
        }


        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        private async Task Login()
        {
            try
            {
                if (AppDelegate.IsOfflineMode())
                {
                    BTProgressHUD.ShowErrorWithStatus(ToastMessage.NoInternet, Helpers.ToastTime.ErrorTime);

                    return;
                }

                if (string.IsNullOrEmpty(Email.Text))
                {
                    BTProgressHUD.ShowErrorWithStatus(ToastMessage.EmptyEmail, Helpers.ToastTime.ErrorTime);

                    return;
                }

                if (string.IsNullOrEmpty(Password.Text))
                {
                    BTProgressHUD.ShowErrorWithStatus(ToastMessage.EmptyPassword, Helpers.ToastTime.ErrorTime);

                    return;
                }

                BTProgressHUD.Show(ToastMessage.LoggingIn, -1f, ProgressHUD.MaskType.Black);

                var user = await AppDelegate.UsersFactory.Login(Email.Text.Trim(), Password.Text.Trim());

                if (user != null && user.TabsAdmin)
                {
                    AppDelegate.DeleteCurrentUser();
                    user.PasswordHash = string.Empty;
                    AppDelegate.SaveCurrentUser(user);
                    AppDelegate.SaveEnvironment(Shared.MyEnvironment.Environment);
                    AppDelegate.CurrentUser = user;

                    UIViewController controller = this.Storyboard.InstantiateViewController("AdminHomeController") as AdminHomeController;
                    this.NavigationController.SetViewControllers(new UIViewController[] { controller }, true);

                    BTProgressHUD.Dismiss();
                }
                else
                {
                    BTProgressHUD.ShowErrorWithStatus(ToastMessage.InValidUsernameOrPassword, Helpers.ToastTime.ErrorTime);
                }
            }
            catch (Exception)
            {
                BTProgressHUD.Dismiss();
            }
        }

        #endregion

    }
}