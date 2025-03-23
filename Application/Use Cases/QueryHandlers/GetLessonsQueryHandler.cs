using Application.DTOs;
using Application.Use_Cases.Queries;
using AutoMapper;
using Domain.Repositories;
using MediatR;

namespace Application.Use_Cases.QueryHandlers
{
    public class GetLessonsQueryHandler : IRequestHandler<GetLessonsQuery, List<LessonDTO>>
    {
        private readonly ILessonRepository repository;
        private readonly ICourseRepository courseRepository;
        private readonly IProfessorRepository professorRepository;
        private readonly IStudentCourseRepository studentCourseRepository;
        private readonly IStudentRepository studentRepository;
        private readonly IMapper mapper;

        public GetLessonsQueryHandler(ILessonRepository repository, IMapper mapper, ICourseRepository courseRepository, IProfessorRepository professorRepository, IStudentCourseRepository studentCourseRepository, IStudentRepository studentRepository)
        {
            this.repository = repository;
            this.mapper = mapper;
            this.courseRepository = courseRepository;
            this.professorRepository = professorRepository;
            this.studentCourseRepository = studentCourseRepository;
            this.studentRepository = studentRepository;
        }

        public async Task<List<LessonDTO>> Handle(GetLessonsQuery request, CancellationToken cancellationToken)
        {
            var (lessons, totalCount) = await repository.GetFilteredLessonsAsync(request.Name, request.CourseId, request.PageNumber, request.PageSize);
            var lessonDTOs = mapper.Map<List<LessonDTO>>(lessons);

            foreach (var lessonDTO in lessonDTOs)
            {
                var course = await courseRepository.GetCourseByIdAsync(lessonDTO.CourseId);
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
            }

            return lessonDTOs;
        }
    }

}