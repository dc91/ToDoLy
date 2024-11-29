using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using static System.Collections.Specialized.BitVector32;

namespace ToDoLy
{
    internal class TaskManager
    {
        public FileManager fManager = new();
        public List<Task> tasks = new();

        public string ReadEveryKey()
        {// Reads input key-by-key. return null if ESC, or returns full input string
            StringBuilder input = new();
            while (true)
            {
                var key = Console.ReadKey(true);

                if (key.Key == ConsoleKey.Escape) return null;
                else if (key.Key == ConsoleKey.Enter)
                {
                    Console.WriteLine();
                    return input.ToString();
                }
                else if (key.Key == ConsoleKey.Backspace && input.Length > 0)
                {
                    input.Remove(input.Length - 1, 1);
                    Console.Write("\b \b");//needs the space to erase
                }
                else if (!char.IsControl(key.KeyChar))//example of control is \n \t backspace esc etc
                {//small buggs when typing control char, they are invis but cant be erased.
                    input.Append(key.KeyChar);
                    Console.Write(key.KeyChar);
                }
            }
        }

        public string GetInput(string prompt, string errMess, bool expectDateTime)
        {
            while (true)
            {
                Console.Write(prompt);          
                string input = ReadEveryKey();

                if (input == null)
                {
                    PrintInfoManager.PrintCancel();
                    return null;
                }

                if (!expectDateTime)
                    if (!string.IsNullOrWhiteSpace(input)) return input;
                if (expectDateTime)
                    if (DateTime.TryParse(input, out DateTime date)) return date.ToString();
                
                Console.WriteLine(errMess);
            }
        }

        public void AddTask()
        {
            Console.Clear();
            PrintInfoManager.PrintHeader("Add a New Task");
            Console.WriteLine();
            PrintInfoManager.PrintAddTaskInfo(1);

            string details = GetInput("\nEnter Task Details: ",
                "Cannot have an empty task. Try again.", false);
            if (details == null) return;

            Console.Clear();
            PrintInfoManager.PrintHeader("Add a New Task");
            Console.WriteLine();
            PrintInfoManager.PrintAddTaskInfo(2);

            string project = GetInput("\nEnter Project Name: ", 
                "Cannot have an project name. Try again.", false);
            if (project == null) return;

            Console.Clear();
            PrintInfoManager.PrintHeader("Add a New Task");
            Console.WriteLine();
            PrintInfoManager.PrintAddTaskInfo(3);

            string dueDateString = GetInput("\nEnter Due Date (yyyy-mm-dd): ",
                "Invalid date format. Example (2024-12-25)", true);
            if (dueDateString == null) return;

            DateTime dueDate = DateTime.Parse(dueDateString);

            Task task = new Task(details, project, dueDate, false);
            tasks.Add(task);
            fManager.SaveFile("tasks.csv", tasks);
            PrintInfoManager.PrintAddSuccess();
        }

        public void UpdateTask()
        {
            if (tasks.Count == 0)
            {
                Console.WriteLine("No tasks to update. Try adding a new task first.");
                return;
            }

            int selectedIndex = 0;
            PrintTaskList(selectedIndex);

            while (true)
            {
                ConsoleKey key = Console.ReadKey(true).Key;

                if (key == ConsoleKey.DownArrow && selectedIndex < tasks.Count - 1)
                {
                    selectedIndex++;
                    PrintTaskList(selectedIndex);
                }
                else if (key == ConsoleKey.UpArrow && selectedIndex > 0)
                {
                    selectedIndex--;
                    PrintTaskList(selectedIndex);
                }
                else if (key == ConsoleKey.Enter)//Select task
                {
                    Task task = tasks[selectedIndex];
                    UpdateTaskDetails(task);
                    break;
                }
                else if (key == ConsoleKey.Escape)//cancel changes
                    break;
                else if (key == ConsoleKey.Delete)
                {
                    Console.Clear();
                    PrintInfoManager.PrintHeader($"Delete Task: {tasks[selectedIndex].Details}");
                    PrintInfoManager.PrintAreUSure(tasks[selectedIndex]);

                    ConsoleKey confirmDelete = Console.ReadKey(true).Key;
                    if (confirmDelete == ConsoleKey.Enter)
                    {
                        tasks.RemoveAt(selectedIndex);
                        fManager.SaveFile("tasks.csv", tasks);
                        PrintInfoManager.PrintRemoveSuccess();
                    }
                    else
                        PrintInfoManager.PrintRemoveCancelled();
                    break;
                }
            }
        }

