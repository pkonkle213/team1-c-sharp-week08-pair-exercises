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
    public class MoneyController : ControllerBase
    {
        private readonly IMoneyDAO dao;

        public MoneyController(IMoneyDAO moneyDAO)
        {
            this.dao = moneyDAO;
        }

        [HttpPut]
        [Authorize]
        public ActionResult Transfer(Transfer transfer)
        {
            int userId = int.Parse(this.User.FindFirst("sub").Value);
            int destinationId = transfer.ReceiverId;
            if (userId == destinationId)
            {
                return BadRequest("Please select a different user.");
            }
            //Get the user's balance to compare against transfer amount
            decimal balance = Balance();
            decimal amount = transfer.TransferAmount;
            if (balance < amount)
            {
                return BadRequest("Insufficient funds.");
            }
            else
            {
                dao.Transfer(userId, destinationId, amount);
                return Ok("Transfer complete!");
            }
        }

        [HttpGet("balance")]
        [Authorize]
        public decimal Balance()
        {
            int userId = int.Parse(this.User.FindFirst("sub").Value);
            return dao.UserBalance(userId);
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
