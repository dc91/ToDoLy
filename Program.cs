using ToDoLy;
using System;
using System.IO;
using static System.Collections.Specialized.BitVector32;

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

tManager.PrintHeader("Welcome to ToDoLy");
tManager.PrintWelcome();

while (true)
{
    Console.WriteLine();
    PrintOptions();
    string input = Console.ReadLine();
    switch (input)
    {
        case "1":
            tManager.PrintTaskList();
            break;
        case "2":
            tManager.AddTask();
            break;
        case "3":
            tManager.UpdateTask();
            break;
        case "4":
            tManager.SaveFile(filePath);
            Console.WriteLine("Quitting, Goodbye");
            goto EndLoop;
        default:
            Console.WriteLine("input not recognized, try again");
            break;
    }
} EndLoop:;


static void PrintOptions()
{
    Console.Write("Pick an ");
    Console.ForegroundColor = ConsoleColor.Blue;
    Console.Write("option");
    Console.ResetColor();
    Console.WriteLine(":");

    string[] options = [
        ") Show Task List (by date or project)",
        ") Add New Task",
        ") Edit Task (update, mark as done, remove)",
        ") Quit"
        ];

    for (int i = 0; i < 4; i++)
    {
        Console.Write("(");
        Console.ForegroundColor = ConsoleColor.Blue;
        Console.Write(i+1);
        Console.ResetColor();
        Console.WriteLine(options[i]);
    }

}