using System;
using System.Collections.Generic;

namespace LinqInDepthQueriesForFreshExperienced.NorthWind_DB_DBConnect;

public partial class CustomerDemographic
{
    public string CustomerTypeId { get; set; } = null!;

    public string? CustomerDesc { get; set; }

    public virtual ICollection<Customer> Customers { get; set; } = new List<Customer>();
}
