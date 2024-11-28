using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Collections.Specialized.BitVector32;

namespace ToDoLy
{
    internal class TaskManager
    {
        private List<Task> tasks = new();

        public void PrintCancel()
        {
            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine("Cancelling... Press any key");
            Console.ReadKey();
            Console.ResetColor();
        }
        public void AddTask()
        {
            Console.Clear();
            PrintHeader("Add a New Task");
            Console.WriteLine();
            PrintAddTaskInfo();

            string details = null;
            while (string.IsNullOrWhiteSpace(details))
            {
                Console.Write("\n\nEnter Task Detils: ");
                details = ReadInput();
                if (details == null)
                {
                    PrintCancel();
                    return;
                }
                    
                if (string.IsNullOrWhiteSpace(details))
                    Console.WriteLine("Cannot have an empty task. Please try again.");
            }

            string project = null;
            while (string.IsNullOrWhiteSpace(project))
            {
                Console.Write("\nEnter Project Name: ");
                project = ReadInput();
                if (project == null)
                {
                    PrintCancel();
                    return;
                }

                if (string.IsNullOrWhiteSpace(project))
                    Console.WriteLine("Cannot have an empty project name. Please try again.");
            }

            DateTime dueDate;
            string dueDateInput;

            while (true)
            {
                Console.Write("\nEnter Due Date (yyyy-mm-dd): ");
                dueDateInput = ReadInput();
                if (dueDateInput == null)//esc pressed
                {
                    PrintCancel();
                    return;
                }
                if (DateTime.TryParse(dueDateInput, out dueDate)) break;
                else Console.WriteLine("Invalid date format. Example (2024-12-25)");
            }

            Task task = new Task(details, project, dueDate, false);
            tasks.Add(task);
            SaveFile("tasks.csv");
        }

