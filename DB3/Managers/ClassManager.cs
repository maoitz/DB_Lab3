using DB3.Core;
using DB3.Models;
using Microsoft.EntityFrameworkCore;

// Description:
// This class is responsible for managing school classes.
// The class has a method to view all classes and a method to view all students from specific class.
// The class is accessed from the main menu.

namespace DB3.Managers;

public static class ClassManager
{
    public static void ManageClasses()
    {
        const bool isRunning = true;
        while (isRunning)
        {
            var choice = Menu.ShowMenu("| Class Menu |", Menu.GetClassMenuOptions());
            switch (choice)
            {
                case Menu.Options.ViewAllClasses:
                    ViewAllClasses();
                    break;
                case Menu.Options.Back:
                    return;
            }
        }
    }
    
    // Method to read all classes with the option to view class info
    private static void ViewAllClasses()
    {
        const bool isRunning = true;
        while (isRunning)
        {
            Console.Clear();
            using var db = new AppDbContext();
            var classes = db.Classes.ToList();
            Console.WriteLine("ID\tName");
            foreach (var c in classes)
            {
                Console.WriteLine($"{c.ClassId}\t{c.ClassName}");
            }
            Console.WriteLine("-----------------------------");
            Console.WriteLine("View class info");
            Console.WriteLine("[1] 7A");
            Console.WriteLine("[2] 7B");
            Console.WriteLine("[3] 7C");
            Console.WriteLine("[4] Back");
            var choice = Menu.GetMenuChoice(4);

            switch (choice)
            {
                case 1:
                    ViewClassInfo("7A");
                    break;
                case 2:
                    ViewClassInfo("7B");
                    break;
                case 3:
                    ViewClassInfo("7C");
                    break;
                case 4:
                    return;
                default:
                    Menu.InvalidOption();
                    break;
            }            
            
        }
    }

    // Method to view all students in a class
    private static void ViewClassInfo(string choice)
    {
        Console.Clear();
        using var db = new AppDbContext();
        var students = db.Students.Include(c => c.ClassNavigation).Where(s => s.ClassNavigation.ClassName == choice).ToList();
        Console.WriteLine("S-ID\tName \t\tClass");
        foreach (var student in students)
        {
            Console.WriteLine($"{student.StudentId}\t{student.FirstName} {student.LastName}\t{student.ClassNavigation.ClassName}");
        }
        Console.WriteLine("-----------------------------");
        Console.WriteLine("Press any key to continue...");
        Console.ReadKey();
    }
}