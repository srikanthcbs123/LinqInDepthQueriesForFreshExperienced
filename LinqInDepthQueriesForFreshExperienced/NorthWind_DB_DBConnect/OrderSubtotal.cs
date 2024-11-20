using System;
using System.Collections.Generic;

namespace LinqInDepthQueriesForFreshExperienced.NorthWind_DB_DBConnect;

public partial class OrderSubtotal
{
    public int OrderId { get; set; }

    public decimal? Subtotal { get; set; }
}
