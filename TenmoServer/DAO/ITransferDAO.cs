using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TenmoServer.DAO
{
    public interface ITransferDAO
    {
        void Transfer(int userId, int destinationId, decimal transferAmount);
    }
}
