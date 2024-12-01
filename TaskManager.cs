using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using static System.Collections.Specialized.BitVector32;

namespace ToDoLy
{
    internal class TaskManager
    {
        public FileManager fManager = new();
        public List<Task> tasks = new();

        public void AddTask()
        {
            Console.Clear();// clear after each step so the console doesnt get full of eventual misstakes
            PrintInfoManager.PrintHeader("Add a New Task");
            Console.WriteLine();
            PrintInfoManager.PrintAddTaskInfo(1);

            string details = UserInputManager.GetInput("\nEnter Task Details: ",
                "Cannot have an empty task. Try again.", false);
            if (details == null) return;

            Console.Clear();
            PrintInfoManager.PrintHeader("Add a New Task");
            Console.WriteLine();
            PrintInfoManager.PrintAddTaskInfo(2);

            Console.WriteLine(details);

            string project = UserInputManager.GetInput("\nEnter Project Name: ", 
                "Cannot have an project name. Try again.", false);
            if (project == null) return;

            Console.Clear();
            PrintInfoManager.PrintHeader("Add a New Task");
            Console.WriteLine();
            PrintInfoManager.PrintAddTaskInfo(3);
            Console.WriteLine($"{details}\n{project}");

            string dueDateString = UserInputManager.GetInput("\nEnter Due Date (yyyy-mm-dd): ",
                "Invalid date format. Example (2024-12-25)", true);
            if (dueDateString == null) return;

            DateTime dueDate = DateTime.Parse(dueDateString);

            Task task = new Task(details, project, dueDate, false);
            tasks.Add(task);
            fManager.SaveFile("tasks.csv", tasks);
            PrintInfoManager.PrintAddSuccess();
        }

        public void UpdateTask()
        {
            if (tasks.Count == 0)
            {
                Console.WriteLine("No tasks to view. Try adding a new task first.");
                Console.ReadKey();
                return;
            }

            bool showCompletedTasks = true;
            string? selectedProject = null;
            List<Task> FoundMatches = [];
            
            List<Task> filteredTasks = new List<Task>(tasks);
            List<Task> sortedTasks = new List<Task>(tasks);
            List<Task> projectTasks = new List<Task>();

            int currentPage = 0;
            int selectedIndex = 0;
            const int itemsPerPage = 6;// const so we don't divide by 0 when defining totalPages

            bool keepGoing = true;
            while (keepGoing)
            {
                //Apply any filters
                filteredTasks = FilterOptions(ref showCompletedTasks, selectedProject, 
                                              ref FoundMatches, ref sortedTasks, ref projectTasks);

                int totalPages = (int)Math.Ceiling((double)filteredTasks.Count / itemsPerPage);
                int startIndex = currentPage * itemsPerPage;
                int endIndex = Math.Min(startIndex + itemsPerPage, filteredTasks.Count);

                //Print header and table
                Console.Clear();
                string banner = PrintInfoManager.SetBanner(ref FoundMatches, selectedProject, 
                                                           ref currentPage, ref totalPages, ref projectTasks);
                PrintInfoManager.PrintHeader(banner);
                PrintInfoManager.PrintTableHead();
                PrintInfoManager.PrintTableRows(filteredTasks
                    .GetRange(startIndex, endIndex - startIndex), selectedIndex);

                //fill with blank lines, if last page not full
                int remainingLines = itemsPerPage - (endIndex - startIndex);
                for (int i = 0; i < remainingLines; i++) Console.WriteLine();

                PrintInfoManager.PrintSortingOptions(showCompletedTasks);

                // Check what key is pressed, act accordingly.. yes it's long
                keepGoing = UserAction(ref filteredTasks, ref sortedTasks, ref FoundMatches, 
                                       ref showCompletedTasks, ref selectedProject,
                                       ref currentPage, ref selectedIndex, startIndex,
                                       endIndex, totalPages, ref projectTasks);
            }
        }

        private static List<Task> FilterOptions(ref bool showCompletedTasks, string? selectedProject, 
                                                ref List<Task> FoundMatches, ref List<Task> sortedTasks,
                                                ref List<Task> projectTasks)
        {
            List<Task> filteredTasks = showCompletedTasks ? sortedTasks : sortedTasks
                                       .Where(t => t.IsCompleted == false).ToList();

            projectTasks = new List<Task>();

            if (!string.IsNullOrEmpty(selectedProject))
                projectTasks = filteredTasks.Where(t => t.Project == selectedProject).ToList();

            if (projectTasks.Count > 0)//if user deletes last task in project, see all projects
                filteredTasks = projectTasks;


            return FoundMatches.Count == 0
                   ? filteredTasks
                   : filteredTasks.Intersect(FoundMatches).ToList();
        }


