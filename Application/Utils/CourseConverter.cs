using System.Text.Json;
using System.Text.Json.Serialization;
using Application.DTOs;

public class CourseConverter : JsonConverter<CourseDTO>
{
    public override CourseDTO Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        throw new NotImplementedException();
    }

    public override void Write(Utf8JsonWriter writer, CourseDTO value, JsonSerializerOptions options)
    {
        writer.WriteStartObject();
        writer.WriteString("id", value.Id.ToString());
        writer.WriteString("name", value.Name);
        writer.WriteString("description", value.Description);

        if (value.ProfessorId.HasValue)
        {
            writer.WriteString("professorId", value.ProfessorId?.ToString());
            writer.WritePropertyName("professor");
            writer.WriteStartObject();
            writer.WriteString("id", value.Professor?.Id.ToString());
            writer.WriteString("name", value.Professor?.Name);
            writer.WriteString("email", value.Professor?.Email);
            writer.WriteString("password", value.Professor?.Password);
            writer.WriteBoolean("status", value.Professor?.Status ?? false);
            writer.WriteString("createdAt", value.Professor?.CreatedAt.ToString("o"));
            writer.WriteString("lastLogin", value.Professor?.LastLogin?.ToString("o"));
            writer.WriteEndObject();
        }

       
        if (value.StudentCourses != null)
        {
            writer.WritePropertyName("studentCourses");
            writer.WriteStartArray();

            foreach (var studentCourse in value.StudentCourses)
            {
                writer.WriteStartObject();
                writer.WriteString("id", studentCourse.Id.ToString());
                writer.WriteString("studentId", studentCourse.StudentId.ToString());
                writer.WriteStartObject("student");
                writer.WriteString("id", studentCourse.Student?.Id.ToString());
                writer.WriteString("name", studentCourse.Student?.Name);
                writer.WriteString("email", studentCourse.Student?.Email);
                writer.WriteString("password", studentCourse.Student?.Password);
                writer.WriteBoolean("status", studentCourse.Student?.Status ?? false);
                writer.WriteString("createdAt", studentCourse.Student?.CreatedAt.ToString("o"));
                writer.WriteString("lastLogin", studentCourse.Student?.LastLogin?.ToString("o"));
                writer.WriteEndObject();
                writer.WriteEndObject();
            }

            writer.WriteEndArray();
        }

        if (value.Lessons != null)
        {
            writer.WritePropertyName("lessons");
            writer.WriteStartArray();

            foreach (var lesson in value.Lessons)
            {
                writer.WriteStartObject();
                writer.WriteString("id", lesson.Id.ToString());
                writer.WriteString("name", lesson.Name);
                writer.WriteString("content", lesson.Content);
                writer.WriteString("courseId", lesson.CourseId.ToString());
                writer.WriteEndObject();
            }

            writer.WriteEndArray();
        }

        writer.WriteEndObject();
    }
}

