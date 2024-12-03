using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Numerics;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using static System.Collections.Specialized.BitVector32;
using static System.Net.Mime.MediaTypeNames;
namespace ToDoLy
{
    internal class PrintInfoManager
    {
        public static int lastSize = 0;

        public static void ClearLines()
        {
            var currPos = Console.GetCursorPosition();
            for (int i = 0; i < Console.WindowHeight - currPos.Top - 1; i++)
            {
                Console.WriteLine(new string(' ', Console.WindowWidth));
            }
            Console.SetCursorPosition(currPos.Left, currPos.Top);
        }

        public static void ClearConsole()
        {
            Process.Start(new ProcessStartInfo
            {
                FileName = "cmd.exe",
                Arguments = "/C cls", // /C runs the command and exits
                UseShellExecute = false,
                RedirectStandardOutput = false
            })?.WaitForExit();
        }

        public static string SetBanner(ref List<Task> FoundMatches, string? selectedProject,
                                 ref int currentPage, ref int totalPages, ref List<Task> projectTasks)
        {
            if (!string.IsNullOrEmpty(selectedProject) && projectTasks.Count > 0)
            {
                string replaceWithShort = (selectedProject.Length > 20) ? selectedProject[..20] : selectedProject;
                return $"Tasks in Project: {replaceWithShort}";
            }
            else if (FoundMatches.Count != 0)
                return "Search Results";
            return $"Your Tasks - Page {currentPage + 1} of {totalPages}";
        }

        public static void PrintHeader(string section)
        {
            //clear lines in scrollback if lines printed exceed windowHeight
            int cursPos = Console.CursorTop;
            int winHeight = Console.WindowHeight;

            if (Console.WindowHeight > 1 && (cursPos >= winHeight - 1 || lastSize != winHeight))
                ClearConsole();
            else
                Console.SetCursorPosition(0, 0);
            lastSize = winHeight;

            string border = new('=', Console.WindowWidth);
            string title = section;
            string paddedTitle = title.PadLeft((Console.WindowWidth + title.Length) / 2).PadRight(Console.WindowWidth);

            if (Console.WindowHeight > 20)
            {
                Console.ForegroundColor = ConsoleColor.DarkYellow;
                Console.WriteLine(border);
                Console.WriteLine(new string(' ', Console.WindowWidth));
                Console.WriteLine(paddedTitle);
                Console.WriteLine(new string(' ', Console.WindowWidth));
                Console.WriteLine(border);
                Console.ResetColor();
                Console.WriteLine(new string(' ', Console.WindowWidth));
            }
            
            ClearLines();
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
            
            Console.SetCursorPosition(0, Console.WindowHeight - 6);

            string header = string.Format("| {0,-18} | {1,-18} | {2,-18} | {3,-18} |", "", "", "", "");
            int leftPadding = Math.Max((Console.WindowWidth - header.Length ) / 2, 0);

            PrintWithColor(new string(' ', leftPadding) + "SORTING", ConsoleColor.Blue);

            PrintWithColor("\t\tFILTERS/SEARCH", ConsoleColor.DarkYellow);

            PrintWithColor("\t\tACTIONS", ConsoleColor.Red);

            PrintWithColor("\t\t\t[ESC]", ConsoleColor.DarkGray, " to exit." + "\n", false);

            PrintWithColor("[1]".PadLeft(leftPadding + 3), ConsoleColor.Blue, " Default", false);

            PrintWithColor("\t\t" + "[F]", ConsoleColor.DarkYellow, $" {toggleSetting} COMPLETED", false);
            
            PrintWithColor("\t" + "[ARROWS]", ConsoleColor.Red, " NAVIGATE");

            PrintWithColor("[2]".PadLeft(leftPadding + 3), ConsoleColor.Blue, " By PROJECT", false);

            PrintWithColor("\t\t[P]", ConsoleColor.DarkYellow, " SPECIFIC PROJECT", false);

            PrintWithColor("\t[ENTER]", ConsoleColor.Red, $" EDIT Task");

            PrintWithColor("[3]".PadLeft(leftPadding + 3), ConsoleColor.Blue, " By DATE", false);

            PrintWithColor("\t\t[A]", ConsoleColor.DarkYellow, " ALL PROJECTS", false);

            PrintWithColor("\t[DEL]", ConsoleColor.Red, $" DELETE Task");

            PrintWithColor("\t\t\t\t[S]".PadLeft(leftPadding), ConsoleColor.DarkYellow, " SEARCH", false);

            
        }

