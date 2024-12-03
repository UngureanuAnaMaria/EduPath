using Application.Use_Cases.Commands.Create;
using Domain.Common;
using MediatR;

namespace Application.Use_Cases.Commands.Update
{
    public class UpdateLessonCommand : CreateLessonCommand, IRequest<Result<Guid>>
    {
        public Guid Id { get; set; }
    }
}