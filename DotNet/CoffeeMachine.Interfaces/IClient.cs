using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using CoffeeMachine.Interfaces.DTOs;

namespace CoffeeMachine.Interfaces
{
    public interface IClient
    {
        Task<bool> IsBusy();
        Task<bool> StartNewCup(RequestCup requestCup);
    }
}
