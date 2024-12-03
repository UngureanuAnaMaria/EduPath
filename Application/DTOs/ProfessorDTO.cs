﻿using Domain.Entities;

namespace Application.DTOs
{
    public class ProfessorDTO
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public bool Status { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime? LastLogin { get; set; }
        public List<CourseDTO>? Courses { get; set; }
    }
}
