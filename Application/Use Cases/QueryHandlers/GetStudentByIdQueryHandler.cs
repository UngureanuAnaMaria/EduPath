using Application.DTOs;
using Application.Use_Cases.Queries;
using AutoMapper;
using Domain.Entities;
using Domain.Repositories;
using MediatR;

namespace Application.Use_Cases.QueryHandlers
{
    public class GetStudentByIdQueryHandler : IRequestHandler<GetStudentByIdQuery, StudentDTO>
    {
        private readonly IStudentRepository studentRepository;
        private readonly ICourseRepository courseRepository;
        private readonly IProfessorRepository professorRepository;
        private readonly ILessonRepository lessonRepository;
        private readonly IMapper mapper;

        public GetStudentByIdQueryHandler(IStudentRepository studentRepository, ILessonRepository lessonRepository,ICourseRepository courseRepository, IProfessorRepository professorRepository, IMapper mapper)
        {
            this.studentRepository = studentRepository;
            this.courseRepository = courseRepository;
            this.professorRepository = professorRepository;
            this.lessonRepository = lessonRepository;
            this.mapper = mapper;
        }

        public async Task<StudentDTO> Handle(GetStudentByIdQuery request, CancellationToken cancellationToken)
        {
            var student = await studentRepository.GetStudentByIdAsync(request.Id);

            if (student == null)
            {
                Console.WriteLine($"Student with ID {request.Id} not found.");
                return new StudentDTO();
            }

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
                Console.WriteLine("StudentCourses is null");
            }

            var studentDTO = mapper.Map<StudentDTO>(student);

            return studentDTO;
        }


    }
}