        public bool UserAction(ref List<Task> filteredTasks, ref List<Task> sortedTasks,
                               ref List<Task> FoundMatches, ref bool showCompletedTasks, 
                               ref string? selectedProject, ref int currentPage,ref int selectedIndex, 
                               int startIndex, int endIndex, int totalPages, ref List<Task> projectTasks)
        {
            while (true)
            {
                ConsoleKey key = UserInputManager.TrapUntilValidInput();
                switch (key)
                {
                    case ConsoleKey.Enter:
                        Task task = filteredTasks[selectedIndex + (6 * currentPage)];
                        selectedIndex = 0;
                        UpdateTaskDetails(task, ref selectedProject);//if updates give emtpy litst, see all, otherwise stay
                        List<Task> returnToListAfterUpdate = filteredTasks.Where(t => t != task).ToList();
                        if (returnToListAfterUpdate.Count == 0)
                            showCompletedTasks = true;
                        selectedIndex = 0;
                        return true;

                    case ConsoleKey.Escape:
                        return false;

                    case ConsoleKey.Delete:
                        Task taskToDelete = filteredTasks[selectedIndex + (6 * currentPage)];
                        int unsortedIndex = tasks.IndexOf(taskToDelete);
                        Console.Clear();
                        PrintInfoManager.PrintHeader($"Delete Task: {tasks[unsortedIndex].Details}");
                        PrintInfoManager.PrintAreUSure(tasks[unsortedIndex]);
                        ConsoleKey confirmDelete = Console.ReadKey(true).Key;
                        if (confirmDelete == ConsoleKey.Enter)
                        {
                            tasks.RemoveAt(unsortedIndex);
                            fManager.SaveFile("tasks.csv", tasks);
                            List<Task> returnToListAfterDelete = filteredTasks.Where(t => t != taskToDelete).ToList();
                            if (returnToListAfterDelete.Count == 0)
                                showCompletedTasks = true;
                            filteredTasks = tasks;
                            sortedTasks = tasks;
                            FoundMatches = [];
                            PrintInfoManager.PrintRemoveSuccess();
                            if (tasks.Count == 0)
                            {
                                Console.WriteLine("\nDeleted last task. Go back to main menu by pressing any key...");
                                Console.ReadKey();
                                return false;
                            }
                        }
                        else
                            PrintInfoManager.PrintRemoveCancelled();
                        selectedIndex = 0;
                        currentPage = 0;
                        return true;

                    case ConsoleKey.D1:
                        sortedTasks = tasks.OrderBy(t => t.DueDate).ToList();
                        currentPage = 0;
                        return true;

                    case ConsoleKey.D2:
                        sortedTasks = tasks.OrderBy(t => t.Project).ToList();
                        currentPage = 0;
                        return true;

                    case ConsoleKey.D3:
                        sortedTasks = new List<Task>(tasks);
                        currentPage = 0;
                        return true;

                    case ConsoleKey.F:
                        showCompletedTasks = !showCompletedTasks;
                        List<Task> checkEmptyList = filteredTasks.Where(t => t.IsCompleted == false).ToList();
                        if (checkEmptyList.Count == 0)
                        {
                            Console.WriteLine("\nFiltering would only give an empty list. Press any key...");
                            Console.ReadKey();
                            showCompletedTasks = true;
                        }
                        selectedIndex = 0;
                        currentPage = 0;
                        return true;

                    case ConsoleKey.P:
                        FoundMatches = [];
                        selectedProject = ShowProjectSelect();
                        selectedIndex = 0;
                        currentPage = 0;
                        showCompletedTasks = true;
                        return true;

                    case ConsoleKey.A:
                        FoundMatches = [];
                        sortedTasks = new List<Task>(tasks);
                        selectedProject = null;
                        showCompletedTasks = true;
                        currentPage = 0;
                        return true;

                    case ConsoleKey.S:
                        List<Task> searchResults = SearchForTask();
                        if (searchResults.Count == 0)
                        {
                            Console.WriteLine("No Task found... Press any key to continue");
                            Console.ReadKey();
                        }
                        else
                        {
                            selectedProject = null;
                            showCompletedTasks = true;
                            FoundMatches = searchResults;
                        }
                        currentPage = 0;
                        selectedIndex = 0;
                        return true;

                    case ConsoleKey.LeftArrow:
                        if (currentPage > 0)
                        {
                            currentPage--;
                            selectedIndex = 0;
                        }
                        else currentPage = 0;
                        return true;

                    case ConsoleKey.RightArrow:
                        if (currentPage < totalPages - 1)
                        {
                            currentPage++;
                            selectedIndex = 0;
                        }
                        else currentPage = totalPages - 1;
                        return true;

                    case ConsoleKey.DownArrow:
                        if (selectedIndex < (endIndex - startIndex - 1))
                            selectedIndex++;
                        return true;

                    case ConsoleKey.UpArrow:
                        if (selectedIndex > 0)
                            selectedIndex--;
                        return true;

                    default:
                        return true;
                }
            }
        }


