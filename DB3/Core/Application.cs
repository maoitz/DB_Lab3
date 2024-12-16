using DB3.Core;
using DB3.Managers;

// Description:
// This class is responsible for starting the application and displaying the main menu.
// The main menu is used to navigate to different parts of the application.
// The user can choose to manage employees, students, classes, or exit the application.
// The main menu is displayed in a loop until the user chooses to exit.


namespace DB3.Core;

public static class Application
{
    public static void Start()
    {
        var isRunning = true;
        while (isRunning)
        {
            // Set the console title and display the main menu
            Console.Title = "School System Manager";
            var choice = Menu.ShowMenu("| Main Menu |", Menu.GetMainMenuOptions());
            switch (choice)
            {
                case Menu.Options.ManageEmployees:
                    EmployeeManager.ManageEmployees();
                    break;
                case Menu.Options.ManageStudents:
                    StudentManager.ManageStudents();
                    break;
                case Menu.Options.ManageClasses:
                    ClassManager.ManageClasses();
                    break;
                case Menu.Options.Exit:
                    Console.WriteLine("Exiting...");
                    Thread.Sleep(500);
                    return;
            }
        }
    }
}