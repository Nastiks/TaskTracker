using System;
using System.Collections.Generic;
using System.Text;

namespace WebTasks.Shared.Models
{
    public class ProjectItem
    {
        public int Id { get; set; }
        public string? Name { get; set; }
        public virtual IEnumerable<TaskItem>? Tasks { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public ProjectStatus Status { get; set; }
        public int Priority { get; set; }
        public void Update(ProjectItem item)
        {
            Name= item.Name;
            StartDate= item.StartDate;
            EndDate= item.EndDate;
            Status = item.Status;
            Priority = item.Priority;
        }
    }    
}
