using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ToDoLy
{
    internal class Task
    {
        public string Details {  get; set; }
        public string Project { get; set; }
        public DateTime DateTime { get; set; }
        public bool IsCompleted { get; set; }

        public Task(string detalis, string project, DateTime dt, bool complete = false) 
        {
            Details = detalis;
            Project = project;
            DateTime = dt;
            IsCompleted = complete;
        }
    }
}
