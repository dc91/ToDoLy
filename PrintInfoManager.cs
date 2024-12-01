using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;
using static System.Collections.Specialized.BitVector32;

namespace ToDoLy
{
    internal class PrintInfoManager
    {
        public static string SetBanner(ref List<Task> FoundMatches, string? selectedProject,
                                 ref int currentPage, ref int totalPages, ref List<Task> projectTasks)
        {
            if (!string.IsNullOrEmpty(selectedProject) && projectTasks.Count > 0)
                return $"Tasks in Project: {selectedProject}";
            else if (FoundMatches.Count != 0)
                return "Search Results";
            return $"Your Tasks - Page {currentPage + 1} of {totalPages}";
        }

        public static void PrintHeader(string section)
        {
            string border = new('=', 90);
            string title = section;
            string paddedTitle = title.PadLeft((90 + title.Length) / 2).PadRight(90);

            Console.Clear();
            Console.ForegroundColor = ConsoleColor.DarkYellow;
            Console.WriteLine(border);
            Console.WriteLine();
            Console.WriteLine(paddedTitle);
            Console.WriteLine();
            Console.WriteLine(border);
            Console.ResetColor();
            Console.WriteLine();
        }

        public void PrintWelcome(int complete, int pending)
        {
            Console.Write("\n\nYou have completed ");
            PrintWithColor(complete.ToString(), ConsoleColor.DarkRed, " tasks! \nAnd you\'ve got ", false);
            PrintWithColor(pending.ToString(), ConsoleColor.DarkGreen, " tasks pending!");
            Console.WriteLine($"\nYou have a total of {pending + complete} tasks saved");
        }

        public static void PrintOptions()
        {
            Console.Write("Choose an ");
            PrintWithColor("OPTION", ConsoleColor.Blue, ":");
            PrintWithColor("[1]", ConsoleColor.Blue, " Show and Edit Tasks");
            PrintWithColor("[2]", ConsoleColor.Blue, " Add New Task");
            PrintWithColor("[ESC]", ConsoleColor.DarkGray, " Quit");
        }

        public static void PrintSortingOptions(bool showCompletedTasks = true)
        {
            string toggleSetting = showCompletedTasks ? "Hide" : "Show";

            Console.Write("\nHow would you like to ");

            PrintWithColor("SORT", ConsoleColor.Blue, " or ", false);

            PrintWithColor("FILTER", ConsoleColor.DarkYellow, " the tasks?\n");

            PrintWithColor("[1]", ConsoleColor.Blue, " By DATE", false);

            PrintWithColor("\t\t[F]", ConsoleColor.DarkYellow, $" {toggleSetting} COMPLETED", false);
            
            PrintWithColor("\t\t[ARROWS]", ConsoleColor.Red, $" NAVIGATE");

            PrintWithColor("[2]", ConsoleColor.Blue, " By PROJECT", false);

            PrintWithColor("\t\t[P]", ConsoleColor.DarkYellow, " SPECIFIC PROJECT", false);

            PrintWithColor("\t\t[ENTER]", ConsoleColor.Red, $" EDIT Task");

            PrintWithColor("[3]", ConsoleColor.Blue, " Default", false);

            PrintWithColor("\t\t[A]", ConsoleColor.DarkYellow, " ALL PROJECTS", false);

            PrintWithColor("\t\t[DEL]", ConsoleColor.Red, $" DELETE Task");

            PrintWithColor("\t\t\t[S]", ConsoleColor.DarkYellow, " SEARCH", false);

            PrintWithColor("\n[ESC]", ConsoleColor.DarkGray, " to exit.");
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
            PrintWithColor("\nESC to CANCEL", ConsoleColor.DarkGray);
            Console.WriteLine();
        }

        public static void PrintUpdateTaskInfo()
        {
            Console.WriteLine();
            PrintWithColor("\n\n\n\n[UP] or [DOWN]", ConsoleColor.DarkYellow, " to ", false);
            PrintWithColor("HIGHLIGHT", ConsoleColor.DarkYellow, " a value.");
            PrintWithColor("[ENTER]", ConsoleColor.Blue, " to ", false);
            PrintWithColor("UPDATE", ConsoleColor.Blue, " a value.");
            PrintWithColor("[ESC]", ConsoleColor.DarkGray, " to ", false);
            PrintWithColor("CANCEL", ConsoleColor.DarkGray, ".\n", true);
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
                        Console.WriteLine($"\n\nTask: \t\t{task.Details}");
                        break;
                    case "Project":
                        Console.WriteLine($"Project: \t{task.Project}");
                        break;
                    case "Due Date":
                        Console.WriteLine($"Due Date: \t{task.DueDate.ToShortDateString()}");
                        break;
                    case "Completion Status":
                        string status = task.IsCompleted ? "Completed" : "Pending";
                        Console.WriteLine($"Status: \t{status}");
                        break;
                }
                Console.ResetColor();
            }
        }

        public static void PrintAreUSure(Task task)
        {
            Console.Write("Are you sure you want to ");
            PrintWithColor("DELETE", ConsoleColor.Red, " this task?\n");
            Console.WriteLine($"\n\nTask: \t\t{task.Details}");
            Console.WriteLine($"Project: \t{task.Project}");
            Console.WriteLine($"Due Date: \t{task.DueDate.ToShortDateString()}");
            Console.WriteLine($"Status: \t{(task.IsCompleted ? "Completed" : "Pending")}");
            Console.WriteLine(new string('-', 50));
            Console.Write("\nPress 'ENTER' to confirm, or any other key to cancel: ");

        }

        public static void PrintInvalidDate()
        {
            PrintInfoManager.PrintWithColor("\nInvalid date format. Please enter in yyyy-MM-dd format.", ConsoleColor.Red);
            Console.ReadKey();
        }

        public static void PrintInvalidDateEarly()
        {
            PrintInfoManager.PrintWithColor("\nDate is before today. Will not save. Press any key to continue", ConsoleColor.Red);
            Console.ReadKey();
        }

        public static void PrintDeleteCancel()
        {
            PrintInfoManager.PrintWithColor("\nTask was not removed. Press any key...", ConsoleColor.Red);
            Console.ReadKey();
        }

        public static void PrintDeleteConfirm()
        {
            PrintInfoManager.PrintWithColor("\nTask removed successfully! Press any key...", ConsoleColor.Green);
            Console.ReadKey();
        }

        public static void PrintTableHead()
        {
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

        public static void PrintProjectList(ref List<string> projects, ref int selectedIndex)
        {
            for (int i = 0; i < projects.Count; i++)
            {
                if (i == selectedIndex)
                    PrintWithColor("> " + projects[i] + "\n", ConsoleColor.DarkYellow);
                else
                    Console.WriteLine("  " + projects[i]);
            }
        }

        public static void PrintWithColor(string s, ConsoleColor cc, string? post = null, bool newLine = true)
        {
            Console.ForegroundColor = cc;
            Console.Write(s);
            Console.ResetColor();
            if (post != null)
                if (newLine)
                    Console.WriteLine(post);
                else
                    Console.Write(post);
        }

    }
}
