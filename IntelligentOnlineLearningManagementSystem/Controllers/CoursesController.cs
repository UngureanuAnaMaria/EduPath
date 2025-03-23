using Application.DTOs;
using Application.Responses;
using Application.Use_Cases.Commands.Create;
using Application.Use_Cases.Commands.Delete;
using Application.Use_Cases.Commands.Update;
using Application.Use_Cases.Queries;
using Domain.Common;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace IntelligentOnlineLearningManagementSystem.Controllers
{
    [Route("api/v1/[controller]")]
    [ApiController]
    public class CoursesController : ControllerBase
    {
        private readonly IMediator mediator;

        public CoursesController(IMediator mediator)
        {
            this.mediator = mediator;
        }

        [HttpGet]
        public async Task<ActionResult<GetCoursesResponse>> GetCourses([FromQuery] string? name, [FromQuery] Guid? professorId, [FromQuery] int pageNumber = 1, [FromQuery] int pageSize = 10)
        {
            var query = new GetCoursesQuery
            {
                Name = name,
                ProfessorId = professorId,
                PageNumber = pageNumber,
                PageSize = pageSize
            };

            var result = await mediator.Send(query);
            return Ok(result);
        }

        [HttpPost]
        public async Task<ActionResult<Result<Guid>>> CreateCourse(CreateCourseCommand command)
        {
            var result = await mediator.Send(command);
            return CreatedAtAction("GetByIdCourse", new { Id = result.Data }, result.Data);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<CourseDTO>> GetByIdCourse(Guid id)
        {
            return await mediator.Send(new GetCourseByIdQuery { Id = id });
        }

        [HttpDelete("{id:guid}")]
        public async Task<IActionResult> DeleteCourse(Guid id)
        {
            await mediator.Send(new DeleteCourseByIdCommand { Id = id });
            return NoContent();
        }

        [HttpPut("{id:guid}")]
        public async Task<ActionResult<Result<Guid>>> UpdateCourse(Guid id, UpdateCourseCommand command)
        {
            if (id != command.Id)
            {
                return BadRequest("The id should be identical with command.Id");
            }

            var result = await mediator.Send(command);

            if (result.IsSuccess)
            {
                return NoContent();
            }

            return BadRequest(result);
        }
    }
}
