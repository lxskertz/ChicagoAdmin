using System.Collections.Generic;
using System.Threading.Tasks;
using TabsAdmin.Mobile.Shared.Models.Individuals;
using TabsAdmin.Mobile.Shared.Models;

namespace TabsAdmin.Mobile.Shared.Interfaces.Individuals
{
    public interface IIndividualFactory
    {

        #region Methods

        /// <summary>
        /// 
        /// </summary>
        /// <param name="individual"></param>
        Task CreateIndividual(Individual individual);

        /// <summary>
        /// 
        /// </summary>
        Task<ICollection<ToastersSearchItem>> ToasterSearch(SearchParameters param);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        Task<Individual> GetToasterByUserId(int userId);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="individualId"></param>
        /// <returns></returns>
        Task<Individual> GetToaster(int individualId);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="individual"></param>
        Task EditIndividual(Individual individual);

        #endregion

    }
}
