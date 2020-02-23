
using System;

namespace CoffeeMachine.Interfaces.DTOs
{
    public class Order
    {
        public int Id { get; set; }
        public string Coffee { get; set; }
        public DateTime Started { get; set; }
    }
}