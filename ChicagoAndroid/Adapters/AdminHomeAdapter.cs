using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Newtonsoft.Json;
using TabsAdmin.Mobile.Shared.Helpers;

namespace TabsAdmin.Mobile.ChicagoAndroid.Adapters
{
    public class AdminHomeAdapter : BaseAdapter
    {

        #region Contants, Enums, and Variables

        #endregion

        #region Properties

        /// <summary>
        /// Gets or sets the adapter context
        /// </summary>
        private Activities.AdminHomeActivity MyContext { get; set; }

        public string[] Titles { get; set; }

        #endregion

        #region Constructors

        public AdminHomeAdapter(Activities.AdminHomeActivity context, string[] titles)
        {
            this.MyContext = context;
            this.Titles = titles;
        }

        #endregion

        #region Methods

        /// <summary>
        /// Get number of item to be displayed
        /// </summary>
        public override int Count
        {
            get
            {
                return this.Titles.Length;
            }
        }

        /// <summary>
        /// Gets item ID
        /// </summary>
        /// <param name="position"></param>
        /// <returns></returns>
		public override long GetItemId(int position)
        {
            return position;
        }

        /// <summary>
        /// Get the type of View that will be created for the specified item.
        /// </summary>
        /// <param name="position"></param>
        /// <returns></returns>
        public override int GetItemViewType(int position)
        {
            return base.GetItemViewType(position);
        }

        /// <summary>
        /// Get item at specified position
        /// </summary>
        /// <param name="position"></param>
        /// <returns></returns>
        public override Java.Lang.Object GetItem(int position)
        {
            return "";
        }

        /// <summary>
        /// Gets view... list cells
        /// </summary>
        /// <param name="position"></param>
        /// <param name="convertView"></param>
        /// <param name="container"></param>
        /// <returns></returns>
        public override View GetView(int position, View convertView, ViewGroup container)
        {
            if (convertView == null)
            {
                convertView = LayoutInflater.FromContext(this.MyContext).Inflate(Resource.Layout.BasicListview, container, false);
            }

            var item = this.Titles.ElementAt(position);
            var title = convertView.FindViewById<TextView>(Resource.Id.title);
            title.Text = this.Titles.ElementAt(position);

            return convertView;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public void OnListItemClick(object sender, AdapterView.ItemClickEventArgs e)
        {
            var item = this.Titles.ElementAt(e.Position);
            switch (item)
            {
                case MoreScreenHelper.Logout:
                    Logout();
                    break;
                case MoreScreenHelper.CheckinReports:
                    break;
                case MoreScreenHelper.LockedUsers:
                    break;
                case MoreScreenHelper.UnlokcedUsers:
                    break;
                case MoreScreenHelper.UsersReports:
                    break;
            }
        }

        /// <summary>
        /// Logout
        /// </summary>
        /// <returns></returns>
        private async void Logout()
        {
            try
            {
                this.MyContext.DeleteSavedPreferences();
                //DeleteCredentials();

                if (this.MyContext.CheckNetworkConnectivity() != null)
                {
                    await App.UsersFactory.Logout(this.MyContext.CurrentUser.Email);
                }

                this.MyContext.StartActivity(typeof(Activities.HomeActivity));
                this.MyContext.Finish();
            }
            catch (Exception)
            {
                this.MyContext.StartActivity(typeof(Activities.HomeActivity));
                this.MyContext.Finish();
            }
        }



        #endregion

    }
}