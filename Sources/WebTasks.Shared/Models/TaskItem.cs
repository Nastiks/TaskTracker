using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;
using System.Text.Json;

namespace WebTasks.Shared.Models
{
    public class TaskItem
    {
        public int Id { get; set; }
        public string? Name { get; set; }
        public string? Description { get; set; }
        public virtual ProjectItem? ProjectItem { get; set; }
        public int? ProjectItemId { get; set; }
        public TaskStatus Status { get; set; }
        public int Priority { get; set; }
        public string? CustomFields_Data { get; set; }
        [NotMapped]
        public Dictionary<string, object>? CustomFields
        {
            get
            {
                if (string.IsNullOrWhiteSpace(CustomFields_Data))
                {
                    return null;
                }
                try
                {
                    return JsonSerializer.Deserialize<Dictionary<string, object>>(CustomFields_Data);
                }
                catch (Exception)
                {
                    return null;
                }
            }
            set
            {
                if (value == null)
                {
                    CustomFields_Data = null;
                }
                else
                {
                    CustomFields_Data = JsonSerializer.Serialize(value);
                }
            }
        }

        public void Update(TaskItem item)
        {
            Name = item.Name;
            Description = item.Description;
            Status = item.Status;
            Priority = item.Priority;
            CustomFields_Data = item.CustomFields_Data;
        }

        public void SetProject(ProjectItem? project)
        {
            if (project == null)
            {
                ProjectItemId = null;
                ProjectItem = null;
            }
            else
            {
                ProjectItemId = project.Id;
                ProjectItem = project;
            }
        }
    }
}
