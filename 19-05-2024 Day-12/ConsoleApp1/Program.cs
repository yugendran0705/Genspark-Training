﻿using System;
 
class Program
{
    public static void Task1()
    {
        int noOfUsers = 0, noOfPosts = 0;
        noOfUsers = TaskHelper.getValidIntInput("Enter Number of Users: ");
 
        string[][][] arr = new string[noOfUsers][][];
 
        for (int i = 0; i < noOfUsers; i++)
        {
            noOfPosts = TaskHelper.getValidIntInput($"Enter Number of Posts for User {i}: ");
            arr[i] = new string[noOfPosts][];
            Console.WriteLine();
            for (int j = 0; j < noOfPosts; j++)
            {
                arr[i][j] = new string[2];
                arr[i][j][0] = TaskHelper.getValidString("Enter Caption: ");
                arr[i][j][1] = TaskHelper.getValidIntInput("Enter Number of Likes: ").ToString();
                Console.WriteLine();
            }
 
        }
 
        for (int i = 0; i < noOfUsers; i++)
        {
            Console.WriteLine($"User {i}:");
            for (int j = 0; j < arr[i].Length; j++)
            {
                Console.WriteLine($"Post: {arr[i][j][0]} | Likes: {arr[i][j][1]}");
            }
            Console.WriteLine();
        }
    }
    static void Main(string[] args)
    {
        Task1();
 
 
        Console.WriteLine("All tasks completed. Press Enter to exit.");
        Console.ReadLine();
    }
}