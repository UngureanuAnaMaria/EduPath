using Application.DTOs;
using Application.Responses;
using Application.Use_Cases.Queries;
using AutoMapper;
using Domain.Entities;
using Domain.Repositories;
using MediatR;

namespace Application.Use_Cases.QueryHandlers
{
    public class GetStudentsQueryHandler : IRequestHandler<GetStudentsQuery, GetStudentsResponse>
    {
        private readonly IStudentRepository studentRepository;
        private readonly ICourseRepository courseRepository;
        private readonly IProfessorRepository professorRepository;
        private readonly ILessonRepository lessonRepository;
        private readonly IMapper mapper;

        public GetStudentsQueryHandler(
            IStudentRepository studentRepository,
            ILessonRepository lessonRepository,
            ICourseRepository courseRepository,
            IProfessorRepository professorRepository,
            IMapper mapper)
        {
            this.studentRepository = studentRepository;
            this.courseRepository = courseRepository;
            this.professorRepository = professorRepository;
            this.lessonRepository = lessonRepository;
            this.mapper = mapper;
        }

        public async Task<GetStudentsResponse> Handle(GetStudentsQuery request, CancellationToken cancellationToken)
        {
            var (students, totalCount) = await studentRepository.GetFilteredStudentsAsync(request.Name, request.Status, request.PageNumber, request.PageSize);

            if (students == null || !students.Any())
            {
                Console.WriteLine($"Students not found.");
                return new GetStudentsResponse { Students = new List<StudentDTO>(), TotalCount = 0 };
            }

            foreach (var student in students)
            {
                if (student.StudentCourses != null)
                {
                    foreach (var studentCourse in student.StudentCourses)
                    {
                        var course = await courseRepository.GetCourseByIdAsync(studentCourse.CourseId);
                        if (course != null)
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

                            studentCourse.Course = mapper.Map<Course>(courseDTO);
                        }
                    }
                }
                else
                {
                    Console.WriteLine($"StudentCourses is null for Student ID: {student.Id}");
                }
            }

            return new GetStudentsResponse
            {
                Students = mapper.Map<List<StudentDTO>>(students),
                TotalCount = totalCount
            };
        }
    }

}