        public void LoadTasks(string filePath)
        {
            using StreamReader sr = new(filePath);
            string header = sr.ReadLine();

            while (!sr.EndOfStream)
            {
                string line = sr.ReadLine();
                string[] parts = line.Split('|');//use pipe so users can type comma

                string details = parts[0];
                string project = parts[1];
                DateTime dueDate = DateTime.Parse(parts[2]);
                bool completed = parts[3] == "Completed";
                tasks.Add(new Task(details, project, dueDate, completed));
            }
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
                {
                    break;
                }
                else if (key == ConsoleKey.Delete)
                {
                    Console.Clear();
                    PrintHeader($"Delete Task: {tasks[selectedIndex].Details}");

                    Console.Write("\nAre you sure you want to ");
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.Write("DELETE");
                    Console.ResetColor();
                    Console.WriteLine(" this task?\n");

                    Console.WriteLine(
                        "Task: " +
                        $"{tasks[selectedIndex].Details}\n" +
                        $"Project: {tasks[selectedIndex].Project}\n" +
                        $"Due: {tasks[selectedIndex].DueDate.ToShortDateString()}\n" +
                        $"Status: {(tasks[selectedIndex].IsCompleted ? "Completed" : "Pending")}\n");
                    Console.WriteLine(new string('-', 50));
                    Console.Write("\nPress 'y' to confirm, or any other key to cancel: ");
                    

                    ConsoleKey confirmDelete = Console.ReadKey(true).Key;
                    if (confirmDelete == ConsoleKey.Y)
                    {
                        tasks.RemoveAt(selectedIndex);
                        SaveFile("tasks.csv");
                        Console.ForegroundColor = ConsoleColor.Green;
                        Console.WriteLine("Task removed successfully!");
                        Console.ReadKey();
                        Console.ResetColor();
                    }
                    else
                    {
                        Console.ForegroundColor = ConsoleColor.Red;
                        Console.WriteLine("Task was not removed.");
                        Console.ReadKey();
                        Console.ResetColor();
                    }
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
                PrintHeader($"Updating Task: {task.Details}");
                SmartUpdatePrint("UP or DOWN", "HIGHLIGHT", " a value.\n", ConsoleColor.DarkYellow);
                SmartUpdatePrint("ENTER", "UPDATE", " a value.\n", ConsoleColor.Blue);
                SmartUpdatePrint("ESC", "CANCEL", " or go back.\n", ConsoleColor.DarkGray);
                Console.WriteLine();
                string[] fields = { 
                    "Task Details", "Project", "Due Date", "Completion Status" };

                for (int i = 0; i < fields.Length; i++)
                {
                    if (i == fieldIndex)
                    {
                        Console.ForegroundColor = ConsoleColor.Green;
                    }

                    switch(fields[i])
                    {
                        case "Task Details":
                            Console.Write("Task Details: ".PadRight(20));
                            Console.WriteLine($"{task.Details}");
                            break;
                        case "Project":
                            Console.Write("Project: ".PadRight(20));
                            Console.WriteLine($"{task.Project}");
                            break;
                        case "Due Date":
                            Console.Write("Due Date: ".PadRight(20));
                            Console.WriteLine($"{task.DueDate.ToShortDateString()}");
                            break;
                        case "Completion Status":
                            string status = task.IsCompleted ? "Completed" : "Pending";
                            Console.Write("Completion Status: ".PadRight(20));
                            Console.WriteLine($"{status}");
                            break;
                    }
                    Console.ResetColor();
                }

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
                    //Console.Clear();
                    Console.CursorVisible = true;
                    Console.Write($"\nEnter new value for {fields[fieldIndex]}: ");
                    string newValue = Console.ReadLine();
                    Console.CursorVisible = false;

                    // Update the task with the new value
                    switch (fields[fieldIndex])
                    {
                        case "Task Details":
                            if (!string.IsNullOrEmpty(newValue))
                            {
                                task.Details = newValue;
                                SaveFile("tasks.csv"); // Save changes instantly
                            }
                            break;
                        case "Project":
                            if (!string.IsNullOrEmpty(newValue))
                            {
                                task.Project = newValue;
                                SaveFile("tasks.csv"); // Save changes instantly
                            }
                            break;
                        case "Due Date":
                            if (string.IsNullOrEmpty(newValue))
                            {
                                break;
                            }
                            else if (DateTime.TryParse(newValue, out DateTime newDate))
                            {
                                task.DueDate = newDate;
                                SaveFile("tasks.csv"); // Save changes instantly
                            }
                            else
                            {
                                Console.ForegroundColor = ConsoleColor.Red;
                                Console.WriteLine("\nInvalid date format. Please enter in yyyy-MM-dd format.");
                                Console.ResetColor();
                                Console.ReadKey(true);
                            }
                            break;
                        case "Completion Status":
                            if (string.IsNullOrEmpty(newValue))
                            {
                                break;
                            }
                            else if (newValue.Equals("c", StringComparison.OrdinalIgnoreCase))
                            {
                                task.IsCompleted = true;
                                SaveFile("tasks.csv"); // Save changes instantly
                            }
                            else if (newValue.Equals("p", StringComparison.OrdinalIgnoreCase))
                            {
                                task.IsCompleted = false;
                                SaveFile("tasks.csv"); // Save changes instantly
                            }
                            else
                            {
                                Console.ForegroundColor = ConsoleColor.Red;
                                Console.WriteLine("Invalid input. Please enter 'c' for Completed or 'p' for Pending.");
                                Console.ResetColor();
                                Console.ReadKey(true);
                            }
                            break;
                    }
                }
                else if (key == ConsoleKey.Escape) // Finish updating
                {
                    isUpdating = false;
                }
            }
            Console.ResetColor();

        }

        public void SaveFile(string filePath)
        {
            using StreamWriter sw = new (filePath);
            sw.WriteLine("Task Name|Project|Due Date|Status");

            foreach (Task task in tasks)
            {
                sw.WriteLine(
                    $"{task.Details}|" +
                    $"{task.Project}|" +
                    $"{task.DueDate:d}|" +
                    $"{(task.IsCompleted ? "Completed" : "Pending")}");
            }
        }

