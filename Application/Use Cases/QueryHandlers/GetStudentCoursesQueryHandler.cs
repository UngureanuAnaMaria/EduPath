using Application.DTOs;
using Application.Use_Cases.Queries;
using AutoMapper;
using Domain.Repositories;
using MediatR;

namespace Application.Use_Cases.QueryHandlers
{
    public class GetStudentCoursesQueryHandler : IRequestHandler<GetStudentCoursesQuery, List<StudentCourseDTO>>
    {
        private readonly IStudentCourseRepository repository;
        private readonly IMapper mapper;

        public GetStudentCoursesQueryHandler(IStudentCourseRepository repository, IMapper mapper)
        {
            this.repository = repository;
            this.mapper = mapper;
        }

        public async Task<List<StudentCourseDTO>> Handle(GetStudentCoursesQuery request, CancellationToken cancellationToken)
        {
            var (studentCourses, totalCount) = await repository.GetFilteredStudentCoursesAsync(request.StudentId, request.CourseId, request.PageNumber, request.PageSize);

            if (studentCourses == null || !studentCourses.Any())
            {
                Console.WriteLine($"Student courses not found.");
                return new List<StudentCourseDTO>();
            }

            return mapper.Map<List<StudentCourseDTO>>(studentCourses);
        }
    }
}
