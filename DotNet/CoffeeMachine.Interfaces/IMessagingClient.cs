using System.Threading.Tasks;

namespace CoffeeMachine.Interfaces {
    public interface IMessagingClient {
        Task StartNewCup(DTOs.RequestCup requestCup);
    }
}