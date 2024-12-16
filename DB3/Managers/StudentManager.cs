using DB3.Core;
using DB3.Models;
using Microsoft.EntityFrameworkCore;

// Description:
// This class is responsible for managing students.
// The class has methods to view all students, add a new student, delete a student, view subject info, and view grades.
// View all students has the option to sort by first name, last name, or class.
// View subject info has a grade mapping allowing for average, highest, and lowest grade using A-F scale.
// View grades shows all grades set in the last 30 days.
// The class is accessed from the main menu.

namespace DB3.Managers;

public static class StudentManager
{
    public static void ManageStudents()
    {
        const bool isRunning = true;
        while (isRunning)
        {
            var choice = Menu.ShowMenu("| Student Menu |", Menu.GetStudentMenuOptions());
            switch (choice)
            {
                case Menu.Options.ViewAllStudents:
                    ViewAllStudents();
                    break;
                case Menu.Options.AddNewStudent:
                    AddNewStudent();
                    break;
                case Menu.Options.DeleteStudent:
                    DeleteStudent();
                    break;
                case Menu.Options.SubjectInfo:
                    ViewAllSubjects();
                    break;
                case Menu.Options.Grades: // All Grades set in the last 30 days
                    ViewGrades();
                    break;
                case Menu.Options.Back:
                    return;
            }
        }
    }

    // Method to view all students with the option to sort by first name, last name, or class
    private static void ViewAllStudents()
    {
        var isRunning = true;
        List<Student>? students = null;

        while (isRunning)
        {
            if (students == null)
            {
                students = GetStudents();
            }

            PrintStudents(students);

            Console.WriteLine("Sort by:");
            Console.WriteLine("[1] First Name");
            Console.WriteLine("[2] Last Name");
            Console.WriteLine("[3] Class");
            Console.WriteLine("[4] Back");
            var choice = Menu.GetMenuChoice(4);
            if (choice == 4)
            {
                isRunning = false;
            }
            else if (choice is > 0 and < 4)
            {
                while (true)
                {
                    Console.Clear();
                    Console.WriteLine("[1] Ascending");
                    Console.WriteLine("[2] Descending");
                    var sortChoice = Menu.GetMenuChoice(2);
                    if (sortChoice is 1 or 2)
                    {
                        students = SortStudents(choice, sortChoice);
                        break;
                    }

                    Menu.InvalidOption();
                }
            }
            else
            {
                Menu.InvalidOption();
            }
        }
    }

    // Method to get all students from the database to be able to store them in a variable
    private static List<Student> GetStudents()
    {
        using var db = new AppDbContext();
        return db.Students.Include(s => s.ClassNavigation).ToList();
    }

    // Method to print all students to the console
    private static void PrintStudents(List<Student> students)
    {
        Console.Clear();
        Console.WriteLine("S-ID\tName \t\tClass");
        foreach (var student in students)
        {
            Console.WriteLine(
                $"{student.StudentId}\t{student.FirstName} {student.LastName}\t{student.ClassNavigation.ClassName}");
        }

        Console.WriteLine("-----------------------------");
    }

    // Method to sort students by first name, last name, or class depending on the user input
    private static List<Student> SortStudents(int choice, int sortChoice)
    {
        using var db = new AppDbContext();
        return choice switch
        {
            1 => sortChoice == 1
                ? db.Students.Include(s => s.ClassNavigation).OrderBy(s => s.FirstName).ToList()
                : db.Students.Include(s => s.ClassNavigation).OrderByDescending(s => s.FirstName).ToList(),
            2 => sortChoice == 1
                ? db.Students.Include(s => s.ClassNavigation).OrderBy(s => s.LastName).ToList()
                : db.Students.Include(s => s.ClassNavigation).OrderByDescending(s => s.LastName).ToList(),
            3 => sortChoice == 1
                ? db.Students.Include(s => s.ClassNavigation).OrderBy(s => s.ClassNavigation.ClassName).ToList()
                : db.Students.Include(s => s.ClassNavigation).OrderByDescending(s => s.ClassNavigation.ClassName)
                    .ToList(),
            _ => new List<Student>()
        };
    }

    // Method to add a new student
    private static void AddNewStudent()
    {
        Console.Clear();
        Console.WriteLine("| Add New Student |");
        Console.Write("First Name: ");
        var firstName = Console.ReadLine();
        Console.Write("Last Name: ");
        var lastName = Console.ReadLine();
        Console.Write("Social Security Number(SSN): "); // Add a way to validate the SSN(format) against the database
        var ssn = Console.ReadLine();

        // Submenu to select the position of the employee
        var isRunning = true;
        while (isRunning)
        {
            Console.Clear();
            Console.WriteLine("Add to class:");
            Console.WriteLine("[1] 7A");
            Console.WriteLine("[2] 7B");
            Console.WriteLine("[3] 7C");
            var choice = Menu.GetMenuChoice(3);
            if (choice is 1 or 2 or 3)
            {
                string className = choice switch
                {
                    1 => "7A",
                    2 => "7B",
                    3 => "7C",
                    _ => ""
                };
                using var db = new AppDbContext();
                var classEntity = db.Classes.First(c => c.ClassName == className);
                if (classEntity != null)
                {
                    var student = new Student
                    {
                        FirstName = firstName,
                        LastName = lastName,
                        Ssn = ssn,
                        Class = classEntity.ClassId
                    };
                    db.Students.Add(student);
                    db.SaveChanges();
                    Console.WriteLine($"Student [{firstName}] added successfully!");
                    Console.WriteLine("\nPress any key to continue...");
                    Console.ReadKey();
                    isRunning = false;
                }
                else
                {
                    Console.WriteLine("Class not found!");
                    Console.ReadKey();
                }
            }
            else
            {
                Menu.InvalidOption();
            }
        }
    }
    
