using Application.DTOs;
using Application.Use_Cases.Queries;
using AutoMapper;
using Domain.Entities;
using Domain.Repositories;
using MediatR;

namespace Application.Use_Cases.QueryHandlers
{
    public class GetCourseByIdQueryHandler : IRequestHandler<GetCourseByIdQuery, CourseDTO>
    {
        private readonly IStudentCourseRepository studentCourseRepository;
        private readonly ICourseRepository courseRepository;
        private readonly IProfessorRepository professorRepository;
        private readonly IStudentRepository studentRepository;
        private readonly ILessonRepository lessonRepository;
        private readonly IMapper mapper;

        public GetCourseByIdQueryHandler(ICourseRepository courseRepository, IProfessorRepository professorRepository, IStudentRepository studentRepository, ILessonRepository lessonRepository,IStudentCourseRepository studentCourseRepository, IMapper mapper)
        {
            this.courseRepository = courseRepository;
            this.professorRepository = professorRepository;
            this.studentRepository = studentRepository;
            this.studentCourseRepository = studentCourseRepository;
            this.lessonRepository = lessonRepository;
            this.mapper = mapper;
        }

        public async Task<CourseDTO> Handle(GetCourseByIdQuery request, CancellationToken cancellationToken)
        {
            var course = await courseRepository.GetCourseByIdAsync(request.Id);

            if (course == null)
            {
                Console.WriteLine($"Course with ID {request.Id} not found.");
                return new CourseDTO();

            }

            var courseDTO = mapper.Map<CourseDTO>(course);

            if (course.ProfessorId.HasValue && course.ProfessorId != Guid.Empty)
            {
                var professor = await professorRepository.GetProfessorByIdAsync(course.ProfessorId.Value);
                if (professor != null)
                {
                    courseDTO.Professor = mapper.Map<ProfessorDTO>(professor);
                }
            }

            var studentCourses = await studentCourseRepository.GetAllStudentCoursesAsync();

            if (studentCourses != null)
            {
                foreach (var studentCourse in studentCourses)
                {
                    if (studentCourse.CourseId == request.Id)
                    {
                        var student = await studentRepository.GetStudentByIdAsync(studentCourse.StudentId);

                        var studentDTO = new StudentDTO
                        {
                            Id = student.Id,
                            Name = student.Name,
                            Email = student.Email,
                            Password = student.Password,
                            Status = student.Status,
                            CreatedAt = student.CreatedAt,
                            LastLogin = student.LastLogin
                        };
                    }
                }
            }

            var lessons = await lessonRepository.GetAllLessonsAsync();
            if (lessons != null)
            {
                courseDTO.Lessons = new List<LessonDTO>();

                foreach (var lesson in lessons)
                {
                    if (lesson.CourseId == request.Id)
                    {
                        var lessonDTO = mapper.Map<LessonDTO>(lesson);
                        courseDTO.Lessons.Add(lessonDTO);
                    }
                }
            }

            courseDTO = mapper.Map<CourseDTO>(course);

            return courseDTO;
        }
    }
}
