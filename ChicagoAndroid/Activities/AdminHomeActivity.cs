using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Android.Content.PM;
using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using TabsAdmin.Mobile.ChicagoAndroid.Adapters;
using TabsAdmin.Mobile.Shared.Resources;
using TabsAdmin.Mobile.Shared.Helpers;

namespace TabsAdmin.Mobile.ChicagoAndroid.Activities
{
    [Activity(Label = "Home", Theme = "@style/AppTheme", ScreenOrientation = ScreenOrientation.Portrait, ConfigurationChanges = ConfigChanges.Orientation | ConfigChanges.ScreenSize)]

    public class AdminHomeActivity : BaseActivity
    {

        #region Constants, Enums, Variables

        private ListView adminHomeList;
        private AdminHomeAdapter AdminHomeAdapter;

        #endregion

        #region Properties

        #endregion

        #region Methods

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            SetContentView(Resource.Layout.AdminHome);
            adminHomeList = FindViewById<ListView>(Resource.Id.adminHomeList);
            LoadData();
        }

        /// <summary>
        /// Load data
        /// </summary>
        private void LoadData()
        {
            try
            {
                var rows = MoreScreenHelper.TableRows();
                AdminHomeAdapter = new AdminHomeAdapter(this, rows.ToArray());
                adminHomeList.Adapter = AdminHomeAdapter;
                adminHomeList.ItemClick += AdminHomeAdapter.OnListItemClick;
                adminHomeList.DividerHeight = 2;

                //App.Track("Settings", "View");

            }
            catch (Exception ex)
            {
                var a = ex;
            }
        }

        #endregion
    }
}