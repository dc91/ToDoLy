using ToDoLy;
using System;
using System.IO;

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

PrintWelcome(tManager);

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


static void PrintWelcome(TaskManager tManager)
{
    int tasksTodo = tManager.PendingTasksCount();
    int tasksDone = tManager.CompletedTasksCount();

    string welcomeBorder = new('=', 50);
    string welcomeMessage = "Welcome to ToDoLy";
    string paddedTitle = welcomeMessage.PadLeft((50 + welcomeMessage.Length) / 2).PadRight(50);

    Console.ForegroundColor = ConsoleColor.Cyan;
    Console.WriteLine(welcomeBorder);
    Console.WriteLine(paddedTitle);
    Console.WriteLine(welcomeBorder);
    Console.ResetColor();

    Console.Write("\n\nYou have ");
    Console.ForegroundColor = ConsoleColor.DarkRed;
    Console.Write(tasksTodo);
    Console.ResetColor();
    Console.Write(" tasks to do and ");
    Console.ForegroundColor = ConsoleColor.DarkGreen;
    Console.Write(tasksDone);
    Console.ResetColor();
    Console.WriteLine(" tasks are done!\n");
}


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