using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using TabsAdmin.Mobile.Shared.Interfaces.Individuals;

namespace TabsAdmin.Mobile.Shared.Managers.Individuals
{
    public class SMSMessageFactory : ISMSMessageFactory
    {

        #region Constants, Enums, and Variables

        private ISMSMessageFactory _SMSMessageFactory;

        #endregion

        #region Constructors

        public SMSMessageFactory(ISMSMessageFactory smsMessageFactory)
        {
            _SMSMessageFactory = smsMessageFactory;
        }

        #endregion

        #region Methods

        public Task SendInvitation(string phoneNumber, string senderName, string receiverName)
        {
            return _SMSMessageFactory.SendInvitation(phoneNumber, senderName, receiverName);
        }

        #endregion

    }
}
