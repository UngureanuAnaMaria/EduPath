export interface CourseDTO {
  id: string;
  name: string;
  description: string;
  professorId: string;
  professor?: ProfessorDTO[];
  lessons?: LessonDTO[];
  studentCourses?: StudentCourseDTO[];
}

export interface StudentCourseDTO {
  id: string;
  studentId: string;
  courseId: string;
}

export interface LessonDTO {
  id: string;
  name: string;
  content: string;
  courseId: string;
  course?: CourseDTO[];
}

export interface ProfessorDTO {
  id: string;
  name: string;
  email: string;
  password: string;
  status: boolean;
  createdAt: Date;
  lastLogin: Date;
  courses?: CourseDTO[];
}