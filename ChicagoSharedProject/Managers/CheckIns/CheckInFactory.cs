using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using TabsAdmin.Mobile.Shared.Interfaces.CheckIns;
using TabsAdmin.Mobile.Shared.Models.CheckIns;

namespace TabsAdmin.Mobile.Shared.Managers.CheckIns
{
    public class CheckInFactory : ICheckInFactory
    {

        #region Constants, Enums, and Variables

        private ICheckInFactory _CheckInFactory;

        #endregion

        #region Constructors

        public CheckInFactory(ICheckInFactory checkInFactory)
        {
            _CheckInFactory = checkInFactory;
        }

        #endregion

        #region Methods

        /// <summary>
        /// 
        /// </summary>
        /// <param name="checkIn"></param>
        /// <returns></returns>
        public Task<int> CheckIn(CheckIn checkIn)
        {
            return _CheckInFactory.CheckIn(checkIn);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="eventId"></param>
        /// <param name="individualId"></param>
        /// <returns></returns>
        public Task<ICollection<CheckIn>> GetCheckIns(int userId)
        {
            return _CheckInFactory.GetCheckIns(userId);
        }

        public Task<bool> EventCheckInExist(int userId, int eventId, int checkInType)
        {
            return _CheckInFactory.EventCheckInExist(userId, eventId, checkInType);
        }

        public Task<ICollection<CheckIn>> GetBusinessCheckIns(int businessId)
        {
            return _CheckInFactory.GetBusinessCheckIns(businessId);
        }

        public Task<ICollection<CheckIn>> GetEventCheckIns(int eventId)
        {
            return _CheckInFactory.GetEventCheckIns(eventId);
        }

        public Task BlockPost(int blockedByAdminUserId, int checkInId)
        {
            return _CheckInFactory.BlockPost(blockedByAdminUserId, checkInId);
        }

        public Task UnBlockPost(int checkInId)
        {
            return _CheckInFactory.UnBlockPost(checkInId);
        }

        #endregion

    }
}
