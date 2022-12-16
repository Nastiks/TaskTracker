using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;
using WebTasks.Shared.Models.Public;

namespace WebTasks.Shared.Models
{
    public class PublicTaskItem
    {
        public int Id { get; set; }

        [Required(AllowEmptyStrings = false)]
        public string? Name { get; set; }
        public string? Description { get; set; }
        public int? ProjectItemId { get; set; }
        public TaskStatus Status { get; set; }
        public int Priority { get; set; }
        public Dictionary<string, object>? CustomFields { get; set; }
        public string StatusDescription => Status.ToString();
        public bool IsValid()
        {
            if (string.IsNullOrWhiteSpace(Name)) return false;
            return true;
        }

        public static explicit operator PublicTaskItem(TaskItem item)
        {
            return new()
            {
                Id = item.Id,
                Name = item.Name,
                Description = item.Description,
                Status = item.Status,
                Priority = item.Priority,
                CustomFields = item.CustomFields,
                ProjectItemId = item.ProjectItemId
            };
        }

        public static explicit operator TaskItem(PublicTaskItem item)
        {
            return new()
            {
                Name = item.Name,
                Description = item.Description,
                Status = item.Status,
                Priority = item.Priority,
                CustomFields = item.CustomFields,
                ProjectItemId = item.ProjectItemId
            };
        }
    }
}
