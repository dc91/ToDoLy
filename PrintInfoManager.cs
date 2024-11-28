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
    }
}
