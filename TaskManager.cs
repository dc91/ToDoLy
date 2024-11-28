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
        public FileManager fManager = new();
        public List<Task> tasks = new();

        public string ReadInput()
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
                {
                    input.Append(key.KeyChar);
                    Console.Write(key.KeyChar);
                }
            }
        }
        
        public void AddTask()
        {
            Console.Clear();
            PrintInfoManager.PrintHeader("Add a New Task");
            Console.WriteLine();
            PrintInfoManager.PrintAddTaskInfo();

            string details = null;
            while (string.IsNullOrWhiteSpace(details))
            {
                Console.Write("\n\nEnter Task Detils: ");
                details = ReadInput();
                if (details == null)
                {
                    PrintInfoManager.PrintCancel();
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
                    PrintInfoManager.PrintCancel();
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
                    PrintInfoManager.PrintCancel();
                    return;
                }
                if (DateTime.TryParse(dueDateInput, out dueDate)) break;
                else Console.WriteLine("Invalid date format. Example (2024-12-25)");
            }

            Task task = new Task(details, project, dueDate, false);
            tasks.Add(task);
            fManager.SaveFile("tasks.csv", tasks);
            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine("Task successfully saved... Press any key");
            Console.ReadKey();
            Console.ResetColor();
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
                    PrintInfoManager.PrintHeader($"Delete Task: {tasks[selectedIndex].Details}");

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
                        fManager.SaveFile("tasks.csv", tasks);
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
                PrintInfoManager.PrintHeader($"Updating Task: {task.Details}");
                PrintInfoManager.SmartUpdatePrint("UP or DOWN", "HIGHLIGHT", " a value.\n", ConsoleColor.DarkYellow);
                PrintInfoManager.SmartUpdatePrint("ENTER", "UPDATE", " a value.\n", ConsoleColor.Blue);
                PrintInfoManager.SmartUpdatePrint("ESC", "CANCEL", " or go back.\n", ConsoleColor.DarkGray);
                Console.WriteLine();
                string[] fields = {
                    "Task Details", "Project", "Due Date", "Completion Status" };

                for (int i = 0; i < fields.Length; i++)
                {
                    if (i == fieldIndex)
                    {
                        Console.ForegroundColor = ConsoleColor.Green;
                    }

                    switch (fields[i])
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
                    string newValue = ReadInput();
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
                            {
                                break;
                            }
                            else if (DateTime.TryParse(newValue, out DateTime newDate))
                            {
                                task.DueDate = newDate;
                                fManager.SaveFile("tasks.csv", tasks);
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
                                fManager.SaveFile("tasks.csv", tasks);
                            }
                            else if (newValue.Equals("p", StringComparison.OrdinalIgnoreCase))
                            {
                                task.IsCompleted = false;
                                fManager.SaveFile("tasks.csv", tasks);
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
                    PrintInfoManager.PrintHeader("Update/Change Task");
                    PrintInfoManager.PrintUpdateTaskInfo();
                }
                else
                {
                    PrintInfoManager.PrintHeader("All Tasks");
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
                    PrintInfoManager.PrintSortingOptions();
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

        public int PendingTasksCount()
        {
            return tasks.Count(t => !t.IsCompleted);
        }

        public int CompletedTasksCount()
        {
            return tasks.Count(t => t.IsCompleted);
        }
        
    }
}