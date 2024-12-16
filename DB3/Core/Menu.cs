namespace DB3;

// Description:
// This class is responsible for displaying the menu options to the user and returning the selected option.
// The menu options are stored in an enum and a dictionary is used to link each option to a specific text.
// The menu options are divided into different categories with their own methods to return the options.
// The ShowMenu method is universal, and it is possible to use anywhere in the program.

// PSA!
// Everything here is made with the intention of being easily expandable and adjustable.

public class Menu
{
    // Enum for all menu options (Adjustable)
    public enum Options
    {
        // Main Menu
        ManageEmployees,
        ManageStudents,
        ManageClasses,
        
        // Employee Menu
        ViewAllEmployees,
        
        // Student Menu
        
        // Class Menu
        
        // Other
        Exit,
        Back
    }
    
    // Dictionary linking each option to a specific text
    public static Dictionary<Options, string> Texts = new Dictionary<Options, string>
    {
        // Main Menu
        { Options.ManageEmployees, "Manage Employees" },
        { Options.ManageStudents, "Manage Students" },
        { Options.ManageClasses, "Manage Classes" },
        
        // Employee Menu
        { Options.ViewAllEmployees, "View All Employees" },
        
        // Student Menu
        
        // Class Menu
        
        // Other
        { Options.Exit, "Exit" },
        { Options.Back, "Back" }
    };
    
    // Get the text for a specific option
    public static string GetText(Options option)
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
        Options[] option = new Options[2];
        option[0] = Options.ViewAllEmployees;
        option[1] = Options.Back;
        return option;
    }
    
    // Array of all student menu options (Add more options here)
    public static Options[] GetStudentMenuOptions()
    {
        Options[] option = new Options[2];
        option[0] = Options.Back;
        return option;
    }
    
    // Array of all class menu options (Add more options here)
    public static Options[] GetClassMenuOptions()
    {
        Options[] option = new Options[2];
        option[0] = Options.Back;
        return option;
    }

    // Show the menu and return the selected option
    public Options ShowMenu(string title, Menu.Options[] options)
    {

        while (true) // Loop until a valid option is selected
        {
            Console.Clear();
            Console.WriteLine(title);
            Console.WriteLine();

            for (int i = 0; i < options.Length; i++)
            {
                Console.WriteLine($"[{i + 1}] {GetText(options[i])}");
            }
            

            Console.WriteLine();
            Console.Write("[ ]");
            Console.CursorVisible = true;
            Console.SetCursorPosition(1, Console.CursorTop);
            var choice = ValidateOption(Console.ReadLine(), options.Length);
            
            if (choice != -1)
            { 
                return options[choice];
            }
            
            Console.WriteLine("Invalid choice. Please try again.");
            Console.CursorVisible = false;
            Thread.Sleep(1000);
        }
    }
    
    // Validate the user input
    private int ValidateOption(string input, int max)
    {
        if (int.TryParse(input, out int choice) && choice > 0 && choice <= max)
        {
            return choice;
        }
        return -1;
    }
}