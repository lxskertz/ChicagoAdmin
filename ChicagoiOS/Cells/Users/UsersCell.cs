using Foundation;
using System;
using UIKit;
using TabsAdmin.Mobile.ChicagoiOS.DataSource.Users;
using TabsAdmin.Mobile.Shared.Models.Individuals;

namespace TabsAdmin.Mobile.ChicagoiOS
{
    public partial class UsersCell : UITableViewCell
    {

        #region Properties

        /// <summary>
        /// 
        /// </summary>
        public UILabel _Name
        {
            get
            {
                return Name;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public UILabel _Username
        {
            get
            {
                return Username;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public UIButton _UnlockBtn
        {
            get
            {
                return UnlockBtn;
            }
        }

        public UIImageView _ProfilePic
        {
            get
            {
                return Picture;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public LockUnlockUsersDatasource DataSource { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public NSIndexPath IndexPath { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public ToastersSearchItem Item { get; set; }

        #endregion

        #region Constructors

        public UsersCell(IntPtr handle) : base(handle) { }

        #endregion

        #region Methods

        partial void UnlockBtn_TouchUpInside(UIButton sender)
        {
            this.DataSource.Controller.LockUnlock(Item);
        }

        #endregion

    }
}