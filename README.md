![Asset 2](https://github.com/user-attachments/assets/9ffd8b80-6997-4b85-9174-461a69eb08ae)# ToDoLy - A to-do list

This is a simple app that allows users to add/update/remove and save tasks to file.

It consists of three parts:

1. Main menu
2. Add tasks
3. Edit tasks


The code has five classes and a main (Program.cs):

1. Task.cs
2. TaskManager.cs
3. FileManager.cs
4. PrintInfoManager.cs
5. UserInputManager.cs

# Instructions
This how u u se it...
Epic screens showing.


# UML - Classes

### 1. TaskManager
  * **Fields**
    * FileManager fManager
    * List<Task> tasks
  * **Methods**
    * AddTask()
    * UpdateTask()
    * SelectTaskToEdit(Task task, ref string? selectedProject)
    * EditTaskDetails(int fieldIndex, Task task, string[] fields, ref string? selectedProject)
    * SearchForTask()
    * ShowProjectSelect()
    * CalcPageLayout(...)
    * FilterOptions(...)
    * UserAction(...)
    * HandleEnter(...)
    * HandleDeleteKey(...)
    * HandleFKey(...)
    * HandlePKey(...)
    * HandleAKey(...)
    * HandleSKey(...)

### 2. FileManager
  * **Methods**
    * SaveFile(string fileName, List<Task> tasks)
    * LoadTasks(string filePath)

### 3. Task
  * **Fields**
    * **string** _Details_
    * **string** _Project_
    * **DateTime** _DueDate_
    * **bool** _IsCompleted_

### 4. UserInputManager
  * **Methods**
    * static string GetInput(string prompt, string errorMessage, bool allowEmpty)
    * static ConsoleKey TrapUntilValidInput(int mode)
    * static string ReadEveryKey()

### 5. PrintInfoManager
  * **Methods**
    * static string SetBanner(...)
    * static void PrintHeader(string message)
    * void PrintWelcome(int complete, int pending)
    * static void PrintOptions()
    * static void PrintAddTaskInfo(int step)
    * static void PrintWithColor(string message, ConsoleColor color)
    * static void PrintTableHead()
    * static void PrintTableRows(List<Task> tasks, int selectedIndex)
    * static void PrintSortingOptions(bool showCompletedTasks)
    * static void PrintUpdateTaskFields(string[] fields, Task task, int fieldIndex)
    * static void PrintUpdateTaskInfo()
    * static void PrintInvalidDate()
    * static void PrintInvalidDateEarly()
    * static void PrintDeleteConfirm()
    * static void PrintDeleteCancel()
    * static void PrintProjectList(ref List<string> projects, ref int selectedIndex)
    * static void PrintAreUSure(Task task)

# UML - Relashionships and associations

1. **TaskManager** _uses_:
    * **FileManager** to save and load data, with an object. 
    * **PrintInfoManager** to print information, data, user input and output, to console. Without objects.
    * **UserInputManager** to handle user input. Without objects.
2. **TaskManager** has a composition rellationship with **Task**.Since it manages a list of **Task** objects
3. **Task** is a simple data model class with fields for the task details to be saved.
4. **PrintInfoManager** and **UserInputManager** are utility classes.
5. **PrintInfoManager** and **FileManager** _use_ objects of **Task** to get data.

![Upload<?xml version="1.0" encoding="UTF-8"?><svg id="Layer_2" xmlns="http://www.w3.org/2000/svg" viewBox="0 0 1194.64 494.69"><defs><style>.cls-1{font-family:CascadiaCode-Regular, 'Cascadia Code';font-variation-settings:'wght' 400;}.cls-1,.cls-2{font-size:14px;}.cls-3{fill-rule:evenodd;}.cls-3,.cls-4,.cls-5,.cls-6,.cls-7,.cls-8,.cls-9{stroke:#000;stroke-miterlimit:10;}.cls-3,.cls-4,.cls-6,.cls-7,.cls-8,.cls-9{fill:none;}.cls-4{stroke-dasharray:0 0 12.01 12.01;}.cls-4,.cls-6,.cls-7,.cls-8,.cls-9{stroke-width:2px;}.cls-2{font-family:CascadiaCodeRoman-Bold, 'Cascadia Code';font-variation-settings:'wght' 700;font-weight:700;}.cls-7{stroke-dasharray:0 0 11.29 11.29;}.cls-8{stroke-dasharray:0 0 11.76 11.76;}.cls-9{stroke-dasharray:0 0 12 12;}</style></defs><g id="Layer_1-2"><g><text class="cls-2" transform="translate(711.31 31.03)"><tspan x="0" y="0">Task Manager</tspan></text><text class="cls-1" transform="translate(665.66 61.03)"><tspan x="0" y="0">- FileManager</tspan><tspan x="0" y="25">- List&lt;Task&gt; tasks</tspan></text><text class="cls-1" transform="translate(665.66 116.03)"><tspan x="0" y="0">+ AddTask()</tspan><tspan x="0" y="25">+ UpdateTask()</tspan><tspan x="0" y="50">- SelectTaskToEdit()</tspan><tspan x="0" y="75">- EditTaskDetails()</tspan><tspan x="0" y="100">...</tspan></text><g><path d="M865.64,13V225.69h-210V13h210m1-1h-212V226.69h212V12h0Z"/><path class="cls-3" d="M654.64,37.1h0Z"/><path class="cls-3" d="M654.64,94.03h0Z"/></g></g><g><text class="cls-2" transform="translate(744.17 347.03)"><tspan x="0" y="0">Task</tspan></text><text class="cls-1" transform="translate(665.66 377.03)"><tspan x="0" y="0">- string Details</tspan><tspan x="0" y="25">- string Project</tspan><tspan x="0" y="50">- DateTime DueDate</tspan><tspan x="0" y="75">- bool IsCompleted</tspan></text><text class="cls-1" transform="translate(665.66 481.03)"><tspan x="0" y="0">+ Task()</tspan></text><path d="M865.64,329v164.69h-210v-164.69h210m1-1h-212v166.69h212v-166.69h0Z"/><path class="cls-3" d="M654.64,353.1h0Z"/><line class="cls-5" x1="654.64" y1="461.79" x2="866.64" y2="461.79"/></g><g><text class="cls-2" transform="translate(1043.07 153.53)"><tspan x="0" y="0">FileManager</tspan></text><text class="cls-1" transform="translate(994.16 183.53)"><tspan x="0" y="0">+ LoadTasks()</tspan><tspan x="0" y="25">+ SaveFile()</tspan></text><path d="M1193.64,135.5v89.69h-209.5v-89.69h209.5m1-1h-211.5v91.69h211.5v-91.69h0Z"/><path class="cls-3" d="M983.14,159.6h0Z"/></g><g><text class="cls-2" transform="translate(340.7 19.03)"><tspan x="0" y="0">UserInputManager</tspan></text><text class="cls-1" transform="translate(311.66 49.03)"><tspan x="0" y="0">+ ReadEveryKey()</tspan><tspan x="0" y="25">+ GetInput()</tspan><tspan x="0" y="50">+ TrapUntilValidInput()</tspan></text><path d="M511.64,1V116.69h-210V1h210m1-1h-212V117.69h212V0h0Z"/><path class="cls-3" d="M300.64,25.1h0Z"/></g><g><text class="cls-2" transform="translate(342.64 243.03)"><tspan x="0" y="0">PrintInfoManager</tspan></text><text class="cls-1" transform="translate(313.66 273.03)"><tspan x="0" y="0">+ PrintHeader()</tspan><tspan x="0" y="25">+ PrintOptions()</tspan><tspan x="0" y="50">...</tspan></text><path d="M513.64,225v115.69h-210v-115.69h210m1-1h-212v117.69h212v-117.69h0Z"/><path class="cls-3" d="M302.64,249.1h0Z"/></g><g><line class="cls-6" x1="646.64" y1="20.69" x2="640.64" y2="20.69"/><line class="cls-7" x1="629.35" y1="20.69" x2="544.64" y2="20.69"/><line class="cls-6" x1="539" y1="20.69" x2="533" y2="20.69"/><polygon points="535.91 10.72 518.64 20.69 535.91 30.66 535.91 10.72"/></g><g><line class="cls-6" x1="641.64" y1="34.69" x2="638.57" y2="39.84"/><line class="cls-4" x1="632.42" y1="50.16" x2="537.14" y2="210.05"/><line class="cls-6" x1="534.06" y1="215.21" x2="530.99" y2="220.36"/><polygon points="523.92 212.75 523.64 232.69 541.05 222.96 523.92 212.75"/></g><text class="cls-1" transform="translate(929.67 77.03)"><tspan x="0" y="0">uses with object</tspan></text><line class="cls-6" x1="866.64" y1="23.69" x2="982.64" y2="151.69"/><line class="cls-6" x1="765.64" y1="258.69" x2="765.64" y2="327.69"/><text class="cls-1" transform="translate(774.37 271.88)"><tspan x="0" y="0">composition</tspan></text><g><line class="cls-9" x1="983.64" y1="232.69" x2="891.69" y2="326.44"/><polygon points="886.62 317.38 881.64 336.69 900.86 331.34 886.62 317.38"/></g><text class="cls-1" transform="translate(940.44 295.67)"><tspan x="0" y="0">uses only temporary objects</tspan></text><g><text class="cls-2" transform="translate(76.91 140.71)"><tspan x="0" y="0">Console</tspan></text><text class="cls-1" transform="translate(11.01 170.71)"><tspan x="0" y="0">+ WriteLine()</tspan><tspan x="0" y="25">+ ReadLine()</tspan><tspan x="0" y="50">...</tspan></text><path d="M211,122.68v115.69H1V122.68H211m1-1H0v117.69H212V121.68h0Z"/><path class="cls-3" d="M0,146.78H0Z"/></g><g><line class="cls-6" x1="300.64" y1="15.69" x2="297.06" y2="20.5"/><line class="cls-8" x1="290.03" y1="29.93" x2="237.32" y2="100.65"/><line class="cls-6" x1="233.8" y1="105.37" x2="230.22" y2="110.18"/><polygon points="223.97 101.88 221.64 121.69 239.96 113.8 223.97 101.88"/></g><g><line class="cls-9" x1="518.64" y1="246.69" x2="638.99" y2="333.3"/><polygon points="630.8 339.69 650.64 341.69 642.45 323.51 630.8 339.69"/></g><g><line class="cls-9" x1="302.64" y1="239.69" x2="229.91" y2="153.65"/><polygon points="239.41 149.44 220.64 142.69 224.18 162.32 239.41 149.44"/></g><rect class="cls-6" x="755.14" y="232.69" width="21" height="21" transform="translate(396.21 -470.16) rotate(45)"/></g></svg>ing Asset 2.svg…]()




# Known buggs
If the console window is smaller than the content, and the window becomes scrollable, every clear() won't clear everything.
You will still be able to see everything and use the app. But now you will now be able to scroll up and see the miss-prints (not pretty).
The app was not intended to be scrolled in. I want the window size to be static and have the content always fit.

However, if the user doesn't change the window size, we cool.
