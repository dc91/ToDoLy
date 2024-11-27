using ToDoLy;

TaskManager tManager = new TaskManager();

int tasksTodo = 0;
int tasksDone = 0;

Console.WriteLine("Welcome to ToDoLy");
Console.WriteLine($"You have {tasksTodo} tasks todo and {tasksDone} tasks are done!");
Console.WriteLine("Pick an option:");
Console.WriteLine("(1) Show Task List (by date or project)");
Console.WriteLine("(2) Add New Task");
Console.WriteLine("(3) Edit Task (update, mark as done, remove)");
Console.WriteLine("(4) Quit");


while (true)
{
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
            Console.WriteLine("quit");
            goto EndLoop;
        default:
            Console.WriteLine("input not recognized, try again");
            break;
    }
} EndLoop:;


