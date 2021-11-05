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

        public TransferController(ITransferDAO transferDAO)
        {
            this.dao = transferDAO;
        }

        [HttpPut]
        [Authorize]
        public ActionResult Transfer(Transfer transfer)
        {
            int userId = int.Parse(this.User.FindFirst("sub").Value);
            int destinationId = transfer.ReveiverId;
            decimal amount = transfer.TransferAmount;
            dao.Transfer(userId, destinationId, amount);
            return Ok("Transfer complete!");
        }

        [HttpGet]
        [Authorize]
        public List<Transfer> AllTransfers()
        {
            int userId = int.Parse(this.User.FindFirst("sub").Value);
            List<Transfer> result = dao.AllTransfers(userId);
            return result;
        }

        [HttpGet("{id}")]
        [Authorize]
        public Transfer OneTransfer(int id)
        {
            Transfer transfer = dao.SpecificTransfer(id);
            return transfer;
        }
    }
}
