using Application.DTOs;
using Application.Responses;
using Application.Use_Cases.Queries;
using AutoMapper;
using Domain.Entities;
using Domain.Repositories;
using MediatR;

namespace Application.Use_Cases.QueryHandlers
{
    public class GetCoursesQueryHandler : IRequestHandler<GetCoursesQuery, GetCoursesResponse>
    {
        private readonly IStudentCourseRepository studentCourseRepository;
        private readonly ICourseRepository courseRepository;
        private readonly IProfessorRepository professorRepository;
        private readonly IStudentRepository studentRepository;
        private readonly ILessonRepository lessonRepository;
        private readonly IMapper mapper;

        public GetCoursesQueryHandler(ICourseRepository courseRepository, IStudentCourseRepository studentCourseRepository, ILessonRepository lessonRepository, IProfessorRepository professorRepository, IStudentRepository studentRepository, IMapper mapper)
        {
            this.courseRepository = courseRepository;
            this.studentCourseRepository = studentCourseRepository;
            this.professorRepository = professorRepository;
            this.studentRepository = studentRepository;
            this.lessonRepository = lessonRepository;
            this.mapper = mapper;
        }

        public async Task<GetCoursesResponse> Handle(GetCoursesQuery request, CancellationToken cancellationToken)
        {
            var (courses, totalCount) = await courseRepository.GetFilteredCoursesAsync(request.Name, request.ProfessorId, request.PageNumber, request.PageSize);

            if (courses == null || !courses.Any())
            {
                Console.WriteLine($"Courses not found.");
                return new GetCoursesResponse { Courses = new List<CourseDTO>(), TotalCount = 0 };
            }

            var courseDTOs = new List<CourseDTO>();

            foreach (var course in courses)
            {
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
                    courseDTO.StudentCourses = new List<StudentCourseDTO>();

                    foreach (var studentCourse in studentCourses)
                    {
                        if (studentCourse.CourseId == course.Id)
                        {
                            var student = await studentRepository.GetStudentByIdAsync(studentCourse.StudentId);

                            if (student != null)
                            {
                                var studentDTO = mapper.Map<StudentDTO>(student);
                                var studentCourseDTO = new StudentCourseDTO
                                {
                                    Student = studentDTO,
                                    CourseId = studentCourse.CourseId,
                                    StudentId = studentCourse.StudentId
                                };
                                courseDTO.StudentCourses.Add(studentCourseDTO);
                            }
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

                courseDTOs.Add(courseDTO);
            }

            return new GetCoursesResponse
            {
                Courses = courseDTOs,
                TotalCount = totalCount
            };
        }
    }
}
