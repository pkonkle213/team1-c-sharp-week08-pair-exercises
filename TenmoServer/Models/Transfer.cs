using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TenmoServer.Models
{
    public class Transfer
    {
        public int TransferId { get; set; }

        public string Direction { get; set; }

        public string Username { get; set; }

        public decimal TransferAmount { get; set; }
    }
}
