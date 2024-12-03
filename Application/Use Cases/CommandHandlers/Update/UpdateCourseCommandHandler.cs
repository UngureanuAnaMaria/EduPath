using Application.Use_Cases.Commands.Update;
using AutoMapper;
using Domain.Common;
using Domain.Entities;
using Domain.Repositories;
using MediatR;

namespace Application.Use_Cases.CommandHandlers.Update
{
    public class UpdateCourseCommandHandler : IRequestHandler<UpdateCourseCommand, Result<Guid>>
    {
        private readonly ICourseRepository courseRepository;
        private readonly IMapper mapper;

        public UpdateCourseCommandHandler(ICourseRepository courseRepository, IMapper mapper)
        {
            this.courseRepository = courseRepository;
            this.mapper = mapper;
        }

        public async Task<Result<Guid>> Handle(UpdateCourseCommand request, CancellationToken cancellationToken)
        {
            var course = mapper.Map<Course>(request);

            if (course == null)
            {
                return Result<Guid>.Failure("Course not found.");
            }

            course.Name = request.Name;
            course.Description = request.Description;
            course.ProfessorId = request.ProfessorId;


            if (request.StudentCourses != null)
            {
                foreach (var studentCourseCommand in request.StudentCourses)
                {
                    var studentCourse = new StudentCourse
                    {
                        Id = Guid.NewGuid(),
                        StudentId = studentCourseCommand.StudentId,
                        CourseId = course.Id
                    };
                    course.StudentCourses.Add(studentCourse);
                }
            }

            var result = await courseRepository.UpdateCourseAsync(course);

            if (result.IsSuccess)
            {
                return Result<Guid>.Success(course.Id);
            }

            return Result<Guid>.Failure(result.ErrorMessage);
        }
    }
}