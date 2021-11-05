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
        private readonly IUserInfoDAO dao;

        public UserController(IUserInfoDAO newDAO)
        {
            this.dao = newDAO;
        }

        [HttpGet]
        [Authorize]
        public List<User> AllUsers()
        {
            int userId = int.Parse(this.User.FindFirst("sub").Value);
            return dao.AllUsers(userId);
        }
    }
}
