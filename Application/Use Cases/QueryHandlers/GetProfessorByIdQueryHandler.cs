using Application.DTOs;
using Application.Use_Cases.Queries;
using AutoMapper;
using Domain.Common;
using Domain.Entities;
using Domain.Repositories;
using MediatR;

namespace Application.Use_Cases.QueryHandlers
{
    public class GetProfessorByIdQueryHandler : IRequestHandler<GetProfessorByIdQuery, ProfessorDTO>
    {
        private readonly IProfessorRepository repository;
        private readonly ICourseRepository courseRepository;
        private readonly IStudentCourseRepository studentCourseRepository;
        private readonly IStudentRepository studentRepository;
        private readonly ILessonRepository lessonRepository;
        private readonly IMapper mapper;

        public GetProfessorByIdQueryHandler(IProfessorRepository repository, IMapper mapper, ILessonRepository lessonRepository,ICourseRepository courseRepository, IStudentCourseRepository studentCourseRepository, IStudentRepository studentRepository)
        {
            this.repository = repository;
            this.mapper = mapper;
            this.courseRepository = courseRepository;
            this.studentCourseRepository = studentCourseRepository;
            this.studentRepository = studentRepository;
            this.lessonRepository = lessonRepository;
        }
        public async Task<ProfessorDTO> Handle(GetProfessorByIdQuery request, CancellationToken cancellationToken)
        {
            var professor = await repository.GetProfessorByIdAsync(request.Id);

            if (professor == null)
            {
                Console.WriteLine($"Professor with ID {request.Id} not found.");
                return new ProfessorDTO();
            }

            var courses = await courseRepository.GetAllCoursesAsync();
            var professorDTO = mapper.Map<ProfessorDTO>(professor);
            professorDTO.Courses = new List<CourseDTO>();

            if (courses != null)
            {
                foreach (var course in courses)
                {
                    if (course.ProfessorId == request.Id)
                    {
                        var studentCourses = await studentCourseRepository.GetAllStudentCoursesAsync();
                        var courseDTO = new CourseDTO
                        {
                            Id = course.Id,
                            Name = course.Name,
                            Description = course.Description,
                            StudentCourses = new List<StudentCourseDTO>()
                        };

                        if (studentCourses != null)
                        {
                            foreach (var studentCourse in studentCourses)
                            {
                                if (studentCourse.CourseId == course.Id)
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

                                    var studentCourseDTO = new StudentCourseDTO
                                    {
                                        Id = studentCourse.Id,
                                        StudentId = studentCourse.StudentId,
                                        Student = studentDTO,
                                        CourseId = studentCourse.CourseId,
                                        Course = courseDTO
                                    };

                                    courseDTO.StudentCourses.Add(studentCourseDTO);
                                }
                            }
                        }

                        var lessons = await lessonRepository.GetAllLessonsAsync();
                        if (lessons != null)
                        {
                            courseDTO.Lessons = new List<LessonDTO>();

                            foreach (var lesson in lessons)
                            {
                                if (lesson.CourseId == course.Id)
                                {
                                    var lessonDTO = mapper.Map<LessonDTO>(lesson);
                                    courseDTO.Lessons.Add(lessonDTO);
                                }
                            }
                        }

                        professorDTO.Courses.Add(courseDTO);
                    }
                }
            }
            else
            {
                Console.WriteLine("Courses is null");
            }

            return professorDTO;
        }
    }
}
