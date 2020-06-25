using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using TabsAdmin.Mobile.Shared.Models.Individuals;
using TabsAdmin.Mobile.Shared.Models;

namespace TabsAdmin.Mobile.Shared.Interfaces.Individuals
{
    public interface IToastersFactory
    {

        #region Methods

        /// <summary>
        /// 
        /// </summary>
        /// <param name="toasters"></param>
        Task AddToaster(Toasters toasters);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="toasterId"></param>
        Task RemoveToaster(int toasterId);


        /// <summary>
        /// 
        /// </summary>
        /// <param name="userOneId"></param>
        /// <param name="userTwoId"></param>
        /// <param name="actionUserId"></param>
        /// <param name="status"></param>
        Task AcceptRequest(int userOneId, int userTwoId, int actionUserId, Toasters.ToasterRequestStatus status);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        Task<ICollection<Toasters>> GetToasters(SearchParameters param);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        Task<ICollection<Toasters>> SearchToasters(SearchParameters param);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        Task<ICollection<Toasters>> GetPendingToasters(SearchParameters param);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="individualId"></param>
        /// <returns></returns>
        Task<Toasters> Get(int individualId);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="individualId"></param>
        /// <param name="toasterUserIndividualId"></param>
        /// <returns></returns>
        Task<Toasters> Connected(int UserOneId, int UserTwoId);

        Task<int> GetTotalToastersCount(int userId);

        #endregion

    }
}
