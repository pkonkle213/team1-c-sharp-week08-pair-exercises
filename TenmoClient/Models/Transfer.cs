﻿using System;
using System.Collections.Generic;
using System.Text;

namespace TenmoClient.Models
{
    public class Transfer
    {
        public int TransferId { get; set; }

        public string Direction { get; set; }

        public string Username { get; set; }

        public decimal TransferAmount { get; set; }
    }
}
