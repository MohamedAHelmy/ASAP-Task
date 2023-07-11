using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ASAP_Task.Core
{
    public class LoginResponse
    {
        public bool Succeeded { get; set; }
        public string Token { get; set; }
        public IEnumerable<IdentityError> ErrorMessage { get; set; }
    }
}
