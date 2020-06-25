using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace TabsAdmin.Mobile.Shared.Interfaces.Individuals
{
    public interface ISMSMessageFactory
    {

        Task SendInvitation(string phoneNumber, string senderName, string receiverName);

    }
}
