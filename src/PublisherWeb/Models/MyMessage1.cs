using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PublisherWeb.Models
{
    public class MyMessage1
    {
        public string Id { get; set; } = Guid.NewGuid().ToString();
        public DateTime Time { get; set; } = DateTime.UtcNow;
        public string StringProp { get; set; }
    }
}
