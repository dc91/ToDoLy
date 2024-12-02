using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ToDoLy
{
    internal class Task
    {
        private string _details;
        private string _project;

        public string Details 
        {  
            get => _details; 
            set => _details = value; 
        }
       
        public string Project 
        {
            get => _project;
            set => _project = value; 
        }

        public string GetShortDetails { get => _details.Length > 20 ? _details [..20] + "..." : _details; }

        public string GetShortProject { get => _project.Length > 20 ? _project[..20] + "..." : _project; }

        public string GetLineBreakDetails { get => FormatLongString(_details); }

        public string GetLineBreakProject { get => FormatLongString(_project); }

        public DateTime DueDate { get; set; }
        public bool IsCompleted { get; set; }

        public Task(string detalis, string project, DateTime dt, bool complete = false) 
        {
            _details = detalis;
            _project = project;
            DueDate = dt;
            IsCompleted = complete;
        }

        private string FormatLongString(string input)
        {
            const int lineLength = 70;
            const int paddingRight = 0;
            string formatted = string.Empty;

            for (int i = 0; i < input.Length; i += lineLength)
            {
                int remaning = Math.Min(lineLength, input.Length - i);
                string segment = input.Substring(i, remaning);
                formatted += segment.PadRight(lineLength + paddingRight) + Environment.NewLine + "\t\t";
            }
            return formatted;
        }
    }
}
