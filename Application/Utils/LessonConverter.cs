using System;
using System.Text.Json;
using System.Text.Json.Serialization;
using Application.DTOs;

namespace Application.Utils
{
    public class LessonConverter : JsonConverter<LessonDTO>
    {
        public override LessonDTO Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        {
            throw new NotImplementedException();
        }

        public override void Write(Utf8JsonWriter writer, LessonDTO value, JsonSerializerOptions options)
        {
            writer.WriteStartObject();
            writer.WriteString("id", value.Id.ToString());
            writer.WriteString("name", value.Name);
            writer.WriteString("content", value.Content);
            writer.WriteString("courseId", value.CourseId.ToString());

            if (value.Course != null)
            {
                writer.WritePropertyName("course");
                writer.WriteStartObject();
                writer.WriteString("id", value.Course.Id.ToString());
                writer.WriteString("name", value.Course.Name);
                writer.WriteString("description", value.Course.Description);

                if (value.Course.ProfessorId.HasValue)
                {
                    writer.WriteString("professorId", value.Course.ProfessorId?.ToString());
                    writer.WritePropertyName("professor");
                    writer.WriteStartObject();
                    writer.WriteString("id", value.Course.Professor?.Id.ToString());
                    writer.WriteString("name", value.Course.Professor?.Name);
                    writer.WriteString("email", value.Course.Professor?.Email);
                    writer.WriteString("password", value.Course.Professor?.Password);
                    writer.WriteBoolean("status", value.Course.Professor?.Status ?? false);
                    writer.WriteString("createdAt", value.Course.Professor?.CreatedAt.ToString("o"));
                    writer.WriteString("lastLogin", value.Course.Professor?.LastLogin?.ToString("o"));
                    writer.WriteEndObject();
                }

                if (value.Course.StudentCourses != null)
                {
                    writer.WritePropertyName("studentCourses");
                    writer.WriteStartArray();

                    foreach (var studentCourse in value.Course.StudentCourses)
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

                writer.WriteEndObject();
            }

            writer.WriteEndObject();
        }
    }

}