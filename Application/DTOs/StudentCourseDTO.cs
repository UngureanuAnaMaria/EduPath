using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.DTOs
{
    public class StudentCourseDTO
    {
        public Guid Id { get; set; }
        public Guid StudentId { get; set; }
        public StudentDTO? Student { get; set; }
        public Guid CourseId { get; set; }
        public CourseDTO? Course { get; set; }
    }
}
