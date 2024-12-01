using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ToDoLy
{
    internal class UserInputManager
    {
        public static string ReadEveryKey()
        {// Reads input key-by-key. return null if ESC, or returns full input string
            StringBuilder input = new();
            while (true)
            {
                var key = Console.ReadKey(true);

                if (key.Key == ConsoleKey.Escape)
                {
                    Console.WriteLine();
                    return null;
                }
                else if (key.Key == ConsoleKey.Enter)
                {
                    Console.WriteLine();
                    return input.ToString();
                }
                else if (key.Key == ConsoleKey.Backspace && input.Length > 0)
                {
                    input.Remove(input.Length - 1, 1);
                    Console.Write("\b \b");//needs the space to erase
                }
                else if (!char.IsControl(key.KeyChar))//example of control is \n \t backspace esc etc
                {//small buggs when typing control char, they are invis but cant be erased like §.
                    input.Append(key.KeyChar);
                    Console.Write(key.KeyChar);
                }
            }
        }

        public static string GetInput(string prompt, string errMess, bool expectDateTime)
        {
            while (true)
            {
                Console.Write(prompt);
                string input = ReadEveryKey();

                if (input == null)
                {
                    PrintInfoManager.PrintWithColor("\nCancelling... Press any key", ConsoleColor.Green);
                    Console.ReadKey();
                    return null;
                }

                if (!expectDateTime && !string.IsNullOrWhiteSpace(input)) return input;
                if (expectDateTime && DateTime.TryParse(input, out DateTime date))
                {
                    if (date < DateTime.Now)
                    {
                        PrintInfoManager.PrintInvalidDateEarly();
                        continue;
                    }
                    return date.ToString();
                }
                Console.WriteLine(errMess);
            }
        }

        public static ConsoleKey TrapUntilValidInput(int setting = 0)
        {
            // Force a valid input before using keypress
            // Setting 0 - All possible action buttons
            // Setting 1 - Only vertical arrows and enter + esc
            while (true)
            {
                ConsoleKey tryKey = Console.ReadKey(true).Key;

                if (setting == 0 && (tryKey == ConsoleKey.D1 || tryKey == ConsoleKey.D2 ||
                        tryKey == ConsoleKey.D3 || tryKey == ConsoleKey.F ||
                        tryKey == ConsoleKey.P || tryKey == ConsoleKey.A ||
                        tryKey == ConsoleKey.S || tryKey == ConsoleKey.Escape ||
                        tryKey == ConsoleKey.LeftArrow || tryKey == ConsoleKey.RightArrow ||
                        tryKey == ConsoleKey.DownArrow || tryKey == ConsoleKey.UpArrow ||
                        tryKey == ConsoleKey.Enter || tryKey == ConsoleKey.Delete))
                {
                    return tryKey;
                }
                else if (setting == 1 && (tryKey == ConsoleKey.DownArrow || tryKey == ConsoleKey.UpArrow ||
                        tryKey == ConsoleKey.Enter || tryKey == ConsoleKey.Escape))
                    return tryKey;


            }
        }
    }
}