    // Method to delete a student from the database and reset the auto-increment (WIP)
    private static void DeleteStudent()
    {
        Console.Clear();
        using var db = new AppDbContext();
        var students = db.Students
            .Include(s => s.ClassNavigation)
            .ToList();
        Console.WriteLine("| Delete Student |\n");
        Console.WriteLine("ID\tName\tClass");
        foreach (var student in students)
        {
            Console.WriteLine(
                $"{student.StudentId}\t{student.FirstName}\t{student.ClassNavigation.ClassName}");
        }
        Console.WriteLine("-----------------------------");
        Console.Write("Enter ID of student to delete: ");
        // Add error handling here! --
        var id = int.TryParse(Console.ReadLine(), out var studentId) ? studentId : 0;
        // --
        var studentToDelete = db.Students
            .First(s => s.StudentId == id);
        if (studentToDelete != null)
        {
            db.Students.Remove(studentToDelete);
            db.SaveChanges();
            Console.WriteLine($"Student [{studentToDelete.FirstName}] deleted successfully!");
            
            // Get the highest ID in the table
            var maxId = db.Students.Max(s => (int?)s.StudentId) ?? 0;
            
            // Reset auto-increment to the highest ID
            db.Database.ExecuteSql($"DBCC CHECKIDENT ('Student', RESEED, {maxId})");
        }
        else
        {
            Console.WriteLine("Student not found!");
        }
        Console.WriteLine("\nPress any key to continue...");
        Console.ReadKey();
    }

    // Method to show all grades set in the last 30 days
    private static void ViewGrades()
    {
        const bool isRunning = true;
        while (isRunning)
        {
            Console.Clear();
            using var db = new AppDbContext();
            var now = DateOnly.FromDateTime(DateTime.Now);
            var grades = db.Grades
                .Where<Grade>(g => g.DateSet >= now.AddDays(-30))
                .Include(grade => grade.Subject)
                .Include(grade => grade.Student)
                .ToList<Grade>();
            Console.WriteLine("| All Grades set in the last 30 days |\n");
            Console.WriteLine("Student\t\tSubject\t\tGrade");
            foreach (var grade in grades)
            {
                Console.WriteLine(
                    $"{grade.Student.FirstName} {grade.Student.LastName}\t{grade.Subject.SubjectName}\t\t{grade.Grade1}");
            }
            Console.WriteLine("-----------------------------");
            Console.WriteLine("\nPress any key to continue...");
            Console.ReadKey();
            return;
        }
    }
    
    // Method to view subject info with grade mapping allowing for average, highest, and lowest grade using A-F scale
    private static void ViewAllSubjects()
    {
        var gradeMapping = new Dictionary<string, double>
        {
            { "A", 5.0 },
            { "B", 4.0 },
            { "C", 3.0 },
            { "D", 2.0 },
            { "E", 1.0 },
            { "F", 0.0 }
        };

        var reverseGradeMapping = new Dictionary<double, string>
        {
            { 5.0, "A" },
            { 4.0, "B" },
            { 3.0, "C" },
            { 2.0, "D" },
            { 1.0, "E" },
            { 0.0, "F" }
        };

        Console.Clear();
        using var db = new AppDbContext();
        var subjects = db.Subjects
            .Include(s => s.Grades)
            .ToList();

        var subjectGrades = subjects.Select(s => new
        {
            SubjectName = s.SubjectName,
            AverageGrade = s.Grades.Any() ? s.Grades.Average(g => gradeMapping[g.Grade1]) : 0,
            HighestGrade = s.Grades.Any() ? s.Grades.Min(g => g.Grade1) : "N/A",
            LowestGrade = s.Grades.Any() ? s.Grades.Max(g => g.Grade1) : "N/A"
        }).ToList();

        Console.WriteLine("Subject\t Average Grade\tHighest\tLowest");
        foreach (var subject in subjectGrades)
        {
            var averageGrade = reverseGradeMapping[Math.Round(subject.AverageGrade)];
            Console.WriteLine($"{subject.SubjectName}\t {averageGrade}\t\t{subject.HighestGrade}\t{subject.LowestGrade}");
        }
        Console.WriteLine("---------------------------------------");
        Console.WriteLine("\nPress any key to continue...");
        Console.ReadKey();
    }
}