using ToDoLy;

TaskManager tManager = new();
FileManager fManager = new();
PrintInfoManager pManager = new();

string filePath = "tasks.csv";

if (File.Exists(filePath)) // Fix bug with nonexisting file. just loop or something
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


    PrintInfoManager.PrintHeader("Welcome to ToDoLy - Main Menu");
    pManager.PrintWelcome(complete, pending);
    Console.WriteLine();
    PrintInfoManager.PrintOptions();

    Console.CursorVisible = false;
    ConsoleKey key;

    while (true)
    {
        ConsoleKey tryKey = Console.ReadKey(true).Key;
        if (tryKey == ConsoleKey.D1 || tryKey == ConsoleKey.D2 || tryKey == ConsoleKey.Escape)
        {
            key = tryKey;
            break;
        }
    }

    switch (key)
    {
        case ConsoleKey.D1:
            tManager.UpdateTask();
            break;
        case ConsoleKey.D2:
            Console.CursorVisible = true;
            tManager.AddTask();
            break;
        case ConsoleKey.Escape:
            Console.WriteLine("Quitting, Goodbye");
            goto EndLoop;
        default:
            break;
    }
} EndLoop:;

