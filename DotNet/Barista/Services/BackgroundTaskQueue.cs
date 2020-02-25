using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Barista.Interfaces.DTOs;

public interface IBackgroundTaskQueue
{
    void QueueBackgroundWorkItem(Order order, Func<CancellationToken, Task> workItem);


    Task<Tuple<Func<CancellationToken, Task>, Order>> DequeueAsync(
        CancellationToken cancellationToken);

    List<Order> QueuedOrders();
}

public class BackgroundTaskQueue : IBackgroundTaskQueue
{
    private ConcurrentQueue<Func<CancellationToken, Task>> _workItems = 
        new ConcurrentQueue<Func<CancellationToken, Task>>();
    private SemaphoreSlim _signal = new SemaphoreSlim(0);

    private ConcurrentDictionary<Func<CancellationToken, Task>, Order> _dictOrders = new ConcurrentDictionary<Func<CancellationToken, Task>, Order>();

    public void QueueBackgroundWorkItem(Order order,
        Func<CancellationToken, Task> workItem)
    {
        if (workItem == null)
        {
            throw new ArgumentNullException(nameof(workItem));
        }

        _workItems.Enqueue(workItem);
        _dictOrders[workItem] = order;
        _signal.Release();
    }

    public List<Order> QueuedOrders() {
        return _dictOrders.Values.ToList();
    }

    public async Task<Tuple<Func<CancellationToken, Task>, Order>> DequeueAsync(
        CancellationToken cancellationToken)
    {
        await _signal.WaitAsync(cancellationToken);
        _workItems.TryDequeue(out var workItem);
        Order order;
        _dictOrders.TryRemove(workItem, out order);
        return new Tuple<Func<CancellationToken, Task>, Order>(workItem, order);
    }
}