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

        public static void PrintOptions()
        {
            Console.Write("Choose an ");
            Console.ForegroundColor = ConsoleColor.Blue;
            Console.Write("OPTION");
            Console.ResetColor();
            Console.WriteLine(":");

            string[] options = [
                ". Show All Tasks",
                ". Add New Task",
                ". Edit Tasks",
                ". Quit\n"];

            for (int i = 0; i < 4; i++)
            {
                Console.ForegroundColor = ConsoleColor.Blue;
                if (i != 3)
                    Console.Write("* " + (i + 1));
                else
                {
                    Console.ForegroundColor = ConsoleColor.DarkGray;
                    Console.Write("ESC");
                    Console.ResetColor();
                }
                    
                Console.ResetColor();
                Console.WriteLine(options[i]);
            }
        }

        public static void PrintSortingOptions(bool showCompletedTasks = true)
        {
            string toggleSetting = showCompletedTasks ? "Showing" : "Hiding";
            Console.Write("\nHow would you like to ");
            Console.ForegroundColor = ConsoleColor.Blue;
            Console.Write("SORT");
            Console.ResetColor();
            Console.WriteLine(" the tasks?\n");
            Console.ForegroundColor = ConsoleColor.Blue;
            Console.Write("[1]");
            Console.ResetColor();
            Console.WriteLine(" By Due Date");
            Console.ForegroundColor = ConsoleColor.Blue;
            Console.Write("[2]");
            Console.ResetColor();
            Console.WriteLine(" By Project Name");
            Console.ForegroundColor = ConsoleColor.Blue;
            Console.Write("[3]");
            Console.ResetColor();
            Console.WriteLine(" Default");
            

            Console.Write("\nHow would you like to ");
            Console.ForegroundColor = ConsoleColor.DarkYellow;
            Console.Write("FILTER");
            Console.ResetColor();
            Console.WriteLine(" the tasks?\n");
            Console.ForegroundColor = ConsoleColor.DarkYellow;
            Console.Write("[F]");
            Console.ResetColor();
            Console.WriteLine($" Show/Hide Completed Tasks (Currently: {toggleSetting})");
            Console.ForegroundColor = ConsoleColor.DarkYellow;
            Console.Write("[P]");
            Console.ResetColor();
            Console.WriteLine($" View Task From a Specific Project");
            Console.ForegroundColor = ConsoleColor.DarkYellow;
            Console.Write("[S]");
            Console.ResetColor();
            Console.WriteLine($" Search For Task");

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

        public static void PrintUpdateTaskInfo(bool enableDeleteInfo)
        {
            SmartUpdatePrint("UP or DOWN", "HIGHLIGHT", " a task.\n", ConsoleColor.DarkYellow);
            SmartUpdatePrint("ENTER", "UPDATE", " a task.\n", ConsoleColor.Blue);
            if (enableDeleteInfo)
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

        public static void PrintTableRows(List<Task> originalTasks, int? selectedIndex)
        {
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
        }

        public static void PrintWrongInput()
        {
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine("\nInvalid option. Please press 1, 2, 3 or ESC.");
            Console.ResetColor();
        }



        

    }
}
