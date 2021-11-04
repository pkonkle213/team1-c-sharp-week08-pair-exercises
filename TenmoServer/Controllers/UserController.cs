using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TenmoServer.DAO;
using TenmoServer.Models;

namespace TenmoServer.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly INewDAO dao;

        public UserController(INewDAO newDAO)
        {
            this.dao = newDAO;
        }

        [HttpGet("balance")]
        [Authorize]
        public decimal Balance()
        {
            int userId = int.Parse(this.User.FindFirst("sub").Value);

            return dao.UserBalance(userId);
        }

        [HttpGet]
        [AllowAnonymous]
        public List<User> AllUsers()
        {
            int userId = int.Parse(this.User.FindFirst("sub").Value);
            return dao.AllUsers(userId);
        }
    }
}
