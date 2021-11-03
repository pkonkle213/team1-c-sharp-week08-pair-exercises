using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TenmoServer.DAO;

namespace TenmoServer.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly string connectionstring;
        private readonly NewDAO dao;

        [HttpGet("{id}/balance")]
        [Authorize]
        public decimal Balance(int id)
        {
            return dao.GetUserBalance(id);
        }
    }
}
