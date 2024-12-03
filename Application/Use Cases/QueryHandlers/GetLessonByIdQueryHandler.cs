using Application.DTOs;
using Application.Use_Cases.Queries;
using AutoMapper;
using Domain.Entities;
using Domain.Repositories;
using MediatR;

namespace Application.Use_Cases.QueryHandlers
{
    public class GetLessonByIdQueryHandler : IRequestHandler<GetLessonByIdQuery, LessonDTO>
    {
        private readonly ILessonRepository repository;
        private readonly IStudentCourseRepository studentCourseRepository;
        private readonly ICourseRepository courseRepository;
        private readonly IProfessorRepository professorRepository;
        private readonly IStudentRepository studentRepository;
        private readonly IMapper mapper;

        public GetLessonByIdQueryHandler(ILessonRepository repository, IMapper mapper, ICourseRepository courseRepository, IProfessorRepository professorRepository, IStudentCourseRepository studentCourseRepository, IStudentRepository studentRepository)
        {
            this.repository = repository;
            this.courseRepository = courseRepository;
            this.mapper = mapper;
            this.professorRepository = professorRepository;
            this.studentCourseRepository = studentCourseRepository;
            this.studentRepository = studentRepository;
        }

        public async Task<LessonDTO> Handle(GetLessonByIdQuery request, CancellationToken cancellationToken)
        {
            var lesson = await repository.GetLessonByIdAsync(request.Id);

            if (lesson == null)
            {
                Console.WriteLine($"Lesson with ID {request.Id} not found.");
                return new LessonDTO();
            }

            var lessonDTO = mapper.Map<LessonDTO>(lesson);

            var course = await courseRepository.GetCourseByIdAsync(lesson.CourseId);
            if (course != null)
            {
                lessonDTO.Course = mapper.Map<CourseDTO>(course);

                if (course.ProfessorId.HasValue && course.ProfessorId != Guid.Empty)
                {
                    var professor = await professorRepository.GetProfessorByIdAsync(course.ProfessorId.Value);
                    if (professor != null)
                    {
                        lessonDTO.Course.Professor = mapper.Map<ProfessorDTO>(professor);
                    }
                }

                var studentCourses = await studentCourseRepository.GetAllStudentCoursesAsync();
                if (studentCourses != null)
                {
                    lessonDTO.Course.StudentCourses = new List<StudentCourseDTO>();

                    foreach (var studentCourse in studentCourses)
                    {
                        if (studentCourse.CourseId == course.Id)
                        {
                            var student = await studentRepository.GetStudentByIdAsync(studentCourse.StudentId);
                            if (student != null)
                            {
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

                                var studentCourseDTO = new StudentCourseDTO
                                {
                                    Id = studentCourse.Id,
                                    StudentId = studentCourse.StudentId,
                                    Student = studentDTO,
                                    CourseId = studentCourse.CourseId,
                                    Course = lessonDTO.Course
                                };

                                lessonDTO.Course.StudentCourses.Add(studentCourseDTO);
                            }
                        }
                    }
                }
            }

            return lessonDTO;
        }
    }
}
