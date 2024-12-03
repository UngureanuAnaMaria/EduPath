using System.Text.Json;
using System.Text.Json.Serialization;
using Application.DTOs;
using Domain.Repositories;
public class ProfessorConverter : JsonConverter<ProfessorDTO>
{
    public override ProfessorDTO Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        throw new NotImplementedException();
    }

    public override void Write(Utf8JsonWriter writer, ProfessorDTO value, JsonSerializerOptions options)
    {
        writer.WriteStartObject();
        writer.WriteString("id", value.Id.ToString());
        writer.WriteString("name", value.Name);
        writer.WriteString("email", value.Email);
        writer.WriteString("password", value.Password);
        writer.WriteBoolean("status", value.Status);
        writer.WriteString("createdAt", value.CreatedAt.ToString("o"));
        writer.WriteString("lastLogin", value.LastLogin?.ToString("o"));

        writer.WritePropertyName("courses");
        writer.WriteStartArray();
        if (value.Courses != null)
        {
            foreach (var course in value.Courses)
            {
                writer.WriteStartObject();
                writer.WriteString("id", course.Id.ToString());
                writer.WriteString("name", course.Name);
                writer.WriteString("description", course.Description);

                writer.WritePropertyName("studentCourses");
                writer.WriteStartArray();
                if (course.StudentCourses != null)
                {
                    foreach (var studentCourse in course.StudentCourses)
                    {
                        writer.WriteStartObject();
                        writer.WriteString("id", studentCourse.Id.ToString());
                        writer.WriteString("studentId", studentCourse.StudentId.ToString());

                        writer.WritePropertyName("student");
                        if (studentCourse.Student != null)
                        {
                            writer.WriteStartObject();
                            writer.WriteString("id", studentCourse.Student.Id.ToString());
                            writer.WriteString("name", studentCourse.Student.Name);
                            writer.WriteString("email", studentCourse.Student.Email);
                            writer.WriteString("password", studentCourse.Student.Password);
                            writer.WriteBoolean("status", studentCourse.Student.Status);
                            writer.WriteString("createdAt", studentCourse.Student.CreatedAt.ToString("o"));
                            writer.WriteString("lastLogin", studentCourse.Student.LastLogin?.ToString("o"));
                            writer.WriteEndObject();
                        }
                        writer.WriteEndObject();
                    }
                }
                writer.WriteEndArray();

                writer.WritePropertyName("lessons");
                writer.WriteStartArray();
                if (course.Lessons != null)
                {
                    foreach (var lesson in course.Lessons)
                    {
                        writer.WriteStartObject();
                        writer.WriteString("id", lesson.Id.ToString());
                        writer.WriteString("name", lesson.Name);
                        writer.WriteString("content", lesson.Content);
                        writer.WriteString("courseId", lesson.CourseId.ToString());
                        writer.WriteEndObject();
                    }
                }
                writer.WriteEndArray();

                writer.WriteEndObject();
            }
        }
        writer.WriteEndArray();

        writer.WriteEndObject();
    }
}


