using Application.Use_Cases.Commands.Create;
using Domain.Common;
using MediatR;

namespace Application.Use_Cases.Commands.Update
{
    public class UpdateStudentCommand : CreateStudentCommand, IRequest<Result<Guid>>
    {
        public Guid Id { get; set; }
    }
}