using System;
using System.Collections.Generic;

public class EmployeePromotion
{
    public static void RunPromotion()
    {
        List<string> employeeNames = new List<string>();
        bool exit = false;

        while (!exit)
        {
            Console.WriteLine("\n----- Employee Promotion Menu -----");
            Console.WriteLine("1. Enter employee names (in promotion order)");
            Console.WriteLine("2. Check promotion position for an employee");
            Console.WriteLine("3. Show current collection capacity and trim extra space");
            Console.WriteLine("4. Display promoted employee list in ascending order");
            Console.WriteLine("5. Exit");
            Console.Write("Enter your choice: ");
            string choice = Console.ReadLine() ?? string.Empty;

            switch (choice)
            {
                case "1":
                    // Clear the list if re-entering names.
                    employeeNames.Clear();
                    Console.WriteLine("\nPlease enter the employee names in the order of their eligibility for promotion (Enter blank to stop):");
                    
                    while (true)
                    {
                        string name = Console.ReadLine() ?? string.Empty;
                        if (string.IsNullOrEmpty(name))
                            break;
                        employeeNames.Add(name);
                    }
                    break;

                case "2":
                    // Find the promotion position for a given employee by name.
                    Console.Write("\nPlease enter the name of the employee to check promotion position: ");
                    string searchName = Console.ReadLine() ?? string.Empty;
                    int index = employeeNames.IndexOf(searchName);
                    if (index >= 0)
                    {
                        Console.WriteLine($"\"{searchName}\" is in position {index + 1} for promotion.");
                    }
                    else
                    {
                        Console.WriteLine($"\"{searchName}\" was not found in the promotion list.");
                    }
                    break;

                case "3":
                    // Show the current capacity and then trim extra memory.
                    Console.WriteLine($"\nThe current size (capacity) of the collection is: {employeeNames.Capacity}");
                    employeeNames.TrimExcess();
                    Console.WriteLine($"The size after removing the extra space is: {employeeNames.Count}");
                    break;

                case "4":
                    // Display the promotion list sorted in ascending (alphabetical) order.
                    List<string> sortedList = new List<string>(employeeNames);
                    sortedList.Sort();
                    Console.WriteLine("\nPromoted employee list in ascending order:");
                    foreach (var emp in sortedList)
                    {
                        Console.WriteLine(emp);
                    }
                    break;

                case "5":
                    exit = true;
                    break;

                default:
                    Console.WriteLine("Invalid option. Please try again.");
                    break;
            }
        }

        Console.WriteLine("Exiting Employee Promotion application. Goodbye!");
    }
}
