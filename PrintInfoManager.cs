using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ToDoLy
{
    internal class PrintInfoManager
    {
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
            Console.Write("\n\nYou have completed ");
            Console.ForegroundColor = ConsoleColor.DarkRed;
            Console.Write(complete);
            Console.ResetColor();
            Console.Write(" tasks! \nAnd you\'ve got ");
            Console.ForegroundColor = ConsoleColor.DarkGreen;
            Console.Write(pending);
            Console.ResetColor();
            Console.WriteLine(" tasks pending!\n");
            Console.WriteLine($"You have a total of {pending + complete} tasks saved");
        }

        public static void PrintOptions()
        {
            Console.Write("Choose an ");
            Console.ForegroundColor = ConsoleColor.Blue;
            Console.Write("OPTION");
            Console.ResetColor();
            Console.WriteLine(":");

            string[] options = [
                " Show and Edit Tasks",
                " Add New Task",
                " Quit\n"];

            for (int i = 0; i < 3; i++)
            {
                Console.ForegroundColor = ConsoleColor.Blue;
                if (i != 2)
                    Console.Write("[" + (i + 1) + "]  ");
                else
                {
                    Console.ForegroundColor = ConsoleColor.DarkGray;
                    Console.Write("[ESC]");
                    Console.ResetColor();
                }
                    
                Console.ResetColor();
                Console.WriteLine(options[i]);
            }
        }

        public static void PrintSortingOptions(bool showCompletedTasks = true)
        {
            string toggleSetting = showCompletedTasks ? "Hide" : "Show";
            Console.Write("\nHow would you like to ");
            Console.ForegroundColor = ConsoleColor.Blue;
            Console.Write("SORT");
            Console.ResetColor();
            Console.Write(" or ");
            Console.ForegroundColor = ConsoleColor.DarkYellow;
            Console.Write("FILTER");
            Console.ResetColor();
            Console.WriteLine(" the tasks?\n");
            Console.ForegroundColor = ConsoleColor.Blue;
            Console.Write("[1]");
            Console.ResetColor();
            Console.Write(" By DATE");

            Console.ForegroundColor = ConsoleColor.DarkYellow;
            Console.Write("\t\t[F]");
            Console.ResetColor();
            Console.Write($" {toggleSetting} COMPLETED");

            Console.ForegroundColor = ConsoleColor.Red;
            Console.Write("\t\t[ARROWS]");
            Console.ResetColor();
            Console.WriteLine($" NAVIGATE");          

            Console.ForegroundColor = ConsoleColor.Blue;
            Console.Write("[2]");
            Console.ResetColor();
            Console.Write(" By PROJECT");

            Console.ForegroundColor = ConsoleColor.DarkYellow;
            Console.Write("\t\t[P]");
            Console.ResetColor();
            Console.Write($" SPECIFIC PROJECT");

            Console.ForegroundColor = ConsoleColor.Red;
            Console.Write("\t\t[ENTER]");
            Console.ResetColor();
            Console.WriteLine($" EDIT Task");

            Console.ForegroundColor = ConsoleColor.Blue;
            Console.Write("[3]");
            Console.ResetColor();
            Console.Write(" Default");

            Console.ForegroundColor = ConsoleColor.DarkYellow;
            Console.Write("\t\t[A]");
            Console.ResetColor();
            Console.Write($" ALL TASKS");

            Console.ForegroundColor = ConsoleColor.Red;
            Console.Write("\t\t\t[DEL]");
            Console.ResetColor();
            Console.WriteLine($" DELETE Task");

            Console.ForegroundColor = ConsoleColor.DarkYellow;
            Console.Write("\t\t\t[S]");
            Console.ResetColor();
            Console.WriteLine($" SEARCH");

            Console.Write("\nPress ");
            Console.ForegroundColor = ConsoleColor.DarkGray;
            Console.Write("[ESC]");
            Console.ResetColor();
            Console.WriteLine(" to exit.");
        }

        public static void PrintAddTaskInfo(int activeStep = 1)
        {
            string[] prompt = ["Enter Task Details", "Enter Project Name", "Enter Due Date"];

            for (int step = 1; step <= 3; step++)
            {
                string activePrompt = prompt[step-1];
                if (step == activeStep)
                {
                    Console.ForegroundColor = ConsoleColor.DarkYellow;
                    Console.WriteLine($"> {activePrompt}");
                    Console.ResetColor();
                }
                else
                {
                    Console.WriteLine($"  {activePrompt}");
                }
            }
            Console.ForegroundColor = ConsoleColor.DarkGray;
            Console.WriteLine("\nESC to CANCEL");
            Console.ResetColor();
        }

        public static void PrintUpdateTaskInfo()
        {
            Console.WriteLine();
            SmartUpdatePrint("UP or DOWN", "HIGHLIGHT", " a value.", ConsoleColor.DarkYellow);
            SmartUpdatePrint("ENTER", "UPDATE", " a value.", ConsoleColor.Blue);
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

        public static void PrintUpdateTaskFields(string[] fields, Task task, int fieldIndex)
        {
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
            Console.Write("\nPress 'ENTER' to confirm, or any other key to cancel: ");

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

        public static void PrintInvalidDateEarly()
        {
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine("\nDate is before today. Will not save. Press any key to continue");
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
            Console.WriteLine("| {0,-25} | {1,-25} | {2,-12} | {3,-10}",
                      "Task Details", "Project", "Due Date", "Status");
            Console.WriteLine(new string('-', 90));
        }

        public static void PrintTableRows(List<Task> originalTasks, int? selectedIndex)
        {
            for (int i = 0; i < originalTasks.Count; i++)
            {
                Task task = originalTasks[i];
                string status = task.IsCompleted ? "Completed" : "Pending";

                if (selectedIndex.HasValue && i == selectedIndex.Value)
                    Console.ForegroundColor = ConsoleColor.DarkYellow;
                Console.WriteLine("| {0,-25} | {1,-25} | {2,-12} | {3,-10}",
                      task.Details,
                      task.Project,
                      task.DueDate.ToShortDateString(),
                      status);
                Console.ResetColor();
            }
        }

        

    }
}
