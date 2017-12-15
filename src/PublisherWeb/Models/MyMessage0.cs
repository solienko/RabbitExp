using EventBus.Events;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PublisherWeb.Models
{
    public class MyMessage0 : IntegrationEvent
    {
        public Inner InnerProp { get; set; }
    }

    public class Inner
    {
        public string Name { get; set; }
        public int Count { get; set; }
    }
}
