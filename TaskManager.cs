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
        private FileManager fManager = new();
        public List<Task> tasks { get; set; }


        public void AddTask()
        {
            PrintInfoManager.PrintHeader("Add a New Task");
            PrintInfoManager.PrintAddTaskInfo(1);

            string details = UserInputManager.GetInput("\nEnter Task Details: ",
                "Cannot have an empty task. Try again.", false);
            if (details == null) return;

            PrintInfoManager.ClearConsole();
            PrintInfoManager.PrintHeader("Add a New Task");
            PrintInfoManager.PrintAddTaskInfo(2);
            Console.WriteLine(details);

            string project = UserInputManager.GetInput("\nEnter Project Name: ", 
                "Cannot have an project name. Try again.", false);
            if (project == null) return;

            PrintInfoManager.ClearConsole();
            PrintInfoManager.PrintHeader("Add a New Task");
            PrintInfoManager.PrintAddTaskInfo(3);
            Console.WriteLine($"{details}\n{project}");

            string dueDateString = UserInputManager.GetInput("\nEnter Due Date (yyyy-mm-dd): ",
                "Invalid date format. Example (2024-12-25)", true);
            if (dueDateString == null) return;

            DateTime dueDate = DateTime.Parse(dueDateString);

            Task task = new(details, project, dueDate, false);
            tasks.Add(task);
            fManager.SaveFile("tasks.csv", tasks);
            PrintInfoManager.PrintWithColor("Task successfully saved... Press any key", ConsoleColor.Green);
            Console.ReadKey();
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
            List<Task> foundMatches = [];
            List<Task> filteredTasks = new List<Task>(tasks);
            List<Task> sortedTasks = new List<Task>(tasks);
            List<Task> projectTasks = new List<Task>();

            int currentPage = 0;
            int selectedIndex = 0;
            int linesForInfo = 19;
            int itemsPerPage = (Console.WindowHeight < 20) ? 1 : Console.WindowHeight - linesForInfo;//Used to divide, make sure its not 0
            
            int totalPages = 0;
            int startIndex = 0;
            int endIndex = 0;
            bool keepGoing = true;

            while (keepGoing)
            {
                if (PrintInfoManager.lastSize != Console.WindowHeight)
                {
                    selectedIndex = 0;
                    currentPage = 0;
                }

                itemsPerPage = (Console.WindowHeight < 20) ? 1 : Console.WindowHeight - linesForInfo;//Used to divide, make sure its not 0

                filteredTasks = FilterOptions(ref showCompletedTasks, selectedProject, ref foundMatches, ref sortedTasks, ref projectTasks);

                CalcPageLayout(ref totalPages, ref startIndex, ref endIndex, itemsPerPage, ref currentPage, ref filteredTasks);
                
                string banner = PrintInfoManager.SetBanner(ref foundMatches, selectedProject, ref currentPage, ref totalPages, ref projectTasks);
                PrintInfoManager.PrintHeader(banner);
                PrintInfoManager.PrintTableHead();
                PrintInfoManager.PrintTableRows(filteredTasks.GetRange(startIndex, endIndex - startIndex), selectedIndex);

                //fill with blank lines, if last page not full
                int remainingLines = itemsPerPage - (endIndex - startIndex);
                for (int i = 0; i < remainingLines; i++) Console.WriteLine(new string(' ', Console.WindowWidth));
                PrintInfoManager.PrintSortingOptions(showCompletedTasks);

                keepGoing = UserAction(ref filteredTasks, ref sortedTasks, ref foundMatches, ref showCompletedTasks, ref selectedProject,
                                       ref currentPage, ref selectedIndex, startIndex, endIndex, totalPages, itemsPerPage);
            }
        }

        private void SelectTaskToEdit(Task task, ref string? selectedProject)
        {
            int fieldIndex = 0;
            string[] fields = ["Task Details", "Project", "Due Date", "Completion Status"];

            while (true)
            {
                PrintInfoManager.PrintHeader($"Updating the Task \"{task.GetShortDetails}\"");
                PrintInfoManager.PrintUpdateTaskFields(fields, task, fieldIndex);
                PrintInfoManager.PrintUpdateTaskInfo();

                ConsoleKey key = UserInputManager.TrapUntilValidInput(1);
                if (key == ConsoleKey.DownArrow && fieldIndex >= fields.Length - 1)
                    fieldIndex = 0;
                else if (key == ConsoleKey.UpArrow && fieldIndex <= 0)
                    fieldIndex = fields.Length - 1;
                else if (key == ConsoleKey.DownArrow && fieldIndex < fields.Length - 1)
                    fieldIndex++;
                else if (key == ConsoleKey.UpArrow && fieldIndex > 0)
                    fieldIndex--;
                else if (key == ConsoleKey.Escape)
                    break;
                else if (key == ConsoleKey.Enter && fieldIndex == 3)
                {
                    task.IsCompleted = !task.IsCompleted;
                    fManager.SaveFile("tasks.csv", tasks);
                }
                else if (key == ConsoleKey.Enter)
                {
                    EditTaskDetails(fieldIndex, task, fields, ref selectedProject);
                }
            }
        }

        private void EditTaskDetails(int fieldIndex, Task task, string[] fields, ref string? selectedProject)
        {
            Console.CursorVisible = true;
            Console.Write($"\nEnter new value for {fields[fieldIndex]}: ");
            string newValue = UserInputManager.ReadEveryKey();
            Console.CursorVisible = false;

            if (string.IsNullOrEmpty(newValue))
                return;
            if (fields[fieldIndex] == "Task Details")
                task.Details = newValue;
            else if (fields[fieldIndex] == "Project")
            {   //if selectedProject != null, small farfetched bug appears
                selectedProject = null;
                task.Project = newValue;
            }
            else if (fields[fieldIndex] == "Due Date")
            {
                bool rightType = DateTime.TryParse(newValue, out DateTime newDate);

                if (rightType && newDate >= DateTime.Now)
                    task.DueDate = newDate;
                else if (rightType && newDate < DateTime.Now)
                    PrintInfoManager.PrintInvalidDateEarly();
                else
                    PrintInfoManager.PrintInvalidDate();
            }
            fManager.SaveFile("tasks.csv", tasks);
        }

        private List<Task> SearchForTask()
        {
            Console.Write("\nPlease enter your search here:");
            string input = UserInputManager.ReadEveryKey();
            if (input == null)
                return [];
            return tasks.Where(x => x.Details.Trim().ToLower() == input.Trim().ToLower()).ToList();
        }

        private string ShowProjectSelect()
        {
            List<string> projects = tasks.Select(x => x.Project).Distinct().OrderBy(p => p).ToList();
            string selectedProject = "";
            int selectedIndex = 0;
            ConsoleKey key;
            do
            {
                PrintInfoManager.PrintHeader("List Of Projects");
                PrintInfoManager.PrintProjectList(ref projects, ref selectedIndex);

                key = UserInputManager.TrapUntilValidInput(1);
                if (key == ConsoleKey.UpArrow && selectedIndex > 0)
                    selectedIndex--;
                else if (key == ConsoleKey.DownArrow && selectedIndex < projects.Count - 1)
                    selectedIndex++;
                else if (key == ConsoleKey.Escape)
                    return null;
                else if (key == ConsoleKey.Enter)
                    selectedProject = projects[selectedIndex];
            } while (key != ConsoleKey.Enter);

            selectedIndex = 0;
            return selectedProject;
        }
        
        private static void CalcPageLayout(ref int totalPages, ref int startIndex, ref int endIndex, int itemsPerPage, 
                                           ref int currentPage, ref List<Task> filteredTasks)
        {
            totalPages = (int)Math.Ceiling((double)filteredTasks.Count / itemsPerPage);
            startIndex = currentPage * itemsPerPage;
            endIndex = Math.Min(startIndex + itemsPerPage, filteredTasks.Count);
        }

        private static List<Task> FilterOptions(ref bool showCompletedTasks, string? selectedProject, 
                                                ref List<Task> foundMatches, ref List<Task> sortedTasks, ref List<Task> projectTasks)
        {
            List<Task> filteredTasks = showCompletedTasks ? sortedTasks : sortedTasks.Where(t => t.IsCompleted == false).ToList();
            
            projectTasks = new List<Task>();

            if (!string.IsNullOrEmpty(selectedProject))
                projectTasks = filteredTasks.Where(t => t.Project == selectedProject).ToList();

            if (projectTasks.Count > 0)//if user deletes last task in project, see all projects
                filteredTasks = projectTasks;

            return foundMatches.Count == 0 ? filteredTasks : filteredTasks.Intersect(foundMatches).ToList();
        }        

        private bool UserAction(ref List<Task> filteredTasks, ref List<Task> sortedTasks, ref List<Task> foundMatches,
                                ref bool showCompletedTasks, ref string? selectedProject, ref int currentPage,
                                ref int selectedIndex, int startIndex, int endIndex, int totalPages, int itemsPerPage)
        {
            while (true)
            {
            GetKeyAgain:;
                ConsoleKey key = UserInputManager.TrapUntilValidInput(0);

                switch (key)
                {
                    case ConsoleKey.Escape:
                        return false;

                    case ConsoleKey.Enter:
                        return HandleEnter(ref filteredTasks, ref selectedIndex, currentPage,
                                    itemsPerPage, ref selectedProject, ref showCompletedTasks);

                    case ConsoleKey.Delete:
                        return HandleDeleteKey(ref filteredTasks, ref sortedTasks, ref foundMatches,
                                               ref showCompletedTasks, ref selectedIndex, ref currentPage, itemsPerPage);

                    case ConsoleKey.D1:
                        sortedTasks = new List<Task>(tasks);
                        currentPage = 0;
                        return true;

                    case ConsoleKey.D2:
                        sortedTasks = tasks.OrderBy(t => t.Project).ToList();
                        currentPage = 0;
                        return true;

                    case ConsoleKey.D3:
                        sortedTasks = tasks.OrderBy(t => t.DueDate).ToList();
                        currentPage = 0;
                        return true;

                    case ConsoleKey.F:
                        HandleFKey(ref filteredTasks, ref showCompletedTasks);
                        selectedIndex = 0;
                        currentPage = 0;
                        return true;

                    case ConsoleKey.P:
                        HandlePKey(ref foundMatches, ref selectedProject, ref showCompletedTasks);
                        selectedIndex = 0;
                        currentPage = 0;
                        return true;

                    case ConsoleKey.A:
                        HandleAKey(ref sortedTasks, ref selectedProject, ref showCompletedTasks);
                        currentPage = 0;
                        foundMatches.Clear();
                        return true;

                    case ConsoleKey.S:
                        HandleSKey(ref foundMatches, ref selectedProject, ref showCompletedTasks);
                        currentPage = 0;
                        selectedIndex = 0;
                        PrintInfoManager.ClearConsole();
                        return true;

                    case ConsoleKey.DownArrow:
                        if (selectedIndex < (endIndex - startIndex - 1))
                            selectedIndex++;
                        else if (selectedIndex >= (endIndex - startIndex - 1))
                            selectedIndex = 0;
                        return true;

                    case ConsoleKey.UpArrow:
                        if (selectedIndex > 0)
                            selectedIndex--;
                        else if (selectedIndex <= 0)
                            selectedIndex = endIndex - startIndex - 1;
                        return true;

                    case ConsoleKey.LeftArrow:
                        if (currentPage > 0)
                        {
                            currentPage--;
                            selectedIndex = 0;
                        }
                        else if (currentPage <= 0)
                        {
                            selectedIndex = 0;
                            currentPage = totalPages - 1;
                        }
                        return true;

                    case ConsoleKey.RightArrow:
                        if (currentPage < totalPages - 1)
                        {
                            currentPage++;
                            selectedIndex = 0;
                        }
                        else if (currentPage >= totalPages - 1)
                        {
                            selectedIndex = 0;
                            currentPage = 0;
                        }
                            
                        return true;
                    default:
                        return true;
                }
            }
        }

        private bool HandleEnter(ref List<Task> filteredTasks, ref int selectedIndex, int currentPage, 
                                 int itemsPerPage, ref string? selectedProject, ref bool showCompletedTasks)
        {
            Task task = filteredTasks[selectedIndex + (itemsPerPage * currentPage)];
            SelectTaskToEdit(task, ref selectedProject);//if updates give emtpy litst, see all, otherwise stay
            List<Task> returnToListAfterUpdate = filteredTasks.Where(t => t != task).ToList();
            if (returnToListAfterUpdate.Count == 0)
                showCompletedTasks = true;
            selectedIndex = 0;
            return true;
        }
        
        private bool HandleDeleteKey(ref List<Task> filteredTasks, ref List<Task> sortedTasks, 
                                     ref List<Task> foundMatches, ref bool showCompletedTasks, 
                                     ref int selectedIndex, ref int currentPage, int itemsPerPage)
        {
            Task taskToDelete = filteredTasks[selectedIndex + (itemsPerPage * currentPage)];
            PrintInfoManager.PrintHeader($"Delete Task: {taskToDelete.GetShortDetails}");
            PrintInfoManager.PrintAreUSure(taskToDelete);
            ConsoleKey confirmDelete = Console.ReadKey(true).Key;
            if (confirmDelete == ConsoleKey.Enter)
            {
                tasks.Remove(taskToDelete);
                fManager.SaveFile("tasks.csv", tasks);

                List<Task> returnToListAfterDelete = filteredTasks.Where(t => t != taskToDelete).ToList();
                if (returnToListAfterDelete.Count == 0)// to not show an empty list
                    showCompletedTasks = true;

                filteredTasks = tasks;
                sortedTasks = tasks;
                foundMatches.Clear();

                PrintInfoManager.PrintDeleteConfirm();
                if (tasks.Count == 0)
                {
                    Console.WriteLine("\nDeleted last task. Go back to main menu by pressing any key...");
                    Console.ReadKey();
                    return false;
                }
            }
            else
                PrintInfoManager.PrintDeleteCancel();
            selectedIndex = 0;
            currentPage = 0;
            return true;
        }

        private bool HandleFKey(ref List<Task> filteredTasks, ref bool showCompletedTasks)
        {
            showCompletedTasks = !showCompletedTasks;
            List<Task> checkEmptyList = filteredTasks.Where(t => t.IsCompleted == false).ToList();
            if (checkEmptyList.Count == 0)
            {
                Console.WriteLine("\nFiltering would only give an empty list. Press any key...");
                Console.ReadKey();
                showCompletedTasks = true;
            }
            return true;
        }

        private bool HandlePKey(ref List<Task> foundMatches, ref string? selectedProject, ref bool showCompletedTasks)
        {
            foundMatches.Clear();
            selectedProject = ShowProjectSelect();
            showCompletedTasks = true;
            return true;
        }

        private void HandleAKey(ref List<Task> sortedTasks, ref string? selectedProject, ref bool showCompletedTasks)
        {
            sortedTasks = new List<Task>(tasks);
            selectedProject = null;
            showCompletedTasks = true;
        }

        private void HandleSKey(ref List<Task> foundMatches, ref string? selectedProject, ref bool showCompletedTasks)
        {
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
                foundMatches = searchResults;
            }
        }

    }
}