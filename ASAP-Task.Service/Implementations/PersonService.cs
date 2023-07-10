using ASAP_Task.Core;
using ASAP_Task.Service.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ASAP_Task.Service.Implementations
{
    public class PersonService : IPersonService
    {
        private readonly IPersonRepository _personRepository;

        public PersonService(IPersonRepository personRepository)
        {
            _personRepository = personRepository;
        }

        public async Task<Person> GetPersonById(int id)
        {
            return await _personRepository.GetByIdAsync(id);
        }

        public async Task<IEnumerable<Person>> GetAllPeople()
        {
            return await _personRepository.GetAllAsync();
        }

        public async Task AddPerson(Person person)
        {
            await _personRepository.AddAsync(person);
        }

        public async Task UpdatePerson(Person person)
        {
            await _personRepository.UpdateAsync(person);
        }

        public async Task DeletePerson(int id)
        {
            var person = await _personRepository.GetByIdAsync(id);
            if (person != null)
            {
                await _personRepository.DeleteAsync(person);
            }
        }
        public async Task<IEnumerable<Person>> FilterPeople(string name, int age)
        {
            return await _personRepository.FilterPeople(name, age);
        }
    }
}
