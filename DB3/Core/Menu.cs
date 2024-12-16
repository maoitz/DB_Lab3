
// Description:
// This class is responsible for displaying the menu options to the user and returning the selected option.
// The menu options are stored in an enum and a dictionary is used to link each option to a specific text.
// The menu options are divided into different categories with their own methods to return the options.
// The ShowMenu method is universal, and it is possible to use anywhere in the program.

// PSA!
// Everything here is made with the intention of being easily expandable and adjustable.

namespace DB3.Core;

public static class Menu
{
    // Enum for all menu options (Adjustable)
    public enum Options
    {
        // Main Menu
        ManageEmployees = 1, // Start at 1
        ManageStudents,
        ManageClasses,

        // Employee Menu
        ViewAllEmployees,
        AddNewEmployee,
        DeleteEmployee,

        // Student Menu
        ViewAllStudents,
        AddNewStudent,
        DeleteStudent,
        
        // Student Submenu - Grades
        SubjectInfo,
        Grades,

        // Class Menu
        ViewAllClasses,

        // Other
        Exit,
        Back
    }

    // Dictionary linking each option to a specific text
    private static readonly Dictionary<Options, string> Texts = new Dictionary<Options, string>
    {
        // Main Menu
        { Options.ManageEmployees, "Manage Employees" },
        { Options.ManageStudents, "Manage Students" },
        { Options.ManageClasses, "Manage Classes" },

        // Employee Menu
        { Options.ViewAllEmployees, "View All Employees" },
        { Options.AddNewEmployee, "Add New Employee" },
        { Options.DeleteEmployee, "Delete Employee" },

        // Student Menu
        { Options.ViewAllStudents, "View All Students" },
        { Options.AddNewStudent, "Add New Student" },
        { Options.DeleteStudent, "Delete Student" },
        { Options.SubjectInfo, "All Subjects" },
        { Options.Grades, "Grades"},

        // Class Menu
        { Options.ViewAllClasses, "View All Classes" },

        // Other
        { Options.Exit, "Exit" },
        { Options.Back, "Back" }
    };

    // Get the text for a specific option
    private static string GetText(Options option)
    {
        return Texts[option];
    }

    // Array of all main menu options (Add more options here)
    public static Options[] GetMainMenuOptions()
    {
        Options[] option = new Options[4];
        option[0] = Options.ManageEmployees;
        option[1] = Options.ManageStudents;
        option[2] = Options.ManageClasses;
        option[3] = Options.Exit;
        return option;
    }

    // Array of all employee menu options (Add more options here)
    public static Options[] GetEmployeeMenuOptions()
    {
        Options[] option = new Options[4];
        option[0] = Options.ViewAllEmployees;
        option[1] = Options.AddNewEmployee;
        option[2] = Options.DeleteEmployee;
        option[3] = Options.Back;
        return option;
    }

    // Array of all student menu options (Add more options here)
    public static Options[] GetStudentMenuOptions()
    {
        Options[] option = new Options[6];
        option[0] = Options.ViewAllStudents;
        option[1] = Options.AddNewStudent;
        option[2] = Options.DeleteStudent;
        option[3] = Options.SubjectInfo;
        option[4] = Options.Grades;
        option[5] = Options.Back;
        return option;
    }

    // Array of all class menu options (Add more options here)
    public static Options[] GetClassMenuOptions()
    {
        Options[] option = new Options[2];
        option[0] = Options.ViewAllClasses;
        option[1] = Options.Back;
        return option;
    }

    // Show the menu and return the selected option
    public static Options ShowMenu(string title, Menu.Options[] options)
    {
        while (true) // Loop until a valid option is selected
        {
            Console.Clear();
            Logo.PrintAppLogo();
            Console.WriteLine(title);
            Console.WriteLine();

            for (int i = 0; i < options.Length; i++)
            {
                Console.WriteLine($"[{i + 1}] {GetText(options[i])}");
            }
            
            var choice = GetMenuChoice(options.Length);
            if (choice != -1)
            {
                return options[choice - 1];
            }

            InvalidOption();
        }
    }
    
    // Get the user's choice from the menu
    public static int GetMenuChoice(int maxOptions)
    {
        Console.WriteLine();
        Console.Write("[ ]");
        Console.CursorVisible = true;
        Console.SetCursorPosition(1, Console.CursorTop);
        var keyInfo = Console.ReadKey(true);
        Console.SetCursorPosition(0, Console.CursorTop);
        return ValidateOption(keyInfo, maxOptions);
    }

    // Validate the user input
    private static int ValidateOption(ConsoleKeyInfo keyInfo, int max)
    {
        if (int.TryParse(keyInfo.KeyChar.ToString(), out int choice) && choice > 0 && choice <= max)
        {
            return choice;
        }

        return -1;
    }
    
    // Error message for invalid option
    public static void InvalidOption()
    {
        Console.WriteLine("Invalid option. Please try again.");
        Console.CursorVisible = false;
        Thread.Sleep(600);
    }
}