using System;
using System.Linq;
class Program
{

    static void GreetUser()
    {
        Console.Write("Enter your name: ");
        string name = Console.ReadLine();
        Console.WriteLine($"Hello, {name}!");
    }

    static void FindLargestNumber()
    {
        Console.Write("Enter first number: ");
        int num1 = int.Parse(Console.ReadLine());

        Console.Write("Enter second number: ");
        int num2 = int.Parse(Console.ReadLine());

        int largest = (num1 > num2) ? num1 : num2;

        Console.WriteLine($"The largest number is: {largest}");
    }

    static void PerformOperation()
    {
        Console.Write("Enter first number: ");
        double num1 = Convert.ToDouble(Console.ReadLine());

        Console.Write("Enter second number: ");
        double num2 = Convert.ToDouble(Console.ReadLine());

        Console.Write("Enter operation (+, -, *, /): ");
        char operation = Console.ReadLine()[0];

        double result = 0;
        bool valid = true;

        switch (operation)
        {
            case '+': result = num1 + num2; break;
            case '-': result = num1 - num2; break;
            case '*': result = num1 * num2; break;
            case '/': result = num2 != 0 ? num1 / num2 : double.NaN; break;
            default: valid = false; Console.WriteLine("Invalid operation."); break;
        }

        if (valid)
            Console.WriteLine($"Result: {result}");
    }

    static void ValidateLogin()
    {
        const string correctUsername = "Admin";
        const string correctPassword = "pass";
        int attempts = 3;

        for (int i = 1; i <= attempts; i++)
        {
            Console.Write("Enter Username: ");
            string username = Console.ReadLine();

            Console.Write("Enter Password: ");
            string password = Console.ReadLine();

            if (username == correctUsername && password == correctPassword)
            {
                Console.WriteLine("Login successful!");
                return;
            }
            else
            {
                Console.WriteLine($"Invalid credentials. {attempts - i} attempts left.");
            }
        }

        Console.WriteLine("Invalid attempts for 3 times. Exiting...");
    }
    static void CountDivisibleBySeven()
    {
        int count = 0;

        Console.WriteLine("Enter 10 numbers:");
        for (int i = 0; i < 10; i++)
        {
            Console.Write($"Number {i + 1}: ");
            int num = Convert.ToInt32(Console.ReadLine());

            if (num % 7 == 0)
                count++;
        }

        Console.WriteLine($"Total numbers divisible by 7: {count}");
    }

    static void CountFrequency(int[] arr)
    {
        Dictionary<int, int> frequencyMap = new Dictionary<int, int>();

        foreach (int num in arr)
        {
            if (frequencyMap.TryGetValue(num, out int value))
                frequencyMap[num] = ++value;
            else
                frequencyMap[num] = 1;
        }

        foreach (var entry in frequencyMap)
        {
            Console.WriteLine($"{entry.Key} occurs {entry.Value} times");
        }
    }
    static void RotateLeft(int[] arr)
    {
        if (arr.Length == 0) return;

        int first = arr[0];

        for (int i = 0; i < arr.Length - 1; i++)
        {
            arr[i] = arr[i + 1];
        }

        arr[arr.Length - 1] = first;
    }
    static int[] MergeArrays(int[] arr1, int[] arr2)
    {
        int[] merged = new int[arr1.Length + arr2.Length];

        Array.Copy(arr1, 0, merged, 0, arr1.Length);
        Array.Copy(arr2, 0, merged, arr1.Length, arr2.Length);

        return merged;
    }

    static void PlayGame()
    {
        string secretWord = "GAME";
        int attempts = 0;

        Console.WriteLine("Welcome to Bulls & Cows! Try to guess the 4-letter secret word.");

        while (true)
        {
            Console.Write("Enter your 4-letter guess: ");
            string guess = Console.ReadLine()?.ToUpper();

            if (guess.Length != 4)
            {
                Console.WriteLine("Please enter exactly 4 letters.");
                continue;
            }

            attempts++;
            int bulls = 0, cows = 0;

            for (int i = 0; i < 4; i++)
            {
                if (guess[i] == secretWord[i])
                    bulls++;
                else if (secretWord.Contains(guess[i]))
                    cows++;
            }

            Console.WriteLine($"{bulls} Bulls, {cows} Cows");

            if (bulls == 4)
            {
                Console.WriteLine($"Congratulations! You guessed it in {attempts} attempts.");
                break;
            }
        }
    }

    static void ValidateSudokuRow(int[] row)
    {
        if (row.Length != 9)
        {
            Console.WriteLine("Invalid row. Must contain exactly 9 numbers.");
            return;
        }

        bool isValid = row.All(n => n >= 1 && n <= 9) && row.Distinct().Count() == 9;

        Console.WriteLine(isValid ? "Valid Sudoku row" : "Invalid Sudoku row");
    }
    
    static void ValidateSudokuBoard(int[,] board)
    {
        for (int row = 0; row < 9; row++)
        {
            int[] rowValues = new int[9];

            for (int col = 0; col < 9; col++)
            {
                rowValues[col] = board[row, col];
            }

            if (!IsValidSudokuRow(rowValues))
            {
                Console.WriteLine($"Row {row + 1} is invalid");
                return;
            }
        }

        Console.WriteLine("Sudoku board is valid");
    }

    static bool IsValidSudokuRow(int[] row)
    {
        return row.Length == 9 && row.All(n => n >= 1 && n <= 9) && row.Distinct().Count() == 9;
    }
    static void Main(string[] args)
    {
        // GreetUser();

        // FindLargestNumber();

        // PerformOperation();

        // ValidateLogin();

        // CountDivisibleBySeven();

        // int[] numbers = { 1, 2, 2, 3, 4, 4, 4 };
        // CountFrequency(numbers);

        // int[] numbers = { 10, 20, 30, 40, 50 };
        // RotateLeft(numbers);
        // Console.WriteLine("Rotated Array: " + string.Join(", ", numbers));

        // int[] array1 = { 1, 3, 5 };
        // int[] array2 = { 2, 4, 6 };

        // int[] mergedArray = MergeArrays(array1, array2);

        // Console.WriteLine("Merged Array: " + string.Join(", ", mergedArray));

        // PlayGame();

        // int[] sudokuRow = new int[9];

        // Console.WriteLine("Enter 9 numbers for the Sudoku row:");
        // for (int i = 0; i < 9; i++)
        // {
        //     Console.Write($"Number {i + 1}: ");
        //     sudokuRow[i] = Convert.ToInt32(Console.ReadLine());
        // }

        // ValidateSudokuRow(sudokuRow);

        int[,] board = {
            {5, 3, 4, 6, 7, 8, 9, 1, 2},
            {6, 7, 2, 1, 9, 5, 3, 4, 8},
            {1, 9, 8, 3, 4, 2, 5, 6, 7},
            {8, 5, 9, 7, 6, 1, 4, 8, 3},
            {4, 2, 6, 8, 5, 3, 7, 9, 1},
            {7, 1, 3, 9, 2, 4, 8, 5, 6},
            {9, 6, 1, 5, 3, 7, 2, 8, 4},
            {2, 8, 7, 4, 1, 9, 6, 3, 5},
            {3, 4, 5, 2, 8, 6, 1, 7, 9}
        };

        ValidateSudokuBoard(board);
    }
}
