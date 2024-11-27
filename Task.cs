using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ToDoLy
{
    internal class Task
    {
        string Details {  get; set; }
        string Project { get; set; }
        DateTime DateTime { get; set; }
        bool IsCompleted { get; set; }

        public Task(string detalis, string project, DateTime dt, bool complete = false) 
        {
            Details = detalis;
            Project = project;
            DateTime = dt;
            IsCompleted = complete;
        }
    }
}
