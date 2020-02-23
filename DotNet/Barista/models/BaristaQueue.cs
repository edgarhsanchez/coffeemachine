using System;
using System.Runtime;
using System.Collections.Concurrent;
using Barista.Interfaces.DTOs;

namespace Barista.Models
{



    public class Queue
    {
        private static Lazy<ConcurrentDictionary<int, Order>> _queue = null;
        public static ConcurrentDictionary<int, Order> Current
        {
            get
            {
                if (_queue == null)
                {
                    _queue = new Lazy<ConcurrentDictionary<int, Order>>();
                }
                return _queue.Value;
            }
        }
    }
}