        public void PrintTaskList(int? selectedIndex = null)
        {
            bool isRunning = true;
            bool wrongInput = false;
            List<Task> originalTasks = new List<Task>(tasks);

            while (isRunning)
            {
                Console.Clear();
                if (selectedIndex.HasValue)
                {
                    PrintHeader("Update/Change Task");
                    PrintUpdateTaskInfo();
                }
                else
                {
                    PrintHeader("All Tasks");
                }

                if (tasks.Count == 0)
                {
                    Console.WriteLine("No tasks to show. Try adding a new task first.");
                    return;
                }

                Console.WriteLine();
                Console.WriteLine("{0,5} | {1,-25} | {2,-25} | {3,-12} | {4,-10}",
                          "No.", "Task Details", "Project", "Due Date", "Status");
                Console.WriteLine(new string('-', 90));

                for (int i = 0; i < originalTasks.Count; i++)
                {
                    Task task = originalTasks[i];
                    string status = task.IsCompleted ? "Completed" : "Pending";

                    if (selectedIndex.HasValue && i == selectedIndex.Value)
                        Console.ForegroundColor = ConsoleColor.DarkYellow;
                    Console.WriteLine("{0,5} | {1,-25} | {2,-25} | {3,-12} | {4,-10}",
                                  i + 1,
                                  task.Details,
                                  task.Project,
                                  task.DueDate.ToShortDateString(),
                                  status);
                    Console.ResetColor();
                }

                if (!selectedIndex.HasValue)
                {
                    PrintSortingOptions();
                    if (wrongInput)
                    {
                        Console.ForegroundColor = ConsoleColor.Red;
                        Console.WriteLine("\nInvalid option. Please press 1, 2, 3 or ESC.");
                        Console.ResetColor();
                    }
                    wrongInput = false;

                    ConsoleKey key = Console.ReadKey(true).Key;

                    // Handle user input
                    switch (key)
                    {
                        case ConsoleKey.D1: // Sort by Due Date
                            originalTasks = tasks.OrderBy(t => t.DueDate).ToList();
                            break;
                        case ConsoleKey.D2: // Sort by Project
                            originalTasks = tasks.OrderBy(t => t.Project).ToList();
                            break;
                        case ConsoleKey.D3: // Default order (original task list)
                            originalTasks = new List<Task>(tasks); // Copy the original list
                            break;
                        case ConsoleKey.Escape: // Exit the loop
                            isRunning = false;
                            break;
                        default:
                            wrongInput = true;
                            break;
                    }
                }
                else break;
            }   
        }

        public void PrintSortingOptions()
        {
            //sorting options
            Console.Write("\nHow would you like to ");
            Console.ForegroundColor = ConsoleColor.Blue;
            Console.Write("SORT");
            Console.ResetColor();
            Console.WriteLine(" the tasks?\n");
            Console.ForegroundColor = ConsoleColor.Blue;
            Console.Write("1");
            Console.ResetColor();
            Console.WriteLine(". By Due Date");
            Console.ForegroundColor = ConsoleColor.Blue;
            Console.Write("2");
            Console.ResetColor();
            Console.WriteLine(". By Project Name");
            Console.ForegroundColor = ConsoleColor.Blue;
            Console.Write("3");
            Console.ResetColor();
            Console.WriteLine(". Default Order");
            Console.WriteLine("\nPress ESC to exit.");
        }

        public int PendingTasksCount()
        {
            return tasks.Count(t => !t.IsCompleted);
        }

        public int CompletedTasksCount()
        {
            return tasks.Count(t => t.IsCompleted);
        }

        public void PrintHeader(string section)
        {
            string border = new('=', 90);
            string title = section;
            string paddedTitle = title.PadLeft((90 + title.Length) / 2).PadRight(90);

            Console.ForegroundColor = ConsoleColor.DarkYellow;
            Console.WriteLine(border);
            Console.WriteLine();
            Console.WriteLine(paddedTitle);
            Console.WriteLine();
            Console.WriteLine(border);
            Console.ResetColor();
        }
        
        public void PrintWelcome()
        {
            int tasksTodo = PendingTasksCount();
            int tasksDone = CompletedTasksCount();

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

        public void PrintAddTaskInfo()
        {
            Console.WriteLine("1. Enter task details\n2. Enter project name");
            Console.WriteLine("3. Enter due date\n4. Mark task done/pending\n");
            Console.ForegroundColor= ConsoleColor.DarkGray;
            Console.WriteLine("ESC to CANCEL");
            Console.ResetColor();
        }

        public void PrintUpdateTaskInfo()
        {
            SmartUpdatePrint("UP or DOWN", "HIGHLIGHT", " a task.\n", ConsoleColor.DarkYellow);
            SmartUpdatePrint("ENTER", "UPDATE", " a task.\n", ConsoleColor.Blue);
            SmartUpdatePrint("DEL", "DELETE", " a task.\n", ConsoleColor.Red);
            SmartUpdatePrint("ESC", "CANCEL", ".\n", ConsoleColor.DarkGray);
            Console.WriteLine();
        }

        public void SmartUpdatePrint(string key, string action, string ending, ConsoleColor color)
        {
            Console.Write("Press ");
            Console.ForegroundColor = color;
            Console.Write(key);
            Console.ResetColor();
            Console.Write(" to ");
            Console.ForegroundColor = color;
            Console.Write(action);
            Console.ResetColor();
            Console.WriteLine(ending);
        }


        public string ReadInput()
        {
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
                else if (!char.IsControl(key.KeyChar))//ex of control is \n \t backspace esc etc
                {//a lot of work to make esc cancel possible...
                    input.Append(key.KeyChar);
                    Console.Write(key.KeyChar);
                }
            }
        }
    }
}
