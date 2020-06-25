using System.Threading.Tasks;
using TabsAdmin.Mobile.Shared.Models;

namespace TabsAdmin.Mobile.Shared.Interfaces
{
    public interface IVerificationCode
    {

        #region Methods

        /// <summary>
        /// Add new code
        /// </summary>
        /// <param name="verificationCode"></param>
        /// <returns></returns>
        Task AddVerificationCode(VerificationCode verificationCode);

        /// <summary>
        /// Delete code
        /// </summary>
        /// <param name="email"></param>
        /// <param name="userId"></param>
        /// <returns></returns>
        Task DeleteVerificationCode(string email, int userId);

        /// <summary>
        /// Get code
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        Task<VerificationCode> GetVerificationCode(int userId);

        #endregion

    }
}
