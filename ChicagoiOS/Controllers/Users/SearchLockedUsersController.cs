using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using UIKit;
using Foundation;
using BigTed;
using TabsAdmin.Mobile.Shared.Models;
using TabsAdmin.Mobile.Shared.Resources;
using TabsAdmin.Mobile.Shared.Models.Individuals;

namespace TabsAdmin.Mobile.ChicagoiOS
{
    public partial class SearchLockedUsersController : BaseViewController, IUISearchResultsUpdating
    {

        #region Constants, Enums, and Variables

        private UISearchController search;
        public SearchParameters param = new SearchParameters();
        public bool loadMore = true;

        #endregion

        #region Properties

        /// <summary>
        /// 
        /// </summary>
        public List<ImageViewImage> ImageViewImages { get; set; } = new List<ImageViewImage>();

        /// <summary>
        /// Gets or sets data source
        /// </summary>
        private DataSource.Users.LockUnlockUsersDatasource LockUnlockUsersDatasource { get; set; }

        ICollection<ToastersSearchItem> ToastersSearchItems { get; set; }

        #endregion

        #region Constructors

        public SearchLockedUsersController(IntPtr handle) : base(handle) { }

        #endregion

        #region Methods

        /// <summary>
        /// 
        /// </summary>
        private void InitTableView(List<ToastersSearchItem> data)
        {
            LockedTable.EstimatedRowHeight = 88f;
            LockedTable.RowHeight = UITableView.AutomaticDimension;
            this.LockUnlockUsersDatasource = new DataSource.Users.LockUnlockUsersDatasource(this, data, this.ImageViewImages);
            LockedTable.Source = this.LockUnlockUsersDatasource;
            LockedTable.TableFooterView = new UIView();
        }

        /// <summary>
        /// 
        /// </summary>
        public override void ViewDidLoad()
        {
            try
            {
                base.ViewDidLoad();

                search = new UISearchController(searchResultsController: null)
                {
                    DimsBackgroundDuringPresentation = false,
                    HidesNavigationBarDuringPresentation = false
                };

                search.SearchResultsUpdater = this;
                search.SearchBar.Placeholder = AppText.searchToastPlaceHolder;
                this.NavigationItem.SearchController = search;
                this.TabBarController.NavigationItem.HidesSearchBarWhenScrolling = false;
                search.SearchBar.TintColor = UIColor.White;

                // do search, insert search term into DB
                search.SearchBar.SearchButtonClicked += async (sender, e) =>
                {
                    search.SearchBar.ResignFirstResponder();
                    await Search(search.SearchBar.Text);
                };
            }
            catch (Exception)
            {
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public override void DidReceiveMemoryWarning()
        {
            // Release all cached images. This will cause them to be redownloaded
            // later as they're displayed.
            if (this.ImageViewImages != null)
            {
                foreach (var v in this.ImageViewImages)
                    v.Image = null;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public async Task GetPicUris()
        {
            try
            {
                foreach (var b in this.ToastersSearchItems)
                {
                    ImageViewImage itemLogo = new ImageViewImage();
                    itemLogo.Id = b.UserId;
                    var uriString = await Shared.Helpers.BlobStorageHelper.GetToasterBlobUri(b.UserId);
                    if (!string.IsNullOrEmpty(uriString))
                    {
                        Uri imageUri = new Uri(uriString);
                        itemLogo.ImageUrl = imageUri;
                        this.ImageViewImages.Add(itemLogo);
                    }
                }
            }
            catch (Exception)
            {
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="searchController"></param>
        public void UpdateSearchResultsForSearchController(UISearchController searchController)
        {
            var find = searchController.SearchBar.Text;
            //if (!String.IsNullOrEmpty(find))
            //{
            //    searchResults = titles.Where(t => t.ToLower().Contains(find.ToLower())).Select(p => p).ToArray();
            //}
            //else
            //{
            //    searchResults = null;
            //}
            //TableView.ReloadData();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="animated"></param>
        public override void ViewDidAppear(bool animated)
        {
            base.ViewDidAppear(animated);
            //this.NavigationItem.NavigationBarHidden = false;
            this.NavigationItem.RightBarButtonItem = null;

            if (this.NavigationItem.SearchController == null)
            {
                this.NavigationItem.SearchController = search;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="searchTerm"></param>
        private void InitSearchParameters(string searchTerm)
        {
            param = new SearchParameters();
            param.PageSize = 15;
            param.SearchTerm = searchTerm;
            param.PageNumber = 0;
        }

        public async void LockUnlock(ToastersSearchItem item)
        {
            if (AppDelegate.IsOfflineMode())
            {
                BTProgressHUD.ShowErrorWithStatus(ToastMessage.NoInternet, Helpers.ToastTime.ErrorTime);
                return;
            } 
            else
            {
                BTProgressHUD.Show(ToastMessage.PleaseWait, -1f, ProgressHUD.MaskType.Black);

                if (item.AccountLocked)
                {
                    await AppDelegate.UsersFactory.UnlockUser(item.UserId);
                } else
                {
                    await AppDelegate.UsersFactory.LockUser(item.UserId);
                }

                await Search(search.SearchBar.Text);

                BTProgressHUD.Dismiss();

            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="searchTerm"></param>
        /// <returns></returns>
        public async Task Search(string searchTerm)
        {
            try
            {
                if (AppDelegate.IsOfflineMode())
                {
                    BTProgressHUD.ShowErrorWithStatus(ToastMessage.NoInternet, Helpers.ToastTime.ErrorTime);

                    return;
                }
                else
                {
                    loadMore = true;
                    InitSearchParameters(searchTerm);
                    BTProgressHUD.Show(ToastMessage.Searching, -1f, ProgressHUD.MaskType.Black);
                    ToastersSearchItems = await AppDelegate.IndividualFactory.ToasterSearch(param);

                    if (ToastersSearchItems != null && ToastersSearchItems.Count > 0)
                    {
                        await GetPicUris();
                        if (LockUnlockUsersDatasource == null)
                        {
                            InitTableView(ToastersSearchItems.ToList());
                        }
                        else
                        {
                            this.InvokeOnMainThread(() =>
                            {
                                this.LockUnlockUsersDatasource.Rows = ToastersSearchItems.ToList();
                                this.LockUnlockUsersDatasource.ImageViewImages = this.ImageViewImages;
                                LockedTable.ReloadData();
                            });
                        }
                        BTProgressHUD.Dismiss();
                    }
                    else
                    {
                        BTProgressHUD.ShowErrorWithStatus(ToastMessage.NullResult, Helpers.ToastTime.ErrorTime);
                    }
                }
            }
            catch (Exception ex)
            {
                var a = ex;
                BTProgressHUD.Dismiss();
                BTProgressHUD.ShowErrorWithStatus(ToastMessage.ServerError, Helpers.ToastTime.ErrorTime);
            }
        }

        /// <summary>
        /// Called when recycler view is scrolled to the bottom 
        /// </summary>
        public async Task ScrolledToBottom()
        {
            if (!AppDelegate.IsOfflineMode() && loadMore)
            {
                try
                {
                    param.PageNumber += this.LockUnlockUsersDatasource.Rows.Count;
                    var results = await AppDelegate.IndividualFactory.ToasterSearch(param);

                    if (results != null && results.Count > 0)
                    {
                        this.LockUnlockUsersDatasource.AddRowItems(results.ToList());
                        LockedTable.ReloadData();
                    }
                    else
                    {
                        loadMore = false;
                    }
                }
                catch (Exception) { }
            }
        }

        #endregion

    }
}