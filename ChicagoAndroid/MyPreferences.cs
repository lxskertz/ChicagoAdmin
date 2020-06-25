using System;
using Android.Preferences;
using Android.Views;
using Android.Widget;
using Android.Net;
using Android.Content;

namespace TabsAdmin.Mobile.ChicagoAndroid
{
    public class MyPreferences
    {

        Activities.BaseActivity MyContext { get; set; }

        public MyPreferences(Activities.BaseActivity baseActivity)
        {
            this.MyContext = baseActivity;
        }

        #region Methods

        /// <summary>
        /// 
        /// </summary>
        /// <param name="environment"></param>
        public void SaveEnvironment(string value)
        {
            ISharedPreferences prefs = PreferenceManager.GetDefaultSharedPreferences(this.MyContext);
            ISharedPreferencesEditor editor = prefs.Edit();
            editor.PutString(Activities.BaseActivity.environment, value);
            editor.Apply();
        }

        /// <summary>
        /// 
        /// </summary>
        public void DeleteENVIRONMENT()
        {
            ISharedPreferences prefs = PreferenceManager.GetDefaultSharedPreferences(this.MyContext);
            ISharedPreferencesEditor editor = prefs.Edit();
            editor.Remove(Activities.BaseActivity.environment);
            editor.Apply();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public string GetEnvironment()
        {
            ISharedPreferences prefs = PreferenceManager.GetDefaultSharedPreferences(this.MyContext);

            return prefs.GetString(Activities.BaseActivity.environment, "");
        }
        
        #endregion

    }
}