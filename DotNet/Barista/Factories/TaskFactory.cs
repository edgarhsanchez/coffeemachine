using System;
using System.Threading;
using System.Threading.Tasks;
using Barista.ExternalServices;
using Barista.Interfaces.DTOs;
using Microsoft.Extensions.Logging;

namespace Barista.Factories {

    public static class TaskFactory {
        public static Func<CancellationToken, Task> CreateMakeCoffeeJob(ILogger logger, CoffeeMachine.Interfaces.IClient coffeeMachineClient, Order order, IBackgroundTaskQueue queue) {

            var newTask = new Func<CancellationToken, Task>(async token => {
                try
                {
                    var isBusy = true;
                    while(isBusy) {
                        isBusy = await coffeeMachineClient.IsBusy();
                        if(isBusy) {
                            await Task.Delay(TimeSpan.FromSeconds(5));
                        }
                    }
                   
                    var orderProcessed = await coffeeMachineClient.StartNewCup(new CoffeeMachine.Interfaces.DTOs.RequestCup
                    {
                        Id = order.Id,
                        Coffee = order.Coffee
                    });

                    if(!orderProcessed) {
                        await Task.Delay(TimeSpan.FromSeconds(10));
                        queue.QueueBackgroundWorkItem(order, TaskFactory.CreateMakeCoffeeJob(logger, coffeeMachineClient, order, queue));
                    }
                    
                }
                catch (Exception ex)
                {
                    logger.LogError(ex, ex.Message);
                    await Task.Delay(TimeSpan.FromSeconds(10));
                }
            });
            return newTask;
        }
    }
}