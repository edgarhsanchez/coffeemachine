using System.Threading.Tasks;

namespace Maker.Interfaces
{
    public interface IMessagingClient
    {
        Task StartNewCup(DTOs.RequestCup requestCup);
    }
}