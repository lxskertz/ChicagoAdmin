using Foundation;
using System;
using System.Threading.Tasks;
using UIKit;
using BigTed;
using TabsAdmin.Mobile.Shared.Resources;
using TabsAdmin.Mobile.Shared.Models.Users;
using TabsAdmin.Mobile.Shared.Models.Businesses;
using TabsAdmin.Mobile.Shared.Models.Individuals;

namespace TabsAdmin.Mobile.ChicagoiOS
{
    public partial class SignUpController : BaseViewController
    {

        #region Properties

        /// <summary>
        /// Gets or sets SignUpDataSource
        /// </summary>
        private DataSource.SignUpDataSource SignUpDataSource { get; set; }

        /// <summary>
        /// Gets or sets first name
        /// </summary>
        public UITextField Firstname { get; set; }

        /// <summary>
        /// Gets or sets last name
        /// </summary>
        public UITextField Lastname { get; set; }

        /// <summary>
        /// Gets or sets phone number
        /// </summary>
        public UITextField Email { get; set; }
        /// <summary>
        /// Gets or sets last name
        /// </summary>
        public UITextField Password { get; set; }

        /// <summary>
        /// Gets or sets phone number
        /// </summary>
        public UITextField ReEnterPAssword { get; set; }

        #endregion

        #region Constructors

        public SignUpController (IntPtr handle) : base (handle)
        {
        }

        #endregion

        #region Methods

        /// <summary>
        /// Load Data
        /// </summary>
        private void LoadData()
        {
            try
            {
                CreateAcctTable.EstimatedRowHeight = 44f;
                CreateAcctTable.RowHeight = UITableView.AutomaticDimension;
                SignUpDataSource = new DataSource.SignUpDataSource(this);
                CreateAcctTable.Source = SignUpDataSource;
                CreateAcctTable.TableFooterView = new UIView();

                this.NavigationItem.SetRightBarButtonItem(new UIBarButtonItem("Create", UIBarButtonItemStyle.Plain, async (sender, args) =>
                {
                    await Next();
                }), true);

                UITapGestureRecognizer tapGesture = new UITapGestureRecognizer(() =>
                {
                    CreateAcctTable.EndEditing(true);
                });

                tapGesture.CancelsTouchesInView = false;
                CreateAcctTable.AddGestureRecognizer(tapGesture);

                CreateAcctToolbar.Hidden = true;
            }
            catch (Exception)
            {
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="animated"></param>
        public override void ViewDidAppear(bool animated)
        {
            try
            {
                base.ViewDidAppear(animated);
            }
            catch (Exception)
            {
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public override void ViewDidLoad()
        {
            try
            {
                base.ViewDidLoad();
                LoadData();

                //AppDelegate.Track("Sign Up", "View");

            }
            catch (Exception ex)
            {
                var a = ex;
            }
        }

        async partial void CreateAcctBtn_Activated(UIBarButtonItem sender)
        {
            await Next();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public async Task Next()
        {
            try
            {
                if (string.IsNullOrEmpty(Firstname.Text))
                {
                    BTProgressHUD.ShowErrorWithStatus(ToastMessage.EmptyFirstname, Helpers.ToastTime.ErrorTime);
                    return;
                }
                else if (string.IsNullOrEmpty(Lastname.Text))
                {
                    BTProgressHUD.ShowErrorWithStatus(ToastMessage.EmptyLastname, Helpers.ToastTime.ErrorTime);
                    return;
                }
                else if (string.IsNullOrEmpty(Email.Text))
                {
                    BTProgressHUD.ShowErrorWithStatus(ToastMessage.EmptyEmail, Helpers.ToastTime.ErrorTime);
                    return;
                }
                else if (!IsValidEmail(Email.Text))
                {
                    BTProgressHUD.ShowErrorWithStatus(ToastMessage.NotAValidEmail, Helpers.ToastTime.ErrorTime);
                    return;
                }
                else if (string.IsNullOrEmpty(Password.Text))
                {
                    BTProgressHUD.ShowErrorWithStatus(ToastMessage.EmptyPassword, Helpers.ToastTime.ErrorTime);
                    return;
                }
                else if (Password.Text.Length <= 8)
                {
                    BTProgressHUD.ShowErrorWithStatus(ToastMessage.PasswordLength, Helpers.ToastTime.ErrorTime);
                    return;
                }
                else if (!string.Equals(Password.Text, ReEnterPAssword.Text))
                {
                    BTProgressHUD.ShowErrorWithStatus(ToastMessage.UnequalPassword, Helpers.ToastTime.ErrorTime);
                    return;
                }
                else
                {
                    await Register();
                }

                //AppDelegate.Track("Sign Up", "Sign up");

            }
            catch (Exception)
            {
                BTProgressHUD.ShowErrorWithStatus(ToastMessage.ServerError, Helpers.ToastTime.ErrorTime);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        private async Task Register()
        {
            if (AppDelegate.IsOfflineMode())
            {
                BTProgressHUD.ShowErrorWithStatus(ToastMessage.NoInternet, Helpers.ToastTime.ErrorTime);

                return;
            }
            Users user = null;
            BTProgressHUD.Show(ToastMessage.PleaseWait, -1f, ProgressHUD.MaskType.Black);
            try
            {

                var emailExist = await AppDelegate.UsersFactory.EmailExist(Email.Text.Trim());

                if (emailExist)
                {
                    BTProgressHUD.ShowErrorWithStatus(ToastMessage.AccountExist, Helpers.ToastTime.ErrorTime);
                }
                else
                {
                    var newUser = new Users
                    {
                        FirstName = Firstname.Text.Trim(),
                        LastName = Lastname.Text.Trim(),
                        Email = Email.Text.Trim(),
                        PasswordHash = Password.Text.Trim(),                       
                        Username = Email.Text.Trim(),
                        IsBusiness = false,
                        IsIndividual = false,
                        TabsAdmin = true
                    };

                    await AppDelegate.UsersFactory.AddNewUser(newUser);
                    user = await AppDelegate.UsersFactory.GetUser(Email.Text.Trim());

                    if (user != null)
                    {
                        AppDelegate.DeleteCurrentUser();
                        user.PasswordHash = string.Empty;
                        AppDelegate.SaveCurrentUser(user);
                        AppDelegate.SaveEnvironment(Shared.MyEnvironment.Environment); //TODO: set right url
                        AppDelegate.CurrentUser = user;
                        
                        UIViewController controller = this.Storyboard.InstantiateViewController("AdminHomeController") as AdminHomeController;
                        this.NavigationController.SetViewControllers(new UIViewController[] { controller }, true);                   
                    }
                    else
                    {
                        BTProgressHUD.ShowErrorWithStatus(ToastMessage.ServerError, Helpers.ToastTime.ErrorTime);
                    }
                }
            }
            catch (Exception ex)
            {
                var a = ex;
                BTProgressHUD.Dismiss();
            }
            BTProgressHUD.Dismiss();
        }

        #endregion

    }
}