using System.Text.Json;
using System.Text.Json.Serialization;
using Application.DTOs;
using Domain.Entities;

namespace Application.Utils
{
    public class StudentConverter : JsonConverter<StudentDTO>
    {
        //public override StudentDTO Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        //{
        //    throw new NotImplementedException();
        //}
        public override StudentDTO Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        {
            if (reader.TokenType != JsonTokenType.StartObject)
            {
                throw new JsonException();
            }

            var student = new StudentDTO();

            while (reader.Read())
            {
                if (reader.TokenType == JsonTokenType.EndObject)
                {
                    return student;
                }

                if (reader.TokenType == JsonTokenType.PropertyName)
                {
                    string propertyName = reader.GetString();

                    reader.Read();

                    switch (propertyName)
                    {
                        case "id":
                            student.Id = reader.GetGuid();
                            break;
                        case "name":
                            student.Name = reader.GetString();
                            break;
                        case "email":
                            student.Email = reader.GetString();
                            break;
                        case "password":
                            student.Password = reader.GetString();
                            break;
                        case "status":
                            student.Status = reader.GetBoolean();
                            break;
                        case "createdAt":
                            student.CreatedAt = reader.GetDateTime();
                            break;
                        case "lastLogin":
                            student.LastLogin = reader.GetDateTime();
                            break;
                        case "studentCourses":
                            student.StudentCourses = JsonSerializer.Deserialize<List<StudentCourseDTO>>(ref reader, options);
                            break;
                        default:
                            reader.Skip();
                            break;
                    }
                }
            }

            throw new JsonException();
        }

        public override void Write(Utf8JsonWriter writer, StudentDTO value, JsonSerializerOptions options)
        {
            writer.WriteStartObject();
            writer.WriteString("id", value.Id.ToString());
            writer.WriteString("name", value.Name);
            writer.WriteString("email", value.Email);
            writer.WriteString("password", value.Password);
            writer.WriteBoolean("status", value.Status);
            writer.WriteString("createdAt", value.CreatedAt.ToString("o"));
            writer.WriteString("lastLogin", value.LastLogin?.ToString("o"));

            if (value.StudentCourses != null)
            {
                writer.WritePropertyName("studentCourses");
                writer.WriteStartArray();
                foreach (var studentCourse in value.StudentCourses)
                {
                    writer.WriteStartObject();
                    writer.WriteString("id", studentCourse.Id.ToString());
                    writer.WriteString("courseId", studentCourse.CourseId.ToString());
                    writer.WriteStartObject("course");
                    writer.WriteString("id", studentCourse.Course?.Id.ToString());
                    writer.WriteString("name", studentCourse.Course?.Name);
                    writer.WriteString("description", studentCourse.Course?.Description);

                    if (studentCourse.Course.ProfessorId.HasValue)
                    {
                        writer.WriteString("professorId", studentCourse.Course.ProfessorId?.ToString());
                        writer.WriteStartObject("professor");
                        writer.WriteString("id", studentCourse.Course.Professor.Id.ToString());
                        writer.WriteString("name", studentCourse.Course.Professor.Name);
                        writer.WriteString("email", studentCourse.Course.Professor.Email);
                        writer.WriteString("password", studentCourse.Course.Professor.Password);
                        writer.WriteBoolean("status", studentCourse.Course.Professor.Status);
                        writer.WriteString("createdAt", studentCourse.Course.Professor.CreatedAt.ToString("o"));
                        writer.WriteString("lastLogin", studentCourse.Course.Professor.LastLogin?.ToString("o"));
                        writer.WriteEndObject();
                    }

                    if (studentCourse.Course.Lessons != null)
                    {
                        writer.WritePropertyName("lessons");
                        writer.WriteStartArray();

                        foreach (var lesson in studentCourse.Course.Lessons)
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
                    writer.WriteEndObject();
                }
                writer.WriteEndArray();
            }


            writer.WriteEndObject();
        }
    }

}