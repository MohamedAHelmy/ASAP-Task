using ASAP_Task.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ASAP_Task.Service.Interfaces
{
    public interface IAddressService
    {
        Task<Address> GetAddressById(int id);
        Task<IEnumerable<Address>> GetAllAddresses();
        Task AddAddress(Address address);
        Task UpdateAddress(Address address);
        Task DeleteAddress(int id);
        Task<IEnumerable<Address>> FilterAddresses(string Street, string City, string Country);

    }
}
