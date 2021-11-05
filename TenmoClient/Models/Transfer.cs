using System;
using System.Collections.Generic;
using System.Text;

namespace TenmoClient.Models
{
    public class Transfer
    {
        public int TransferId { get; set; }

        public int ReceiverId { get; set; }

        public string Direction { get; set; }

        public string Username { get; set; }

        public decimal TransferAmount { get; set; } 

        public string TransferStatus { get; set; }
        
        public string TransferType { get; set; }
        
        public string SenderName { get; set; }
        
        public string ReceiverName { get; set; }
    }
}
