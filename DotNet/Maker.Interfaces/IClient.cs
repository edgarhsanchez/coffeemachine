using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Maker.Interfaces.DTOs;

namespace Maker.Interfaces
{
    public interface IClient
    {
        Task<bool> IsBusy();
        Task<bool> StartNewCup(RequestCup requestCup);
        Task<IEnumerable<Order>> GetPastOrders();
    }
}
