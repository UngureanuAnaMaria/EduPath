using Application.DTOs;
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
    public class LessonsController : ControllerBase
    {
        private readonly IMediator mediator;

        public LessonsController(IMediator mediator)
        {
            this.mediator = mediator;
        }

        [HttpGet]
        public async Task<ActionResult<List<LessonDTO>>> GetLessons([FromQuery] string? name, [FromQuery] Guid? courseId, [FromQuery] int pageNumber = 1, [FromQuery] int pageSize = 10)
        {
            var query = new GetLessonsQuery
            {
                Name = name,
                CourseId = courseId,
                PageNumber = pageNumber,
                PageSize = pageSize
            };

            var result = await mediator.Send(query);
            return Ok(result);
        }

        [HttpPost]
        public async Task<ActionResult<Result<Guid>>> CreateLesson(CreateLessonCommand command)
        {
            var result = await mediator.Send(command);
            return CreatedAtAction("GetByIdLesson", new { Id = result.Data }, result.Data);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<LessonDTO>> GetByIdLesson(Guid id)
        {
            return await mediator.Send(new GetLessonByIdQuery { Id = id });
        }

        [HttpDelete("{id:guid}")]
        public async Task<IActionResult> DeleteLesson(Guid id)
        {
            await mediator.Send(new DeleteLessonByIdCommand { Id = id });
            return StatusCode(StatusCodes.Status204NoContent);
        }

        [HttpPut("{id:guid}")]
        public async Task<ActionResult<Result<Guid>>> UpdateLesson(Guid id, UpdateLessonCommand command)
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
