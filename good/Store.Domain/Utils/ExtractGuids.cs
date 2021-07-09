using System;
using System.Collections.Generic;
using System.Linq;
using Store.Domain.Commands;

namespace Store.Domain.Utils
{
    internal class ExtractGuids
    {
        internal static IEnumerable<Guid> Extract(IList<CreateOrderItemCommand> items)
        {
            return items.Select(x => x.Product);
        }
    }
}