# ToDoLy - A to-do list

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

## Task.cs
Contains the basic info of a task. It has a constructor so the object can be created faster.

**string** Details 

**string** Project name 

**DateTime** Due Date

**bool** Completion status.

## TaskManager.cs
This class contains the methods for creating/updating/displaying/removing tasks.
It is responsible for the bulk of the functionality.

## FileManager.cs
This class contains the methods for loading and creating files. Short and simple.

## PrintInfoManager.cs
This class contains the methods for printing information and instructions to the user. It has more code than it needs, since I wanted to make it colorfull and user friendly.

## UserInputManager.cs
This class contains the methods for taking input from user. This class only handles keypresses when user is inputing a string, not when "action keys" are used.

The method "ReadEveryKey" builds a string while the user types, making it possible to escape from an active input prompt. 
Instead of reading the string only after completed input. 


# Known buggs
If the console window is smaller than the content, and the window becomes scrollable, every clear() won't clear everything.
You will still be able to see everything and use the app. But now you will now be able to scroll up and see the miss-prints (not pretty).
The app was not intended to be scrolled in. I want the window size to be static and have the content always fit.

However, if the user doesn't change the window size, we cool.
