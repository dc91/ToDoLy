using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ToDoLy
{
    internal class TaskManager
    {
        private List<Task> tasks = new();

        public void AddTask()
        {
            Console.Clear();
            PrintHeader("Add a New Task");
            PrintAddTaskInfo();

            string details = null;
            while (string.IsNullOrWhiteSpace(details))
            {
                Console.Write("\n\nEnter Task Detils: ");
                details = ReadInput();
                if (details == null) return;

                if (string.IsNullOrWhiteSpace(details))
                    Console.WriteLine("Cannot have an empty task. Please try again.");
            }

            string project = null;
            while (string.IsNullOrWhiteSpace(project))
            {
                Console.Write("\nEnter Project Name: ");
                project = ReadInput();
                if (project == null) return;

                if (string.IsNullOrWhiteSpace(project))
                    Console.WriteLine("Cannot have an empty project name. Please try again.");
            }

            DateTime dueDate;
            string dueDateInput;

            while (true)
            {
                Console.Write("\nEnter Due Date (yyyy-mm-dd): ");
                dueDateInput = ReadInput();
                if (dueDateInput == null) return;//esc pressed
                if (DateTime.TryParse(dueDateInput, out dueDate)) break;
                else Console.WriteLine("Invalid date format. Example (2024-12-25)");
            }

            Task task = new Task(details, project, dueDate, false);
            tasks.Add(task);
            SaveFile("tasks.csv");
            Console.Clear();
            PrintHeader("Welcome to ToDoLy");
            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine("Task added successfully");
            Console.ResetColor();
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
            //Print list and highlight selectedIndex
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

                    Console.Clear();
                    PrintHeader($"Updating Task: {task.Details}");
                    Console.WriteLine("\n");
                    PrintUpdateTaskInfo2(task);

                    Console.Write("Enter New Task Details: ");
                    string newDetails = Console.ReadLine();
                    if (!string.IsNullOrEmpty(newDetails))
                        task.Details = newDetails;

                    Console.Write("\nEnter New Project for Task: ");
                    string newProject = Console.ReadLine();
                    if (!string.IsNullOrEmpty(newProject))
                        task.Project = newProject;

                    Console.Write("\nEnter New Due Date (yyyy-MM-dd): ");
                    string dateInput = Console.ReadLine();
                    if (!string.IsNullOrEmpty(dateInput) && DateTime.TryParse(dateInput, out DateTime newDate))
                        task.DueDate = newDate;

                    while (true)
                    {
                        Console.Write("\nMark as Completed? (y/n):");
                        string newStatus = Console.ReadLine().Trim().ToLower();
                        if (newStatus == "y")
                        {
                            task.IsCompleted = true;
                            break;
                        }
                        else if (newStatus == "n")
                        {
                            task.IsCompleted = false;
                            break;
                        }
                        else
                        {
                            Console.WriteLine("Invalid input. Please enter \'y\' for yes and \'n\' for no.");
                        }
                    }
                    
                    SaveFile("tasks.csv");
                    Console.WriteLine("Task updated successfully!");
                    break;
                }
                else if (key == ConsoleKey.Escape)//cancel changes
                {
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine("\nUpdate cancelled.");
                    Console.ResetColor();
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
                        Console.ResetColor();
                    }
                    else
                    {
                        Console.ForegroundColor = ConsoleColor.Red;
                        Console.WriteLine("Task was not removed.");
                        Console.ResetColor();
                    }
                    break;
                }
            }
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

            for (int i = 0; i < tasks.Count; i++)
            {
                Task task = tasks[i];
                string status = task.IsCompleted ? "Completed" : "Pending";

                if (selectedIndex.HasValue && i == selectedIndex.Value)
                    Console.ForegroundColor = ConsoleColor.Green;
                Console.WriteLine("{0,5} | {1,-25} | {2,-25} | {3,-12} | {4,-10}",
                              i + 1,
                              task.Details,
                              task.Project,
                              task.DueDate.ToShortDateString(),
                              status);
                Console.ResetColor();

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

        public void PrintHeader(string section)
        {
            string border = new('=', 90);
            string title = section;
            string paddedTitle = title.PadLeft((90 + title.Length) / 2).PadRight(90);

            Console.ForegroundColor = ConsoleColor.Cyan;
            Console.WriteLine(border);
            Console.WriteLine(paddedTitle);
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
            Console.ForegroundColor= ConsoleColor.Cyan;
            Console.WriteLine(
                "1. Enter task details\n2. Enter project name\n" +
                "3. Enter due date\n4. Mark task done/pending\n\nESC to cancel");
            Console.ResetColor();
            Console.WriteLine();
        }

        public void PrintUpdateTaskInfo()
        {
            Console.ForegroundColor = ConsoleColor.Cyan;
            Console.WriteLine(
                "Use the UP and DOWN arrow keys to select a task.\n" +
                "Press ENTER to update or change a task.\n" +
                "Press DEL to DELETE a task.\n" +
                "Press ESC to cancel.\n");
            Console.ResetColor();
            Console.WriteLine();
        }

        public void PrintUpdateTaskInfo2(Task task)
        {
            Console.ForegroundColor = ConsoleColor.Cyan;
            Console.WriteLine(
                "Leave prompt empty if you want to keep current info:\n\n" +
                $"* Task Details = {task.Details}\n" +
                $"* Task Project = {task.Project}\n" +
                $"* Task Due Date = {task.DueDate:d}\n" +
                $"* Task Done Status = {task.IsCompleted}");
            Console.ResetColor();
            Console.WriteLine();
        }

        public string ReadInput()
        {
            StringBuilder input = new();
            while (true)
            {
                var key = Console.ReadKey(true);

                if (key.Key == ConsoleKey.Escape)
                {
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine("Input cancelled.");
                    Console.ResetColor();
                    return null;
                }
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
