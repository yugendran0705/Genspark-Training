using System;

public class Program
{
    public static void Main()
    {
        bool exit = false;
        while (!exit)
        {
            Console.WriteLine("\n--- Main Menu ---");
            Console.WriteLine("Select an option:");
            Console.WriteLine("1. Employee Promotion (Easy)");
            Console.WriteLine("2. Employee Collection Manager (Medium)");
            Console.WriteLine("3. Employee Management Menu (Hard)");
            Console.WriteLine("4. Exit");
            Console.Write("Enter your choice: ");
            string choice = Console.ReadLine() ?? string.Empty;

            switch (choice)
            {
                case "1":
                    EmployeePromotion.RunPromotion();
                    break;
                case "2":
                    EmployeeCollectionManager.RunEmployeeCollectionManager();
                    break;
                case "3":
                    EmployeeManagementMenu.RunEmployeeManagementMenu();
                    break;
                case "4":
                    exit = true;
                    break;
                default:
                    Console.WriteLine("Invalid option. Please try again.");
                    break;
            }
        }

        Console.WriteLine("Exiting application. Goodbye!");
    }
}
