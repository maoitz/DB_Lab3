using System;
using System.Collections.Generic;

namespace DB3.Models;

public partial class Grade
{
    public int GradeId { get; set; }

    public int StudentId { get; set; }

    public int SubjectId { get; set; }

    public string Grade1 { get; set; } = null!;

    public int EmployeeId { get; set; }

    public DateOnly DateSet { get; set; }

    public virtual Employee Employee { get; set; } = null!;

    public virtual Student Student { get; set; } = null!;

    public virtual Subject Subject { get; set; } = null!;
}
