using Application.DTOs;
using Application.Use_Cases.Queries;
using AutoMapper;
using Domain.Repositories;
using MediatR;

namespace Application.Use_Cases.QueryHandlers
{
    public class GetProfessorsQueryHandler : IRequestHandler<GetProfessorsQuery, List<ProfessorDTO>>
    {
        private readonly IProfessorRepository professorRepository;
        private readonly ICourseRepository courseRepository;
        private readonly IStudentCourseRepository studentCourseRepository;
        private readonly IStudentRepository studentRepository;
        private readonly ILessonRepository lessonRepository;
        private readonly IMapper mapper;

        public GetProfessorsQueryHandler(IProfessorRepository professorRepository, ILessonRepository lessonRepository, IMapper mapper, ICourseRepository courseRepository, IStudentCourseRepository studentCourseRepository, IStudentRepository studentRepository)
        {
            this.professorRepository = professorRepository;
            this.mapper = mapper;
            this.courseRepository = courseRepository;
            this.studentCourseRepository = studentCourseRepository;
            this.studentRepository = studentRepository;
            this.lessonRepository = lessonRepository;
        }

        public async Task<List<ProfessorDTO>> Handle(GetProfessorsQuery request, CancellationToken cancellationToken)
        {
            var (professors, totalCount) = await professorRepository.GetFilteredProfessorsAsync(request.Name, request.Status, request.PageNumber, request.PageSize);

            if (professors == null || !professors.Any())
            {
                Console.WriteLine($"Professors not found.");
                return new List<ProfessorDTO>();
            }

            var courses = await courseRepository.GetAllCoursesAsync();
            var professorDTOs = mapper.Map<List<ProfessorDTO>>(professors);

            foreach (var professorDTO in professorDTOs)
            {
                professorDTO.Courses = new List<CourseDTO>();

                foreach (var course in courses)
                {
                    if (course.ProfessorId == professorDTO.Id)
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

            return professorDTOs;
        }
    }
}
