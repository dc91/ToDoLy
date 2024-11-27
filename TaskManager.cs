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
