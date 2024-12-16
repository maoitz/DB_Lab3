namespace DB3;

// Description:
// This class is responsible for starting the application and displaying the main menu.
// The main menu is used to navigate to different parts of the application.
// The user can choose to manage employees, students, classes, or exit the application.
// The main menu is displayed in a loop until the user chooses to exit.

public class Application
{
    public void Start()
    {
        Console.Title = "School Management System";
        var menu = new Menu();
        var choice = menu.ShowMenu("Main Menu", Menu.GetMainMenuOptions());
        switch (choice)
        {
            case Menu.Options.ManageEmployees:
                // ManageEmployees();
                break;
            case Menu.Options.ManageStudents:
                // ManageStudents();
                break;
            case Menu.Options.ManageClasses:
                // ManageClasses();
                break;
            case Menu.Options.Exit:
                return;
        }
    }
}