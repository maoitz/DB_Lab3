using System;
using System.Collections.Generic;

namespace DB3.Models;

public partial class StudentGrade
{
    public string FirstName { get; set; } = null!;

    public string LastName { get; set; } = null!;

    public string SubjectName { get; set; } = null!;

    public string Grade { get; set; } = null!;

    public string Teacher { get; set; } = null!;

    public DateOnly DateSet { get; set; }
}
