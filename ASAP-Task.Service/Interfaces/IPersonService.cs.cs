using ASAP_Task.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ASAP_Task.Service.Interfaces
{
    public interface IPersonService
    {
        Task<Person> GetPersonById(int id);
        Task<IEnumerable<Person>> GetAllPeople();
        Task AddPerson(Person person);
        Task UpdatePerson(Person person);
        Task DeletePerson(int id);
        Task<IEnumerable<Person>> FilterPeople(string name, int age);
    }
}
