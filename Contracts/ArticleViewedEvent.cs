using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Contracts
{
    public record ArticleViewedEvent
    {
        public Guid Id { get; set; }
        public DateTime ViewedOnUtc { get; set; }
    }
}
