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
            Console.WriteLine("Enter Task Detils:");
            string details = Console.ReadLine();

            Console.WriteLine("Enter Project Name:");
            string project = Console.ReadLine();

            Console.WriteLine("Enter Due Date (yyyy-mm-dd):");
            DateTime dueDate;

            while (!DateTime.TryParse(Console.ReadLine(), out dueDate))
                Console.WriteLine("Invalid date format. Example (2024-12-25)");

            Task task = new Task(details, project, dueDate, false);
            tasks.Add(task);
            Console.WriteLine("Task added successfully");
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
                    Console.WriteLine($"Updating Task: {task.Details}");

                    Console.WriteLine("Enter New Details (leave empty to keep current):");
                    string newDetails = Console.ReadLine();
                    if (!string.IsNullOrEmpty(newDetails))
                        task.Details = newDetails;

                    Console.WriteLine("Enter New Project (leave empty to keep current):");
                    string newProject = Console.ReadLine();
                    if (!string.IsNullOrEmpty(newProject))
                        task.Project = newProject;

                    Console.WriteLine("Enter New Due Date (yyyy-MM-dd, leave empty to keep current):");
                    string dateInput = Console.ReadLine();
                    if (!string.IsNullOrEmpty(dateInput) && DateTime.TryParse(dateInput, out DateTime newDate))
                        task.DueDate = newDate;

                    Console.WriteLine("Mark as Completed? (y/n):");
                    string newStatus = Console.ReadLine();
                    if (newStatus.ToLower() == "y")
                    {
                        task.IsCompleted = true;
                    }
                    else if (newStatus.ToLower() == "n")
                    {
                        task.IsCompleted = false;
                    }

                    Console.WriteLine("Task updated successfully!");
                    break;
                }
                else if (key == ConsoleKey.Escape)//cancel changes
                {
                    Console.WriteLine("Update cancelled.");
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

        public void PrintList()
        {
            if (tasks.Count == 0)
            {
                Console.WriteLine("No tasks to show. Try adding a new task first.");
                return;
            }

            for (int i = 0; i < tasks.Count; i++)
            {
                Task task = tasks[i];
                Console.WriteLine(
                    $"{i + 1}. {task.Details} - " +
                    $"Project: {task.Project}, " +
                    $"Due: {task.DueDate.ToShortDateString()}, " +
                    $"Status: {(task.IsCompleted ? "Completed" : "Pending")}");
            }
        }
        private void PrintTaskList(int selectedIndex)
        {
            Console.Clear();
            Console.WriteLine("Use the UP and DOWN arrow keys to select a task. " +
                "Press ENTER to select. Press ESC to cancel.\n");


            for (int i = 0; i < tasks.Count; i++)
            {
                if (i == selectedIndex)
                {
                    Console.ForegroundColor = ConsoleColor.Green; // Highlight the selected task
                    Console.WriteLine(
                        $"> {tasks[i].Details} - " +
                        $"Project: {tasks[i].Project}, " +
                        $"Due: {tasks[i].DueDate.ToShortDateString()}, " +
                        $"Status: {(tasks[i].IsCompleted ? "Completed" : "Pending")}");
                    Console.ResetColor();
                }
                else
                {
                    Console.WriteLine(
                        $"  {tasks[i].Details} - " +
                        $"Project: {tasks[i].Project}, " +
                        $"Due: {tasks[i].DueDate.ToShortDateString()}, " +
                        $"Status: {(tasks[i].IsCompleted ? "Completed" : "Pending")}");
                } 
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