        public void UpdateTaskDetails(Task task)
        {
            int fieldIndex = 0;
            bool isUpdating = true;

            while (isUpdating)
            {
                Console.Clear();
                PrintInfoManager.PrintHeader($"Updating Task: {task.Details}");
                PrintInfoManager.PrintUpdateTaskInfo(false);

                string[] fields = ["Task Details", "Project", "Due Date", "Completion Status"];

                PrintInfoManager.PrintUpdateTaskFields(fields, task, fieldIndex);

                

                ConsoleKey key = Console.ReadKey(true).Key;

                if (key == ConsoleKey.DownArrow && fieldIndex < fields.Length - 1)
                {
                    fieldIndex++;
                }
                else if (key == ConsoleKey.UpArrow && fieldIndex > 0)
                {
                    fieldIndex--;
                }
                else if (key == ConsoleKey.Enter) // Editing the selected field
                {
                    EditTaskDetails(fieldIndex, task, fields);
                }
                else if (key == ConsoleKey.Escape) // Finish updating
                {
                    isUpdating = false;
                }
            }
            Console.ResetColor();

        }

        public void EditTaskDetails(int fieldIndex, Task task, string[] fields)
        {
            Console.CursorVisible = true;
            Console.Write($"\nEnter new value for {fields[fieldIndex]}: ");
            string newValue = ReadEveryKey();
            Console.CursorVisible = false;

            // Update the task with the new value
            switch (fields[fieldIndex])
            {
                case "Task Details":
                    if (!string.IsNullOrEmpty(newValue))
                    {
                        task.Details = newValue;
                        fManager.SaveFile("tasks.csv", tasks);
                    }
                    break;
                case "Project":
                    if (!string.IsNullOrEmpty(newValue))
                    {
                        task.Project = newValue;
                        fManager.SaveFile("tasks.csv", tasks);
                    }
                    break;
                case "Due Date":
                    if (string.IsNullOrEmpty(newValue))
                        break;
                    else if (DateTime.TryParse(newValue, out DateTime newDate))
                    {
                        task.DueDate = newDate;
                        fManager.SaveFile("tasks.csv", tasks);
                    }
                    else
                        PrintInfoManager.PrintInvalidDate();
                    break;
                case "Completion Status":
                    if (string.IsNullOrEmpty(newValue))
                        break;
                    else if (newValue.Equals("c", StringComparison.OrdinalIgnoreCase))
                    {
                        task.IsCompleted = true;
                        fManager.SaveFile("tasks.csv", tasks);
                    }
                    else if (newValue.Equals("p", StringComparison.OrdinalIgnoreCase))
                    {
                        task.IsCompleted = false;
                        fManager.SaveFile("tasks.csv", tasks);
                    }
                    else
                        PrintInfoManager.PrintInvalidBool();
                    break;
            }
        }
        
        public void PrintTaskList(int? selectedIndex = null)
        {
            bool isRunning = true;
            bool showCompletedTasks = true;

            List<Task> filteredTasks = new List<Task>(tasks);
            List<Task> sortedTasks = new List<Task>(tasks);

            while (isRunning)
            {
                Console.Clear();
                if (selectedIndex.HasValue)
                {
                    PrintInfoManager.PrintHeader("Update/Change Task");
                    PrintInfoManager.PrintUpdateTaskInfo(true);
                }
                else
                    PrintInfoManager.PrintHeader("All Tasks");

                if (tasks.Count == 0)
                {
                    Console.WriteLine("No tasks to show. Try adding a new task first.");
                    return;
                }

                if (showCompletedTasks)
                    filteredTasks = sortedTasks;
                else
                    filteredTasks = sortedTasks.Where(t => !t.IsCompleted).ToList();


                PrintInfoManager.PrintTableHead();
                PrintInfoManager.PrintTableRows(filteredTasks, selectedIndex);
                
                if (!selectedIndex.HasValue)
                {
                    PrintInfoManager.PrintSortingOptions(showCompletedTasks);

                    //User Options
                    ConsoleKey key;
                    
                   //Force a valid input before refreshing
                    while (true)
                    {
                        ConsoleKey tryKey = Console.ReadKey(true).Key;
                        if (tryKey == ConsoleKey.D1 || tryKey == ConsoleKey.D2 ||
                            tryKey == ConsoleKey.D3 || tryKey == ConsoleKey.F || 
                            tryKey == ConsoleKey.Escape)
                        {
                            key = tryKey;
                            break;
                        }
                    }
                    
                    switch (key)
                    {
                        case ConsoleKey.D1: // Sort by Due Date
                            sortedTasks = tasks.OrderBy(t => t.DueDate).ToList();
                            break;
                        case ConsoleKey.D2: // Sort by Project Name
                            sortedTasks = tasks.OrderBy(t => t.Project).ToList();
                            break;
                        case ConsoleKey.D3: // Default order (original task list)
                            sortedTasks = new List<Task>(tasks); // Copy the original list
                            break;
                        case ConsoleKey.F: // Toggle showCompletedTasks
                            showCompletedTasks = !showCompletedTasks;
                            break;
                        case ConsoleKey.Escape: // Exit the loop
                            isRunning = false;
                            break;
                        default:
                            break;
                    }
                }
                else break;
            }
        }
        
    }
}