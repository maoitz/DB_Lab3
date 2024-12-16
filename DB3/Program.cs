using DB3.Core;
using DB3.Models;
using Microsoft.EntityFrameworkCore;

// M.Almstedt
// 2024-12-10

/*---------- TO-DO List ---------------
Employee positions are hardcoded in the database: Make it possible to add new positions (dynamic)
Classes are hardcoded in the database: Make it possible to add new classes (dynamic)
Change Grading scale to numeric value -- DONE (ish)
Move grades and subjects to separate classes/Class.cs -- Maybe not necessary
Re-structure the code so properties of DB objects are (not hardcoded and) readable if null
FIX BUG IN: StudentManager.cs - ViewAllStudents() - Sorting -- DONE
Set methods to public/private correctly
---------------------------------------*/

namespace DB3;

public static class Program
{
    public static void Main(string[] args)
    {
        Application.Start();
    }
}