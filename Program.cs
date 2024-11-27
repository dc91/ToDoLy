using ToDoLy;
using System;
using System.IO;

TaskManager tManager = new TaskManager();
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

int tasksTodo = tManager.PendingTasksCount();
int tasksDone = tManager.CompletedTasksCount();

Console.WriteLine("Welcome to ToDoLy");
Console.WriteLine($"You have {tasksTodo} tasks todo and {tasksDone} tasks are done!");

while (true)
{
    PrintOptions();
    string input = Console.ReadLine();
    switch (input)
    {
        case "1":
            tManager.PrintList();
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
    Console.WriteLine("Pick an option:");
    Console.WriteLine("(1) Show Task List (by date or project)");
    Console.WriteLine("(2) Add New Task");
    Console.WriteLine("(3) Edit Task (update, mark as done, remove)");
    Console.WriteLine("(4) Quit");
}