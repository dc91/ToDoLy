using ToDoLy;
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
{
    tManager.tasks = fManager.LoadTasks(filePath);
}
else
{
    using StreamWriter sw = new(filePath);
    sw.WriteLine("Task Name,Project,Due Date,Status");
}

bool wrongKey = false;
while (true)
{
    int pending = tManager.tasks.Count(t => !t.IsCompleted);
    int complete = tManager.tasks.Count(t => t.IsCompleted);

    Console.Clear();
    PrintInfoManager.PrintHeader("Welcome to ToDoLy");
    pManager.PrintWelcome(complete, pending);
    Console.WriteLine();
    PrintInfoManager.PrintOptions();

    if (wrongKey)
    {
        Console.ForegroundColor = ConsoleColor.Red;
        Console.WriteLine("\nInvalid option. Please press 1, 2, 3, or 4.");
        Console.ResetColor();
    }
    wrongKey = false;
    Console.CursorVisible = false;
    ConsoleKey key = Console.ReadKey(true).Key;

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
        case ConsoleKey.D4:
            Console.WriteLine("Quitting, Goodbye");
            goto EndLoop;
        default:
            wrongKey = true;
            break;
    }
} EndLoop:;

