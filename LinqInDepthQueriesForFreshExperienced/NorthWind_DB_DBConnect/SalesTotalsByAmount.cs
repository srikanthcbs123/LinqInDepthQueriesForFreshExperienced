﻿using System;
using System.Collections.Generic;

namespace LinqInDepthQueriesForFreshExperienced.NorthWind_DB_DBConnect;

public partial class SalesTotalsByAmount
{
    public decimal? SaleAmount { get; set; }

    public int OrderId { get; set; }

    public string CompanyName { get; set; } = null!;

    public DateTime? ShippedDate { get; set; }
}