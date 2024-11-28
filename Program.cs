using ToDoLy;
using System;
using System.IO;
using static System.Collections.Specialized.BitVector32;
using System.Threading.Tasks;

TaskManager tManager = new();
string filePath = "tasks.csv";

if (File.Exists(filePath))
{
    tManager.LoadTasks(filePath);
}
else
{
    using StreamWriter sw = new(filePath);
    sw.WriteLine("Task Name,Project,Due Date,Status");
}




bool wrongKey = false;
while (true)
{
    Console.Clear();
    tManager.PrintHeader("Welcome to ToDoLy");
    tManager.PrintWelcome();
    Console.WriteLine();
    PrintOptions();

    if (wrongKey)
    {
        Console.ForegroundColor = ConsoleColor.Red;
        Console.WriteLine("\nInvalid option. Please press 1, 2, 3, or 4.");
        Console.ResetColor();
    }
    wrongKey = false;
    Console.CursorVisible = false;
    ConsoleKey key = Console.ReadKey(true).Key;

    // Handle user input
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


static void PrintOptions()
{
    Console.Write("Choose an ");
    Console.ForegroundColor = ConsoleColor.Blue;
    Console.Write("OPTION");
    Console.ResetColor();
    Console.WriteLine(":");

    string[] options = [
        ". Show Task List (by date or project)",
        ". Add New Task",
        ". Edit Task (update, mark as done, remove)",
        ". Quit\n"
        ];

    for (int i = 0; i < 4; i++)
    {
        Console.ForegroundColor = ConsoleColor.Blue;
        Console.Write(i+1);
        Console.ResetColor();
        Console.WriteLine(options[i]);
    }

}