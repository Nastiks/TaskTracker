using Gridify;
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;
using WebTasks.Server.Data;
using WebTasks.Shared;
using WebTasks.Shared.Models;
using WebTasks.Shared.Models.Public;

namespace WebTasks.Server.Controllers
{
    public class ProjectController : ControllerBase
    {
        private readonly ApplicationDbContext _dbContext;

        public ProjectController(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        /// <summary>
        /// Returns all project items. 
        /// See https://alirezanet.github.io/Gridify/guide/ for filtering details.
        /// </summary>
        /// <param name="query">Filtering, sorting and paging options.</param>
        /// <response code="200"> Successfully received data. </response>
        [Produces("application/json", Type = typeof(IEnumerable<PublicProjectItem>))]
        [HttpGet(Routes.V1.Projects)]
        public IActionResult GetAll([FromQuery] GridifyQuery query)
        {
            var all = _dbContext.ProjectItems.Select(x => (PublicProjectItem)x);
            var result = all.Gridify(query);
            Response.Headers.Add("X-Total-Count", result.Count.ToString());
            return Ok(result.Data);            
        }

        /// <summary>
        /// Returns project item by specified Id.
        /// </summary>
        /// <param name="projectId">Specified project Id.</param>
        /// <response code="200"> The project was successfully received. </response>
        /// <response code="404"> Project was not found. </response>
        [Produces("application/json", Type = typeof(PublicProjectItem))]
        [HttpGet(Routes.V1.Projects + "/{projectId}")]
        public IActionResult GetById([FromRoute][Required] int projectId)
        {
            var result = _dbContext.ProjectItems.FirstOrDefault(p => p.Id == projectId);
            if (result == null)
            {
                return NotFound();
            }
            return Ok(result);
        }

        /// <summary>
        /// Create a new project.
        /// </summary>
        /// <param name="item">New item model request.</param>
        /// <response code="201"> The project has been created successfully. </response>
        /// <response code="500"> Received entity contains bad values. </response>
        [Produces("application/json", Type = typeof(PublicProjectItem))]
        [HttpPost(Routes.V1.Projects)]
        public IActionResult Create([FromBody][Required] PublicProjectItem item)
        {
            if (!item.IsValid())
            {
                return BadRequest();
            }
            var newItem = (ProjectItem)item;
            _dbContext.ProjectItems.Add(newItem);
            _dbContext.SaveChanges();
            return Created(Routes.V1.Projects + '/' + newItem.Id, (PublicProjectItem)newItem);
        }

        /// <summary>
        /// Delete project item by specified Id.
        /// </summary>
        /// <param name="projectId">Specified project Id.</param>
        /// <param name="deleteTasks">If "true" all project tasks will be deleted</param>
        /// <response code="200"> The project was successfully deleted. </response>
        /// <response code="404"> Project was not found. </response>
        [Produces("application/json", Type = typeof(PublicProjectItem))]
        [HttpDelete(Routes.V1.Projects + "/{projectId}")]
        public IActionResult DeleteById([FromRoute][Required] int projectId, [FromQuery] bool deleteTasks = false)
        {
            var result = _dbContext.ProjectItems.FirstOrDefault(p => p.Id == projectId);
            if (result == null)
            {
                return NotFound();
            }
            if (deleteTasks)
            {
                if (result.Tasks != null && result.Tasks.Any())
                {
                    _dbContext.TaskItems.RemoveRange(result.Tasks);
                }
            }
            _dbContext.ProjectItems.Remove(result);
            _dbContext.SaveChanges();
            return Ok((PublicProjectItem)result);
        }

        /// <summary>
        /// Update project item by specified Id.
        /// </summary>
        /// <param name="projectId">Specified project Id.</param>
        /// <param name="item">New item model request.</param>
        /// <response code="200"> The project was successfully updated. </response>
        /// <response code="404"> Project was not found. </response>
        /// <response code="500"> Received entity contains bad values. </response>
        [Produces("application/json", Type = typeof(PublicProjectItem))]
        [HttpPut(Routes.V1.Projects + "/{projectId}")]
        public IActionResult UpdateById([FromRoute][Required] int projectId, [FromBody][Required] PublicProjectItem item)
        {
            if (!item.IsValid())
            {
                return BadRequest();
            }
            var result = _dbContext.ProjectItems.FirstOrDefault(p => p.Id == projectId);
            if (result == null)
            {
                return NotFound();
            }
            result.Update((ProjectItem)item);
            _dbContext.SaveChanges();
            return Ok((PublicProjectItem)result);
        }
    }
}