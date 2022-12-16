using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;
using WebTasks.Server.Data;
using WebTasks.Shared.Models.Public;
using WebTasks.Shared.Models;
using WebTasks.Shared;

namespace WebTasks.Server.Controllers
{
    public class TaskController : ControllerBase
    {
        private readonly ApplicationDbContext _dbContext;

        public TaskController(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        /// <summary>
        /// Returns all tasks items.
        /// </summary>
        /// <param name="projectId">Specified project Id (optional).</param>
        /// <response code="200"> Successfully received data. </response>
        /// <response code="404"> Project was not found. </response>
        [Produces("application/json", Type = typeof(IEnumerable<PublicTaskItem>))]
        [HttpGet(Routes.V1.Tasks)]
        public IActionResult GetAll([FromQuery] int projectId)
        {
            IQueryable<TaskItem> tasks = _dbContext.TaskItems;
            if (projectId > 0)
            {
                var foundProject = _dbContext.ProjectItems.FirstOrDefault(x => x.Id == projectId);
                if (foundProject == null)
                {
                    return NotFound();
                }
                tasks = tasks.Where(x => x.ProjectItemId == foundProject.Id);
            }
            var result = _dbContext.TaskItems.Select(x => (PublicTaskItem)x);
            return Ok(result);
        }

        /// <summary>
        /// Returns task item by specified Id.
        /// </summary>
        /// <param name="taskId">Specified task Id.</param>
        /// <response code="200"> The task was successfully received. </response>
        /// <response code="404"> Task was not found. </response>
        [Produces("application/json", Type = typeof(PublicTaskItem))]
        [HttpGet(Routes.V1.Tasks + "/{taskId}")]
        public IActionResult GetById([FromRoute][Required] int taskId)
        {
            var result = _dbContext.TaskItems.FirstOrDefault(p => p.Id == taskId);
            if (result == null)
            {
                return NotFound();
            }
            return Ok((PublicTaskItem)result);
        }

        /// <summary>
        /// Create a new task.
        /// </summary>
        /// <param name="item">New item model request.</param>
        /// <response code="201"> The task has been created successfully. </response>
        /// <response code="500"> Received entity contains bad values. </response>
        [Produces("application/json", Type = typeof(PublicTaskItem))]
        [HttpPost(Routes.V1.Tasks)]
        public IActionResult Create([FromBody][Required] PublicTaskItem item)
        {
            if (!item.IsValid())
            {
                return BadRequest();
            }
            var newItem = (TaskItem)item;
            if (item.ProjectItemId != default)
            {
                var foundProject = _dbContext.ProjectItems.FirstOrDefault(x => x.Id == item.ProjectItemId);
                if (foundProject != null)
                {
                    newItem.SetProject(foundProject);
                }
            }
            _dbContext.TaskItems.Add(newItem);
            _dbContext.SaveChanges();
            return Created(Routes.V1.Tasks + '/' + newItem.Id, (PublicTaskItem)newItem);
        }

        /// <summary>
        /// Delete task item by specified Id.
        /// </summary>
        /// <param name="taskId">Specified task Id.</param>
        /// <response code="200"> The task was successfully deleted. </response>
        /// <response code="404"> Task was not found. </response>
        [Produces("application/json", Type = typeof(PublicTaskItem))]
        [HttpDelete(Routes.V1.Tasks + "/{taskId}")]
        public IActionResult DeleteById([FromRoute][Required] int taskId)
        {
            var result = _dbContext.TaskItems.FirstOrDefault(p => p.Id == taskId);
            if (result == null)
            {
                return NotFound();
            }
            _dbContext.TaskItems.Remove(result);
            _dbContext.SaveChanges();
            return Ok((PublicTaskItem)result);
        }

        /// <summary>
        /// Update task item by specified Id.
        /// </summary>
        /// <param name="taskId">Specified task Id.</param>
        /// <param name="item">New item model request.</param>
        /// <response code="200"> The task was successfully updated. </response>
        /// <response code="404"> Task was not found. </response>
        /// <response code="500"> Received entity contains bad values. </response>
        [Produces("application/json", Type = typeof(PublicTaskItem))]
        [HttpPut(Routes.V1.Tasks + "/{taskId}")]
        public IActionResult UpdateById([FromRoute][Required] int taskId, [FromBody][Required] PublicTaskItem item)
        {
            if (!item.IsValid())
            {
                return BadRequest();
            }
            var result = _dbContext.TaskItems.FirstOrDefault(p => p.Id == taskId);
            if (result == null)
            {
                return NotFound();
            }
            result.Update((TaskItem)item);
            _dbContext.SaveChanges();
            return Ok((PublicTaskItem)result);
        }

        /// <summary>
        /// Update a project for a specific task.
        /// </summary>
        /// <param name="taskId">Specified task Id.</param>
        /// <param name="projectId">Specified project Id.</param>
        /// <response code="200"> The task project has been successfully updated. </response>
        /// <response code="404"> Task was not found. </response>
        [Produces("application/json", Type = typeof(PublicTaskItem))]
        [HttpPatch(Routes.V1.Tasks + "/{taskId}")]
        public IActionResult UpdateTaskProject([FromRoute][Required] int taskId, [FromQuery] int projectId)
        {
            var foundTask = _dbContext.TaskItems.FirstOrDefault(p => p.Id == taskId);
            if (foundTask == null)
            {
                return NotFound(nameof(taskId));
            }
            var foundProject = projectId > 0 ? _dbContext.ProjectItems.FirstOrDefault(p => p.Id == projectId) : null;
            foundTask.SetProject(foundProject);
            _dbContext.SaveChanges();
            return Ok((PublicTaskItem)foundTask);
        }
    }
}