using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using CoffeeMachine.Interfaces.DTOs;

namespace CoffeeMachine.Interfaces
{
    public interface IClient
    {
        Task<bool> IsBusy(Uri uri);
        Task<bool> StartNewCup(Uri uri, RequestCup requestCup);
        Task<IEnumerable<Uri>> Services();
    }
}
