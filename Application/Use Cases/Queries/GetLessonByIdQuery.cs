using Application.DTOs;
using MediatR;

namespace Application.Use_Cases.Queries
{
    public class GetLessonByIdQuery : IRequest<LessonDTO>
    {
        public Guid Id { get; set; }
    }
}
