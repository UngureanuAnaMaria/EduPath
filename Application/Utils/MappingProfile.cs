using Application.DTOs;
using Application.Use_Cases.Commands.Create;
using Application.Use_Cases.Commands.Update;
using AutoMapper;
using Domain.Entities;

namespace Application.Utils
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<Student, StudentDTO>().ReverseMap();
            CreateMap<CreateStudentCommand, Student>().ReverseMap();
            CreateMap<UpdateStudentCommand, Student>().ReverseMap();

            CreateMap<StudentCourse, StudentCourseDTO>().ReverseMap();
            CreateMap<CreateStudentCourseCommand, StudentCourse>().ReverseMap();
            CreateMap<CreateCourseStudentCommand, StudentCourse>().ReverseMap();

            CreateMap<Professor, ProfessorDTO>().ReverseMap();
            CreateMap<CreateProfessorCommand, Professor>().ReverseMap();
            CreateMap<UpdateProfessorCommand, Professor>().ReverseMap();

            CreateMap<Course, CourseDTO>().ReverseMap();
            CreateMap<CreateCourseCommand, Course>().ReverseMap();
            CreateMap<UpdateCourseCommand, Course>().ReverseMap();

            CreateMap<Lesson, LessonDTO>().ReverseMap();
            CreateMap<CreateLessonCommand, Lesson>().ReverseMap();
            CreateMap<UpdateLessonCommand, Lesson>().ReverseMap();
        }
    }
}
