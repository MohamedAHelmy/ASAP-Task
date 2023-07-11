using ASAP_Task.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ASAP_Task.Service.Interfaces
{
    public interface IUserService
    {
        Task<LoginResponse> RegisterAsync(RegisterViewModel user);
        Task<dynamic> LoginAsync(LoginViewModel model);
    }
}
