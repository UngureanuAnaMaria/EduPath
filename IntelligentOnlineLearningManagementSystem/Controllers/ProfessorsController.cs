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
    public class ProfessorsController : ControllerBase
    {
        private readonly IMediator mediator;

        public ProfessorsController(IMediator mediator)
        {
            this.mediator = mediator;
        }

        [HttpGet]
        public async Task<ActionResult<List<ProfessorDTO>>> GetProfessors([FromQuery] string? name, [FromQuery] bool? status, [FromQuery] int pageNumber = 1, [FromQuery] int pageSize = 10)
        {
            var query = new GetProfessorsQuery
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
        public async Task<ActionResult<Result<Guid>>> CreateProfessor(CreateProfessorCommand command)
        {
            var result = await mediator.Send(command);
            return CreatedAtAction("GetByIdProfessor", new { Id = result.Data }, result.Data);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<ProfessorDTO>> GetByIdProfessor(Guid id)
        {
            return await mediator.Send(new GetProfessorByIdQuery { Id = id });
        }

        [HttpDelete("{id:guid}")]
        public async Task<IActionResult> DeleteProfessor(Guid id)
        {
            await mediator.Send(new DeleteProfessorByIdCommand { Id = id });
            return StatusCode(StatusCodes.Status204NoContent);
        }

        [HttpPut("{id:guid}")]
        public async Task<ActionResult<Result<Guid>>> UpdateProfessor(Guid id, UpdateProfessorCommand command)
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
