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

        public string GetLineBreakDetails { get => PrintInfoManager.FormatLongString(_details); }

        public string GetLineBreakProject { get => PrintInfoManager.FormatLongString(_project); }

        public DateTime DueDate { get; set; }
        public bool IsCompleted { get; set; }

        public Task(string detalis, string project, DateTime dt, bool complete = false) 
        {
            _details = detalis;
            _project = project;
            DueDate = dt;
            IsCompleted = complete;
        }

    }
}
