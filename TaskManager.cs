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

        public void UpdateTask()
        {
            while (true)
            {
                //Add stuff
            }
        }

        public void DeleteTask(Task task)
        {
            //Add stuff
        }

        public void PrintList()
        {
            //Add stuff
        }

        public void SearchTask(string input)
        {
            //Add stuff
        }

        public void SearchProject(string input)
        {
            //Add stuff
        }
    }
}
