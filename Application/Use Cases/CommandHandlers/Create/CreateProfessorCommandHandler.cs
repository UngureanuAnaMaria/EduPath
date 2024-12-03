using Domain.Common;
using Application.Use_Cases.Commands.Create;
using AutoMapper;
using Domain.Entities;
using Domain.Repositories;
using MediatR;

namespace Application.Use_Cases.CommandHandlers.Create
{
    public class CreateProfessorCommandHandler : IRequestHandler<CreateProfessorCommand, Result<Guid>>
    {
        private readonly IProfessorRepository professorRepository;
        private readonly IMapper mapper;

        public CreateProfessorCommandHandler(IProfessorRepository professorRepository, IMapper mapper)
        {
            this.professorRepository = professorRepository;
            this.mapper = mapper;
        }
        public async Task<Result<Guid>> Handle(CreateProfessorCommand request, CancellationToken cancellationToken)
        {
            var professor = mapper.Map<Professor>(request);

            var result = await professorRepository.AddProfessorAsync(professor);

            if (result.IsSuccess)
            {
                return Result<Guid>.Success(result.Data);
            }
            return Result<Guid>.Failure(result.ErrorMessage);
        }
    }
}