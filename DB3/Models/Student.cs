using System;
using System.Collections.Generic;

namespace DB3.Models;

public partial class Student
{
    public int StudentId { get; set; }

    public string FirstName { get; set; } = null!;

    public string LastName { get; set; } = null!;

    public string Ssn { get; set; } = null!;

    public int Class { get; set; }

    public virtual Class ClassNavigation { get; set; } = null!;

    public virtual ICollection<Grade> Grades { get; set; } = new List<Grade>();
}
