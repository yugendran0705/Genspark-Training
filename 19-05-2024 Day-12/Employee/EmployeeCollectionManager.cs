using System;
using System.Collections.Generic;
using System.Linq;

public class EmployeeCollectionManager
{
    public static void RunEmployeeCollectionManager()
    {
        Dictionary<int, Employee> employeeDict = new Dictionary<int, Employee>();

        bool exit = false;
        while (!exit)
        {
            Console.WriteLine("\n--- Employee Collection Manager Menu ---");
            Console.WriteLine("1. Add Employee(s)");
            Console.WriteLine("2. Display all Employees sorted by salary");
            Console.WriteLine("3. Find Employee by ID");
            Console.WriteLine("4. Find Employee(s) by Name");
            Console.WriteLine("5. Find Employee(s) elder than a given Employee (by ID)");
            Console.WriteLine("6. Exit");
            Console.Write("Enter your choice: ");
            string choice = Console.ReadLine() ?? string.Empty;

            switch (choice)
            {
                case "1":
                    AddEmployees(employeeDict);
                    break;
                case "2":
                    DisplaySortedEmployees(employeeDict);
                    break;
                case "3":
                    SearchEmployeeById(employeeDict);
                    break;
                case "4":
                    SearchEmployeesByName(employeeDict);
                    break;
                case "5":
                    SearchEmployeesElderThan(employeeDict);
                    break;
                case "6":
                    exit = true;
                    break;
                default:
                    Console.WriteLine("Invalid selection. Please try again.");
                    break;
            }
        }
        Console.WriteLine("Exiting Employee Collection Manager.");
    }

    static void AddEmployees(Dictionary<int, Employee> employeeDict)
    {
        Console.Write("How many employees do you want to enter? ");
        if (int.TryParse(Console.ReadLine(), out int n))
        {
            for (int i = 0; i < n; i++)
            {
                Console.WriteLine($"\nEnter details for employee {i + 1}:");
                Employee emp = new Employee();
                emp.TakeEmployeeDetailsFromUser();

                if (!employeeDict.ContainsKey(emp.Id))
                {
                    employeeDict.Add(emp.Id, emp);
                }
                else
                {
                    Console.WriteLine("Employee with this ID already exists. Skipping entry.");
                }
            }
        }
        else
        {
            Console.WriteLine("Invalid number entered.");
        }
    }

    static void DisplaySortedEmployees(Dictionary<int, Employee> employeeDict)
    {
        if (employeeDict.Count == 0)
        {
            Console.WriteLine("No employees in the collection.");
            return;
        }

        // Convert to list and sort using IComparable (by Salary)
        List<Employee> employeeList = employeeDict.Values.ToList();
        employeeList.Sort();

        Console.WriteLine("\nEmployees sorted by salary:");
        foreach (var emp in employeeList)
        {
            Console.WriteLine(emp);
            Console.WriteLine("------------------");
        }
    }

    static void SearchEmployeeById(Dictionary<int, Employee> employeeDict)
    {
        Console.Write("\nEnter an employee ID to search for details: ");
        if (int.TryParse(Console.ReadLine(), out int searchId))
        {
            var foundEmp = employeeDict.Values.FirstOrDefault(e => e.Id == searchId);
            if (foundEmp != null)
            {
                Console.WriteLine("\nEmployee found:");
                Console.WriteLine(foundEmp);
            }
            else
            {
                Console.WriteLine("\nEmployee not found with ID " + searchId);
            }
        }
        else
        {
            Console.WriteLine("Invalid ID entered.");
        }
    }

    static void SearchEmployeesByName(Dictionary<int, Employee> employeeDict)
    {
        Console.Write("\nEnter an employee name to search for: ");
        string searchName = Console.ReadLine() ?? string.Empty;

        var foundEmpsByName = employeeDict.Values
            .Where(e => e.Name.Equals(searchName, StringComparison.OrdinalIgnoreCase))
            .ToList();

        if (foundEmpsByName.Count > 0)
        {
            Console.WriteLine($"\nEmployees with the name \"{searchName}\":");
            foreach (var emp in foundEmpsByName)
            {
                Console.WriteLine(emp);
                Console.WriteLine("------------------");
            }
        }
        else
        {
            Console.WriteLine($"\nNo employees found with the name \"{searchName}\".");
        }
    }

    static void SearchEmployeesElderThan(Dictionary<int, Employee> employeeDict)
    {
        Console.Write("\nEnter an employee ID to find all employees elder than this employee: ");
        if (int.TryParse(Console.ReadLine(), out int baseEmpId))
        {
            if (employeeDict.TryGetValue(baseEmpId, out Employee baseEmp))
            {
                var elderEmployees = employeeDict.Values
                    .Where(e => e.Age > baseEmp.Age)
                    .ToList();

                if (elderEmployees.Count > 0)
                {
                    Console.WriteLine($"\nEmployees elder than employee ID {baseEmpId}:");
                    foreach (var emp in elderEmployees)
                    {
                        Console.WriteLine(emp);
                        Console.WriteLine("------------------");
                    }
                }
                else
                {
                    Console.WriteLine("\nNo employees are older than the given employee.");
                }
            }
            else
            {
                Console.WriteLine("\nEmployee not found with ID " + baseEmpId);
            }
        }
        else
        {
            Console.WriteLine("Invalid ID entered.");
        }
    }
}
