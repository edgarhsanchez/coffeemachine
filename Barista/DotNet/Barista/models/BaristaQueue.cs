using System;
using System.Runtime;
using System.Collections.Concurrent;


namespace Barista.Models
{



    public static class Queue
    {
        private static Lazy<ConcurrentDictionary<int, Barista.Models.Order>> _queue = null;
        public static ConcurrentDictionary<int, Barista.Models.Order> Current
        {
            get
            {
                if (_queue == null)
                {
                    _queue = new Lazy<ConcurrentDictionary<int, Barista.Models.Order>>();
                }
                return _queue.Value;
            }
        }
    }
}
