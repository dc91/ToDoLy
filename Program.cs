﻿using ToDoLy;
using System;
using System.IO;
using static System.Collections.Specialized.BitVector32;
using System.Threading.Tasks;
using System.Numerics;


TaskManager tManager = new();
FileManager fManager = new();
PrintInfoManager pManager = new();
string filePath = "tasks.csv";

if (File.Exists(filePath))
    tManager.tasks = fManager.LoadTasks(filePath);
else
{
    using StreamWriter sw = new(filePath);
    sw.WriteLine("Task Name,Project,Due Date,Status");
}

while (true)
{
    int pending = tManager.tasks.Count(t => !t.IsCompleted);
    int complete = tManager.tasks.Count(t => t.IsCompleted);

    Console.Clear();
    PrintInfoManager.PrintHeader("Welcome to ToDoLy - Main Menu");
    pManager.PrintWelcome(complete, pending);
    Console.WriteLine();
    PrintInfoManager.PrintOptions();

    Console.CursorVisible = false;
    ConsoleKey key;

    while (true)
    {
        ConsoleKey tryKey = Console.ReadKey(true).Key;
        if (tryKey == ConsoleKey.D1 || tryKey == ConsoleKey.D2 ||
            tryKey == ConsoleKey.D3 || tryKey == ConsoleKey.Escape)
        {
            key = tryKey;
            break;
        }
    }

    switch (key)
    {
        case ConsoleKey.D1:
            tManager.PrintTaskList();
            break;
        case ConsoleKey.D2:
            Console.CursorVisible = true;
            tManager.AddTask();
            break;
        case ConsoleKey.D3:
            tManager.UpdateTask();
            break;
        case ConsoleKey.Escape:
            Console.WriteLine("Quitting, Goodbye");
            goto EndLoop;
        default:
            break;
    }
} EndLoop:;

