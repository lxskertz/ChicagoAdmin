using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using TabsAdmin.Mobile.Shared.Models.Businesses;

namespace TabsAdmin.Mobile.Shared.Interfaces.Businesses
{
    public interface IBusinessPhotoFactory
    {

        Task<int> Add(BusinesPhoto businesPhoto);

        Task<ICollection<BusinesPhoto>> Get(int businessId);
    }
}
