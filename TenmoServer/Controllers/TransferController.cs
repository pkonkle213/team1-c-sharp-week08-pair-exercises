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
    public class TransferController : ControllerBase
    {
        private readonly ITransferDAO dao;

        public TransferController(ITransferDAO newDAO)
        {
            this.dao = newDAO;
        }

        [HttpPut]
        [Authorize]
        public void Transfer(int destinationId = 0, decimal amount = 0)
        {
            int userId = int.Parse(this.User.FindFirst("sub").Value);
            dao.Transfer(userId, destinationId, amount);
        }
    }
}
