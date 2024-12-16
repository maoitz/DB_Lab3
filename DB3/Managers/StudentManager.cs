using DB3.Models;
using Microsoft.EntityFrameworkCore;

// Description:

namespace DB3;

public class StudentManager
{
    public static void ManageStudents()
    {
        var isRunning = true;
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

    // Method to view all students and the option to sort them
    private static void ViewAllStudents(bool? isAlreadySorted = false)
    {
        var isRunning = true;
        var isSorted = false; // Flag to allow initial read of all students

        while (isRunning)
        {
            if (!isSorted)
            {
                Console.Clear();
                using var db = new AppDbContext();
                var students = db.Students.Include(s => s.ClassNavigation).ToList();
                Console.WriteLine("S-ID\tName \t\tClass");
                foreach (var student in students)
                {
                    Console.WriteLine(
                        $"{student.StudentId}\t{student.FirstName} {student.LastName}\t{student.ClassNavigation.ClassName}");
                }

                Console.WriteLine("-----------------------------");
            }
            else
            {
                
            }

            // Submenu to sort students (Can be put into Menu.cs)
            Console.WriteLine("Sort by:");
            Console.WriteLine("[1] First Name");
            Console.WriteLine("[2] Last Name");
            Console.WriteLine("[3] Class");
            Console.WriteLine("[4] Back");
            var choice = Menu.GetMenuChoice(4);

            if (choice == 4)
            {
                return;
            }
            
            if (choice > 0 && choice < 4)
            {
                while (true)
                {
                    Console.Clear();
                    isSorted = true;
                    Console.WriteLine("[1] Ascending");
                    Console.WriteLine("[2] Descending");
                    var sortChoice = Menu.GetMenuChoice(2);
                    if (sortChoice == 1 || sortChoice == 2)
                    {
                        SortStudent(choice, sortChoice);
                        break; // Break out of the loop if a valid sort choice is made
                    }
                    Menu.InvalidOption();
                }
            }
            else
            {
                // Fix bug: Does not handle console correctly when "invalid option" is chosen after sorting
                // Possible fix: Store sorted list in variable, print new variable instead of re-reading from database
                Menu.InvalidOption();
                
            }
        }
    }

    // Method to sort students based on user choice
    private static void SortStudent(int choice, int sortChoice)
    {
        Console.Clear();
        using var db = new AppDbContext();
        List<Student> students = null;
        
        switch (choice)
        {
            // Sort the list by using two different queries for each case depending on the sort choice
            
            case 1:
                students = sortChoice == 1
                    ? db.Students.Include(s => s.ClassNavigation).OrderBy(s => s.FirstName).ToList()
                    : db.Students.Include(s => s.ClassNavigation).OrderByDescending(s => s.FirstName).ToList();
                break;
            case 2:
                students = sortChoice == 1
                    ? db.Students.Include(s => s.ClassNavigation).OrderBy(s => s.LastName).ToList()
                    : db.Students.Include(s => s.ClassNavigation).OrderByDescending(s => s.LastName).ToList();
                break;
            case 3:
                students = sortChoice == 1
                    ? db.Students.Include(s => s.ClassNavigation).OrderBy(s => s.ClassNavigation.ClassName).ToList()
                    : db.Students.Include(s => s.ClassNavigation).OrderByDescending(s => s.ClassNavigation.ClassName).ToList();
                break;
        }
        
        Console.WriteLine("S-ID\tName \t\tClass");
        foreach (var student in students)
        {
            Console.WriteLine($"{student.StudentId}\t{student.FirstName} {student.LastName}\t{student.ClassNavigation.ClassName}");
        }
        Console.WriteLine("-----------------------------");
    }
    
    // Method to add a new student
    public static void AddNewStudent()
    {
        Console.Clear();
        Console.WriteLine("| Add New Student |");
        Console.Write("First Name: ");
        var firstName = Console.ReadLine();
        Console.Write("Last Name: ");
        var lastName = Console.ReadLine();
        Console.Write("Social Security Number(SSN): ");
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
            if (choice == 1 || choice == 2 || choice == 3)
            {
                string _class = choice switch
                {
                    1 => "7A",
                    2 => "7B",
                    3 => "7C",
                    _ => ""
                };
                using var db = new AppDbContext();
                var student = new Student()
                {
                    FirstName = firstName,
                    LastName = lastName,
                    Ssn = ssn,
                    Class = db.Classes.First(c => c.ClassName == _class).ClassId
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
                Menu.InvalidOption();
            }
        }
    }
    
    // Method to delete a student from the database and reset the auto-increment
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
        var id = Menu.GetMenuChoice(students.Count);
        var studentToDelete = db.Students.First(s => s.StudentId == id);
        if (studentToDelete != null)
        {
            db.Students.Remove(studentToDelete);
            db.SaveChanges();
            Console.WriteLine($"Student [{studentToDelete.FirstName}] deleted successfully!");
            
            // Get the highest ID in the table
            var maxId = db.Students.Max(s => (int?)s.StudentId) ?? 0;
            
            // Reset auto-increment to the highest ID
            db.Database.ExecuteSqlRaw($"DBCC CHECKIDENT ('Student', RESEED, {maxId})");
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
        var isRunning = true;
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
    
    // Method to view subject info
    private static void ViewAllSubjects()
    {
        Console.Clear();
        using var db = new AppDbContext();
        var subjects = db.Subjects.Include(s => s.Grades).ToList();
        
        Console.WriteLine("| Subject Information |\n");
        Console.WriteLine("Subject\tAverage\tHighest\tLowest");
        foreach (var subject in subjects)
        {
            Console.WriteLine(
                $"{subject.SubjectName}\t WIP\t{subject.Grades.Min(g => g.Grade1)}\t{subject.Grades.Max(g => g.Grade1)}");
        }

        Console.WriteLine("-----------------------------");
        Console.WriteLine("\nPress any key to continue...");
        Console.ReadKey();
    }
}