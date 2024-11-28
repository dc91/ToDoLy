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
            PrintInfoManager.PrintAddTaskInfo();

            string details = GetInput("\nEnter Task Details: ",
                "Cannot have an empty task. Try again.", false);
            if (details == null) return;

            string project = GetInput("\nEnter Project Name: ", 
                "Cannot have an project name. Try again.", false);
            if (project == null) return;

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
                {
                    break;
                }
                else if (key == ConsoleKey.Delete)
                {
                    Console.Clear();
                    PrintInfoManager.PrintHeader($"Delete Task: {tasks[selectedIndex].Details}");
                    PrintInfoManager.PrintAreUSure(tasks[selectedIndex]);

                    ConsoleKey confirmDelete = Console.ReadKey(true).Key;
                    if (confirmDelete == ConsoleKey.Y)
                    {
                        tasks.RemoveAt(selectedIndex);
                        fManager.SaveFile("tasks.csv", tasks);
                        PrintInfoManager.PrintRemoveSuccess();
                    }
                    else
                    {
                        PrintInfoManager.PrintRemoveCancelled();
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
                                PrintInfoManager.PrintInvalidDate();
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
                                PrintInfoManager.PrintInvalidBool();
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

                PrintInfoManager.PrintTableHead();
                
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
                        PrintInfoManager.PrintWrongInput();
                    wrongInput = false;


                    //User Options
                    ConsoleKey key = Console.ReadKey(true).Key;
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
        
    }
}