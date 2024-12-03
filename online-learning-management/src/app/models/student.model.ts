export interface StudentDTO {
  id: string;
  name: string;
  email: string;
  password: string;
  status: boolean;
  createdAt: Date;
  lastLogin: Date;
  studentCourses?: StudentCourseDTO[];
}

export interface StudentCourseDTO {
  id: string;
  studentId: string;
  courseId: string;
}