        public static void PrintAddTaskInfo(int activeStep = 1)
        {
            ClearLines();

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
            PrintWithColor("CANCEL", ConsoleColor.DarkGray, ".", false);
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
                        Console.WriteLine($"Task: \t\t{task.GetLineBreakDetails}");
                        break;
                    case "Project":
                        Console.WriteLine($"Project: \t{task.GetLineBreakProject}");
                        break;
                    case "Due Date":
                        Console.WriteLine($"Due Date: \t{task.DueDate.ToShortDateString()}\n");
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
            Console.WriteLine($"\n\nTask: \t\t{task.GetLineBreakDetails}");
            Console.WriteLine($"Project: \t{task.GetLineBreakProject}");
            Console.WriteLine($"Due Date: \t{task.DueDate.ToShortDateString()}");
            Console.WriteLine($"\nStatus: \t{(task.IsCompleted ? "Completed" : "Pending")}");
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
            // Define the table header and separator
            string header = string.Format("| {0,-25} | {1,-25} | {2,-12} | {3,-10} |",
                                           "Task Details", "Project", "Due Date", "Status");
            string separator = new('-', header.Length);
            int leftPadding = Math.Max((Console.WindowWidth - header.Length) / 2, 0);

            Console.WriteLine(new string(' ', leftPadding) + header);
            Console.WriteLine(new string(' ', leftPadding) + separator);
        }

        public static void PrintTableRows(List<Task> originalTasks, int? selectedIndex)
        {
            lastSize = Console.WindowHeight;
            string row = "| {0,-25} | {1,-25} | {2,-12} | {3,-10} |";
            // Need row length based on the format. Using row.Length isnt enough
            int rowLength = string.Format(row, "", "", "", "").Length;

            for (int i = 0; i < originalTasks.Count; i++)
            {
                Task task = originalTasks[i];
                string status = task.IsCompleted ? "Completed" : "Pending";

                int leftPadding = Math.Max((Console.WindowWidth - rowLength) / 2, 0);

                if (selectedIndex.HasValue && i == selectedIndex.Value)
                    Console.ForegroundColor = ConsoleColor.DarkYellow;
                Console.WriteLine(
                    new string(' ', leftPadding) +
                    string.Format(row, task.GetShortDetails, task.GetShortProject, task.DueDate.ToShortDateString(), status));
                Console.ResetColor();
            }
            Console.WriteLine(new string(' ', Console.WindowWidth));
        }

        public static void PrintProjectList(ref List<string> projects, ref int selectedIndex)
        {
            for (int i = 0; i < projects.Count; i++)
            {
                string formatted = FormatLongString(projects[i]);
                string shortProjectName = projects[i].Length > 50 ? projects[i][..47] + "..." : projects[i];
                if (i == selectedIndex)
                    PrintWithColor("\n" + "> ".PadLeft(16) + formatted + "\n", ConsoleColor.DarkYellow);
                else
                    Console.WriteLine("  ".PadLeft(16) + shortProjectName);
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


        public static string FormatLongString(string input)
        {
            const int lineLength = 50;
            string formatted = string.Empty;

            for (int i = 0; i < input.Length; i += lineLength)
            {
                int remaning = Math.Min(lineLength, input.Length - i);
                string segment = input.Substring(i, remaning);
                formatted += segment.PadRight(lineLength) + Environment.NewLine + "\t\t";
            }
            return formatted;
        }
    }
}
