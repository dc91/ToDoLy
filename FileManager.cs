using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ToDoLy
{
    internal class FileManager
    {
        public List<Task> LoadTasks(string filePath)
        {
            List<Task> tasks = new();
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
            return tasks;
        }

        public void SaveFile(string filePath, List<Task> tasks)
        {
            using StreamWriter sw = new(filePath);
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
    }
}
