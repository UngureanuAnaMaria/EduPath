using Application.AI_ML_Module;
using Application.DTOs;
using Application.Responses;
using Application.Use_Cases.Commands.Create;
using Application.Use_Cases.Commands.Delete;
using Application.Use_Cases.Commands.Update;
using Application.Use_Cases.Queries;
using Domain.Common;
using Domain.Predictions;
using Domain.Repositories;
using Infrastructure.Repositories;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace IntelligentOnlineLearningManagementSystem.Controllers
{
    [ApiController]
    [Route("api/v1/[controller]")]
    public class StudentsController : ControllerBase
    {
        private readonly IMediator mediator;
        private readonly IStudentRepository studentRepository;

        public StudentsController(IMediator mediator, IStudentRepository studentRepository)
        {
            this.mediator = mediator;
            this.studentRepository = studentRepository;
        }

        [HttpGet]
        [Authorize]
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
        [Authorize]
        public async Task<ActionResult<Result<Guid>>> CreateStudent(CreateStudentCommand command)
        {
            var result = await mediator.Send(command);
            return CreatedAtAction("GetById", new { Id = result.Data }, result.Data);
        }

        [HttpGet("{id}")]
        [Authorize]
        public async Task<ActionResult<StudentDTO>> GetById(Guid id)
        {
            var student = await mediator.Send(new GetStudentByIdQuery { Id = id });
            if (student == null)
            {
                return NotFound();
            }
            return Ok(student);
        }

        [HttpDelete("{id}")]
        [Authorize]
        public async Task<IActionResult> Delete(Guid id)
        {
            await mediator.Send(new DeleteStudentByIdCommand { Id = id });
            return NoContent();
        }

        [HttpPut("{id:guid}")]
        [Authorize]
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

        [HttpGet("{id}/predictions")]
        [Authorize]
        public async Task<ActionResult<Domain.Predictions.StudentPredictions>> GetPredictions(Guid id)
        {
            try
            {
                var predictions = await studentRepository.GetPredictionForStudentAsync(id);
                return Ok(predictions);
            }
            catch (InvalidOperationException ex)
            {
                return NotFound(ex.Message);
            }
        }

        [HttpPost("predictions-extern")]
        public async Task<ActionResult<StudentPredictionsExtern>> PostPredictionsExtern(StudentPredictionDatas student)
        {
            try
            {
                var predictions = await studentRepository.PostPredictionForStudentExternAsync(student);
                return Ok(predictions);
            }
            catch (InvalidOperationException ex)
            {
                return NotFound(ex.Message);
            }
        }
    }
}





