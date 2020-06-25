using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using TabsAdmin.Mobile.Shared.Models.Individuals;

namespace TabsAdmin.Mobile.Shared.Interfaces.Individuals
{
    public interface IToasterPhotoFactory
    {

        Task<int> Add(ToasterPhoto toasterPhoto);

        Task<ICollection<ToasterPhoto>> Get(int businessId);

    }
}
