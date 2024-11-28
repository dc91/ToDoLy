using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ToDoLy
{
    internal class PrintInfoManager
    {
        //PrintInfoManager
        public static void PrintHeader(string section)
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

        public void PrintWelcome(int complete, int pending)
        {
            Console.Write("\n\nYou have ");
            Console.ForegroundColor = ConsoleColor.DarkRed;
            Console.Write(pending);
            Console.ResetColor();
            Console.Write(" tasks to do and ");
            Console.ForegroundColor = ConsoleColor.DarkGreen;
            Console.Write(complete);
            Console.ResetColor();
            Console.WriteLine(" tasks are done!\n");
        }

        public static void PrintSortingOptions()
        {
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

        public static void PrintAddTaskInfo()
        {
            Console.WriteLine("1. Enter task details\n2. Enter project name");
            Console.WriteLine("3. Enter due date\n4. Mark task done/pending\n");
            Console.ForegroundColor = ConsoleColor.DarkGray;
            Console.WriteLine("ESC to CANCEL");
            Console.ResetColor();
        }

        public static void PrintUpdateTaskInfo()
        {
            SmartUpdatePrint("UP or DOWN", "HIGHLIGHT", " a task.\n", ConsoleColor.DarkYellow);
            SmartUpdatePrint("ENTER", "UPDATE", " a task.\n", ConsoleColor.Blue);
            SmartUpdatePrint("DEL", "DELETE", " a task.\n", ConsoleColor.Red);
            SmartUpdatePrint("ESC", "CANCEL", ".\n", ConsoleColor.DarkGray);
            Console.WriteLine();
        }

        public static void SmartUpdatePrint(string key, string action, string ending, ConsoleColor color)
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

        public static void PrintCancel()
        {
            Console.ForegroundColor = ConsoleColor.Green;
            Console.Write("\nCancelling... Press any key");
            Console.ReadKey();
            Console.ResetColor();
        }

        public static void PrintAddSuccess()
        {
            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine("Task successfully saved... Press any key");
            Console.ReadKey();
            Console.ResetColor();
        }

        public static void PrintAreUSure(Task task)
        {
            Console.Write("\nAre you sure you want to ");
            Console.ForegroundColor = ConsoleColor.Red;
            Console.Write("DELETE");
            Console.ResetColor();
            Console.WriteLine(" this task?\n");

            Console.WriteLine(
                        "Task: " +
                        $"{task.Details}\n" +
                        $"Project: {task.Project}\n" +
                        $"Due: {task.DueDate.ToShortDateString()}\n" +
                        $"Status: {(task.IsCompleted ? "Completed" : "Pending")}\n");
            Console.WriteLine(new string('-', 50));
            Console.Write("\nPress 'y' to confirm, or any other key to cancel: ");

        }

        public static void PrintRemoveSuccess()
        {
            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine("Task removed successfully!");
            Console.ReadKey();
            Console.ResetColor();
        }

        public static void PrintRemoveCancelled()
        {
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine("Task was not removed.");
            Console.ReadKey();
            Console.ResetColor();
        }

        public static void PrintInvalidDate()
        {
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine("\nInvalid date format. Please enter in yyyy-MM-dd format.");
            Console.ResetColor();
            Console.ReadKey();
        }

        public static void PrintInvalidBool()
        {
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine("Invalid input. Please enter 'c' for Completed or 'p' for Pending.");
            Console.ResetColor();
            Console.ReadKey();
        }

        public static void PrintTableHead()
        {
            Console.WriteLine();
            Console.WriteLine("{0,5} | {1,-25} | {2,-25} | {3,-12} | {4,-10}",
                      "No.", "Task Details", "Project", "Due Date", "Status");
            Console.WriteLine(new string('-', 90));
        }

        public static void PrintWrongInput()
        {
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine("\nInvalid option. Please press 1, 2, 3 or ESC.");
            Console.ResetColor();
        }

        

    }
}
