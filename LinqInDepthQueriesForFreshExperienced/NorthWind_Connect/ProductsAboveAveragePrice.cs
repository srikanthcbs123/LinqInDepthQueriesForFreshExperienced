using System;
using System.Collections.Generic;

namespace LinqInDepthQueriesForFreshExperienced.NorthWind_Connect;

public partial class ProductsAboveAveragePrice
{
    public string ProductName { get; set; } = null!;

    public decimal? UnitPrice { get; set; }
}
