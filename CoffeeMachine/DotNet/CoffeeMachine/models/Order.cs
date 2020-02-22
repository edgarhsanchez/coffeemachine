using System;

namespace CoffeeMachine.Models {


    public class RequestCup {
        public int Id {get;set;}
        public string Coffee {get;set;}
    }

    public class Order {
        public int Id {get;set;}
        public string Coffee {get;set;}
        public DateTime Started {get;set;}
    }
}