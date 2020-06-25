using System.Threading.Tasks;
using TabsAdmin.Mobile.Shared.Models.Businesses;

namespace TabsAdmin.Mobile.Shared.Interfaces.Businesses
{
    public interface IBusinessTypesFactory
    {

        #region Methods

        /// <summary>
        /// 
        /// </summary>
        /// <param name="businessTypes"></param>
        Task AddBusinessType(BusinessTypes businessTypes);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="businessTypes"></param>
        Task Update(BusinessTypes businessTypes);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="businessId"></param>
        /// <returns></returns>
        Task<BusinessTypes> GetBusinessType(int businessId);

        #endregion

    }
}
