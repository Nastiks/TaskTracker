using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace WebTasks.Shared.Models.Public
{
    public class PublicProjectItem
    {
        public int Id { get; set; }

        [Required(AllowEmptyStrings = false)]
        public string? Name { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public ProjectStatus Status { get; set; }
        public int Priority { get; set; }
        public IEnumerable<int>? Tasks { get; set; }
        public string StatusDescription => Status.ToString();

        public bool IsValid()
        {
            if (string.IsNullOrWhiteSpace(Name)) return false;
            return true;
        }

        public static explicit operator PublicProjectItem(ProjectItem item)
        {
            return new()
            {
                Id = item.Id,
                Name = item.Name,
                StartDate = item.StartDate,
                EndDate = item.EndDate,
                Status = item.Status,
                Priority = item.Priority,
                Tasks = item.Tasks == null ? 
                        Enumerable.Empty<int>() : item.Tasks.Select(x => x.Id)
            };
        }

        public static explicit operator ProjectItem(PublicProjectItem item)
        {
            return new()
            {
                Name = item.Name,
                StartDate = item.StartDate,
                EndDate = item.EndDate,
                Status = item.Status,
                Priority = item.Priority,
            };
        }
    }    
}
