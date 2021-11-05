﻿using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TenmoServer.Models;

namespace TenmoServer.DAO
{
    public interface ITransferDAO
    {
        void Transfer(int userId, int destinationId, decimal transferAmount);

        List<Transfer> AllTransfers(int userId);

        Transfer SpecificTransfer(int transferId);
    }
}
