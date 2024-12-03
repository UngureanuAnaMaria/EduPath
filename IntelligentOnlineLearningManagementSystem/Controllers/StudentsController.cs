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
    public class StudentsController : ControllerBase
    {
        private readonly IMediator mediator;

        public StudentsController(IMediator mediator)
        {
            this.mediator = mediator;
        }

        [HttpGet]
        public async Task<ActionResult<GetStudentsResponse>> GetStudents([FromQuery] string? name, [FromQuery] bool? status, [FromQuery] int pageNumber = 1, [FromQuery] int pageSize = 2)
        {
            var query = new GetStudentsQuery
            {
                Name = name,
                Status = status,
                PageNumber = pageNumber,
                PageSize = pageSize
            };

            var result = await mediator.Send(query);

            return Ok(result);
        }

        [HttpPost]
        public async Task<ActionResult<Result<Guid>>> CreateStudent(CreateStudentCommand command)
        {
            var result = await mediator.Send(command);
            return CreatedAtAction("GetById", new { Id = result.Data }, result.Data);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<StudentDTO>> GetById(Guid id)
        {
            return await mediator.Send(new GetStudentByIdQuery { Id = id });
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(Guid id)
        {
            await mediator.Send(new DeleteStudentByIdCommand { Id = id });
            return NoContent();
        }

        [HttpPut("{id:guid}")]
        public async Task<ActionResult<Result<Guid>>> Update(Guid id, UpdateStudentCommand command)
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

