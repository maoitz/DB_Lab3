using System;
using System.Collections.Generic;

namespace DB3.Models;

public partial class EmployeePosition
{
    public int PositionId { get; set; }

    public string PositionName { get; set; } = null!;

    public virtual ICollection<Employee> Employees { get; set; } = new List<Employee>();
}
