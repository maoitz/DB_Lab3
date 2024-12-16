using DB3.Models;
using Microsoft.EntityFrameworkCore;

// Description:

namespace DB3;

public class EmployeeManager
{
    public static void ManageEmployees()
    {
        var isRunning = true;
        while (isRunning)
        {
            var choice = Menu.ShowMenu("| Employee Menu |", Menu.GetEmployeeMenuOptions());
            switch (choice)
            {
                case Menu.Options.ViewAllEmployees:
                    ViewAllEmployees();
                    break;
                case Menu.Options.AddNewEmployee:
                    AddNewEmployee();
                    break;
                case Menu.Options.DeleteEmployee:
                    DeleteEmployee();
                    break;
                case Menu.Options.Back:
                    return;
            }
        }
    }
    
    // Method to view all employees and the option to view employees by position
    private static void ViewAllEmployees()
    {
        var isRunning = true;
        while (isRunning)
        {
            Console.Clear();
            using var db = new AppDbContext();
            // Include the PositionNavigation property to get the PositionName instead of the PositionId
            var employees = db.Employees
                .Include(e => e.PositionNavigation)
                .ToList();
            Console.WriteLine("| All Employees |\n");
            Console.WriteLine("ID\tName\t Position");
            foreach (var employee in employees)
            {
                Console.WriteLine(
                    $"{employee.EmployeeId}\t{employee.FirstName}\t {employee.PositionNavigation.PositionName}");
            }
            Console.WriteLine("-----------------------------");

            // Submenu to view employees by position (Can be put into Menu.cs)
            Console.WriteLine("\nView employees by position");
            Console.WriteLine("[1] Teacher");
            Console.WriteLine("[2] Administrator");
            Console.WriteLine("[3] Principal");
            Console.WriteLine("[4] Back");
            var choice = Menu.GetMenuChoice(4);
            switch (choice)
            {
                case 1:
                    ViewEmployeeByPosition("Teacher");
                    break;
                case 2:
                    ViewEmployeeByPosition("Administrator");
                    break;
                case 3:
                    ViewEmployeeByPosition("Principal");
                    break;
                case 4:
                    isRunning = false;
                    break;
                default:
                    Menu.InvalidOption();
                    break;
            }
        }
    }

    // Method to view employees by position
    private static void ViewEmployeeByPosition(string position)
    {
        Console.Clear();
        using var db = new AppDbContext();
        var employees = db.Employees
            .Include(e => e.PositionNavigation)
            .Where(e => e.PositionNavigation.PositionName == position)
            .ToList();
        Console.WriteLine("ID\tName\tPosition");
        foreach (var employee in employees)
        {
            Console.WriteLine($"{employee.EmployeeId}\t{employee.FirstName}\t{employee.PositionNavigation.PositionName}");
        }
        Console.WriteLine("\nPress any key to continue...");
        Console.ReadKey();
    }

    // Method to add a new employee stored in the database
    public static void AddNewEmployee()
    {
        Console.Clear();
        Console.WriteLine("| Add New Employee |");
        Console.Write("First Name: ");
        var firstName = Console.ReadLine();
        Console.Write("Last Name: ");
        var lastName = Console.ReadLine();

        // Submenu to select the position of the employee
        var isRunning = true;
        while (isRunning)
        {
            Console.Clear();
            Console.WriteLine("Select position:");
            Console.WriteLine("[1] Teacher");
            Console.WriteLine("[2] Administrator");
            Console.WriteLine("[3] Principal");
            var choice = Menu.GetMenuChoice(3);
            if (choice == 1 || choice == 2 || choice == 3)
            {
                string position = choice switch
                {
                    1 => "Teacher",
                    2 => "Administrator",
                    3 => "Principal",
                    _ => ""
                };
                using var db = new AppDbContext();
                var employee = new Employee
                {
                    FirstName = firstName,
                    LastName = lastName,
                    Position = db.EmployeePositions.FirstOrDefault(p => p.PositionName == position).PositionId
                };
                db.Employees.Add(employee);
                db.SaveChanges();
                Console.WriteLine($"Employee [{firstName}] added successfully!");
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
    
    // Method to delete an employee from the database and reset the auto-increment
    private static void DeleteEmployee()
    {
        Console.Clear();
        using var db = new AppDbContext();
        var employees = db.Employees
            .Include(e => e.PositionNavigation)
            .ToList();
        Console.WriteLine("| Delete Employee |\n");
        Console.WriteLine("ID\tName\tPosition");
        foreach (var employee in employees)
        {
            Console.WriteLine(
                $"{employee.EmployeeId}\t{employee.FirstName}\t{employee.PositionNavigation.PositionName}");
        }
        Console.WriteLine("-----------------------------");
        Console.Write("Enter ID of employee to delete: ");
        var id = Menu.GetMenuChoice(employees.Count);
        var employeeToDelete = db.Employees.FirstOrDefault(e => e.EmployeeId == id);
        if (employeeToDelete != null)
        {
            db.Employees.Remove(employeeToDelete);
            db.SaveChanges();
            Console.WriteLine($"Employee [{employeeToDelete.FirstName}] deleted successfully!");
            
            // Get the highest ID in the table
            var maxId = db.Employees.Max(e => (int?)e.EmployeeId) ?? 0;
            
            // Reset auto-increment to the highest ID
            db.Database.ExecuteSqlRaw($"DBCC CHECKIDENT ('Employee', RESEED, {maxId})");
        }
        else
        {
            Console.WriteLine("Employee not found!");
        }
        Console.WriteLine("\nPress any key to continue...");
        Console.ReadKey();
    }
}