        public void UpdateTaskDetails(Task task, ref string? selectedProject)
        {
            int fieldIndex = 0;
            bool isUpdating = true;

            while (isUpdating)
            {
                Console.Clear();
                PrintInfoManager.PrintHeader($"Updating Task: {task.Details}");
                PrintInfoManager.PrintUpdateTaskInfo();

                string[] fields = ["Task Details", "Project", "Due Date", "Completion Status"];

                PrintInfoManager.PrintUpdateTaskFields(fields, task, fieldIndex);

                ConsoleKey key = Console.ReadKey(true).Key;

                if (key == ConsoleKey.DownArrow && fieldIndex < fields.Length - 1)
                    fieldIndex++;
                else if (key == ConsoleKey.UpArrow && fieldIndex > 0)
                    fieldIndex--;
                else if (key == ConsoleKey.Enter)
                {
                    if (fieldIndex == 3)//User just presses enter to toggle complete/pending
                        task.IsCompleted = !task.IsCompleted;
                    else
                        EditTaskDetails(fieldIndex, task, fields, ref selectedProject);
                }
                else if (key == ConsoleKey.Escape)
                    isUpdating = false;
            }
            Console.ResetColor();
        }

        public void EditTaskDetails(int fieldIndex, Task task, string[] fields, ref string? selectedProject)
        {
            Console.CursorVisible = true;
            Console.Write($"\nEnter new value for {fields[fieldIndex]}: ");
            string newValue = UserInputManager.ReadEveryKey();// dont check for null since null == no changes
            Console.CursorVisible = false;

            // Update the task with the new value
            switch (fields[fieldIndex])
            {
                case "Task Details":
                    if (!string.IsNullOrEmpty(newValue))
                    {
                        task.Details = newValue;
                        fManager.SaveFile("tasks.csv", tasks);
                    }
                    break;
                case "Project":
                    if (!string.IsNullOrEmpty(newValue))
                    {
                        selectedProject = null;
                        task.Project = newValue;
                        fManager.SaveFile("tasks.csv", tasks);
                    }
                    break;
                case "Due Date":
                    if (string.IsNullOrEmpty(newValue))
                        break;
                    else if (DateTime.TryParse(newValue, out DateTime newDate))
                    {
                        if (newDate < DateTime.Now)
                        {
                            PrintInfoManager.PrintInvalidDateEarly();
                        }
                        else
                        {
                            task.DueDate = newDate;
                            fManager.SaveFile("tasks.csv", tasks);
                        }
                    }
                    else
                        PrintInfoManager.PrintInvalidDate();
                    break;
            }
        }

        public List<Task> SearchForTask()
        {
            List<Task> inputMatches = new List<Task>();
            Console.Write("\nPlease enter your search here:");
            string input = UserInputManager.ReadEveryKey().Trim().ToLower();

            if (input != null)
                inputMatches = tasks.Where(x => x.Details.Trim().ToLower() == input).ToList();
            return inputMatches;
        }

        public string ShowProjectSelect()
        {
            string selectedProject = "";

            List<string> projects = tasks.Select(x => x.Project).Distinct().ToList();
            int selectedIndex = 0;

            ConsoleKey key;

            do
            {
                Console.Clear();
                PrintInfoManager.PrintHeader("List Of Projects");
                Console.WriteLine();
                for (int i = 0; i < projects.Count; i++)
                {
                    if (i == selectedIndex)
                    {
                        Console.ForegroundColor = ConsoleColor.DarkYellow;
                        Console.WriteLine("> " + projects[i]);
                        Console.ResetColor();
                    }
                    else
                    {
                        Console.WriteLine("  " + projects[i]);
                    }
                }

                key = Console.ReadKey(true).Key;

                switch (key)
                {
                    case ConsoleKey.UpArrow:
                        if (selectedIndex > 0)
                            selectedIndex--;
                        break;

                    case ConsoleKey.DownArrow:
                        if (selectedIndex < projects.Count - 1)
                            selectedIndex++;
                        break;
                    case ConsoleKey.Enter:
                        selectedProject = projects[selectedIndex];
                        break;

                    case ConsoleKey.Escape:
                        return null;
                    default:
                        break;
                }
            } while (key != ConsoleKey.Enter);
            selectedIndex = 0;
            return selectedProject;
        }

    }
}