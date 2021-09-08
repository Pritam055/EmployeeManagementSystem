using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EMS.Services
{
    public class UserService
    {
        private readonly IHttpContextAccessor httpContext;
        public UserService(IHttpContextAccessor httpContext)
        {
            this.httpContext = httpContext;
        }
         
    }
}
