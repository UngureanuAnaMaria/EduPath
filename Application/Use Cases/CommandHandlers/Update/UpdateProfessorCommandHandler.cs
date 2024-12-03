using Application.Use_Cases.Commands.Create;
using Application.Use_Cases.Commands.Update;
using AutoMapper;
using Domain.Common;
using Domain.Entities;
using Domain.Repositories;
using MediatR;

namespace Application.Use_Cases.CommandHandlers.Update
{
    public class UpdateProfessorCommandHandler : IRequestHandler<UpdateProfessorCommand, Result<Guid>>
    {
        private readonly IProfessorRepository professorRepository;
        private readonly ICourseRepository courseRepository;
        private readonly IMapper mapper;

        public UpdateProfessorCommandHandler(IProfessorRepository professorRepository, ICourseRepository courseRepository, IMapper mapper)
        {
            this.professorRepository = professorRepository;
            this.courseRepository = courseRepository;
            this.mapper = mapper;
        }

        public async Task<Result<Guid>> Handle(UpdateProfessorCommand request, CancellationToken cancellationToken)
        {
            var professor = mapper.Map<Professor>(request);

            /*if (request.CourseIds != null)
            {
                professor.Courses = new List<Course>();
                foreach (var courseId in request.CourseIds)
                {
                    var course = await courseRepository.GetCourseByIdAsync(courseId);
                    if (course != null)
                    {
                        professor.Courses.Add(course);
                    }
                }
            }*/

            var result = await professorRepository.AddProfessorAsync(professor);
            if (result.IsSuccess)
            {
                return Result<Guid>.Success(professor.Id);
            }
            return Result<Guid>.Failure(result.ErrorMessage);
        }
    }
}
