using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using TabsAdmin.Mobile.Shared.Models.Individuals;
using TabsAdmin.Mobile.Shared.Interfaces.Individuals;
using TabsAdmin.Mobile.Shared.Models;

namespace TabsAdmin.Mobile.Shared.Managers.Individuals
{
    public class ToastersFactory : IToastersFactory
    {

        #region Constants, Enums, and Variables

        private IToastersFactory _ToastersFactory;

        #endregion

        #region Constructors

        public ToastersFactory(IToastersFactory toastersFactory)
        {
            _ToastersFactory = toastersFactory;
        }

        #endregion

        #region Methods

        /// <summary>
        /// 
        /// </summary>
        /// <param name="toasters"></param>
        public Task AddToaster(Toasters toaster)
        {
            return this._ToastersFactory.AddToaster(toaster);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="toasterId"></param>
        public Task RemoveToaster(int toasterId)
        {
            return this._ToastersFactory.RemoveToaster(toasterId);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="userOneId"></param>
        /// <param name="userTwoId"></param>
        /// <param name="actionUserId"></param>
        /// <param name="status"></param>
        public Task AcceptRequest(int userOneId, int userTwoId, int actionUserId, Toasters.ToasterRequestStatus status)
        {
            return this._ToastersFactory.AcceptRequest(userOneId, userTwoId, actionUserId, status);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        public Task<ICollection<Toasters>> GetToasters(SearchParameters param)
        {
            return this._ToastersFactory.GetToasters(param);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        public Task<ICollection<Toasters>> SearchToasters(SearchParameters param)
        {
            return this._ToastersFactory.SearchToasters(param);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        public Task<ICollection<Toasters>> GetPendingToasters(SearchParameters param)
        {
            return this._ToastersFactory.GetPendingToasters(param);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="individualId"></param>
        /// <returns></returns>
        public Task<Toasters> Get(int individualId)
        {
            return this._ToastersFactory.Get(individualId);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="individualId"></param>
        /// <param name="toasterUserIndividualId"></param>
        /// <returns></returns>
        public Task<Toasters> Connected(int UserOneId, int UserTwoId)
        {
            return this._ToastersFactory.Connected(UserOneId, UserTwoId);
        }

        public Task<int> GetTotalToastersCount(int userId)
        {
            return this._ToastersFactory.GetTotalToastersCount(userId);
        }

        #endregion

    }
}
