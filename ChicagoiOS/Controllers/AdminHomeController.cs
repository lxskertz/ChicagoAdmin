using Foundation;
using System;
using System.Collections.Generic;
using TabsAdmin.Mobile.Shared.Helpers;
using UIKit;

namespace TabsAdmin.Mobile.ChicagoiOS
{
    public partial class AdminHomeController : BaseViewController
    {

        #region Properties

        /// <summary>
        /// Gets or sets data source
        /// </summary>
        private DataSource.AdminHomeDataSource AdminHomeDataSource { get; set; }

        #endregion

        #region Constructors

        public AdminHomeController (IntPtr handle) : base (handle)
        {
        }

        #endregion

        #region Methods

        public override void ViewDidLoad()
        {
            base.ViewDidLoad();

            List<string> rows; //= new List<string>();
            rows = MoreScreenHelper.TableRows();
            AdminHomeTable.EstimatedRowHeight = 44f;
            AdminHomeTable.RowHeight = UITableView.AutomaticDimension;
            AdminHomeDataSource = new DataSource.AdminHomeDataSource(this, rows);
            AdminHomeTable.Source = AdminHomeDataSource;
            AdminHomeTable.TableFooterView = new UIView();
        }

        #endregion

    }
}