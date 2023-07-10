using ASAP_Task.Core;
using ASAP_Task.Service.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ASAP_Task.Service.Implementations
{
    public class AddressService : IAddressService
    {
        private readonly IAddressRepository _addressRepository;

        public AddressService(IAddressRepository addressRepository)
        {
            _addressRepository = addressRepository;
        }

        public async Task<Address> GetAddressById(int id)
        {
            return await _addressRepository.GetByIdAsync(id);
        }

        public async Task<IEnumerable<Address>> GetAllAddresses()
        {
            return await _addressRepository.GetAllAsync();
        }

        public async Task AddAddress(Address address)
        {
            await _addressRepository.AddAsync(address);
        }

        public async Task UpdateAddress(Address address)
        {
            await _addressRepository.UpdateAsync(address);
        }

        public async Task DeleteAddress(int id)
        {
            var address = await _addressRepository.GetByIdAsync(id);
            if (address != null)
            {
                await _addressRepository.DeleteAsync(address);
            }
        }
        public async Task<IEnumerable<Address>> FilterAddresses(string Street, string City, string Country)
        {
            return await _addressRepository.FilterAddresses(Street, City, Country);
        }
    }
}
